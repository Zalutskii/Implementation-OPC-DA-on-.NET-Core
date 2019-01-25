using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using OPCDataAccessLibraries.Enums;

namespace OPCDataAccessLibraries.Interfaces
{
	/// <summary>
	/// This interface is similar to IOPCAsync. This interface is intended to replace IOPCAsyncIO.
	/// </summary>
	[Guid("39c13a71-011e-11d0-9675-0020afd8adb3"), ComImport, InterfaceType(1)]
	internal interface IOPCAsyncIO2
	{
		/// <summary>
		/// Read one or more items in a group. The results are returned via the client’s IOPCDataCallback connection established through the server’s IConnectionPointContainer.
		/// </summary>
		/// <param name="dwCount"></param>
		/// <param name="phServer"></param>
		/// <param name="dwTransactionId"></param>
		/// <param name="pdwCancelId"></param>
		/// <param name="ppErrors"></param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Read(
			[In] uint dwCount,
			[In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] int[] phServer,
			[In] int dwTransactionId,
			[Out] out int pdwCancelId,
			[Out] out IntPtr ppErrors);

		/// <summary>
		/// Write one or more items in a group. The results are returned via the client’s IOPCDataCallback connection established through the server’s IConnectionPointContainer.
		/// </summary>
		/// <param name="dwCount"></param>
		/// <param name="phServer"></param>
		/// <param name="values"></param>
		/// <param name="dwTransactionId"></param>
		/// <param name="pdwCancelId"></param>
		/// <param name="ppErrors"></param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Write(
			[In] uint dwCount,
			[In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] int[] phServer,
			[In] IntPtr values,
			[In] int dwTransactionId,
			[Out] out int pdwCancelId,
			[Out] out IntPtr ppErrors);

		/// <summary>
		/// Force a callback to IOPCDataCallback::OnDataChange for all active items in the group (whether they have changed or not). Inactive items are not included in the callback.
		/// </summary>
		/// <param name="dwSource"></param>
		/// <param name="dwTransactionId"></param>
		/// <param name="pdwCancelId"></param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Refresh2(
			[In] DataSource dwSource,
			[In] int dwTransactionId,
			[Out] out int pdwCancelId);

		/// <summary>
		/// Request that the server cancel an outstanding transaction.
		/// </summary>
		/// <param name="pdwCancelId"></param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Cancel2([In] int pdwCancelId);

		/// <summary>
		/// Controls the operation of OnDataChange. Basically setting Enable to FALSE will disable any OnDataChange callbacks with a transaction ID of 0 (those which are not the result of a Refresh).
		/// </summary>
		/// <param name="bEnable"></param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetEnable([In] int bEnable);

		/// <summary>
		/// Retrieves the last Callback Enable value set with SetEnable.
		/// </summary>
		/// <param name="bEnable"></param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetEnable([Out] out int bEnable);
	}
}
