using System;
using System.Runtime.Serialization;

namespace OPCDataAccessLibraries
{
	/// <inheritdoc />
	/// <summary>
	/// Exception for OPC Toolkit library.
	/// </summary>
	[Serializable]
	public class OpcException : Exception
	{
		/// <inheritdoc />
		/// <summary>
		/// Default constructor.
		/// </summary>
		public OpcException()
		{
		}

		/// <inheritdoc />
		/// <summary>
		/// Constructor based on error description.
		/// </summary>
		/// <param name="message">Error description.</param>
		public OpcException(string message) :
			base(message)
		{
		}

		/// <inheritdoc />
		/// <summary>
		/// Constructor based on error description and other exception.
		/// </summary>
		/// <param name="message">Error description.</param>
		/// <param name="innerException">Error that happened earlier.</param>
		public OpcException(string message, Exception innerException) :
			base(message, innerException)
		{
		}

		private OpcException(SerializationInfo info, StreamingContext context) :
			base(info, context)
		{
		}
	}

}