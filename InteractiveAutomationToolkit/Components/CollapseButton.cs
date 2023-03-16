namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
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

		public CollapseButton(IEnumerable<Widget> linkedWidgets, bool isCollapsed)
		{
			Type = UIBlockType.Button;
			LinkedWidgets = new List<Widget>(linkedWidgets);
			CollapseText = COLLAPSE;
			ExpandText = EXPAND;

			IsCollapsed = isCollapsed;

			WantsOnChange = true;
		}

		public CollapseButton(bool isCollapsed = false) : this(new Widget[0], isCollapsed)
		{
		}

		/// <summary>
		///     Event triggers when the button is pressed.
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
		/// Gets or sets the text to be displayed in the CollapseButton when the button is expanded.
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
		///     Gets or sets the Tooltip.
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
		/// Gets or sets the text to be displayed in the CollapseButton when the button is collapsed.
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

		public List<Widget> LinkedWidgets { get; private set; }

		/// <summary>
		/// This method is used to collapse the Collapse Button.
		/// </summary>
		public void Collapse()
		{
			IsCollapsed = true;
		}

		/// <summary>
		/// This method is used to expand the Collapse Button.
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
		/// Retrieves a list of Widgets that are affected when the state of the provided CollapseButton is changed.
		/// This method was introduced to support nested CollapseButtons.
		/// </summary>
		/// <param name="collapseButton">CollapseButton that is checked.</param>
		/// <param name="collapse">Indicates if the top collapse button is going to be collapsed or expanded.</param>
		/// <returns>List of affected Widgets.</returns>
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
