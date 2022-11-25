namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using System.Linq;
	using Skyline.DataMiner.Automation;

	/// <summary>
	///     A text box for passwords.
	/// </summary>
	/// <remarks>Available from DataMiner 9.6.6 onwards.</remarks>
	public class PasswordBox : InteractiveWidget
	{
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

		/// <summary>
		///     Gets or sets a value indicating whether the peek icon to reveal the password is shown.
		///     Default: <c>false</c>
		/// </summary>
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

		/// <summary>
		///     Gets or sets the password set in the password box.
		/// </summary>
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
		///		Gets or sets the text that should be displayed as a placeholder.
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
			string result = uiResults.GetString(this);
			Password = result;
		}

		/// <inheritdoc />
		internal override void RaiseResultEvents()
		{
			// Nothing to trigger
		}
	}
}