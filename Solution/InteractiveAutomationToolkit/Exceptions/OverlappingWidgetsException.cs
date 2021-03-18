namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	[Serializable]
	public class OverlappingWidgetsException : Exception
	{
		public OverlappingWidgetsException()
		{
		}

		public OverlappingWidgetsException(string message) : base(message)
		{
		}

		public OverlappingWidgetsException(string message, Exception inner) : base(message, inner)
		{
		}

		protected OverlappingWidgetsException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
