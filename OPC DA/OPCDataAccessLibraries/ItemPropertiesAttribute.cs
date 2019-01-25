using System;

namespace OPCDataAccessLibraries
{
	[AttributeUsage(AttributeTargets.All)]
	public class ItemPropertiesAttribute : Attribute
	{
		public ItemPropertiesAttribute(int namber, string name, string discription)
		{
			Namber = namber;
			Name = name;
			Discription = discription;
		}
		public int Namber { get; }
		public string Name { get; }
		public string Discription { get; }
	}
}
