using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using OPCDataAccessLibraries.Enums;

namespace OPCDataAccessLibraries.Interfaces
{
	/// <summary>
	/// IOPCSyncIO allows a client to perform synchronous read and write operations to a server. The operations will run to completion.
	/// </summary>
	[Guid("39C13A52-011E-11D0-9675-0020AFD8ADB3"), ComImport, InterfaceType(1)]
	internal interface IOPCSyncIO
	{
		/// <summary>
		/// This function reads the value, quality and timestamp information for one or more items in a group. The function runs to completion before returning. The data can be read from CACHE in which case it should be accurate to within the ‘UpdateRate’ and percent deadband of the group. The data can be read from the DEVICE in which case an actual read of the physical device is to be performed. The exact implementation of CACHE and DEVICE reads is not defined by this specification.
		/// </summary>
		/// <param name="dwSource"></param>
		/// <param name="dwCount"></param>
		/// <param name="phServer"></param>
		/// <param name="ppItemValues"></param>
		/// <param name="ppErrors"></param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Read(
			[In] DataSource dwSource,
			[In] uint dwCount,
			[In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] int[] phServer,
			[Out] out IntPtr ppItemValues,
			[Out] out IntPtr ppErrors);

		/// <summary>
		/// Writes values to one or more items in a group. The function runs to completion. The values are written to the DEVICE. That is, the function should not return until it verifies that the device has actually accepted (or rejected) the data.
		/// </summary>
		/// <param name="dwCount"></param>
		/// <param name="phServer"></param>
		/// <param name="ppItemValues"></param>
		/// <param name="ppErrors"></param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Write(
			[In] uint dwCount,
			[In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] int[] phServer,
			[In] IntPtr ppItemValues,
			[Out] out IntPtr ppErrors);
	}
}
