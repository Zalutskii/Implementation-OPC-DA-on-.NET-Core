using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Core.Interfaces
{
	[Guid("55C382C8-21C7-4E88-96C1-BECFB1E3F483"), ComImport, InterfaceType(1)]
	internal interface IOPCEnumGUID
	{
		[PreserveSig]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		int Next(
			[In] uint celt,
			[Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] Guid[] rgelt,
			[Out] out uint celtFetched);

		[PreserveSig]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		int Skip([In] uint celt);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Reset();

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Clone([Out, MarshalAs(UnmanagedType.Interface)] out IOPCEnumGUID @enum);
	}

}