using System;
using System.Collections.Generic;
using OPCDataAccessLibraries.BaseEntity;

namespace OPCDataAccessLibraries
{
	public class ServerHost
    {
        public List<Server> ServersOnLocalHost { get; }
		public ServerHost() : this(string.Empty) { }

		public ServerHost(string ipAddress)
        {
            ServersOnLocalHost = new List<Server>();
             using (var @enum = new ServerBrowser(ipAddress))
            {
                var serverDescriptions = @enum.GetEnumerator(DaServer.VersionOpc10, DaServer.VersionPoc20,
                    DaServer.VersionOpc30, DaServer.VersionXml10);
                               
                foreach (var d in serverDescriptions)
                    ServersOnLocalHost.Add(new Server(d));
            }
        }
        public void Dispose()
        {
            foreach (var serverNode in ServersOnLocalHost)
            {
                serverNode.Dispose();
            }

            GC.SuppressFinalize(this);
        }
    }

}