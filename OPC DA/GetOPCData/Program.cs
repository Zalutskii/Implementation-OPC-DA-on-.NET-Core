using System;
using System.Linq;
using System.Threading.Tasks;
using OPCDataAccessLibraries;
using OPCDataAccessLibraries.BaseEntity;


namespace GetOPCData
{
	 public static class Program
	{
		public static async Task Main(string[] args)
		{
			var localHostNode = new ServerHost("127.0.0.1");

			var serverNode = localHostNode.ServersOnLocalHost.FirstOrDefault(x => x.ServerName == "AP.OPCDAServer");
			await serverNode.ConnectAsync();


			var daGroup = await serverNode.CreateDaGroupAsync(new GroupProperties()
			{
				Name = Guid.NewGuid().ToString(),
				Active = true,
				UpdateRate = 0,
				PercentDeadband = 0
			});

			var daGroupItemBase1 = daGroup.GetDaGroupItem<DaGroupItemBase>("");
			daGroupItemBase1.OnValueChaged += (s, e) =>
			{
				if(s is DaGroupItemBase dgib)
				Console.WriteLine($@"{dgib.Tag}: {dgib.Value}; ");
			};
			var daGroupItemBase2 = daGroup.GetDaGroupItem<DaGroupItemBase>("");
			
			daGroupItemBase2.OnValueChaged += (s, e) =>
			{
				if(s is DaGroupItemBase dgib)
					Console.WriteLine($@"{dgib.Tag}: {dgib.Value}");
			};


			daGroup.SyncWriteDaGroupItem(daGroupItemBase2, 1);
			daGroup.SyncWriteDaGroupItem(daGroupItemBase2, 2);
			daGroup.SyncWriteDaGroupItem(daGroupItemBase2, 3);
			daGroup.SyncWriteDaGroupItem(daGroupItemBase2, 4);
			daGroup.SyncWriteDaGroupItem(daGroupItemBase2, 5);
			daGroup.SyncWriteDaGroupItem(daGroupItemBase2, 6);
			daGroup.SyncWriteDaGroupItem(daGroupItemBase2, 7);

			Console.ReadKey();
		}
	}
}
