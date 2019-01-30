using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Permissions;
using OPCDataAccessLibraries.BaseEntity.Enums;
using OPCDataAccessLibraries.BaseEntity.Interfaces;
using OPCDataAccessLibraries.BaseEntity.Structures;

namespace OPCDataAccessLibraries.BaseEntity
{
	/// <inheritdoc />
	/// <summary>
	/// OPC DA group. Helps to access to item values.
	/// </summary>
	public class Group : IDisposable
	{
		#region DataCallback

		private class DataCallback : IOPCDataCallback
		{
			public DataCallback(Group @group)
			{
				_group = @group;
			}

			public void OnDataChange(int transactionId, int groupId, int quality, int error, uint count, int[] clientIds, IntPtr values, short[] qualities, long[] timeStamps, int[] errors)
			{
				if (_group.ClientId != groupId)
					return;

				_group._dataChangeHandlers?.Invoke(_group, new DataChangeEventArgs(
						groupId,
						transactionId,
						quality,
						error,
						ItemValueReader.Read(clientIds, values, qualities, timeStamps, errors)));
			}

			public void OnReadComplete(int transactionId, int groupId, int quality, int error, uint count, int[] clientIds, IntPtr values, short[] qualities, long[] timeStamps, int[] errors)
			{
				if (_group.ClientId != groupId)
					return;

				_group._readCompleteHandlers?.Invoke(_group, new DataChangeEventArgs(
						groupId,
						transactionId,
						quality,
						error,
						ItemValueReader.Read(clientIds, values, qualities, timeStamps, errors)));
			}

			public void OnWriteComplete(int transactionId, int groupId, int error, uint count, int[] clientIds, int[] errors)
			{
				if (_group.ClientId != groupId)
					return;

				var handler = _group._writeCompleteHandlers;
				if (handler != null)
				{
					var results = new KeyValuePair<int, int>[count];
					for (var i = 0; i < count; i++)
						results[i] = new KeyValuePair<int, int>(clientIds[i], errors[i]);

					handler(_group, new WriteCompleteEventArgs(groupId, transactionId, error, results));
				}
			}

			public void OnCancelComplete(int transactionId, int groupId)
			{
				if (_group.ClientId != groupId)
					return;

				_group._cancelCompleteHandlers?.Invoke(_group, new CompleteEventArgs(groupId, transactionId));
			}

			private readonly Group _group;
		}

		#endregion

		internal Group(DaServer server, int clientId, int serverId, string name, int updateRate, IOPCItemMgt @group)
		{
			_server = server;
			ClientId = clientId;
			ServerId = serverId;
			Name = name;
			_group = @group;
			UpdateRate = updateRate;

			_syncIo = (IOPCSyncIO)@group;
			_groupManagement = (IOPCGroupStateMgt)@group;
			try
			{
				_asyncIo = (IOPCAsyncIO2)@group;
			}
			catch (InvalidCastException)
			{
			}
			try
			{
				_connectionPointContainer = (IConnectionPointContainer)@group;
			}
			catch (InvalidCastException)
			{
			}
		}

		[SecurityPermission(SecurityAction.LinkDemand)]
		~Group()
		{
			Dispose(false);
		}

		/// <summary>
		/// Item values is changed.
		/// </summary>
		public event EventHandler<DataChangeEventArgs> DataChange
		{
			add
			{
				InitializeAsyncMode();

				_dataChangeHandlers += value;
			}
			remove
			{
				// ReSharper disable DelegateSubtraction
				_dataChangeHandlers -= value;
				// ReSharper restore DelegateSubtraction
			}
		}

		/// <summary>
		/// Reading data is completed.
		/// </summary>
		public event EventHandler<DataChangeEventArgs> ReadComplete
		{
			add
			{
				InitializeAsyncMode();

				_readCompleteHandlers += value;
			}
			remove
			{
				// ReSharper disable DelegateSubtraction
				_readCompleteHandlers -= value;
				// ReSharper restore DelegateSubtraction
			}
		}

		/// <summary>
		/// Writing data is completed.
		/// </summary>
		public event EventHandler<WriteCompleteEventArgs> WriteComplete
		{
			add
			{
				InitializeAsyncMode();

				_writeCompleteHandlers += value;
			}
			remove
			{
				// ReSharper disable DelegateSubtraction
				_writeCompleteHandlers -= value;
				// ReSharper restore DelegateSubtraction
			}
		}

		/// <summary>
		/// Cancellation is completed.
		/// </summary>
		public event EventHandler<CompleteEventArgs> CancelComplete
		{
			add
			{
				InitializeAsyncMode();

				_cancelCompleteHandlers += value;
			}
			remove
			{
				// ReSharper disable DelegateSubtraction
				_cancelCompleteHandlers -= value;
				// ReSharper restore DelegateSubtraction
			}
		}

