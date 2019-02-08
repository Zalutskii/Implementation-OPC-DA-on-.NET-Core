﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using OPCDataAccessLibraries.BaseEntity.Structures;

namespace OPCDataAccessLibraries.BaseEntity
{
	internal class ItemResultReader : IDisposable
	{
		[SecurityPermission(SecurityAction.LinkDemand)]
		public ItemResultReader(ICollection<Item> items)
		{
			_size = items.Count;
			Items = Marshal.AllocCoTaskMem(ItemSize * items.Count);
			_stringsToClear = new List<IntPtr>(items.Count * 2);

			var position = 0;
			foreach (var item in items)
			{
				// string szAccessPath;
				var accessPath = Marshal.StringToCoTaskMemUni(item.AccessPath);
				_stringsToClear.Add(accessPath);
				Marshal.WriteIntPtr(Items, position, accessPath);
				position += IntPtr.Size;

				// string szItemID;
				var itemId = Marshal.StringToCoTaskMemUni(item.Tag);
				_stringsToClear.Add(itemId);
				Marshal.WriteIntPtr(Items, position, itemId);
				position += IntPtr.Size;

				// int bActive;
				Marshal.WriteInt32(Items, position, item.Active ? 1 : 0);
				position += sizeof(int);

				// uint hClient;
				Marshal.WriteInt32(Items, position, item.ClientId);
				position += sizeof(int);

				// uint dwBlobSize;
				Marshal.WriteInt32(Items, position, 0);
				position += sizeof(int);
				if (IntPtr.Size == 8)
					position += sizeof(int);

				// IntPtr pBlob;
				Marshal.WriteIntPtr(Items, position, IntPtr.Zero);
				position += IntPtr.Size;

				var type = (short)item.RequestedDataType;
				if (item.RequestedDataSubType != VarEnum.VT_EMPTY)
					type = (short)(type | (short)item.RequestedDataSubType);

				// ushort vtRequestedDataType;
				Marshal.WriteInt16(Items, position, type);
				position += sizeof(short);

				// ushort wReserved;
				Marshal.WriteInt16(Items, position, 0);
				position += sizeof(short);

				if (IntPtr.Size == 8)
					position += sizeof(int);
			}
		}

		~ItemResultReader()
		{
			Dispose(false);
		}

		public IntPtr Items { get; private set; }

		[SecurityPermission(SecurityAction.LinkDemand)]
		public ItemResult[] Read(IntPtr dataPtr, IntPtr errorsPtr)
		{
			try
			{
				var position = 0;
				var result = new ItemResult[_size];
				for (var i = 0; i < _size; i++)
				{
					// ReSharper disable UseObjectOrCollectionInitializer
					result[i] = new ItemResult();
					// ReSharper restore UseObjectOrCollectionInitializer

					// uint hServer;
					result[i].ServerId = Marshal.ReadInt32(dataPtr, position);
					position += sizeof(int);

					// ushort vtCanonicalDataType;
					var type = Marshal.ReadInt16(dataPtr, position);
					position += sizeof(short);
					if (type > (short)VarEnum.VT_VECTOR)
					{
						result[i].CanonicalDataType = (VarEnum)(type & 0xFF00);
						result[i].CanonicalDataSubType = (VarEnum)(type & 0x00FF);
					}
					else
					{
						result[i].CanonicalDataType = (VarEnum)type;
						result[i].CanonicalDataSubType = VarEnum.VT_EMPTY;
					}

					// ushort wReserved;
					position += sizeof(short);

					// uint dwAccessRights;
					result[i].AccessRights = Marshal.ReadInt32(dataPtr, position);
					position += sizeof(int);

					// uint dwBlobSize;
					var blobSize = Marshal.ReadInt32(dataPtr, position);
					position += sizeof(int);

					// IntPtr pBlob;
					var blob = Marshal.ReadIntPtr(dataPtr, position);
					position += IntPtr.Size;
					if (blob != IntPtr.Zero)
					{
						result[i].Blob = new byte[blobSize];
						for (var blobByteIndex = 0; blobByteIndex < blobSize; blobByteIndex++)
							result[i].Blob[blobByteIndex] = Marshal.ReadByte(blob, blobByteIndex);

						Marshal.FreeCoTaskMem(blob);
					}

					result[i].Error = Marshal.ReadInt32(errorsPtr, i * sizeof(int));
				}

				return result;
			}
			finally
			{
				Marshal.FreeCoTaskMem(dataPtr);
				Marshal.FreeCoTaskMem(errorsPtr);
			}
		}

		[SecurityPermission(SecurityAction.LinkDemand)]
		public static int[] Read(int size, IntPtr dataPtr)
		{
			try
			{
				var result = new int[size];
				for (var i = 0; i < size; i++)
					result[i] = Marshal.ReadInt32(dataPtr, i * sizeof(int));
				return result;
			}
			finally
			{
				Marshal.FreeCoTaskMem(dataPtr);
			}
		}

		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		protected void Dispose(bool disposing)
		{
			if (Items != IntPtr.Zero)
				Marshal.FreeCoTaskMem(Items);
			Items = IntPtr.Zero;

			if (_stringsToClear != null)
				foreach (var @string in _stringsToClear)
					Marshal.FreeCoTaskMem(@string);
			_stringsToClear = null;
		}

		private readonly int _size;

		private List<IntPtr> _stringsToClear;

		private static readonly int ItemSize =
			IntPtr.Size +
			IntPtr.Size +
			sizeof(int) +
			sizeof(int) +
			sizeof(int) +
			(IntPtr.Size == 8 ? sizeof(int) : 0) +
			IntPtr.Size +
			sizeof(short) +
			sizeof(short) +
			(IntPtr.Size == 8 ? sizeof(int) : 0);
	}
}
