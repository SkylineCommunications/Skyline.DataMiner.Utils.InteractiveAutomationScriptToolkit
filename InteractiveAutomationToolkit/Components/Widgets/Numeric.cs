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
	public class Numeric : InteractiveWidget, INumeric
	{
		private readonly Validation validation;
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
			validation = new Validation(this);
			Maximum = Double.MaxValue;
			Minimum = Double.MinValue;
			Decimals = 0;
			StepSize = 1;
			Value = value;
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

		/// <inheritdoc/>
		public bool IsRequired
		{
			get => BlockDefinition.IsRequired;
			set => BlockDefinition.IsRequired = value;
		}

		/// <inheritdoc />
		public double Maximum
		{
			get => BlockDefinition.RangeHigh;

			set
			{
				ThrowIfNaNOrInfinity(value);

				BlockDefinition.RangeHigh = value;
			}
		}

		/// <inheritdoc />
		public double Minimum
		{
			get => BlockDefinition.RangeLow;

			set
			{
				ThrowIfNaNOrInfinity(value);

				BlockDefinition.RangeLow = value;
			}
		}

		/// <inheritdoc />
		public double StepSize
		{
			get => BlockDefinition.RangeStep;

			set
			{
				ThrowIfNaNOrInfinity(value);
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
		public double Value
		{
			get => value;

			set
			{
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
				// The double cannot be parsed if the numeric was cleared (is empty).
				// However, when any interaction occurs and the window is updated, cube and web will convert this empty value to 0.
				// To avoid confusion, the api should return the same value (0).
				result = 0;
			}

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
		private static void ThrowIfNaNOrInfinity(double value)
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