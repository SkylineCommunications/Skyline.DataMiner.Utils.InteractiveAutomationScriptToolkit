﻿namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Linq;

	using Skyline.DataMiner.Automation;

	/// <summary>
	///     Widget that is used to edit and display text.
	/// </summary>
	public class TextBox : InteractiveWidget, ITextBox
	{
		private readonly Validation validation;
		private bool changed;
		private string previous;

		/// <summary>
		///     Initializes a new instance of the <see cref="TextBox" /> class.
		/// </summary>
		/// <param name="text">The text displayed in the text box.</param>
		public TextBox(string text)
		{
			Type = UIBlockType.TextBox;
			validation = new Validation(this);
			Text = text;
			PlaceHolder = String.Empty;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="TextBox" /> class.
		/// </summary>
		public TextBox()
			: this(String.Empty)
		{
		}

		/// <inheritdoc />
		public event EventHandler<ChangedEventArgs> Changed
		{
			add
			{
				OnChanged += value;
				WantsOnChange = true;
			}

			remove
			{
				OnChanged -= value;
				if (OnChanged == null || !OnChanged.GetInvocationList().Any())
				{
					WantsOnChange = false;
				}
			}
		}

		private event EventHandler<ChangedEventArgs> OnChanged;

		/// <inheritdoc />
		public bool IsMultiline
		{
			get => BlockDefinition.IsMultiline;
			set => BlockDefinition.IsMultiline = value;
		}

		/// <inheritdoc/>
		public bool IsRequired
		{
			get => BlockDefinition.IsRequired;
			set => BlockDefinition.IsRequired = value;
		}

		/// <inheritdoc />
		public string PlaceHolder
		{
			get => BlockDefinition.PlaceholderText;
			set => BlockDefinition.PlaceholderText = value ?? String.Empty;
		}

		/// <inheritdoc />
		public string Text
		{
			get => BlockDefinition.InitialValue;
			set => BlockDefinition.InitialValue = value ?? String.Empty;
		}

		/// <inheritdoc />
		public string Tooltip
		{
			get => BlockDefinition.TooltipText;
			set => BlockDefinition.TooltipText = value ?? String.Empty;
		}

		/// <inheritdoc />
		public UIValidationState ValidationState
		{
			get => validation.ValidationState;
			set => validation.ValidationState = value;
		}

		/// <inheritdoc />
		public string ValidationText
		{
			get => validation.ValidationText;
			set => validation.ValidationText = value;
		}

		/// <inheritdoc />
		protected internal override void LoadResult(UIResults results)
		{
			string value = results.GetString(this);
			if (WantsOnChange)
			{
				changed = value != Text;
				previous = Text;
			}

			Text = value;
		}

		/// <inheritdoc />
		protected internal override void RaiseResultEvents()
		{
			if (changed && OnChanged != null)
			{
				OnChanged(this, new ChangedEventArgs(Text, previous));
			}

			changed = false;
		}

		/// <summary>
		///     Provides data for the <see cref="Changed" /> event.
		/// </summary>
		public class ChangedEventArgs : EventArgs
		{
			/// <summary>
			///     Initializes a new instance of the <see cref="ChangedEventArgs" /> class.
			/// </summary>
			/// <param name="value">The changed text.</param>
			/// <param name="previous">The text before the change.</param>
			public ChangedEventArgs(string value, string previous)
			{
				Value = value;
				Previous = previous;
			}

			/// <summary>
			///     Gets the text before the change.
			/// </summary>
			public string Previous { get; }

			/// <summary>
			///     Gets the changed text.
			/// </summary>
			public string Value { get; }
		}
	}
}