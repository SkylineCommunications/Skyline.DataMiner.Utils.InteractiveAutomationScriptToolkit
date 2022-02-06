namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using System.ComponentModel;

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
		/// <remarks>Use methods <see cref="UiResultsExtensions" /> to retrieve the result instead.</remarks>
		internal string DestVar
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

		/// <inheritdoc />
		[EditorBrowsable(EditorBrowsableState.Never)]
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
