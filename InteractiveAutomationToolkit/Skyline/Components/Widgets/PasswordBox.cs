namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;

	using Skyline.DataMiner.Automation;

	/// <summary>
	///     A text box for passwords.
	/// </summary>
	/// <remarks>Available from DataMiner 9.6.6 onwards.</remarks>
	public class PasswordBox : InteractiveWidget, IPasswordBox
	{
		private bool changed;
		private string previous;
		private readonly Validation validation;

		/// <summary>
		///     Initializes a new instance of the <see cref="PasswordBox" /> class.
		/// </summary>
		/// <param name="hasPeekIcon">A value indicating whether the peek icon to reveal the password is shown.</param>
		public PasswordBox(bool hasPeekIcon)
		{
			Type = UIBlockType.PasswordBox;
			validation = new Validation(this);
			HasPeekIcon = hasPeekIcon;
			PlaceHolder = String.Empty;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="PasswordBox" /> class.
		/// </summary>
		public PasswordBox()
			: this(false)
		{
		}

		/// <inheritdoc />
		public event EventHandler<ChangedEventArgs> Changed;

		/// <inheritdoc />
		public bool HasPeekIcon
		{
			get => BlockDefinition.HasPeekIcon;
			set => BlockDefinition.HasPeekIcon = value;
		}

		/// <inheritdoc />
		public string Password
		{
			get => BlockDefinition.InitialValue;
			set => BlockDefinition.InitialValue = value ?? String.Empty;
		}

		/// <inheritdoc />
		public string PlaceHolder
		{
			get => BlockDefinition.PlaceholderText;
			set => BlockDefinition.PlaceholderText = value ?? String.Empty;
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
			string result = results.GetString(this);
			if (WantsOnChange && result != Password)
			{
				changed = true;
				previous = Password;
			}

			Password = result;
		}

		/// <inheritdoc />
		protected internal override void RaiseResultEvents()
		{
			if (changed && Changed != null)
			{
				Changed(this, new ChangedEventArgs(Password, previous));
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
			/// <param name="password">The new password.</param>
			/// <param name="previous">The previous password.</param>
			public ChangedEventArgs(string password, string previous)
			{
				Password = password;
				Previous = previous;
			}

			/// <summary>
			///     Gets the new password.
			/// </summary>
			public string Password { get; }

			/// <summary>
			///     Gets the previous password.
			/// </summary>
			public string Previous { get; }
		}
	}
}