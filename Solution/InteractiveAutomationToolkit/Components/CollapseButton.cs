namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Skyline.DataMiner.Automation;

	/// <summary>
	///		A button that can be used to show/hide a collection of widgets.
	/// </summary>
	public class CollapseButton : InteractiveWidget
	{
		private const string COLLAPSE = "Collapse";
		private const string EXPAND = "Expand";

		private string collapseText;
		private string expandText;

		private bool pressed;
		private bool isCollapsed;

		/// <summary>
		/// Initializes a new instance of the CollapseButton class.
		/// </summary>
		/// <param name="linkedWidgets">Widgets that are linked to this collapse button.</param>
		/// <param name="isCollapsed">State of the collapse button.</param>
		public CollapseButton(IEnumerable<Widget> linkedWidgets, bool isCollapsed)
		{
			Type = UIBlockType.Button;
			LinkedWidgets = new List<Widget>(linkedWidgets);
			CollapseText = COLLAPSE;
			ExpandText = EXPAND;

			IsCollapsed = isCollapsed;

			WantsOnChange = true;
		}

		/// <summary>
		/// Initializes a new instance of the CollapseButton class.
		/// </summary>
		/// <param name="isCollapsed">State of the collapse button.</param>
		public CollapseButton(bool isCollapsed = false) : this(new Widget[0], isCollapsed)
		{
		}

		/// <summary>
		///     Triggered when the button is pressed.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<EventArgs> Pressed
		{
			add
			{
				OnPressed += value;
			}

			remove
			{
				OnPressed -= value;
			}
		}

		private event EventHandler<EventArgs> OnPressed;

		/// <summary>
		/// Indicates if the collapse button is collapsed or not.
		/// If the collapse button is collapsed, the IsVisible property of all linked widgets is set to false.
		/// If the collapse button is not collapsed, the IsVisible property of all linked widgets is set to true.
		/// </summary>
		public bool IsCollapsed
		{
			get
			{
				return isCollapsed;
			}

			set
			{
				isCollapsed = value;
				BlockDefinition.Text = value ? ExpandText : CollapseText;
				foreach(Widget widget in GetAffectedWidgets(this, value))
				{
					widget.IsVisible = !value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the text to be displayed in the collapse button when the button is expanded.
		/// </summary>
		public string CollapseText
		{
			get
			{
				return collapseText;
			}

			set
			{
				if (String.IsNullOrWhiteSpace(value)) throw new ArgumentException("The Collapse text cannot be empty.");

				collapseText = value;
				if (!IsCollapsed) BlockDefinition.Text = collapseText;
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
		/// Gets or sets the text to be displayed in the collapse button when the button is collapsed.
		/// </summary>
		public string ExpandText
		{
			get
			{
				return expandText;
			}

			set
			{
				if (String.IsNullOrWhiteSpace(value)) throw new ArgumentException("The Expand text cannot be empty.");

				expandText = value;
				if (IsCollapsed) BlockDefinition.Text = expandText;
			}
		}

		/// <summary>
		/// Collection of widgets that are affected by this collapse button.
		/// </summary>
		public List<Widget> LinkedWidgets { get; private set; }

		/// <summary>
		/// This method is used to collapse the collapse button.
		/// </summary>
		public void Collapse()
		{
			IsCollapsed = true;
		}

		/// <summary>
		/// This method is used to expand the collapse button.
		/// </summary>
		public void Expand()
		{
			IsCollapsed = false;
		}

		internal override void LoadResult(UIResults uiResults)
		{
			pressed = uiResults.WasCollapseButtonPressed(this);
		}

		internal override void RaiseResultEvents()
		{
			if (pressed)
			{
				IsCollapsed = !IsCollapsed;
				if (OnPressed != null) OnPressed(this, EventArgs.Empty);
			}

			pressed = false;
		}

		/// <summary>
		/// Retrieves a list of Widgets that are affected when the state of the provided collapse button is changed.
		/// This method was introduced to support nested collapse buttons.
		/// </summary>
		/// <param name="collapseButton">Collapse button that is checked.</param>
		/// <param name="collapse">Indicates if the top collapse button is going to be collapsed or expanded.</param>
		/// <returns>List of affected widgets.</returns>
		private static List<Widget> GetAffectedWidgets(CollapseButton collapseButton, bool collapse)
		{
			List<Widget> affectedWidgets = new List<Widget>();
			affectedWidgets.AddRange(collapseButton.LinkedWidgets);

			var nestedCollapseButtons = collapseButton.LinkedWidgets.OfType<CollapseButton>();
			foreach (CollapseButton nestedCollapseButton in nestedCollapseButtons)
			{
				if (collapse)
				{
					// Collapsing top collapse button
					affectedWidgets.AddRange(GetAffectedWidgets(nestedCollapseButton, collapse));
				}
				else if (!nestedCollapseButton.IsCollapsed)
				{
					// Expanding top collapse button
					affectedWidgets.AddRange(GetAffectedWidgets(nestedCollapseButton, collapse));
				}
			}

			return affectedWidgets;
		}
	}
}
