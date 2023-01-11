namespace Skyline.DataMiner.Utils.InteractiveAutomationToolkit
{
	using System;

	using Skyline.DataMiner.Automation;

	/// <summary>
	///     A widget that requires user input.
	/// </summary>
	public abstract class InteractiveWidget : Widget, IInteractiveWidget
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="InteractiveWidget" /> class.
		/// </summary>
		protected InteractiveWidget()
		{
			BlockDefinition.DestVar = Guid.NewGuid().ToString();
			WantsOnChange = false;
		}

		/// <inheritdoc />
		public bool IsEnabled
		{
			get => BlockDefinition.IsEnabled;
			set => BlockDefinition.IsEnabled = value;
		}

		/// <summary>
		///     Gets the alias that will be used to retrieve the value entered or selected by the user from the UIResults object.
		/// </summary>
		protected internal string DestVar => BlockDefinition.DestVar;

		/// <summary>
		///     Gets or sets a value indicating whether an update of the current value of the dialog box item will trigger an
		///     event.
		/// </summary>
		/// <remarks>Should not be used as this value is set automatically when subscribing to events.</remarks>
		protected internal bool WantsOnChange
		{
			get => BlockDefinition.WantsOnChange;
			protected set => BlockDefinition.WantsOnChange = value;
		}

		/// <summary>
		///     Load any changes made through user interaction.
		/// </summary>
		/// <param name="results">
		///     Represents the information a user has entered or selected in a dialog box of an interactive
		///     Automation script.
		/// </param>
		/// <remarks><see cref="DestVar" /> should be used as key to get the changes for this widget.</remarks>
		protected internal abstract void LoadResult(UIResults results);

		/// <summary>
		///     Raises zero or more events of the widget.
		///     This method is called after <see cref="LoadResult" /> was called on all widgets.
		/// </summary>
		/// <remarks>It is up to the implementer to determine if an event must be raised.</remarks>
		protected internal abstract void RaiseResultEvents();
	}
}