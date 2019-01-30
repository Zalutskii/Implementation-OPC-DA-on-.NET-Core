using System;
using System.Security.Permissions;
using OPCDataAccessLibraries.BaseEntity.Interfaces;
using OPCDataAccessLibraries.BaseEntity.Structures;

namespace OPCDataAccessLibraries.BaseEntity
{
	/// <summary>
	/// Access to OPC DA group item properties.
	/// </summary>
	public class ItemProperties
	{
		internal ItemProperties(IOPCServer server)
		{
			try
			{
				_itemProperties = (IOPCItemProperties)server;
			}
			catch (InvalidCastException)
			{
				throw new NotSupportedException();
			}
		}

		/// <summary>
		/// Retrieves list of item property descriptions.
		/// </summary>
		/// <param name="itemId">Name of item.</param>
		/// <returns>List of item property descriptions.</returns>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public ItemProperty[] QueryAvailableProperties(string itemId)
		{
			if (itemId == null)
				throw new ArgumentNullException(nameof(itemId));

			_itemProperties.QueryAvailableProperties(
				itemId, out var size, out var idsPtr, out var descriptionsPtr, out var typesPtr);

			return ItemPropertyResultReader.ReadItemProperties(
				size, idsPtr, descriptionsPtr, typesPtr);
		}

		/// <summary>
		/// Retrieves list of item properties.
		/// </summary>
		/// <param name="itemId">Name of item.</param>
		/// <param name="propertyIds">ID of properties.</param>
		/// <returns>List of item properties.</returns>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public ItemPropertyValue[] GetItemProperties(string itemId, int[] propertyIds)
		{
			if (itemId == null)
				throw new ArgumentNullException(nameof(itemId));
			if (propertyIds == null)
				throw new ArgumentNullException(nameof(propertyIds));

			if (propertyIds.Length == 0)
				return new ItemPropertyValue[0];

			_itemProperties.GetItemProperties(
				itemId, (uint)propertyIds.Length, propertyIds, out var dataPtr, out var errorsPtr);

			return ItemPropertyResultReader.ReadItemPropertyValues(
				propertyIds.Length, dataPtr, errorsPtr);
		}

		/// <summary>
		/// Retrieves item property names.
		/// </summary>
		/// <param name="itemId">Name of item.</param>
		/// <param name="propertyIds">ID of properties.</param>
		/// <returns></returns>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public ItemPropertyId[] LookupItemIds(string itemId, int[] propertyIds)
		{
			if (itemId == null)
				throw new ArgumentNullException(nameof(itemId));
			if (propertyIds == null)
				throw new ArgumentNullException(nameof(propertyIds));
			if (propertyIds.Length == 0)
				return new ItemPropertyId[0];

			_itemProperties.LookupItemIDs(
				itemId, (uint)propertyIds.Length, propertyIds, out var dataPtr, out var errorsPtr);

			return ItemPropertyResultReader.ReadItemPropertyIds(
				propertyIds.Length, dataPtr, errorsPtr);
		}

		private readonly IOPCItemProperties _itemProperties;
	}
}
