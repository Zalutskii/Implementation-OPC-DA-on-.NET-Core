using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using OPCDataAccessLibraries.Enums;
using OPCDataAccessLibraries.Interfaces;

namespace OPCDataAccessLibraries
{
    /// <inheritdoc />
    /// <summary>
    /// OPC DA Server client. Helps to connect to the OPC DA server.
    /// </summary>
    public class DaServer : Server
    {
        /// <summary>
        /// Category UUID of OPC DA 1.0.
        /// </summary>
        public static readonly Guid Version10 = new Guid("{63D5F430-CFE4-11d1-B2C8-0060083BA1FB}");

        /// <summary>
        /// Category UUID of OPC DA 2.0.
        /// </summary>
        public static readonly Guid Version20 = new Guid("{63D5F432-CFE4-11d1-B2C8-0060083BA1FB}");

        /// <inheritdoc />
        /// <summary>
        /// Connects to specified OPC DA Server.
        /// </summary>
        /// <param name="id">UUID of OPC DA Server.</param>
        public DaServer(Guid id) : this(id, string.Empty) { }

        /// <inheritdoc />
        /// <summary>
        /// Connects to specified OPC DA Server.
        /// </summary>
        /// <param name="id">Program ID of OPC DA Server.</param>
        [SecurityPermission(SecurityAction.LinkDemand)]
        public DaServer(string id) : this(id, string.Empty) { }
        /// <inheritdoc />
        /// <summary>
        /// Connects to specified remote OPC DA Server.
        /// </summary>
        /// <param name="id">UUID of OPC DA Server.</param>
        /// <param name="host">Host name or IP address of computer with OPC DA Server.</param>
        public DaServer(Guid id, string host) : base(id, host)
        {
            _server = (IOPCServer)Common;
            ServerProperties = new ServerProperties();
        }

        /// <inheritdoc />
        /// <summary>
        /// Connects to specified remote OPC DA Server.
        /// </summary>
        /// <param name="id">Program ID of OPC DA Server.</param>
        /// <param name="host">Host name or IP address of computer with OPC DA Server.</param>
        [SecurityPermission(SecurityAction.LinkDemand)]
        public DaServer(string id, string host) : base(id, host)
        {
            _server = (IOPCServer)Common;
            ServerProperties = new ServerProperties();
        }

        /// <summary>
        /// Returns browser of OPC DA Server namespace.
        /// </summary>
        /// <param name="blockSize">Number of item names to read in one call.</param>
        /// <returns>Browser of OPC DA Server namespace.</returns>
        public ServerAddressSpaceBrowser GetAddressSpaceBrowser(int blockSize = 1000)
        {
            DisposedCheck();

            return new ServerAddressSpaceBrowser(_server, blockSize);
        }

        /// <summary>
        /// Returns an object that helps to read the information about items.
        /// </summary>
        /// <returns>Object that helps to read the information about items.</returns>
        public ItemProperties GetItemProperties()
        {
            DisposedCheck();

            return new ItemProperties(_server);
        }

        /// <summary>
        /// Creates new OPC DA group.
        /// </summary>
        /// <param name="clientId">Group client ID.</param>
        /// <param name="name">Group name.</param>
        /// <param name="active">Initial state.</param>
        /// <param name="updateRate">Item values update rate in milliseconds.</param>
        /// <param name="percentDeadband">Item values deadband in percents.</param>
        /// <returns>New OPC DA group.</returns>
        /// <remarks>Uses current timezone.</remarks>
        public Group AddGroup(int clientId, string name, bool active, int updateRate, float percentDeadband)
        {
            var timeBias = (int)TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).TotalMinutes;

            return AddGroup(clientId, name, active, updateRate, percentDeadband, timeBias);
        }

