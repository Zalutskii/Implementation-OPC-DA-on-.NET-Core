using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using OPCDataAccessLibraries.BaseEntity.Enums;

namespace OPCDataAccessLibraries.BaseEntity.Interfaces
{
	/// <summary>
	/// This interface provides a way for clients to browse the available data items in the server, giving the user a list of the valid definitions for an ITEM ID. It allows for either flat or hierarchical address spaces and is designed to work well over a network. It also insulates the client from the syntax of a server vendor specific ITEM ID.
	/// </summary>
	[Guid("39C13A4F-011E-11D0-9675-0020AFD8ADB3"), ComImport, InterfaceType(1)]
	internal interface IOPCBrowseServerAddressSpace
	{
		/// <summary>
		/// Provides a way to determine if the underlying system is inherently flat or hierarchical and how the server may represent the information of the address space to the client.
		/// </summary>
		/// <param name="pNameSpaceType">Place to put OPCNAMESPACE result which will be OPC_NS_HIERARCHIAL or OPC_NS_FLAT</param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void QueryOrganization([Out] out NamespaceType pNameSpaceType);

		/// <summary>
		/// Provides a way to move ‘up’ or ‘down’ or 'to' in a hierarchical space.
		/// </summary>
		/// <param name="dwBrowseDirection">OPC_BROWSE_UP or OPC_BROWSE_DOWN or OPC_BROWSE_TO.</param>
		/// <param name="szString">For DOWN, the name of the branch to move into. This would be one of the strings returned from BrowseOPCItemIDs. E.g. REACTOR10 For UP this parameter is ignored and should point to a NUL string. For TO a fully qualified name (e.g. as returned from GetItemID) or a pointer to a NUL string to go to the 'root'. E.g. AREA1.REACTOR10.TIC1001</param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void ChangeBrowsePosition(
			[In] BrowseDirection dwBrowseDirection,
			[In, MarshalAs(UnmanagedType.LPWStr)] string szString);

		/// <summary>
		/// Returns an IENUMString for a list of ItemIDs as determined by the passed parameters. The position from the which the browse is done can be set via ChangeBrowsePosition.
		/// </summary>
		/// <param name="dwBrowseFilterType">OPC_BRANCH - returns only items that have children
		/// OPC_LEAF - returns only items that don’t have children
		/// OPC_FLAT - returns everything at and below this level
		/// including all children of children - basically ‘pretends’ that
		/// the address space in actually FLAT
		/// This parameter is ignored for FLAT address space.</param>
		/// <param name="szFilterCriteria">A server specific filter string. This is entirely free format and may be entered by the user via an EDIT field. Although the valid criteria are vendor specific, source code for a recommended filter function is included in an Apppendix at the end of this document. This particular filter function is commonly used by OPC interfaces and is very similar in functionality to the LIKE function in visual basic. A pointer to a NUL string indicates no filtering.</param>
		/// <param name="vtDataTypeFilter">Filter the returned list based in the available datatypes (those that would succeed if passed to AddItem). VT_EMPTY indicates no filtering.</param>
		/// <param name="dwAccessRightsFilter">Filter based on the AccessRights bit mask (OPC_READABLE or OPC_WRITEABLE). The bits passed in the bitmask are 'ANDed' with the bits that would be returned for this Item by AddItem, ValidateItem or EnumOPCItemAttributes. If the result is non-zero then the item is returned. A 0 value in the bitmask indicates that the AccessRights bits should be ignored during the filtering process..</param>
		/// <param name="ppIEnumString">Where to save the returned interface pointer. NULL if the HRESULT is other than S_OK or S_FALSE</param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void BrowseOPCItemIDs(
			[In] BrowseType dwBrowseFilterType,
			[In, MarshalAs(UnmanagedType.LPWStr)] string szFilterCriteria,
			[In] short vtDataTypeFilter,
			[In] int dwAccessRightsFilter,
			[Out, MarshalAs(UnmanagedType.Interface)] out IEnumString ppIEnumString);

		/// <summary>
		/// Provides a way to assemble a ‘fully qualified’ ITEM ID in a hierarchical space. This is required since the browsing functions return only the components or tokens which make up an ITEMID and do NOT return the delimiters used to separate those tokens. Also, at each point one is browsing just the names ‘below’ the current node (e.g. the ‘units’ in a ‘cell’).
		/// </summary>
		/// <param name="szItemDataId">The name of a BRANCH or LEAF at the current level. or a pointer to a NUL string. Passing in a NUL string results in a return string which represents the current position in the hierarchy.</param>
		/// <param name="szItemId">Where to return the resulting ItemID.</param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetItemID(
			[In, MarshalAs(UnmanagedType.LPWStr)] string szItemDataId,
			[Out, MarshalAs(UnmanagedType.LPWStr)] out string szItemId);

		/// <summary>
		/// Provides a way to browse the available AccessPaths for an ITEM ID.
		/// </summary>
		/// <param name="szItemId">Fully Qualified ItemID</param>
		/// <param name="ppIEnumString">Where to save the returned string enumerator. NULL if the HRESULT is other than S_OK or S_FALSE.</param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]

		void BrowseAccessPaths(
			[In, MarshalAs(UnmanagedType.LPWStr)] string szItemId,
			[Out, MarshalAs(UnmanagedType.Interface)] out IEnumString ppIEnumString);
	}
}
