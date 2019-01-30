using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace OPCDataAccessLibraries.BaseEntity.Interfaces
{
	/// <summary>
	/// The OPC Common interface is used by various OPC Server types (DataAccess, Alarm and Event, Historical Data). It provides the ability to set and query a LocaleID for the particular client/server session.
	/// </summary>
	[Guid("55C382C8-21C7-4E88-96C1-BECFB1E3F483"), ComImport, InterfaceType(1)]
	internal interface IOPCEnumGUID
	{
		/// <summary>
		/// Retrieves the specified number of items in the enumeration sequence.
		/// </summary>
		/// <param name="celt"></param>
		/// <param name="rgelt"></param>
		/// <param name="celtFetched"></param>
		[PreserveSig]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		int Next(
			[In] uint celt,
			[Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] Guid[] rgelt,
			[Out] out uint celtFetched);

		/// <summary>
		/// Skips over the specified number of items in the enumeration sequence.
		/// </summary>
		/// <param name="celt"></param>
		[PreserveSig]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		int Skip([In] uint celt);

		/// <summary>
		/// Resets the enumeration sequence to the beginning.
		/// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Reset();

		/// <summary>
		/// Creates a new enumerator that contains the same enumeration state as the current one.
		/// </summary>
		/// <param name="enum"></param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Clone([Out, MarshalAs(UnmanagedType.Interface)] out IOPCEnumGUID @enum);
	}

}