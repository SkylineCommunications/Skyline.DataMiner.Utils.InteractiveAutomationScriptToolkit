namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Automation;

	/// <summary>
	///		A button that can be used to show/hide a collection of widgets.
	/// </summary>
	public class CollapseButton : InteractiveWidget, ICollapseButton
	{
		private string collapseText;
		private string expandText;

		private bool pressed;
		private bool isCollapsed;

		/// <summary>
		/// Initializes a new instance of the CollapseButton class.
		/// </summary>
		/// <param name="linkedWidgets">Widgets that are linked to this collapse button.</param>
		/// <param name="isCollapsed">State of the collapse button.</param>
		public CollapseButton(IEnumerable<IWidget> linkedWidgets, bool isCollapsed)
		{
			Type = UIBlockType.Button;
			LinkedWidgets = new List<IWidget>(linkedWidgets);
			CollapseText = "Collapse";
			ExpandText = "Expand";

			IsCollapsed = isCollapsed;

			WantsOnChange = true;
		}

		/// <inheritdoc />
		public CollapseButton(bool isCollapsed = false) : this(Array.Empty<IWidget>(), isCollapsed)
		{
		}

		/// <inheritdoc />
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

		/// <inheritdoc />
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
				foreach (IWidget widget in GetAffectedWidgets(this, value))
				{
					widget.IsVisible = !value;
				}
			}
		}

		/// <inheritdoc />
		public string CollapseText
		{
			get
			{
				return collapseText;
			}

			set
			{
				if (String.IsNullOrWhiteSpace(value))
				{
					throw new ArgumentException("The Collapse text cannot be empty.");
				}

				collapseText = value;
				if (!IsCollapsed)
				{
					BlockDefinition.Text = collapseText;
				}
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
		public string ExpandText
		{
			get
			{
				return expandText;
			}

			set
			{
				if (String.IsNullOrWhiteSpace(value))
				{
					throw new ArgumentException("The Expand text cannot be empty.");
				}

				expandText = value;
				if (IsCollapsed)
				{
					BlockDefinition.Text = expandText;
				}
			}
		}

		/// <inheritdoc />
		public List<IWidget> LinkedWidgets { get; private set; }

		/// <inheritdoc />
		public void Collapse()
		{
			IsCollapsed = true;
		}

		/// <inheritdoc />
		public void Expand()
		{
			IsCollapsed = false;
		}

		/// <inheritdoc />
		protected internal override void LoadResult(UIResults uiResults)
		{
			pressed = uiResults.WasCollapseButtonPressed(this);
		}

		/// <inheritdoc />
		protected internal override void RaiseResultEvents()
		{
			if (pressed)
			{
				IsCollapsed = !IsCollapsed;
				if (OnPressed != null)
				{
					OnPressed(this, EventArgs.Empty);
				}
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
		private static List<IWidget> GetAffectedWidgets(ICollapseButton collapseButton, bool collapse)
		{
			List<IWidget> affectedWidgets = new List<IWidget>();
			affectedWidgets.AddRange(collapseButton.LinkedWidgets);

			var nestedCollapseButtons = collapseButton.LinkedWidgets.OfType<ICollapseButton>();
			foreach (ICollapseButton nestedCollapseButton in nestedCollapseButtons)
			{
				if (collapse)
				{
					// Collapsing top collapse button
					affectedWidgets.AddRange(GetAffectedWidgets(nestedCollapseButton, true));
					continue;
				}

				if (!nestedCollapseButton.IsCollapsed)
				{
					// Expanding top collapse button
					affectedWidgets.AddRange(GetAffectedWidgets(nestedCollapseButton, false));
				}
			}

			return affectedWidgets;
		}
	}
}
