namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;
	using System.ComponentModel;
	using System.Globalization;
	using System.Linq;

	using Skyline.DataMiner.Automation;

	/// <summary>
	///     A spinner or numeric up-down control.
	///     Has a slider when the range is limited.
	/// </summary>
	public class Numeric : InteractiveWidget, INumeric
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
		public Numeric()
			: this(0)
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
		public int Decimals
		{
			get => BlockDefinition.Decimals;

			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException(nameof(value));
				}

				BlockDefinition.Decimals = value;
			}
		}

		/// <inheritdoc />
		public double Maximum
		{
			get => BlockDefinition.RangeHigh;

			set
			{
				if (value < Minimum)
				{
					throw new ArgumentException("Maximum can't be smaller than Minimum", nameof(value));
				}

				CheckDouble(value);

				BlockDefinition.RangeHigh = value;
				Value = ClipToRange(Value);
			}
		}

		/// <inheritdoc />
		public double Minimum
		{
			get => BlockDefinition.RangeLow;

			set
			{
				if (value > Maximum)
				{
					throw new ArgumentException("Minimum can't be larger than Maximum", nameof(value));
				}

				CheckDouble(value);

				BlockDefinition.RangeLow = value;
				Value = ClipToRange(Value);
			}
		}

		/// <inheritdoc />
		public double StepSize
		{
			get => BlockDefinition.RangeStep;

			set
			{
				CheckDouble(value);
				BlockDefinition.RangeStep = value;
			}
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
			get => BlockDefinition.ValidationState;

			set
			{
				if (!Enum.IsDefined(typeof(UIValidationState), value))
				{
					throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(UIValidationState));
				}

				BlockDefinition.ValidationState = value;
			}
		}

		/// <inheritdoc />
		public string ValidationText
		{
			get => BlockDefinition.ValidationText;
			set => BlockDefinition.ValidationText = value ?? String.Empty;
		}

		/// <inheritdoc />
		public double Value
		{
			get => value;

			set
			{
				value = ClipToRange(value);
				this.value = value;
				BlockDefinition.InitialValue = value.ToString(CultureInfo.InvariantCulture);
			}
		}

		/// <inheritdoc />
		protected internal override void LoadResult(UIResults results)
		{
			if (!Double.TryParse(
					results.GetString(this),
					NumberStyles.Float,
					CultureInfo.InvariantCulture,
					out double result))
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
		protected internal override void RaiseResultEvents()
		{
			if (changed && OnChanged != null)
			{
				OnChanged(this, new ChangedEventArgs(Value, previous));
			}

			changed = false;
		}

		// ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
		private static void CheckDouble(double value)
		{
			if (Double.IsNaN(value))
			{
				throw new ArgumentException("NAN is not allowed", nameof(value));
			}

			if (Double.IsInfinity(value))
			{
				throw new ArgumentException("Infinity is not allowed", nameof(value));
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
		public class ChangedEventArgs : EventArgs
		{
			/// <summary>
			///     Initializes a new instance of the <see cref="ChangedEventArgs" /> class.
			/// </summary>
			/// <param name="value">The new value of the numeric.</param>
			/// <param name="previous">The previous value of the numeric.</param>
			public ChangedEventArgs(double value, double previous)
			{
				Value = value;
				Previous = previous;
			}

			/// <summary>
			///     Gets the previous value of the numeric.
			/// </summary>
			public double Previous { get; }

			/// <summary>
			///     Gets the new value of the numeric.
			/// </summary>
			public double Value { get; }
		}
	}
}