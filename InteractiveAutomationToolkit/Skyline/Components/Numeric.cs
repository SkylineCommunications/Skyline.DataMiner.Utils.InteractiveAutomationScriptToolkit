namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
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

		private event EventHandler<NumericChangedEventArgs> OnChanged;

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
				Value = ClipToRange(Value);
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
				Value = ClipToRange(Value);
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
				value = ClipToRange(value);
				this.value = value;
				BlockDefinition.InitialValue = value.ToString(CultureInfo.InvariantCulture);
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

		/// <inheritdoc />
		internal override void LoadResult(UIResults uiResults)
		{
			double result;
			if (!Double.TryParse(
				uiResults.GetString(this),
				NumberStyles.Float,
				CultureInfo.InvariantCulture,
				out result))
			{
				return;
			}

			result = ClipToRange(result);
			bool isNotEqual = !IsEqualWithinDecimalMargin(result, value);
			if (isNotEqual && WantsOnChange)
			{
				changed = true;
				previous = result;
			}

			Value = result;
		}

		/// <inheritdoc />
		internal override void RaiseResultEvents()
		{
			if (changed && (OnChanged != null))
			{
				OnChanged(this, new NumericChangedEventArgs(Value, previous));
			}

			changed = false;
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

		private double ClipToRange(double number)
		{
			number = Math.Min(Maximum, number);
			number = Math.Max(Minimum, number);
			return number;
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
	}
}
