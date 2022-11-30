namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using Skyline.DataMiner.Automation;

	/// <summary>
	/// A widget that requires user input.
	/// </summary>
	public abstract class InteractiveWidget : Widget
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
		/// <remarks>Use methods <see cref="UiResultsExtensions" /> to retrieve the result instead.</remarks>
		internal string DestVar
		{
			get
			{
				return BlockDefinition.DestVar;
			}
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
		///     Gets or sets a value indicating whether an update of the current value of the dialog box item will trigger an
		///     event.
		/// </summary>
		/// <remarks>Is <c>false</c> by default except for <see cref="Button" />.</remarks>
		public bool WantsOnChange
		{
			get
			{
				return BlockDefinition.WantsOnChange;
			}

			set
			{
				BlockDefinition.WantsOnChange = value;
			}
		}

		internal abstract void LoadResult(UIResults uiResults);

		internal abstract void RaiseResultEvents();
	}
}
