using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using OPCDataAccessLibraries.BaseEntity;
using OPCDataAccessLibraries.BaseEntity.Enums;
using OPCDataAccessLibraries.BaseEntity.EventArgs;
using OPCDataAccessLibraries.BaseEntity.Structures;

namespace OPCDataAccessLibraries
{
	public class DaGroup
	{
	    public DaGroup(Server server, Group group)
        {
            Server = server;
            Group = group;
            ItemsDictionary = new ConcurrentDictionary<int, DaGroupItemBase>();
            Group.DataChange += OnDataChange;
            Group.ReadComplete += OnDataChange;
	    }

        public Group Group { get; }

	    public Server Server { get; }

        public ConcurrentDictionary<int, DaGroupItemBase> ItemsDictionary { get; }

        public GroupProperties GetProperties()
        {
	        return Group.GetProperties();
        }

        public async Task<GroupProperties> GetPropertiesAsync()
        {
			return await Task.Run(GetProperties);
        }

        public void ChangeProperties(GroupProperties properties)
        {
				Group.SetProperties(properties);
        }

        public async void ChangePropertiesAsync(GroupProperties properties)
        {
	        await Task.Run(() => ChangeProperties(properties));
        }

        public string[] GetItemsFlat()
        {
				var browser = Server.QueryAddressSpaceBrowser();

				return browser.GetItemIds(
					BrowseType.Flat, string.Empty, VarEnum.VT_EMPTY, 0).ToArray();
        }
        public async Task<string[]> GetItemsFlatAsync()
        {
	        return await Task.Run(GetItemsFlat);
        }
        
        public KeyValuePair<IList<Item>, IList<ItemResult>> ValidateItems(IEnumerable<string> itemIds)
        {
		        var items = itemIds.Select(x => new Item
		        {
			        Tag = x,
			        RequestedDataType = VarEnum.VT_EMPTY,
			        Active = true,
			        ClientId = Interlocked.Increment(ref _daItemClientId),

		        }).ToArray();

		        var result = Group.ValidateItems(items);

		        return new KeyValuePair<IList<Item>, IList<ItemResult>>(items, result);
        }
        
        public async Task<KeyValuePair<IList<Item>, IList<ItemResult>>> ValidateItemsAsync(IEnumerable<string> itemIds)
        {
			return await Task.Run(() => ValidateItems(itemIds));
        }

		public T GetDaGroupItem<T>(string tag) where T : DaGroupItemBase, new()
		{
			var item = new Item
			{
				Tag = tag,
				RequestedDataType = VarEnum.VT_EMPTY,
				Active = true,
				ClientId = Interlocked.Increment(ref _daItemClientId),				
			};

			var addResult = Group.AddItems(new[] { item }).First();

			var daGroupItem = new T
			{
				Tag = item.Tag,
				ClientId = item.ClientId,
				ServerId = addResult.ServerId,
				CanonicalDataType = addResult.CanonicalDataType,
				CanonicalDataSubType = addResult.CanonicalDataSubType,
				AccessRights = addResult.AccessRights
			};
			ItemsDictionary[daGroupItem.ClientId] = daGroupItem;

			return daGroupItem;
		}

		public void SyncWriteDaGroupItem<T>(T daGroupItem, object value) where T : DaGroupItemBase, new()
		{
			Group.SyncWriteItems(new[] { daGroupItem.ServerId }, new[] { value });
		}

		private async void OnDataChange(object sender, DataChangeEventArgs dataChangeEventArgs)
        {
			await Task.Run(() => {
				foreach (var itemValue in dataChangeEventArgs.Values)
				{
					var item = ItemsDictionary[itemValue.ClientId];
					if (item == null) continue;
					item.Error = itemValue.Error;
					if (itemValue.Error != 0) continue;
					item.Value = itemValue.Value;
					item.Quality = itemValue.Quality;
					item.Timestamp = itemValue.Timestamp;
					item.ValueChaged(dataChangeEventArgs);
				}				
			});
        }

	    private static int _daItemClientId;
	}

}