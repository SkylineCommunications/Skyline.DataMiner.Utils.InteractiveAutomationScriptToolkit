namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;

	[Serializable]
	public class TreeViewDuplicateItemsException : Exception
	{
		public TreeViewDuplicateItemsException()
		{

		}

		public TreeViewDuplicateItemsException(string key) : base(String.Format("An item with key {0} is already present in the TreeView", key))
		{

		}

		public TreeViewDuplicateItemsException(string key, Exception inner) : base(String.Format("An item with key {0} is already present in the TreeView", key), inner)
		{

		}

		protected TreeViewDuplicateItemsException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
