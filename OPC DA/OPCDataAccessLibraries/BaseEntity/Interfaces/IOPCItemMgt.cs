using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace OPCDataAccessLibraries.BaseEntity.Interfaces
{
	/// <summary>
	/// IOPCItemMgt allows a client to add, remove and control the behavior of items is a group.
	/// </summary>
	[Guid("39C13A54-011E-11D0-9675-0020AFD8ADB3"), ComImport, InterfaceType(1)]
	internal interface IOPCItemMgt
	{
		/// <summary>
		/// Add one or more items to a group.
		/// </summary>
		/// <param name="dwCount"></param>
		/// <param name="pItemArray"></param>
		/// <param name="ppAddResults"></param>
		/// <param name="ppErrors"></param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void AddItems(
			[In] uint dwCount,
			[In] IntPtr pItemArray,
			[Out] out IntPtr ppAddResults,
			[Out] out IntPtr ppErrors);

		/// <summary>
		/// Determines if an item is valid (could it be added without error). Also returns information about the item such as canonical datatype. Does not affect the group in any way.
		/// </summary>
		/// <param name="dwCount"></param>
		/// <param name="pItemArray"></param>
		/// <param name="bBlobUpdate"></param>
		/// <param name="ppValidationResults"></param>
		/// <param name="ppErrors"></param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void ValidateItems(
			[In] uint dwCount,
			[In] IntPtr pItemArray,
			[In] int bBlobUpdate,
			[Out] out IntPtr ppValidationResults,
			[Out] out IntPtr ppErrors);

		/// <summary>
		/// Removes (deletes) items from a group. Basically this is the reverse of AddItems.
		/// </summary>
		/// <param name="dwCount"></param>
		/// <param name="phServer"></param>
		/// <param name="ppErrors"></param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void RemoveItems(
			[In] uint dwCount,
			[In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] int[] phServer,
			[Out] out IntPtr ppErrors);

		/// <summary>
		/// Sets one or more items in a group to active or inactive. This controls whether or not valid data can be obtained from Read CACHE for those items and whether or not they are included in the IAdvise subscription to the group.
		/// </summary>
		/// <param name="dwCount"></param>
		/// <param name="phServer"></param>
		/// <param name="bActive"></param>
		/// <param name="ppErrors"></param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetActiveState(
			[In] uint dwCount,
			[In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] int[] phServer,
			[In] int bActive,
			[Out] out IntPtr ppErrors);

		/// <summary>
		/// Changes the client handle for one or more items in a group.
		/// </summary>
		/// <param name="dwCount"></param>
		/// <param name="phServer"></param>
		/// <param name="phClient"></param>
		/// <param name="ppErrors"></param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetClientHandles(
			[In] uint dwCount,
			[In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] int[] phServer,
			[In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] int[] phClient,
			[Out] out IntPtr ppErrors);

		/// <summary>
		/// Changes the requested data type for one or more items in a group.
		/// </summary>
		/// <param name="dwCount"></param>
		/// <param name="phServer"></param>
		/// <param name="requestedDatatypes"></param>
		/// <param name="ppErrors"></param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetDatatypes(
			[In] uint dwCount,
			[In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] int[] phServer,
			[In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] short[] requestedDatatypes,
			[Out] out IntPtr ppErrors);

		/// <summary>
		/// Create an enumerator for the items in the group.
		/// </summary>
		/// <param name="riid"></param>
		/// <param name="ppUnk"></param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void CreateEnumerator(
			[In] ref Guid riid,
			[MarshalAs(UnmanagedType.IUnknown)] out object ppUnk);
	}
}
