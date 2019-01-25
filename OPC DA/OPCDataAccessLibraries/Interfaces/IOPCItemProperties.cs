using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace OPCDataAccessLibraries.Interfaces
{
	/// <summary>
	/// This interface can be used by clients to browse the available properties (also refered to as attributes or parameters) associated with an ITEMID and to read the current values of these properties.
	/// </summary>
	[Guid("39C13A72-011E-11D0-9675-0020AFD8ADB3"), ComImport, InterfaceType(1)]
	internal interface IOPCItemProperties
	{
		/// <summary>
		/// Return a list of ID codes and descriptions for the available properties for this ITEMID. This list may differ for different ItemIDs. This list is expected to be relatively stable for a particular ItemID. That is, it could be affected from time to time by changes to the underlying system’s configuration.
		/// </summary>
		/// <param name="zItemId">The ItemID for which the caller wants to know the available properties</param>
		/// <param name="pdwCount">The number of properties returned</param>
		/// <param name="ppPropertyIDs">DWORD IDs for the returned properties. These IDs can be passed to GetItemProperties or LookupItemIDs</param>
		/// <param name="ppDescriptions">A brief vendor supplied text description of each property. NOTE LocaleID does not apply to Descriptions. They are from the tables above.</param>
		/// <param name="ppvtDataTypes">The datatype which will be returned for this property by GetItemProperties.</param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void QueryAvailableProperties(
			[In, MarshalAs(UnmanagedType.LPWStr)] string zItemId,
			[Out] out uint pdwCount,
			[Out] out IntPtr ppPropertyIDs,
			[Out] out IntPtr ppDescriptions,
			[Out] out IntPtr ppvtDataTypes);

		/// <summary>
		/// Return a list of the current data values for the passed ID codes.
		/// </summary>
		/// <param name="szItemId">The ItemID for which the caller wants to read the list of properties.</param>
		/// <param name="dwCount">The number of properties passed</param>
		/// <param name="ppPropertyIDs">DWORD IDs for the requested properties. These IDs were returned by QueryAvailableProperties or obtained from the fixed list described earlier.</param>
		/// <param name="ppvData">An array of count VARIANTS returned by the server which contain the current values of the requested properties.</param>
		/// <param name="ppErrors">Error array indicating wether each property was returned.</param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetItemProperties(
			[In, MarshalAs(UnmanagedType.LPWStr)] string szItemId,
			[In] uint dwCount,
			[In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] int[] ppPropertyIDs,
			[Out] out IntPtr ppvData,
			[Out] out IntPtr ppErrors);

		/// <summary>
		/// Return a list of ITEMIDs (if available) for each of the passed ID codes. These indicate the ITEMID which could be added to an OPCGroup and used for more efficient access to the data corresponding to the Item Properties.
		/// </summary>
		/// <param name="szItemId">The ItemID for which the caller wants to lookup the list of properties</param>
		/// <param name="dwCount">The number of properties passed</param>
		/// <param name="pdwPropertyIDs">DWORDIDs for the requested properties. These IDs were returned by QueryAvailableProperties</param>
		/// <param name="ppszNewItemIDs">The returned list of ItemIDs.</param>
		/// <param name="ppErrors">Error array indicating wether each New ItemID was returned.</param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void LookupItemIDs(
			[In, MarshalAs(UnmanagedType.LPWStr)] string szItemId,
			[In] uint dwCount,
			[In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] int[] pdwPropertyIDs,
			[Out] out IntPtr ppszNewItemIDs,
			[Out] out IntPtr ppErrors);
	}
}
