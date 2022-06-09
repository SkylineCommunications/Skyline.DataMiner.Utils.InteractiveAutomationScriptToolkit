namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using System.Runtime.Serialization;
	using System.Text;

	/// <summary>
	/// This exception is used to indicate that two widgets have overlapping locations on the same dialog.
	/// </summary>
	[Serializable]
	public class OverlappingWidgetsException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OverlappingWidgetsException" /> class.
		/// </summary>
		public OverlappingWidgetsException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OverlappingWidgetsException" /> class with a specified error message.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public OverlappingWidgetsException(string message) : base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OverlappingWidgetsException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="inner">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
		public OverlappingWidgetsException(string message, Exception inner) : base(message, inner)
		{
		}

		/// <summary>
		/// Initializes a new instance of the OverlappingWidgetException class with the serialized data.
		/// </summary>
		/// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual information about the source or destination.</param>
		protected OverlappingWidgetsException(
			SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}

		internal class Builder
		{
			private readonly StringBuilder stringBuilder = new StringBuilder();

			public int Count { get; private set; }

			public Builder Add(IWidget widget, WidgetLocation location, IWidget otherWidget, WidgetLocation otherLocation)
			{
				Count++;

				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}

				stringBuilder.AppendFormat(
					"{0} (Row {1}, Column {2}, RowSpan {3} ColumnSpan {4}) overlaps with {5} (Row {6}, Column {7}, RowSpan {8} ColumnSpan {9}).",
					widget.GetType().Name,
					location.Row,
					location.Column,
					location.RowSpan,
					location.ColumnSpan,
					otherWidget.GetType().Name,
					otherLocation.Row,
					otherLocation.Column,
					otherLocation.RowSpan,
					otherLocation.ColumnSpan);

				return this;
			}

			public OverlappingWidgetsException Build()
			{
				return new OverlappingWidgetsException(stringBuilder.ToString());
			}
		}
	}
}