		/// <summary>
		/// OPC DA group client ID.
		/// </summary>
		public int ClientId { get; private set; }

		/// <summary>
		/// OPC DA group server ID.
		/// </summary>
		public int ServerId { get; private set; }

		/// <summary>
		/// Name of OPC DA group.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Item values update rate.
		/// </summary>
		public int UpdateRate { get; private set; }

		/// <summary>
		/// true - OPC DA server supports asynchronous reading.
		/// </summary>
		public bool IsAsyncIoSupported
		{
			get { return _asyncIo != null && _connectionPointContainer != null; }
		}

		/// <summary>
		/// Retrieves OPC DA group properties.
		/// </summary>
		/// <returns>OPC DA group properties.</returns>
		public GroupProperties GetProperties()
		{
			if (_group == null)
				throw new ObjectDisposedException("Group");

			_groupManagement.GetState(
				out var updateRate,
				out var activeAsInt,
				out var name,
				out var timeBias,
				out var percentDeadband,
				out var locale,
				out var clientId,
				out var serverId);

			return new GroupProperties
			{
				UpdateRate = updateRate,
				Active = activeAsInt != 0,
				Name = name,
				TimeBias = timeBias,
				PercentDeadband = percentDeadband,
				Locale = new CultureInfo(locale),
				ClientId = clientId,
				ServerId = serverId,
			};
		}

		/// <summary>
		/// Sets new OPC DA group properties.
		/// </summary>
		/// <param name="properties">New OPC DA group properties.</param>
		public void SetProperties(GroupProperties properties)
		{
			if (_group == null)
				throw new ObjectDisposedException("Group");

			_groupManagement.SetState(
				properties.UpdateRate,
				out var revisedUpdateRate,
				properties.Active ? 1 : 0,
				properties.TimeBias,
				properties.PercentDeadband,
				properties.Locale.LCID,
				properties.ClientId);

			ClientId = properties.ClientId;
			UpdateRate = revisedUpdateRate;

			if (!string.Equals(properties.Name, Name))
				_groupManagement.SetName(properties.Name);
			Name = properties.Name;
		}

		/// <summary>
		/// Adds items to OPC DA group to read/write their values.
		/// </summary>
		/// <param name="items">Items to add.</param>
		/// <returns>Result of each item adding .</returns>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public ItemResult[] AddItems(Item[] items)
		{
			if (_group == null)
				throw new ObjectDisposedException("Group");
			if (items == null)
				throw new ArgumentNullException(nameof(items));

			if (items.Length == 0)
				return new ItemResult[0];

			using (var reader = new ItemResultReader(items))
			{
				_group.AddItems((uint)items.Length, reader.Items, out var dataPtr, out var errorsPtr);
				return reader.Read(dataPtr, errorsPtr);
			}
		}

		/// <summary>
		/// Tests OPC DA group items before adding.
		/// </summary>
		/// <param name="items">Items to test.</param>
		/// <returns>>Result of each item testing .</returns>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public ItemResult[] ValidateItems(Item[] items)
		{
			if (_group == null)
				throw new ObjectDisposedException("Group");
			if (items == null)
				throw new ArgumentNullException(nameof(items));
			if (items.Length == 0)
				return new ItemResult[0];

			using (var reader = new ItemResultReader(items))
			{
				_group.ValidateItems((uint)items.Length, reader.Items, 0, out var dataPtr, out var errorsPtr);
				return reader.Read(dataPtr, errorsPtr);
			}
		}

		/// <summary>
		/// Removes OPC DA group items to stop reading/writing their values.
		/// </summary>
		/// <param name="serverIds">Server ID of items.</param>
		/// <returns>Result of each item removing.</returns>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public int[] RemoveItems(int[] serverIds)
		{
			if (_group == null)
				throw new ObjectDisposedException("Group");
			if (serverIds == null)
				throw new ArgumentNullException(nameof(serverIds));
			if (serverIds.Length == 0)
				return new int[0];

			_group.RemoveItems((uint)serverIds.Length, serverIds, out var errorsPtr);

			return ItemResultReader.Read(serverIds.Length, errorsPtr);
		}

		/// <summary>
		/// Changes OPC Da group item state.
		/// </summary>
		/// <param name="serverIds">Server ID of items.</param>
		/// <param name="active">true - active state.</param>
		/// <returns>Result of each item changing.</returns>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public int[] SetActiveState(int[] serverIds, bool active)
		{
			if (_group == null)
				throw new ObjectDisposedException("Group");
			if (serverIds == null)
				throw new ArgumentNullException(nameof(serverIds));
			if (serverIds.Length == 0)
				return new int[0];

			_group.SetActiveState((uint)serverIds.Length, serverIds, active ? 1 : 0, out var errorsPtr);

			return ItemResultReader.Read(serverIds.Length, errorsPtr);
		}

