namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;

	using Automation;

	/// <summary>
	/// A widget that requires user input.
	/// </summary>
	public abstract class InteractiveWidget : Widget, IInteractiveWidget
	{
		/// <summary>
		/// Initializes a new instance of the InteractiveWidget class.
		/// </summary>
		protected InteractiveWidget()
		{
			BlockDefinition.DestVar = Guid.NewGuid().ToString();
			WantsOnChange = false;
		}

		/// <summary>
		///     Gets the alias that will be used to retrieve the value entered or selected by the user from the UIResults object.
		/// </summary>
		protected internal string DestVar
		{
			get
			{
				return BlockDefinition.DestVar;
			}
		}

		/// <inheritdoc />
		public bool IsEnabled
		{
			get
			{
				return BlockDefinition.IsEnabled;
			}

			set
			{
				BlockDefinition.IsEnabled = value;
			}
		}

		/// <summary>
		///     Gets or sets a value indicating whether an update of the current value of the dialog box item will trigger an event.
		/// </summary>
		/// <remarks>Should not be used as this value is set automatically when subscribing to events!</remarks>
		protected internal bool WantsOnChange
		{
			get
			{
				return BlockDefinition.WantsOnChange;
			}

			protected set
			{
				BlockDefinition.WantsOnChange = value;
			}
		}

		/// <summary>
		/// Load any changes made through user interaction.
		/// </summary>
		/// <param name="uiResults">Represents the information a user has entered or selected in a dialog box of an interactive Automation script.</param>
		/// <remarks><see cref="DestVar"/> should be used as key to get the changes for this widget.</remarks>
		protected internal abstract void LoadResult(UIResults uiResults);

		/// <summary>
		/// Raises zero or more events of the widget.
		/// This method is called after <see cref="LoadResult"/> was called on all widgets.
		/// </summary>
		/// <remarks>It is up to the implementer to determine if an event must be raised.</remarks>
		protected internal abstract void RaiseResultEvents();
	}
}
