namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;

	/// <summary>
	/// This exception is used to indicate that a tree view contains multiple items with the same key.
	/// </summary>
	[Serializable]
	public class TreeViewDuplicateItemsException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TreeViewDuplicateItemsException"/> class.
		/// </summary>
		public TreeViewDuplicateItemsException()
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TreeViewDuplicateItemsException"/> class with a specified error message.
		/// </summary>
		/// <param name="key">The key of the duplicate tree view items.</param>
		public TreeViewDuplicateItemsException(string key) : base(String.Format("An item with key {0} is already present in the TreeView", key))
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TreeViewDuplicateItemsException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="key">The key of the duplicate tree view items.</param>
		/// <param name="inner">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
		public TreeViewDuplicateItemsException(string key, Exception inner) : base(String.Format("An item with key {0} is already present in the TreeView", key), inner)
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TreeViewDuplicateItemsException"/> class with the serialized data.
		/// </summary>
		/// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual information about the source or destination.</param>
		protected TreeViewDuplicateItemsException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
