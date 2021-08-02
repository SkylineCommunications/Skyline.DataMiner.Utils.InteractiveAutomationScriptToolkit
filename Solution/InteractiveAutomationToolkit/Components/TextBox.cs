namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using System.Linq;
	using Skyline.DataMiner.Automation;

	/// <summary>
	///     Widget that is used to edit and display text.
	/// </summary>
	public class TextBox : InteractiveWidget
	{
		private bool changed;
		private string previous;

		/// <summary>
		///     Initializes a new instance of the <see cref="TextBox" /> class.
		/// </summary>
		/// <param name="text">The text displayed in the text box.</param>
		public TextBox(string text)
		{
			Type = UIBlockType.TextBox;
			Text = text;
			PlaceHolder = String.Empty;
			ValidationText = "Invalid Input";
			ValidationState = UIValidationState.NotValidated;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="TextBox" /> class.
		/// </summary>
		public TextBox() : this(String.Empty)
		{
		}

		/// <summary>
		///     Triggered when the text in the text box changes.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<TextBoxChangedEventArgs> Changed
		{
			add
			{
				OnChanged += value;
				WantsOnChange = true;
			}

			remove
			{
				OnChanged -= value;
				if(OnChanged == null || !OnChanged.GetInvocationList().Any())
				{
					WantsOnChange = false;
				}
			}
		}

		private event EventHandler<TextBoxChangedEventArgs> OnChanged;

		/// <summary>
		///     Gets or sets a value indicating whether users are able to enter multiple lines of text.
		/// </summary>
		public bool IsMultiline
		{
			get
			{
				return BlockDefinition.IsMultiline;
			}

			set
			{
				BlockDefinition.IsMultiline = value;
			}
		}

		/// <summary>
		///     Gets or sets the text displayed in the text box.
		/// </summary>
		public string Text
		{
			get
			{
				return BlockDefinition.InitialValue;
			}

			set
			{
				BlockDefinition.InitialValue = value;
			}
		}

		/// <summary>
		///     Gets or sets the tooltip.
		/// </summary>
		/// <exception cref="ArgumentNullException">When the value is <c>null</c>.</exception>
		public string Tooltip
		{
			get
			{
				return BlockDefinition.TooltipText;
			}

			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}

				BlockDefinition.TooltipText = value;
			}
		}

		/// <summary>
		///		Gets or sets the text that should be displayed as a placeholder.
		/// </summary>
		/// <remarks>Available from DataMiner Feature Release 10.0.5 and Main Release 10.1.0 onwards.</remarks>
		public string PlaceHolder
		{
			get
			{
				return BlockDefinition.PlaceholderText;
			}

			set
			{
				BlockDefinition.PlaceholderText = value;
			}
		}

		/// <summary>
		///		Gets or sets the state indicating if a given input field was validated or not and if the validation was valid.
		///		This should be used by the client to add a visual marker on the input field.
		/// </summary>
		/// <remarks>Available from DataMiner Feature Release 10.0.5 and Main Release 10.1.0 onwards.</remarks>
		public UIValidationState ValidationState
		{
			get
			{
				return BlockDefinition.ValidationState;
			}

			set
			{
				BlockDefinition.ValidationState = value;
			}
		}

		/// <summary>
		///		Gets or sets the text that is shown if the validation state is invalid.
		///		This should be used by the client to add a visual marker on the input field.
		/// </summary>
		/// <remarks>Available from DataMiner Feature Release 10.0.5 and Main Release 10.1.0 onwards.</remarks>
		public string ValidationText
		{
			get
			{
				return BlockDefinition.ValidationText;
			}

			set
			{
				BlockDefinition.ValidationText = value;
			}
		}

		internal override void LoadResult(UIResults uiResults)
		{
			string value = uiResults.GetString(this);
			if (WantsOnChange)
			{
				changed = value != Text;
				previous = Text;
			}

			Text = value;
		}

		/// <inheritdoc />
		internal override void RaiseResultEvents()
		{
			if (changed && OnChanged != null)
			{
				OnChanged(this, new TextBoxChangedEventArgs(Text, previous));
			}

			changed = false;
		}

		/// <summary>
		///     Provides data for the <see cref="Changed" /> event.
		/// </summary>
		public class TextBoxChangedEventArgs : EventArgs
		{
			internal TextBoxChangedEventArgs(string value, string previous)
			{
				Value = value;
				Previous = previous;
			}

			/// <summary>
			///     Gets the text before the change.
			/// </summary>
			public string Previous { get; private set; }

			/// <summary>
			///     Gets the changed text.
			/// </summary>
			public string Value { get; private set; }
		}
	}
}
