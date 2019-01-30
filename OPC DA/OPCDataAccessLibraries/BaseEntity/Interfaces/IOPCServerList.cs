using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace OPCDataAccessLibraries.BaseEntity.Interfaces
{
	/// <summary>
	/// 
	/// </summary>
	[Guid("13486D50-4821-11D2-A494-3CB306C10000"), ComImport, InterfaceType(1)]
	internal interface IOPCServerList
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="implemented"></param>
		/// <param name="catidImpl"></param>
		/// <param name="required"></param>
		/// <param name="catidReq"></param>
		/// <param name="enumGuid"></param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void EnumClassesOfCategories(
			[In] uint implemented,
			[In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] Guid[] catidImpl,
			[In] uint required,
			[In] ref Guid catidReq,
			[Out, MarshalAs(UnmanagedType.Interface)] out IEnumGUID enumGuid);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="clsid"></param>
		/// <param name="progId"></param>
		/// <param name="userType"></param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetClassDetails(
			[In] ref Guid clsid,
			[Out, MarshalAs(UnmanagedType.LPWStr)] out string progId,
			[Out, MarshalAs(UnmanagedType.LPWStr)] out string userType);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="progId"></param>
		/// <param name="clsid"></param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void CLSIDFromProgID(
			[In, MarshalAs(UnmanagedType.LPWStr)] string progId,
			[Out] out Guid clsid);
	}

}