		/// <summary>
		/// Sets client ID for each OPC DA group item.
		/// </summary>
		/// <param name="serverIds">Server ID of items.</param>
		/// <param name="clientIds">New client ID of items.</param>
		/// <returns>Result of each item changing.</returns>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public int[] SetClientHandles(int[] serverIds, int[] clientIds)
		{
			if (_group == null)
				throw new ObjectDisposedException("Group");
			if (serverIds == null)
				throw new ArgumentNullException(nameof(serverIds));
			if (serverIds.Length == 0)
				return new int[0];

			_group.SetClientHandles((uint)serverIds.Length, serverIds, clientIds, out var errorsPtr);

			return ItemResultReader.Read(serverIds.Length, errorsPtr);
		}

		/// <summary>
		/// Sets data type for each OPC DA group item.
		/// </summary>
		/// <param name="serverIds">Server ID of items.</param>
		/// <param name="types">New data type of items.</param>
		/// <returns>Result of each item changing.</returns>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public int[] SetDatatypes(int[] serverIds, VarEnum[] types)
		{
			if (_group == null)
				throw new ObjectDisposedException("Group");
			if (serverIds == null)
				throw new ArgumentNullException(nameof(serverIds));
			if (serverIds.Length == 0)
				return new int[0];

			var typesAsShort = new short[types.Length];
			for (var i = 0; i < types.Length; i++)
				typesAsShort[i] = (short)types[i];

			_group.SetDatatypes((uint)serverIds.Length, serverIds, typesAsShort, out var errorsPtr);

			return ItemResultReader.Read(serverIds.Length, errorsPtr);
		}

		/// <summary>
		/// Reads OPC DA group item values synchronous.
		/// </summary>
		/// <param name="source">Read mode, see <see cref="DataSource"/>.</param>
		/// <param name="serverIds">Server ID of items.</param>
		/// <returns>OPC DA group item values.</returns>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public ItemValue[] SyncReadItems(DataSource source, int[] serverIds)
		{
			if (_group == null)
				throw new ObjectDisposedException("Group");
			if (serverIds == null)
				throw new ArgumentNullException(nameof(serverIds));
			if (serverIds.Length == 0)
				return new ItemValue[0];

			_syncIo.Read(source, (uint)serverIds.Length, serverIds, out var dataPtr, out var errorsPtr);

			return ItemValueReader.Read(serverIds.Length, dataPtr, errorsPtr);
		}

		/// <summary>
		/// Writes OPC DA group item values synchronous.
		/// </summary>
		/// <param name="serverIds">Server ID of items.</param>
		/// <param name="values">Item values.</param>
		/// <returns>>Result of each item writing.</returns>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public int[] SyncWriteItems(int[] serverIds, object[] values)
		{
			if (_group == null)
				throw new ObjectDisposedException("Group");
			if (serverIds == null)
				throw new ArgumentNullException(nameof(serverIds));
			if (values == null)
				throw new ArgumentNullException(nameof(values));
			if (serverIds.Length == 0)
				return new int[0];

			using (var writer = new ItemValueWriter(values))
			{
				_syncIo.Write((uint)serverIds.Length, serverIds, writer.Values, out var errorsPtr);

				return ItemResultReader.Read(serverIds.Length, errorsPtr);
			}
		}

		/// <summary>
		/// Reads OPC DA group item values asynchronous.
		/// </summary>
		/// <param name="serverIds">Server ID of items.</param>
		/// <param name="transactionId">Transaction ID.</param>
		/// <param name="cancelId">Cancellation ID.</param>
		/// <returns>Result of starting of each item reading.</returns>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public int[] AsyncReadItems(int[] serverIds, int transactionId, out int cancelId)
		{
			cancelId = 0;
			if (_group == null)
				throw new ObjectDisposedException("Group");
			if (_asyncIo == null)
				throw new NotSupportedException();
			if (serverIds == null)
				throw new ArgumentNullException(nameof(serverIds));
			if (serverIds.Length == 0)
				return new int[0];

			_asyncIo.Read((uint)serverIds.Length, serverIds, transactionId, out var tmp, out var errorsPtr);

			cancelId = tmp;
			return ItemResultReader.Read(serverIds.Length, errorsPtr);
		}

