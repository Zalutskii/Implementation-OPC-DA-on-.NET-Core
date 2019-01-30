using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace OPCDataAccessLibraries.BaseEntity.Interfaces
{
	/// <summary>
	/// IOPCGroupStateMgt allows the client to manage the overall state of the group. Primarily this allows changes to the update rate and active state of the group.
	/// </summary>
	[Guid("39C13A50-011E-11D0-9675-0020AFD8ADB3"), ComImport, InterfaceType(1)]
	internal interface IOPCGroupStateMgt
	{
		/// <summary>
		/// Get the current state of the group.
		/// </summary>
		/// <param name="pUpdateRate"></param>
		/// <param name="pActive"></param>
		/// <param name="ppName"></param>
		/// <param name="pTimeBias"></param>
		/// <param name="pPercentDeadband"></param>
		/// <param name="pLcid"></param>
		/// <param name="clientId"></param>
		/// <param name="phServerGroup"></param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetState(
			[Out] out int pUpdateRate,
			[Out] out int pActive,
			[Out, MarshalAs(UnmanagedType.LPWStr)] out string ppName,
			[Out] out int pTimeBias,
			[Out] out float pPercentDeadband,
			[Out] out int pLcid,
			[Out] out int clientId,
			[Out] out int phServerGroup);

		/// <summary>
		/// Client can set various properties of the group. Pointers to ‘in’ items are used so that the client can omit properties he does not want to change by passing a NULL pointer.
		/// </summary>
		/// <param name="requestedUpdateRate"></param>
		/// <param name="revisedUpdateRate"></param>
		/// <param name="pActive"></param>
		/// <param name="pTimeBias"></param>
		/// <param name="pPercentDeadband"></param>
		/// <param name="pLcid"></param>
		/// <param name="phClientGroup"></param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetState(
			[In] ref int requestedUpdateRate,
			[Out] out int revisedUpdateRate,
			[In] ref int pActive,
			[In] ref int pTimeBias,
			[In] ref float pPercentDeadband,
			[In] ref int pLcid,
			[In] ref int phClientGroup);

		/// <summary>
		/// Change the name of a private group. The name must be unique. The name cannot be changed for public groups.
		/// </summary>
		/// <param name="name"></param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetName([In, MarshalAs(UnmanagedType.LPWStr)] string name);

		/// <summary>
		/// Creates a second copy of a group with a unique name. This works for both public and private groups. However, the new group is always a private group. All of the group and item properties are duplicated (as if the same set of AddItems calls had been made for the new group). That is, the new group contains the same update rate, items, group and item clienthandles, requested data types, etc as the original group. Once the new group is created it is entirely independent of the old group. You can add and delete items from it without affecting the old group.
		/// </summary>
		/// <param name="szName"></param>
		/// <param name="riid"></param>
		/// <param name="ppUnk"></param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void CloneGroup(
			[In, MarshalAs(UnmanagedType.LPWStr)] string szName,
			[In] ref Guid riid,
			[MarshalAs(UnmanagedType.IUnknown)] out object ppUnk);
	}
}
