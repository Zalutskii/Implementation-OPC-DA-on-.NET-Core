using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace OPCDataAccessLibraries.Interfaces
{
	/// <summary>
	/// Enables clients to enumerate through a collection of class IDs for COM classes.
	/// </summary>
	[Guid("0002E000-0000-0000-C000-000000000046"), ComImport, InterfaceType(1)]
	internal interface IEnumGUID
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
		void Clone([Out, MarshalAs(UnmanagedType.Interface)] out IEnumGUID @enum);
	}
}
