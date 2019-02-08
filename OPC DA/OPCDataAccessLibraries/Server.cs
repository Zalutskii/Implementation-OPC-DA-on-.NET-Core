using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using OPCDataAccessLibraries.BaseEntity;
using OPCDataAccessLibraries.BaseEntity.Enums;
using OPCDataAccessLibraries.BaseEntity.Structures;

namespace OPCDataAccessLibraries
{
	/// <inheritdoc />
	public sealed class Server : IDisposable
	{

		public Server(ServerDescription description)
		{
			Id = description.Id;
			ServerName = description.Name;
			OnConnected += TestServerConnect;
			DaGroupsOnServer = new ObservableCollection<DaGroup>();
		}

		public string Name { get; set; }
		public string ServerName { get; private set; }

		public Guid Id { get; private set; }

		public ServerProperties ServerProperties { get; set; }

		//public bool Connected
		//{
		//    get {return _serverThread != null; }
		//}
		public void Connect()
		{
			_server = new DaServer(Id);
			OnConnected?.Invoke(this, EventArgs.Empty);
		}

		public async Task ConnectAsync()
		{
			await Task.Run(Connect);
		}

		public void Disconnect()
		{
			_server.Dispose();
			OnDisconnected?.Invoke(this, EventArgs.Empty);
		}

		private void TestServerConnect(object sender, EventArgs e)
		{
			new Thread(() =>
			{
				var bb = true;
				while (bb)
				{
					try
					{
						_server.GetProperties();
						Thread.Sleep(100);
					}
					catch (Exception)
					{
						bb = false;
					}
				}

				Disconnect();
			}).Start();
		}

		public event EventHandler OnDisconnected;
		public event EventHandler OnConnected;

		public ServerProperties GetActiveProperties()
		{
			return _server.GetProperties();
		}

		public async Task<ServerProperties> GetActivePropertiesAsync()
		{
			return await Task.Run(GetActiveProperties);

		}

		public string[] GetTagByCriteria(BrowseType type, string filterCriteria)
		{
			return _server
				.GetAddressSpaceBrowser()
				.GetItemIds(type, filterCriteria, VarEnum.VT_EMPTY, 0)
				.ToArray();
		}

		public Dictionary<string, ItemPropertyValue[]> GetItemPropertyes(IEnumerable<string> tags, int[] propertyIds)
		{
			return tags
				.ToDictionary(
					tag => tag,
					tag => _server.GetItemProperties()
						.GetItemProperties(tag, propertyIds));
		}
		public Dictionary<string, Dictionary<PropertyId, object>> GetItemPropertyes(IEnumerable<string> tags, PropertyId[] propertyId)
		{
			var tagDictionary = new Dictionary<string, Dictionary<PropertyId, object>>();
			
			var propertyIds = propertyId.Cast<int>().ToArray();
			foreach (var tag in tags)
			{
				var dictionary = new Dictionary<PropertyId, object>();
				var pro =_server.GetItemProperties().GetItemProperties(tag, propertyIds);			
				for (var i = 0; i < pro.Length; i++)
				{
					dictionary[propertyId[i]] = pro[i].Value;
				}
				tagDictionary.Add(tag, dictionary);				
			}
			return tagDictionary;
		}
		public async Task<Dictionary<string, Dictionary<PropertyId, object>>> GetItemPropertyesAsync(IEnumerable<string> tags, PropertyId[] propertyId)
		{
			return await Task.Run(() => GetItemPropertyes(tags, propertyId));
		}

		public async Task<Dictionary<string, ItemPropertyValue[]>> GetItemPropertyesAsync(IEnumerable<string> tags,
			int[] propertyIds)
		{
			return await Task.Run(() => GetItemPropertyes(tags, propertyIds));
		}
		public KeyValuePair<ItemProperty[], ItemPropertyValue[]> GetAvailableItemProperties(string itemId)
		{
			var browser = _server.GetItemProperties();
			var definitions = browser.QueryAvailableProperties(itemId);
			var values = browser.GetItemProperties(itemId, definitions.Select(x => x.Id).ToArray());
			return new KeyValuePair<ItemProperty[], ItemPropertyValue[]>(definitions, values);
		}

		public async Task<KeyValuePair<ItemProperty[], ItemPropertyValue[]>> GetAvailableItemPropertiesAsync(string itemId)
		{
			return await Task.Run(() => GetAvailableItemProperties(itemId));
		}
		public DaGroup CreateDaGroup(GroupProperties properties)
		{
			var server = _server.AddGroup(
				Interlocked.Increment(ref _daGroupClientId),
				properties.Name,
				properties.Active,
				properties.UpdateRate,
				properties.PercentDeadband);

			var daGroupNode = new DaGroup(this, server);
			DaGroupsOnServer.Add(daGroupNode);
			OnCreatedDaGroupAsync?.Invoke(daGroupNode, null);
			return daGroupNode;
		}

		public async Task<DaGroup> CreateDaGroupAsync(GroupProperties properties)
		{
			return await Task.Run(() => CreateDaGroup(properties));
		}

		public ObservableCollection<DaGroup> DaGroupsOnServer { get; private set; }
		public event EventHandler OnCreatedDaGroupAsync = delegate { };

		public ServerAddressSpaceBrowser QueryAddressSpaceBrowser()
		{
			return _server.GetAddressSpaceBrowser();
		}

		public ItemProperties QueryPropertyBrowser()
		{
			return _server.GetItemProperties();
		}

		public override string ToString()
		{
			return Name;
		}

		private DaServer _server;

		private static int _daGroupClientId = 1;


		private void Dispose(bool disposing)
		{
			if (!disposing) return;
			Disconnect();
			_server?.Dispose();
		}

		public void Dispose()
		{
			Dispose(true);
		}
	}
}