		/// <summary>
		/// Writes OPC DA group item values asynchronous.
		/// </summary>
		/// <param name="serverIds">Server ID of items.</param>
		/// <param name="values">Item values to write.</param>
		/// <param name="transactionId">Transaction ID.</param>
		/// <param name="cancelId">Cancellation ID.</param>
		/// <returns>Result of starting of each item writing.</returns>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public int[] AsyncWriteItems(int[] serverIds, object[] values, int transactionId, out int cancelId)
		{
			cancelId = 0;
			if (_group == null)
				throw new ObjectDisposedException("Group");
			if (_asyncIo == null)
				throw new NotSupportedException();
			if (serverIds == null)
				throw new ArgumentNullException(nameof(serverIds));
			if (serverIds.Length == 0)
				return new int[0];

			using (var writer = new ItemValueWriter(values))
			{
				_asyncIo.Write((uint)serverIds.Length, serverIds, writer.Values, transactionId, out var tmp, out var errorsPtr);

				cancelId = tmp;
				return ItemResultReader.Read(serverIds.Length, errorsPtr);
			}
		}

		/// <summary>
		/// Starts refresh of item values reading in OPC DA group.
		/// </summary>
		/// <param name="source">Read mode, see <see cref="DataSource"/>.</param>
		/// <param name="transactionId">Transaction ID.</param>
		/// <param name="cancelId">Cancellation ID.</param>
		public void AsyncRefresh(DataSource source, int transactionId, out int cancelId)
		{
			cancelId = 0;
			if (_group == null)
				throw new ObjectDisposedException("Group");
			if (_asyncIo == null)
				throw new NotSupportedException();

			_asyncIo.Refresh2(source, transactionId, out var tmp);

			cancelId = tmp;
		}

		/// <summary>
		/// Cancels transaction.
		/// </summary>
		/// <param name="cancelId">Cancellation ID.</param>
		public void AsyncCancel(int cancelId)
		{
			if (_group == null)
				throw new ObjectDisposedException("Group");

			_asyncIo.Cancel2(cancelId);
		}

		/// <summary>
		/// Enables asynchronous mode.
		/// </summary>
		/// <param name="enable">Asynchronous mode.</param>
		public void AsyncSetEnable(bool enable)
		{
			if (_group == null)
				throw new ObjectDisposedException("Group");
			if (_asyncIo == null)
				throw new NotSupportedException();

			_asyncIo.SetEnable(enable ? 1 : 0);
		}

		/// <summary>
		/// Retrieves asynchronous mode.
		/// </summary>
		/// <returns>Asynchronous mode.</returns>
		public bool AsyncGetEnable()
		{
			if (_group == null)
				throw new ObjectDisposedException("Group");
			if (_asyncIo == null)
				throw new NotSupportedException();

			_asyncIo.GetEnable(out var tmp);

			return tmp != 0;
		}

		/// <summary>
		/// Removes OPC DA group from server.
		/// </summary>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Removes OPC DA group from server.
		/// </summary>
		/// <param name="disposing">true - call in Dispose.</param>
		[SecurityPermission(SecurityAction.LinkDemand)]
		protected virtual void Dispose(bool disposing)
		{
			if (_group != null)
			{
				if (_connectionPoint != null)
				{
					try
					{
						//connectionPoint.Unadvise(asyncCookie);
					}
					finally
					{
						Marshal.ReleaseComObject(_connectionPoint);
						_connectionPoint = null;
					}
				}

				Marshal.ReleaseComObject(_group);

				if (disposing)
					_server.RemoveGroup(ServerId);

				_group = null;
			}
		}

		private void InitializeAsyncMode()
		{
			if (_group == null)
				throw new ObjectDisposedException("Group");
			if (_asyncIo == null || _connectionPointContainer == null)
				throw new NotSupportedException();

			if (_asyncCallback != null)
				return;

			var dataCallbackId = new Guid("39c13a70-011e-11d0-9675-0020afd8adb3");
			_connectionPointContainer.FindConnectionPoint(ref dataCallbackId, out var point);
			if (point == null)
				throw new NotSupportedException();

			var callback = new DataCallback(this);
			point.Advise(callback, out _asyncCookie);

			_asyncCallback = callback;
			_connectionPoint = point;
		}

		private readonly DaServer _server;

		private IOPCItemMgt _group;

		private readonly IOPCGroupStateMgt _groupManagement;

		private readonly IOPCSyncIO _syncIo;

		private readonly IOPCAsyncIO2 _asyncIo;

		private readonly IConnectionPointContainer _connectionPointContainer;

		private DataCallback _asyncCallback;

		private int _asyncCookie;

		private IConnectionPoint _connectionPoint;

		private EventHandler<DataChangeEventArgs> _dataChangeHandlers;

		private EventHandler<DataChangeEventArgs> _readCompleteHandlers;

		private EventHandler<WriteCompleteEventArgs> _writeCompleteHandlers;

		private EventHandler<CompleteEventArgs> _cancelCompleteHandlers;
	}
}
