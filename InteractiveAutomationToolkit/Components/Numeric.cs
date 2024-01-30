namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Globalization;
	using System.Linq;

	using Skyline.DataMiner.Automation;

	/// <summary>
	///     A spinner or numeric up-down control.
	///     Has a slider when the range is limited.
	/// </summary>
	public class Numeric : InteractiveWidget
	{
		private bool changed;
		private bool focusLost;

		private double previous;
		private double value;

		/// <summary>
		///     Initializes a new instance of the <see cref="Numeric" /> class.
		/// </summary>
		/// <param name="value">Current value of the numeric.</param>
		public Numeric(double value)
		{
			Type = UIBlockType.Numeric;
			Maximum = Double.MaxValue;
			Minimum = Double.MinValue;
			Decimals = 0;
			StepSize = 1;
			Value = value;
			ValidationText = "Invalid Input";
			ValidationState = UIValidationState.NotValidated;
			IsReadOnly = false;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Numeric" /> class.
		/// </summary>
		public Numeric() : this(0)
		{
		}

		/// <summary>
		///     Triggered when the value of the numeric changes.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<NumericChangedEventArgs> Changed
		{
			add
			{
				OnChanged += value;
				BlockDefinition.WantsOnChange = true;
			}

			remove
			{
				OnChanged -= value;
				if (OnChanged == null || !OnChanged.GetInvocationList().Any())
				{
					BlockDefinition.WantsOnChange = false;
				}
			}
		}

		/// <summary>
		///     Triggered when the user loses focus of the Numeric.
		///     WantsOnFocusLost will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<NumericFocusLostEventArgs> FocusLost
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

		private event EventHandler<NumericChangedEventArgs> OnChanged;

		private event EventHandler<NumericFocusLostEventArgs> OnFocusLost;

		/// <summary>
		///     Gets or sets the number of decimals to show.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">When the value is smaller than 0.</exception>
		public int Decimals
		{
			get
			{
				return BlockDefinition.Decimals;
			}

			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				BlockDefinition.Decimals = value;
			}
		}

		/// <summary>
		///     Gets or sets the maximum value of the range.
		/// </summary>
		/// <exception cref="ArgumentException">When the value is smaller than the minimum.</exception>
		/// <exception cref="ArgumentException">When the value is <c>Double.NaN</c> or infinity.</exception>
		public double Maximum
		{
			get
			{
				return BlockDefinition.RangeHigh;
			}

			set
			{
				if (value < Minimum)
				{
					throw new ArgumentException("Maximum can't be smaller than Minimum", "value");
				}

				CheckDouble(value);

				BlockDefinition.RangeHigh = value;
			}
		}

		/// <summary>
		///     Gets or sets the minimum value of the range.
		/// </summary>
		/// <exception cref="ArgumentException">When the value is larger than the maximum.</exception>
		/// <exception cref="ArgumentException">When the value is <c>Double.NaN</c> or infinity.</exception>
		public double Minimum
		{
			get
			{
				return BlockDefinition.RangeLow;
			}

			set
			{
				if (value > Maximum)
				{
					throw new ArgumentException("Minimum can't be larger than Maximum", "value");
				}

				CheckDouble(value);

				BlockDefinition.RangeLow = value;
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
		///     Gets or sets the step size.
		/// </summary>
		/// <exception cref="ArgumentException">When the value is <c>Double.NaN</c> or infinity.</exception>
		public double StepSize
		{
			get
			{
				return BlockDefinition.RangeStep;
			}

			set
			{
				CheckDouble(value);
				BlockDefinition.RangeStep = value;
			}
		}

		/// <summary>
		///     Gets or sets the value of the numeric.
		/// </summary>
		public double Value
		{
			get
			{
				return value;
			}

			set
			{
				this.value = value;
				BlockDefinition.InitialValue = value.ToString(CultureInfo.InvariantCulture);
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
		///		Gets or sets a value indicating whether the control is displayed in read-only mode.
		///		Read-only mode causes the widgets to appear read-write but the user won't be able to change their value.
		///		This only affects interactive scripts running in a web environment.
		/// </summary>
		/// <remarks>Available from DataMiner 10.4.1 onwards.</remarks>
		public virtual bool IsReadOnly
		{
			get
			{
				return BlockDefinition.IsReadOnly;
			}

			set
			{
				BlockDefinition.IsReadOnly = value;
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
			bool wasOnFocusLost = uiResults.WasOnFocusLost(this);
			if (BlockDefinition.WantsOnFocusLost)
			{
				focusLost = wasOnFocusLost;
			}

			double result;
			if (!Double.TryParse(
				uiResults.GetString(this),
				NumberStyles.Float,
				CultureInfo.InvariantCulture,
				out result))
			{
				return;
			}

			bool isNotEqual = !IsEqualWithinDecimalMargin(result, value);
			if (isNotEqual && BlockDefinition.WantsOnChange)
			{
				changed = true;
				previous = result;
			}

			Value = result;
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
				OnChanged?.Invoke(this, new NumericChangedEventArgs(Value, previous));
			}

			if (focusLost)
			{
				OnFocusLost?.Invoke(this, new NumericFocusLostEventArgs(Value));
			}

			changed = false;
			focusLost = false;
		}

		// ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
		private static void CheckDouble(double value)
		{
			if (Double.IsNaN(value))
			{
				throw new ArgumentException("NAN is not allowed", "value");
			}

			if (Double.IsInfinity(value))
			{
				throw new ArgumentException("Infinity is not allowed", "value");
			}
		}

		private bool IsEqualWithinDecimalMargin(double a, double b)
		{
			return Math.Abs(a - b) < Math.Pow(10, -Decimals);
		}

		/// <summary>
		///     Provides data for the <see cref="Changed" /> event.
		/// </summary>
		public class NumericChangedEventArgs : EventArgs
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="NumericChangedEventArgs"/> class.
			/// </summary>
			/// <param name="value">The new value.</param>
			/// <param name="previous">The previous value.</param>
			internal NumericChangedEventArgs(double value, double previous)
			{
				Value = value;
				Previous = previous;
			}

			/// <summary>
			///     Gets the previous value of the numeric.
			/// </summary>
			public double Previous { get; private set; }

			/// <summary>
			///     Gets the new value of the numeric.
			/// </summary>
			public double Value { get; private set; }
		}

		/// <summary>
		///     Provides data for the <see cref="FocusLost" /> event.
		/// </summary>
		public class NumericFocusLostEventArgs : EventArgs
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="NumericFocusLostEventArgs"/> class.
			/// </summary>
			/// <param name="value">The new value.</param>
			internal NumericFocusLostEventArgs(double value)
			{
				Value = value;
			}

			/// <summary>
			///     Gets the new value of the numeric.
			/// </summary>
			public double Value { get; private set; }
		}
	}
}
