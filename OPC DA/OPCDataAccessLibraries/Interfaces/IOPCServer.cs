using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using OPCDataAccessLibraries.Enums;

namespace OPCDataAccessLibraries.Interfaces
{
	/// <summary>
	/// This is the main interface to an OPC server. The OPC server is registered with the operating system as specified in the Installation and Registration Chapter of this specification.
	/// This interface must be provided, and all functions implemented as specified.
	/// </summary>
	[Guid("39C13A4D-011E-11D0-9675-0020AFD8ADB3"), ComImport, InterfaceType(1)]
	internal interface IOPCServer
	{
		/// <summary>
		/// Add a Group to a Server.
		/// </summary>
		/// <param name="name">Name of the group. The name must be unique among the other groups created by this client. If no name is provided (szName is pointer to a NUL string) the server will generate a unique name.</param>
		/// <param name="bActive">FALSE if the Group is to be created as inactive. TRUE if the Group is to be created as active.</param>
		/// <param name="dwRequestedUpdateRate">Client Specifies the fastest rate at which data changes may be sent to OnDataChange for items in this group. This also indicates the desired accuracy of Cached Data. This is intended only to control the behavior of the interface. How the server deals with the update rate and how often it actually polls the hardware internally is an implementation detail. Passing 0 indicates the server should use the fastest practical rate. The rate is specified in milliseconds.</param>
		/// <param name="hClientGroup">Client provided handle for this group. [refer to description of data types, parameters, and structures for more information about this parameter]</param>
		/// <param name="pTimeBias">Pointer to Long containing the initial TimeBias (in minutes) for the Group. Pass a NULL Pointer if you wish the group to use the default system TimeBias. See discussion of TimeBias in General Properties Section See Comments below.</param>
		/// <param name="pPercentDeadband">The percent change in an item value that will cause a subscription callback for that value to a client. This parameter only applies to items in the group that have dwEUType of Analog. [See discussion of Percent Deadband in General Properties Section]. A NULL pointer is equivalent to 0.0.</param>
		/// <param name="dwLcid">The language to be used by the server when returning values (including EU enumeration’s) as text for operations on this group. This could also include such things as alarm or status conditions or digital contact states.</param>
		/// <param name="phServerGroup">Place to store the unique server generated handle to the newly created group. The client will use the server provided handle for many of the subsequent functions that the client requests the server to perform on the group.</param>
		/// <param name="pRevisedUpdateRate">The server returns the value it will actually use for the UpdateRate which may differ from the RequestedUpdateRate.
		///  Note that this may also be slower than the rate at which the server is internally obtaining the data and updating the cache.
		///  In general the server should ‘round up’ the requested rate to the next available supported rate. The rate is specified in milliseconds. Server returns HRESULT of OPC_S_UNSUPPORTEDRATE when it returns a value in revisedUpdateRate that is different than RequestedUpdateRate.</param>
		/// <param name="riid">The type of interface desired (e.g. IID_IOPCItemMgt)</param>
		/// <param name="ppUnk">Where to store the returned interface pointer. NULL is returned for any FAILED HRESULT.</param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void AddGroup(
			[In, MarshalAs(UnmanagedType.LPWStr)] string name,
			[In] int bActive,
			[In] int dwRequestedUpdateRate,
			[In] int hClientGroup,
			[In] ref int pTimeBias,
			[In] ref float pPercentDeadband,
			[In] uint dwLcid,
			[Out] out int phServerGroup,
			[Out] out int pRevisedUpdateRate,
			[In] ref Guid riid,
			[Out, MarshalAs(UnmanagedType.IUnknown)] out object ppUnk);

		/// <summary>
		/// Returns the error string for a server specific error code.
		/// </summary>
		/// <param name="dwError">A server specific error code that the client application had returned from an interface function from the server, and for which the client application is requesting the server’s textual representation.</param>
		/// <param name="dwLocale">The locale for the returned string.</param>
		/// <param name="ppString">Pointer to pointer where server supplied result will be saved</param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetErrorString(
			[In, MarshalAs(UnmanagedType.Error)] int dwError,
			[In] uint dwLocale,
			[Out, MarshalAs(UnmanagedType.LPWStr)] out string ppString);

		/// <summary>
		/// Given the name of a private group (created earlier by the same client), return an additional interface pointer.
		/// </summary>
		/// <param name="szName">The name of the group. That is the group must have been created by the caller.</param>
		/// <param name="riid">The type of interface desired for the group (e.g. IOPCItemMgt)</param>
		/// <param name="ppUnk">Pointer to where the group interface pointer should be returned. NULL is returned for any HRESULT other than S_OK.</param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetGroupByName(
			[In, MarshalAs(UnmanagedType.LPWStr)] string szName,
			[In] ref Guid riid,
			[Out, MarshalAs(UnmanagedType.IUnknown)] out object ppUnk);

		/// <summary>
		/// Returns current status information for the server.
		/// </summary>
		/// <param name="ppServerStatus">Pointer to where the OPCSERVERSTATUS structure pointer should be returned. The structure is allocated by the server.</param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetStatus([Out] out IntPtr ppServerStatus);

		/// <summary>
		/// Deletes the Group
		/// </summary>
		/// <param name="hServerGroup">Handle for the group to be removed</param>
		/// <param name="bForce">Forces deletion of the group even if references are outstanding</param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void RemoveGroup([In] int hServerGroup, [In] int bForce);

		/// <summary>
		/// Create various enumerators for the groups provided by the Server.
		/// </summary>
		/// <param name="dwScope">Indicates the class of groups to be enumerated
		/// OPC_ENUM_PRIVATE_CONNECTIONS or
		/// OPC_ENUM_PRIVATE enumerates all of the private groups created by the client
		///	OPC_ENUM_ALL_CONNECTIONS or
		///	OPC_ENUM_ALL enumerates all private groups .</param>
		/// <param name="riid">The interface requested. This must be IID_IEnumUnknown or IID_IEnumString.</param>
		/// <param name="ppUnk">Where to return the interface. NULL is returned for any HRESULT other than S_OK or S_FALSE.</param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void CreateGroupEnumerator(
			[In] EnumScope dwScope,
			[In] ref Guid riid,
			[Out, MarshalAs(UnmanagedType.IUnknown)] out object ppUnk);

	}
}
