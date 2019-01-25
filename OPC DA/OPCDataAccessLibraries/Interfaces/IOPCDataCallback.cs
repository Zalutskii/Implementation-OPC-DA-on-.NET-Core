using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace OPCDataAccessLibraries.Interfaces
{
    /// <summary>
    /// In order to use connection points, the client must create an object that supports both the IUnknown and IOPCDataCallback Interface. The client would pass a pointer to the IUnknown interface (NOT the IOPCDataCallback) to the Advise method of the proper IConnectionPoint in the server (as obtained from IConnectionPointContainer:: FindConnectionPoint or EnumConnectionPoints). The Server will call QueryInterface on the client object to obtain the IOPCDataCallback interface. Note that the transaction must be performed in this way in order for the interface marshalling to work properly for Local or Remote servers.
    /// </summary>
    [Guid("39c13a70-011e-11d0-9675-0020afd8adb3"), ComImport, InterfaceType(1)]
    internal interface IOPCDataCallback
    {
        /// <summary>
        /// This method is provided by the client to handle notifications from the OPC Group for exception based data changes and Refreshes.
        /// </summary>
        /// <param name="dwTransid"></param>
        /// <param name="pvValues"></param>
        /// <param name="qualities"></param>
        /// <param name="hGroup"></param>
        /// <param name="hrMasterquality"></param>
        /// <param name="hrMastererror"></param>
        /// <param name="dwCount"></param>
        /// <param name="phClientItems"></param>
        /// <param name="pftTimeStamps"></param>
        /// <param name="pErrors"></param>
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void OnDataChange(
            [In] int dwTransid,
            [In] int hGroup,
            [In] int hrMasterquality,
            [In] int hrMastererror,
            [In] uint dwCount,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)]
            int[] phClientItems,
            [In] IntPtr pvValues,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)]
            short[] qualities,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)]
            long[] pftTimeStamps,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)]
            int[] pErrors);

        /// <summary>
        /// This method is provided by the client to handle notifications from the OPC Group on completion of Async Reads.
        /// </summary>
        /// <param name="hrMasterquality"></param>
        /// <param name="hrMastererror"></param>
        /// <param name="dwTransid"></param>
        /// <param name="hGroup"></param>
        /// <param name="dwCount"></param>
        /// <param name="phClientItems"></param>
        /// <param name="pvValues"></param>
        /// <param name="pwQualities"></param>
        /// <param name="pftTimeStamps"></param>
        /// <param name="pErrors"></param>
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void OnReadComplete(
            [In] int dwTransid,
            [In] int hGroup,
            [In] int hrMasterquality,
            [In] int hrMastererror,
            [In] uint dwCount,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)]
            int[] phClientItems,
            [In] IntPtr pvValues,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)]
            short[] pwQualities,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)]
            long[] pftTimeStamps,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)]
            int[] pErrors);

        /// <summary>
        /// This method is provided by the client to handle notifications from the OPC Group on completion of AsyncIO2 Writes.
        /// </summary>
        /// <param name="hGroup"></param>
        /// <param name="hrMastererror"></param>
        /// <param name="dwTransid"></param>
        /// <param name="dwCount"></param>
        /// <param name="phClientItems"></param>
        /// <param name="pErrors"></param>
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void OnWriteComplete(
            [In] int dwTransid,
            [In] int hGroup,
            [In] int hrMastererror,
            [In] uint dwCount,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)]
            int[] phClientItems,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)]
            int[] pErrors);

        /// <summary>
        /// This method is provided by the client to handle notifications from the OPC Group on completion of Async Cancel.
        /// </summary>
        /// <param name="dwTransid"></param>
        /// <param name="hGroup"></param>
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void OnCancelComplete(
            [In] int dwTransid,
            [In] int hGroup);
    }
}