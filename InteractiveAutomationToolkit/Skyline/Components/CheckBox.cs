namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using System.Linq;
	using Skyline.DataMiner.Automation;

	/// <summary>
	///     A checkbox that can be selected or cleared.
	/// </summary>
	public class CheckBox : InteractiveWidget
	{
		private bool changed;
		private bool focusLost;
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

		/// <summary>
		///     Triggered when the state of the checkbox changes.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<CheckBoxChangedEventArgs> Changed
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
				bool noOnCheckedEvents = OnChecked == null || !OnChecked.GetInvocationList().Any();
				bool noOnUnCheckedEvents = OnUnChecked == null || !OnUnChecked.GetInvocationList().Any();

				if (noOnChangedEvents && noOnCheckedEvents && noOnUnCheckedEvents)
				{
					BlockDefinition.WantsOnChange = false;
				}
			}
		}

		/// <summary>
		///     Triggered when the checkbox is selected.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<EventArgs> Checked
		{
			add
			{
				OnChecked += value;
				BlockDefinition.WantsOnChange = true;
			}

			remove
			{
				OnChecked -= value;
				bool noOnChangedEvents = OnChanged == null || !OnChanged.GetInvocationList().Any();
				bool noOnCheckedEvents = OnChecked == null || !OnChecked.GetInvocationList().Any();
				bool noOnUnCheckedEvents = OnUnChecked == null || !OnUnChecked.GetInvocationList().Any();

				if (noOnChangedEvents && noOnCheckedEvents && noOnUnCheckedEvents)
				{
					BlockDefinition.WantsOnChange = false;
				}
			}
		}

		/// <summary>
		///     Triggered when the checkbox is cleared.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<EventArgs> UnChecked
		{
			add
			{
				OnUnChecked += value;
				BlockDefinition.WantsOnChange = true;
			}

			remove
			{
				OnUnChecked -= value;
				bool noOnChangedEvents = OnChanged == null || !OnChanged.GetInvocationList().Any();
				bool noOnCheckedEvents = OnChecked == null || !OnChecked.GetInvocationList().Any();
				bool noOnUnCheckedEvents = OnUnChecked == null || !OnUnChecked.GetInvocationList().Any();

				if (noOnChangedEvents && noOnCheckedEvents && noOnUnCheckedEvents)
				{
					BlockDefinition.WantsOnChange = false;
				}
			}
		}

		private event EventHandler<CheckBoxChangedEventArgs> OnChanged;

		private event EventHandler<EventArgs> OnChecked;

		private event EventHandler<EventArgs> OnUnChecked;

		/// <summary>
		///     Triggered when the user loses focus of the CheckBox.
		///     WantsOnFocusLost will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<CheckBoxFocusLostEventArgs> FocusLost
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

		private event EventHandler<CheckBoxFocusLostEventArgs> OnFocusLost;

		/// <summary>
		///     Gets or sets a value indicating whether the checkbox is selected.
		/// </summary>
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

		/// <summary>
		///     Gets or sets the displayed text next to the checkbox.
		/// </summary>
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


		internal override void LoadResult(UIResults uiResults)
		{
			bool result = uiResults.GetChecked(this);
			bool wasOnFocusLost = uiResults.WasOnFocusLost(this);

			if (BlockDefinition.WantsOnChange) changed = result != IsChecked;
			if (BlockDefinition.WantsOnFocusLost) focusLost = wasOnFocusLost;

			IsChecked = result;
		}

		/// <inheritdoc />
		internal override void RaiseResultEvents()
		{
			if (changed) OnChanged?.Invoke(this, new CheckBoxChangedEventArgs(IsChecked));
			if (changed && IsChecked) OnChecked?.Invoke(this, EventArgs.Empty);
			if (changed && !IsChecked) OnUnChecked?.Invoke(this, EventArgs.Empty);
			if (focusLost) OnFocusLost?.Invoke(this, new CheckBoxFocusLostEventArgs(IsChecked));

			changed = false;
			focusLost = false;
		}

		/// <summary>
		///     Provides data for the <see cref="Changed" /> event.
		/// </summary>
		public class CheckBoxChangedEventArgs : EventArgs
		{
			internal CheckBoxChangedEventArgs(bool isChecked)
			{
				IsChecked = isChecked;
			}

			/// <summary>
			///     Gets a value indicating whether the checkbox has been checked.
			/// </summary>
			public bool IsChecked { get; private set; }
		}

		/// <summary>
		///     Provides data for the <see cref="FocusLost" /> event.
		/// </summary>
		public class CheckBoxFocusLostEventArgs : EventArgs
		{
			internal CheckBoxFocusLostEventArgs(bool isChecked)
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
