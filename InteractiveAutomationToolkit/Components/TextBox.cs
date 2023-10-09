namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
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
		private bool focusLost;
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
				BlockDefinition.WantsOnChange = true;
			}

			remove
			{
				OnChanged -= value;

				bool noOnChangedEvents = OnChanged == null || !OnChanged.GetInvocationList().Any();
				if (noOnChangedEvents)
				{
					BlockDefinition.WantsOnChange = false;
				}
			}
		}

		/// <summary>
		///     Triggered when the user loses focus of the TextBox. E.g. clicking somewhere else other than the TextBox widget in the Dialog.
		///     WantsOnFocusLost will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<TextBoxFocusLostEventArgs> FocusLost
		{
			add
			{
				OnFocusLost += value;
				BlockDefinition.WantsOnFocusLost = true;
			}

			remove
			{
				OnFocusLost -= value;
				if (OnFocusLost == null || !OnFocusLost.GetInvocationList().Any())
				{
					BlockDefinition.WantsOnFocusLost = false;
				}
			}
		}

		private event EventHandler<TextBoxChangedEventArgs> OnChanged;

		private event EventHandler<TextBoxFocusLostEventArgs> OnFocusLost;

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
		/// 	Gets or sets the text that should be displayed as a placeholder.
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
		/// 	Gets or sets the state indicating if a given input field was validated or not and if the validation was valid.
		/// 	This should be used by the client to add a visual marker on the input field.
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
		/// 	Gets or sets the text that is shown if the validation state is invalid.
		/// 	This should be used by the client to add a visual marker on the input field.
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

		/// <summary>
		///     Load any changes made through user interaction.
		/// </summary>
		/// <param name="uiResults">
		///     Represents the information a user has entered or selected in a dialog box of an interactive
		///     Automation script.
		/// </param>
		/// <remarks><see cref="InteractiveWidget.DestVar" /> should be used as key to get the changes for this widget.</remarks>
		protected internal override void LoadResult(UIResults uiResults)
		{
			string value = uiResults.GetString(this);
			bool wasOnFocusLost = uiResults.WasOnFocusLost(this);

			if (BlockDefinition.WantsOnChange)
			{
				changed = value != Text;
				previous = Text;
			}

			if (BlockDefinition.WantsOnFocusLost)
			{
				focusLost = wasOnFocusLost;
			}

			Text = value;
		}

		/// <summary>
		///     Raises zero or more events of the widget.
		///     This method is called after <see cref="InteractiveWidget.LoadResult" /> was called on all widgets.
		/// </summary>
		/// <remarks>It is up to the implementer to determine if an event must be raised.</remarks>
		protected internal override void RaiseResultEvents()
		{
			if (changed)
			{
				OnChanged?.Invoke(this, new TextBoxChangedEventArgs(Text, previous));
			}

			if (focusLost)
			{
				OnFocusLost?.Invoke(this, new TextBoxFocusLostEventArgs(Text));
			}

			changed = false;
			focusLost = false;
		}

		/// <summary>
		///     Provides data for the <see cref="Changed" /> event.
		/// </summary>
		public class TextBoxChangedEventArgs : EventArgs
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="TextBoxChangedEventArgs"/> class.
			/// </summary>
			/// <param name="value">The new value.</param>
			/// <param name="previous">The previous value.</param>
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

		/// <summary>
		///     Provides data for the <see cref="FocusLost" /> event.
		/// </summary>
		public class TextBoxFocusLostEventArgs : EventArgs
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="TextBoxFocusLostEventArgs"/> class.
			/// </summary>
			/// <param name="value">The new value.</param>
			internal TextBoxFocusLostEventArgs(string value)
			{
				Value = value;
			}

			/// <summary>
			///     Gets the changed text.
			/// </summary>
			public string Value { get; private set; }
		}
	}
}
