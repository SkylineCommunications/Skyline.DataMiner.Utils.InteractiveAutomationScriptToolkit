namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;

	using Automation;

	/// <summary>
	///     A text box for passwords.
	/// </summary>
	/// <remarks>Available from DataMiner 9.6.6 onwards.</remarks>
	public class PasswordBox : InteractiveWidget, IPasswordBox
	{
		private bool changed;
		private string previous;

		/// <summary>
		///     Initializes a new instance of the <see cref="PasswordBox" /> class.
		/// </summary>
		/// <param name="hasPeekIcon">A value indicating whether the peek icon to reveal the password is shown.</param>
		public PasswordBox(bool hasPeekIcon)
		{
			Type = UIBlockType.PasswordBox;
			HasPeekIcon = hasPeekIcon;
			PlaceHolder = String.Empty;
			ValidationText = "Invalid Input";
			ValidationState = UIValidationState.NotValidated;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="PasswordBox" /> class.
		/// </summary>
		public PasswordBox() : this(false)
		{
		}

		/// <inheritdoc />
		public event EventHandler<ChangedEventArgs> Changed;

		/// <inheritdoc />
		public bool HasPeekIcon
		{
			get
			{
				return BlockDefinition.HasPeekIcon;
			}

			set
			{
				BlockDefinition.HasPeekIcon = value;
			}
		}

		/// <inheritdoc />
		public string Password
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

		/// <inheritdoc />
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

		/// <inheritdoc />
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
		protected internal override void LoadResult(UIResults uiResults)
		{
			string result = uiResults.GetString(this);
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
			/// Initializes a new instance of the <see cref="ChangedEventArgs"/> class.
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
			public string Password { get; private set; }

			/// <summary>
			///     Gets the previous password.
			/// </summary>
			public string Previous { get; private set; }
		}
	}
}