        /// <summary>
        /// Creates new OPC DA group.
        /// </summary>
        /// <param name="clientId">Group client ID.</param>
        /// <param name="name">Group name.</param>
        /// <param name="active">Initial state.</param>
        /// <param name="updateRate">Item values update rate in milliseconds.</param>
        /// <param name="percentDeadband">Item values deadband in percents.</param>
        /// <param name="timeBias">Timezone time bias.</param>
        /// <returns>New OPC DA group.</returns>
        public Group AddGroup(int clientId, string name, bool active, int updateRate, float percentDeadband, int timeBias)
        {
            DisposedCheck();
            name.ArgumentNotNullOrEmpty("name");

            var id = typeof(IOPCItemMgt).GUID;

            _server.AddGroup(
                name,
                active ? 1 : 0,
                updateRate,
                clientId,
                ref timeBias,
                ref percentDeadband,
                (uint)CultureInfo.CurrentUICulture.LCID,
                out var serverId,
                out updateRate,
                ref id,
                out var group);

            return new Group(this, clientId, serverId, name, updateRate, (IOPCItemMgt)group);
        }

        /// <inheritdoc />
        /// <summary>
        /// Converts error code to localized message based on current OPC Server locale.
        /// </summary>
        /// <param name="code">Error code.</param>
        /// <returns>Message for specified error code.</returns>
        public override string GetErrorString(int code)
        {
            DisposedCheck();

            _server.GetErrorString(code, (uint)CultureInfo.CurrentUICulture.LCID, out var result);
            return result;
        }

        public ServerProperties ServerProperties { get; }
        /// <summary>
        /// Retrieves OPC DA Server properties.
        /// </summary>
        /// <returns>OPC DA Server properties.</returns>
        [SecurityPermission(SecurityAction.LinkDemand)]
        public ServerProperties GetProperties()
        {
            DisposedCheck();

            _server.GetStatus(out var dataPtr);
            try
            {
                var position = 0;

                // FILETIME ftStartTime;
                var time = Marshal.ReadInt64(dataPtr, position);
                ServerProperties.StartTime = DateTime.FromFileTimeUtc(time);
                position += sizeof(long);

                // FILETIME ftCurrentTime;
                time = Marshal.ReadInt64(dataPtr, position);
                ServerProperties.CurrentTime = DateTime.FromFileTimeUtc(time);
                position += sizeof(long);

                // FILETIME ftLastUpdateTime;
                time = Marshal.ReadInt64(dataPtr, position);
                ServerProperties.LastUpdateTime = DateTime.FromFileTimeUtc(time);
                position += sizeof(long);

                // tagOPCSERVERSTATE dwServerState;
                ServerProperties.ServerState = (ServerState)Marshal.ReadInt32(dataPtr, position);
                position += sizeof(int);

                // uint dwGroupCount;
                ServerProperties.GroupCount = Marshal.ReadInt32(dataPtr, position);
                position += sizeof(int);

                // uint dwBandWidth;
                ServerProperties.Bandwidth = Marshal.ReadInt32(dataPtr, position);
                position += sizeof(int);

                // ushort wMajorVersion;
                ServerProperties.MajorVersion = Marshal.ReadInt16(dataPtr, position);
                position += sizeof(short);

                // ushort wMinorVersion;
                ServerProperties.MinorVersion = Marshal.ReadInt16(dataPtr, position);
                position += sizeof(short);

                // ushort wBuildNumber;
                ServerProperties.BuildNumber = Marshal.ReadInt16(dataPtr, position);
                position += sizeof(short);

                // ushort wReserved;
                position += sizeof(short);

                if (IntPtr.Size == 8)
                    position += sizeof(int);

                // string szVendorInfo;
                var vendorInfo = Marshal.ReadIntPtr(dataPtr, position);
                if (vendorInfo != IntPtr.Zero)
                {
                    ServerProperties.VendorInfo = Marshal.PtrToStringUni(vendorInfo);
                    Marshal.FreeCoTaskMem(vendorInfo);
                }
                ServerProperties.Refreshed();
                return ServerProperties;
            }
            finally
            {
                if (dataPtr != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(dataPtr);
            }
        }

        internal void RemoveGroup(int groupId)
        {
            if (Common == null)
                return;

            _server.RemoveGroup(groupId, 0);
        }

        private readonly IOPCServer _server;
    }
}