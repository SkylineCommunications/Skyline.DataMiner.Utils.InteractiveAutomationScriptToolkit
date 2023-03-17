namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;

	using Skyline.DataMiner.Automation;

	/// <summary>
	/// A widget that requires user input.
	/// </summary>
	public abstract class InteractiveWidget : Widget
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="InteractiveWidget"/> class.
		/// </summary>
		protected InteractiveWidget()
		{
			BlockDefinition.DestVar = Guid.NewGuid().ToString();
		}

		/// <summary>
		///     Gets or sets a value indicating whether the control is enabled in the UI.
		///     Disabling causes the widgets to be grayed out and disables user interaction.
		/// </summary>
		/// <remarks>Available from DataMiner 9.5.3 onwards.</remarks>
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
		///     Gets the alias that will be used to retrieve the value entered or selected by the user from the UIResults object.
		/// </summary>
		/// <remarks>Use methods <see cref="UiResultsExtensions" /> to retrieve the result instead.</remarks>
		internal string DestVar
		{
			get
			{
				return BlockDefinition.DestVar;
			}
		}

		/// <summary>
		///     Load any changes made through user interaction.
		/// </summary>
		/// <param name="uiResults">
		///     Represents the information a user has entered or selected in a dialog box of an interactive
		///     Automation script.
		/// </param>
		/// <remarks><see cref="DestVar" /> should be used as key to get the changes for this widget.</remarks>
		internal abstract void LoadResult(UIResults uiResults);

		/// <summary>
		///     Raises zero or more events of the widget.
		///     This method is called after <see cref="LoadResult" /> was called on all widgets.
		/// </summary>
		/// <remarks>It is up to the implementer to determine if an event must be raised.</remarks>
		internal abstract void RaiseResultEvents();
	}
}
