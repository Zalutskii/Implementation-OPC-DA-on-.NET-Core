﻿using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using OPCDataAccessLibraries.Structures;

namespace OPCDataAccessLibraries
{
	internal class ItemValueReader
	{
		public static ItemValue[] Read(int[] clientIds, IntPtr values, short[] qualities, long[] timeStamps, int[] errors)
		{
			var result = new ItemValue[qualities.Length];

			var position = 0;
			var valuesAsLong = values.ToInt64();
			for (var i = 0; i < clientIds.Length; i++)
			{
				var variant = new IntPtr(valuesAsLong + position);
				position += NativeMethods.VariantSize;

				result[i] = new ItemValue
				{
					ClientId = clientIds[i],
					Timestamp = DateTime.FromFileTimeUtc(timeStamps[i]),
					Quality = qualities[i],
					Value = Marshal.GetObjectForNativeVariant(variant),
					Error = errors[i],
				};
			}

			return result;
		}

		[SecurityPermission(SecurityAction.LinkDemand)]
		public static ItemValue[] Read(int size, IntPtr dataPtr, IntPtr errorsPtr)
		{
			try
			{
				var position = 0;
				var dataPtrAsLong = dataPtr.ToInt64();
				var result = new ItemValue[size];
				for (var i = 0; i < size; i++)
				{
					// ReSharper disable UseObjectOrCollectionInitializer
					result[i] = new ItemValue();
					// ReSharper restore UseObjectOrCollectionInitializer

					// uint hClient;
					result[i].ClientId = Marshal.ReadInt32(dataPtr, position);
					position += sizeof(int);

					// FILETIME ftTimeStamp;
					var time = Marshal.ReadInt64(dataPtr, position);
					result[i].Timestamp = DateTime.FromFileTimeUtc(time);
					position += sizeof(long);

					// ushort wQuality;
					result[i].Quality = Marshal.ReadInt16(dataPtr, position);
					position += sizeof(short);

					// ushort wReserved;
					position += sizeof(short);

					// VARIANT vDataValue;
					var variant = new IntPtr(dataPtrAsLong + position);
					if (variant != IntPtr.Zero)
					{
						result[i].Value = Marshal.GetObjectForNativeVariant(variant);
						NativeMethods.VariantClear(variant);
					}
					position += NativeMethods.VariantSize;

					result[i].Error = Marshal.ReadInt32(errorsPtr, i * sizeof(int));
				}

				return result;
			}
			finally
			{
				Marshal.FreeCoTaskMem(dataPtr);
				Marshal.FreeCoTaskMem(errorsPtr);
			}
		}
	}
}
