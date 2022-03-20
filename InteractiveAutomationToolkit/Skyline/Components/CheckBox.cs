namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using System.Linq;

	using Automation;

	/// <summary>
	///     A checkbox that can be selected or cleared.
	/// </summary>
	public class CheckBox : InteractiveWidget, ICheckBox
	{
		private bool changed;
		private bool isChecked;

		/// <summary>
		///     Initializes a new instance of the <see cref="CheckBox" /> class.
		/// </summary>
		/// <param name="text">Text displayed next to the checkbox.</param>
		public CheckBox(string text)
		{
			Type = UIBlockType.CheckBox;
			IsChecked = false;
			Text = text;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="CheckBox" /> class.
		/// </summary>
		public CheckBox() : this(String.Empty)
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
				bool noOnChangedEvents = OnChanged == null || !OnChanged.GetInvocationList().Any();
				bool noOnCheckedEvents = OnChecked == null || !OnChecked.GetInvocationList().Any();
				bool noOnUnCheckedEvents = OnUnChecked == null || !OnUnChecked.GetInvocationList().Any();

				if (noOnChangedEvents && noOnCheckedEvents && noOnUnCheckedEvents)
				{
					WantsOnChange = false;
				}
			}
		}

		/// <inheritdoc />
		public event EventHandler<EventArgs> Checked
		{
			add
			{
				OnChecked += value;
				WantsOnChange = true;
			}

			remove
			{
				OnChecked -= value;
				bool noOnChangedEvents = OnChanged == null || !OnChanged.GetInvocationList().Any();
				bool noOnCheckedEvents = OnChecked == null || !OnChecked.GetInvocationList().Any();
				bool noOnUnCheckedEvents = OnUnChecked == null || !OnUnChecked.GetInvocationList().Any();

				if (noOnChangedEvents && noOnCheckedEvents && noOnUnCheckedEvents)
				{
					WantsOnChange = false;
				}
			}
		}

		/// <inheritdoc />
		public event EventHandler<EventArgs> UnChecked
		{
			add
			{
				OnUnChecked += value;
				WantsOnChange = true;
			}

			remove
			{
				OnUnChecked -= value;
				bool noOnChangedEvents = OnChanged == null || !OnChanged.GetInvocationList().Any();
				bool noOnCheckedEvents = OnChecked == null || !OnChecked.GetInvocationList().Any();
				bool noOnUnCheckedEvents = OnUnChecked == null || !OnUnChecked.GetInvocationList().Any();

				if (noOnChangedEvents && noOnCheckedEvents && noOnUnCheckedEvents)
				{
					WantsOnChange = false;
				}
			}
		}

		private event EventHandler<ChangedEventArgs> OnChanged;

		private event EventHandler<EventArgs> OnChecked;

		private event EventHandler<EventArgs> OnUnChecked;

		/// <inheritdoc />
		public bool IsChecked
		{
			get
			{
				return isChecked;
			}

			set
			{
				isChecked = value;
				BlockDefinition.InitialValue = value.ToString();
			}
		}

		/// <inheritdoc />
		public string Text
		{
			get
			{
				return BlockDefinition.Text;
			}

			set
			{
				BlockDefinition.Text = value;
			}
		}

		/// <inheritdoc />
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
					throw new ArgumentNullException(nameof(value));
				}

				BlockDefinition.TooltipText = value;
			}
		}

		/// <inheritdoc />
		protected internal override void LoadResult(UIResults uiResults)
		{
			bool result = uiResults.GetChecked(this);
			if (WantsOnChange)
			{
				changed = result != IsChecked;
			}

			IsChecked = result;
		}

		/// <inheritdoc />
		protected internal override void RaiseResultEvents()
		{
			if (!changed)
			{
				return;
			}

			if (OnChanged != null)
			{
				OnChanged(this, new ChangedEventArgs(IsChecked));
			}

			if (OnChecked != null && IsChecked)
			{
				OnChecked(this, EventArgs.Empty);
			}

			if (OnUnChecked != null && !IsChecked)
			{
				OnUnChecked(this, EventArgs.Empty);
			}

			changed = false;
		}

		/// <summary>
		///     Provides data for the <see cref="Changed" /> event.
		/// </summary>
		public class ChangedEventArgs : EventArgs
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="ChangedEventArgs"/> class.
			/// </summary>
			/// <param name="isChecked">The new checked state.</param>
			public ChangedEventArgs(bool isChecked)
			{
				IsChecked = isChecked;
			}

			/// <summary>
			///     Gets a value indicating whether the checkbox has been checked.
			/// </summary>
			public bool IsChecked { get; private set; }
		}
	}
}
