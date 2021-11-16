namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
using Skyline.DataMiner.Automation;
using Skyline.DataMiner.Net.AutomationUI.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
///<summary>
///Specifies the horizontal alignment of a widget added to a dialog or section.
///</summary>
public enum HorizontalAlignment
{
		/// <summary>
		/// Specifies that the widget will be centered across its assigned cell(s).
		/// </summary>
		Center,
		/// <summary>
		/// Specifies that the widget will be aligned to the left across its assigned cell(s).
		/// </summary>
		Left,
		/// <summary>
		/// Specifies that the widget will be aligned to the right across its assigned cell(s).
		/// </summary>
		Right,
		/// <summary>
		/// Specifies that the widget will be stretched horizontally across its assigned cell(s).
		/// </summary>
		Stretch
	}

///<summary>
///Style of the displayed text.
///</summary>
public enum TextStyle
{
		/// <summary>
		/// Default value, no explicit styling.
		/// </summary>
		None = 0,
		/// <summary>
		/// Text should be styled as a title.
		/// </summary>
		Title = 1,
		/// <summary>
		/// Text should be styled in bold.
		/// </summary>
		Bold = 2,
		/// <summary>
		/// Text should be styled as a heading.
		/// </summary>
		Heading = 3
	}

///<summary>
///Specifies the vertical alignment of a widget added to a dialog or section.
///</summary>
public enum VerticalAlignment
{
		/// <summary>
		/// Specifies that the widget will be centered vertically across its assigned cell(s).
		/// </summary>
		Center,
		/// <summary>
		/// Specifies that the widget will be aligned to the top of its assigned cell(s).
		/// </summary>
		Top,
		/// <summary>
		/// Specifies that the widget will be aligned to the bottom of its assigned cell(s).
		/// </summary>
		Bottom,
		/// <summary>
		/// Specifies that the widget will be stretched vertically across its assigned cell(s).
		/// </summary>
		Stretch
	}

///<summary>
///Used to define the position of an item in a grid layout.
///</summary>
public interface ILayout
{
		/// <summary>
		///     Gets the column location of the widget on the grid.
		/// </summary>
		/// <remarks>The top-left position is (0, 0) by default.</remarks>
		int Column { get; }

		/// <summary>
		///     Gets the row location of the widget on the grid.
		/// </summary>
		/// <remarks>The top-left position is (0, 0) by default.</remarks>
		int Row { get; }
	}

///<summary>
///Used to define the position of a widget in a grid layout.
///</summary>
public interface IWidgetLayout : ILayout
{
		/// <summary>
		///     Gets how many columns the widget spans on the grid.
		/// </summary>
		int ColumnSpan { get; }

		/// <summary>
		///     Gets or sets the horizontal alignment of the widget.
		/// </summary>
		HorizontalAlignment HorizontalAlignment { get; set; }

		/// <summary>
		///     Gets or sets the margin around the widget.
		/// </summary>
		/// <exception cref="ArgumentNullException">When the value is null.</exception>
		Margin Margin { get; set; }

		/// <summary>
		///     Gets how many rows the widget spans on the grid.
		/// </summary>
		int RowSpan { get; }

		/// <summary>
		///     Gets or sets the vertical alignment of the widget.
		/// </summary>
		VerticalAlignment VerticalAlignment { get; set; }
	}

///<summary>
///A button that can be pressed.
///</summary>
public class Button : InteractiveWidget
{
		private bool pressed;

		/// <summary>
		///     Initializes a new instance of the <see cref="Button" /> class.
		/// </summary>
		/// <param name="text">Text displayed in the button.</param>
		public Button(string text)
		{
			Type = UIBlockType.Button;
			Text = text;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Button" /> class.
		/// </summary>
		public Button() : this(String.Empty)
		{
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
		///     Triggered when the button is pressed.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<EventArgs> Pressed
		{
			add
			{
				OnPressed += value;
				WantsOnChange = true;
			}

			remove
			{
				OnPressed -= value;
				if(OnPressed == null || !OnPressed.GetInvocationList().Any())
				{
					WantsOnChange = false;
				}
			}
		}

		private event EventHandler<EventArgs> OnPressed;

		/// <summary>
		///     Gets or sets the text displayed in the button.
		/// </summary>
		public string Text
		{
			get
			{
				return BlockDefinition.Text;
			}

			set
			{
				BlockDefinition.Text = value;
			}
		}

		internal override void LoadResult(UIResults uiResults)
		{
			pressed = uiResults.WasButtonPressed(this);
		}

		/// <inheritdoc />
		internal override void RaiseResultEvents()
		{
			if ((OnPressed != null) && pressed)
			{
				OnPressed(this, EventArgs.Empty);
			}

			pressed = false;
		}
	}

///<summary>
///Widget to show/edit a datetime.
///</summary>
public class Calendar : InteractiveWidget
{
		private bool changed;
		private DateTime dateTime;
		private DateTime previous;
		private bool displayServerTime = false;

		/// <summary>
		///     Initializes a new instance of the <see cref="Calendar" /> class.
		/// </summary>
		/// <param name="dateTime">Value displayed on the calendar.</param>
		public Calendar(DateTime dateTime)
		{
			Type = UIBlockType.Calendar;
			DateTime = dateTime;
			ValidationText = "Invalid Input";
			ValidationState = UIValidationState.NotValidated;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Calendar" /> class.
		/// </summary>
		public Calendar() : this(DateTime.Now)
		{
		}

		/// <summary>
		///     Triggered when a different datetime is picked.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<CalendarChangedEventArgs> Changed
		{
			add
			{
				OnChanged += value;
				WantsOnChange = true;
			}

			remove
			{
				OnChanged -= value;
				if (OnChanged == null || !OnChanged.GetInvocationList().Any())
				{
					WantsOnChange = false;
				}
			}
		}

		private event EventHandler<CalendarChangedEventArgs> OnChanged;

		/// <summary>
		///		Gets or sets whether the displayed time is the server time or local time.
		/// </summary>
		public bool DisplayServerTime
		{
			get
			{
				return displayServerTime;
			}

			set
			{
				displayServerTime = value;
				DateTime = dateTime;
			}
		}

		/// <summary>
		///     Gets or sets the datetime displayed on the calendar.
		/// </summary>
		public DateTime DateTime
		{
			get
			{
				return dateTime;
			}

			set
			{
				dateTime = value;
				if (DisplayServerTime)
				{
					BlockDefinition.InitialValue = value.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
				}
				else
				{
					BlockDefinition.InitialValue = value.ToString(AutomationConfigOptions.GlobalDateTimeFormat, CultureInfo.InvariantCulture);
				}
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
		///		Gets or sets the state indicating if a given input field was validated or not and if the validation was valid.
		///		This should be used by the client to add a visual marker on the input field.
		/// </summary>
		/// <remarks>Available from DataMiner 10.0.5 onwards.</remarks>
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
		/// <remarks>Available from DataMiner 10.0.5 onwards.</remarks>
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
			string isoString = uiResults.GetString(DestVar);
			DateTime result = DateTime.Parse(isoString);

			if (WantsOnChange && (result != DateTime))
			{
				changed = true;
				previous = DateTime;
			}

			DateTime = result;
		}

		/// <inheritdoc />
		internal override void RaiseResultEvents()
		{
			if (changed && OnChanged != null)
			{
				OnChanged(this, new CalendarChangedEventArgs(DateTime, previous));
			}

			changed = false;
		}

		/// <summary>
		///     Provides data for the <see cref="Changed" /> event.
		/// </summary>
		public class CalendarChangedEventArgs : EventArgs
		{
			internal CalendarChangedEventArgs(DateTime dateTime, DateTime previous)
			{
				DateTime = dateTime;
				Previous = previous;
			}

			/// <summary>
			///     Gets the new datetime value.
			/// </summary>
			public DateTime DateTime { get; private set; }

			/// <summary>
			///     Gets the previous datetime value.
			/// </summary>
			public DateTime Previous { get; private set; }
		}
	}

///<summary>
///A checkbox that can be selected or cleared.
///</summary>
public class CheckBox : InteractiveWidget
{
		private bool changed;
		private bool isChecked;

		/// <summary>
		///     Initializes a new instance of the <see cref="CheckBox" /> class.
		/// </summary>
		/// <param name="text">Text displayed next to the checkbox.</param>
		public CheckBox(string text)
		{
			Type = UIBlockType.CheckBox;
			IsChecked = false;
			Text = text;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="CheckBox" /> class.
		/// </summary>
		public CheckBox() : this(String.Empty)
		{
		}

		/// <summary>
		///     Triggered when the state of the checkbox changes.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<CheckBoxChangedEventArgs> Changed
		{
			add
			{
				OnChanged += value;
				WantsOnChange = true;
			}

			remove
			{
				OnChanged -= value;
				bool noOnChangedEvents = OnChanged == null || !OnChanged.GetInvocationList().Any();
				bool noOnCheckedEvents = OnChecked == null || !OnChecked.GetInvocationList().Any();
				bool noOnUnCheckedEvents = OnUnChecked == null || !OnUnChecked.GetInvocationList().Any();

				if (noOnChangedEvents && noOnCheckedEvents && noOnUnCheckedEvents)
				{
					WantsOnChange = false;
				}
			}
		}

		/// <summary>
		///     Triggered when the checkbox is selected.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<EventArgs> Checked
		{
			add
			{
				OnChecked += value;
				WantsOnChange = true;
			}

			remove
			{
				OnChecked -= value;
				bool noOnChangedEvents = OnChanged == null || !OnChanged.GetInvocationList().Any();
				bool noOnCheckedEvents = OnChecked == null || !OnChecked.GetInvocationList().Any();
				bool noOnUnCheckedEvents = OnUnChecked == null || !OnUnChecked.GetInvocationList().Any();

				if (noOnChangedEvents && noOnCheckedEvents && noOnUnCheckedEvents)
				{
					WantsOnChange = false;
				}
			}
		}

		/// <summary>
		///     Triggered when the checkbox is cleared.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<EventArgs> UnChecked
		{
			add
			{
				OnUnChecked += value;
				WantsOnChange = true;
			}

			remove
			{
				OnUnChecked -= value;
				bool noOnChangedEvents = OnChanged == null || !OnChanged.GetInvocationList().Any();
				bool noOnCheckedEvents = OnChecked == null || !OnChecked.GetInvocationList().Any();
				bool noOnUnCheckedEvents = OnUnChecked == null || !OnUnChecked.GetInvocationList().Any();

				if (noOnChangedEvents && noOnCheckedEvents && noOnUnCheckedEvents)
				{
					WantsOnChange = false;
				}
			}
		}

		private event EventHandler<CheckBoxChangedEventArgs> OnChanged;

		private event EventHandler<EventArgs> OnChecked;

		private event EventHandler<EventArgs> OnUnChecked;

		/// <summary>
		///     Gets or sets a value indicating whether the checkbox is selected.
		/// </summary>
		public bool IsChecked
		{
			get
			{
				return isChecked;
			}

			set
			{
				isChecked = value;
				BlockDefinition.InitialValue = value.ToString();
			}
		}

		/// <summary>
		///     Gets or sets the displayed text next to the checkbox.
		/// </summary>
		public string Text
		{
			get
			{
				return BlockDefinition.Text;
			}

			set
			{
				BlockDefinition.Text = value;
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


		internal override void LoadResult(UIResults uiResults)
		{
			bool result = uiResults.GetChecked(this);
			if (WantsOnChange)
			{
				changed = result != IsChecked;
			}

			IsChecked = result;
		}

		/// <inheritdoc />
		internal override void RaiseResultEvents()
		{
			if (!changed)
			{
				return;
			}

			if (OnChanged != null)
			{
				OnChanged(this, new CheckBoxChangedEventArgs(IsChecked));
			}

			if ((OnChecked != null) && IsChecked)
			{
				OnChecked(this, EventArgs.Empty);
			}

			if ((OnUnChecked != null) && !IsChecked)
			{
				OnUnChecked(this, EventArgs.Empty);
			}

			changed = false;
		}

		/// <summary>
		///     Provides data for the <see cref="Changed" /> event.
		/// </summary>
		public class CheckBoxChangedEventArgs : EventArgs
		{
			internal CheckBoxChangedEventArgs(bool isChecked)
			{
				IsChecked = isChecked;
			}

			/// <summary>
			///     Gets a value indicating whether the checkbox has been checked.
			/// </summary>
			public bool IsChecked { get; private set; }
		}
	}

///<summary>
///A list of checkboxes.
///</summary>
public class CheckBoxList : InteractiveWidget
{
		private readonly IDictionary<string, bool> options = new Dictionary<string, bool>();
		private bool changed;
		private string changedOption;
		private bool changedValue;

		/// <summary>
		///     Initializes a new instance of the <see cref="CheckBoxList" /> class.
		/// </summary>
		public CheckBoxList() : this(Enumerable.Empty<string>())
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="CheckBoxList" /> class.
		/// </summary>
		/// <param name="options">Name of options that can be selected.</param>
		/// <exception cref="ArgumentNullException">When options is null.</exception>
		public CheckBoxList(IEnumerable<string> options)
		{
			Type = UIBlockType.CheckBoxList;
			SetOptions(options);
			ValidationText = "Invalid Input";
			ValidationState = UIValidationState.NotValidated;
		}

		/// <summary>
		///     Triggered when the state of a checkbox changes.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<CheckBoxListChangedEventArgs> Changed
		{
			add
			{
				OnChanged += value;
				WantsOnChange = true;
			}

			remove
			{
				OnChanged -= value;
				if(OnChanged == null || !OnChanged.GetInvocationList().Any())
				{
					WantsOnChange = false;
				}
			}
		}

		private event EventHandler<CheckBoxListChangedEventArgs> OnChanged;

		/// <summary>
		///     Gets all selected options.
		/// </summary>
		public IEnumerable<string> Checked
		{
			get
			{
				return options.Where(option => option.Value).Select(option => option.Key);
			}
		}

		/// <summary>
		///     Gets or sets a value indicating whether the options are sorted naturally.
		/// </summary>
		/// <remarks>Available from DataMiner 9.5.6 onwards.</remarks>
		public bool IsSorted
		{
			get
			{
				return BlockDefinition.IsSorted;
			}

			set
			{
				BlockDefinition.IsSorted = value;
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
		///     Gets all options.
		/// </summary>
		public IEnumerable<string> Options
		{
			get
			{
				return options.Keys;
			}
		}

		/// <summary>
		///     Gets all options that are not selected.
		/// </summary>
		public IEnumerable<string> Unchecked
		{
			get
			{
				return options.Where(option => !option.Value).Select(option => option.Key);
			}
		}

		/// <summary>
		///		Gets or sets the state indicating if a given input field was validated or not and if the validation was valid.
		///		This should be used by the client to add a visual marker on the input field.
		/// </summary>
		/// <remarks>Available from DataMiner 10.0.5 onwards.</remarks>
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
		///		The validation text is not displayed for a checkbox list, but if this value is not explicitly set, the validation state will have no influence on the way the component is displayed.
		/// </summary>
		/// <remarks>Available from DataMiner 10.0.5 onwards.</remarks>
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

		/// <summary>
		///     Adds an option to the checkbox list.
		/// </summary>
		/// <param name="option">Option to add.</param>
		/// <exception cref="ArgumentNullException">When options is null.</exception>
		public void AddOption(string option)
		{
			if (option == null)
			{
				throw new ArgumentNullException("option");
			}

			if (!options.ContainsKey(option))
			{
				options.Add(option, false);
				BlockDefinition.AddCheckBoxListOption(option);
			}
		}

		/// <summary>
		///     Selects an option.
		/// </summary>
		/// <param name="option">Option to be selected.</param>
		/// <exception cref="ArgumentNullException">When option is null.</exception>
		/// <exception cref="ArgumentException">When the option does not exist.</exception>
		public void Check(string option)
		{
			if (option == null)
			{
				throw new ArgumentNullException("option");
			}

			if (!options.ContainsKey(option))
			{
				throw new ArgumentException("CheckboxList does not have option: " + option, option);
			}

			if (!options[option])
			{
				options[option] = true;
				BlockDefinition.InitialValue = String.Join(";", BlockDefinition.InitialValue, option);
			}
		}

		/// <summary>
		///     Selects all options.
		/// </summary>
		public void CheckAll()
		{
			foreach (string option in options.Keys.ToList())
			{
				options[option] = true;
			}

			BlockDefinition.InitialValue = String.Join(";", options.Keys);
		}

		/// <summary>
		///     Sets the displayed options.
		///     Replaces existing options.
		/// </summary>
		/// <param name="options">Options to set.</param>
		/// <exception cref="ArgumentNullException">When options is null.</exception>
		public void SetOptions(IEnumerable<string> options)
		{
			ClearOptions();
			foreach (string option in options)
			{
				AddOption(option);
			}
		}

		/// <summary>
		/// 	Removes an option from the checkbox list.
		/// </summary>
		/// <param name="option">Option to remove.</param>
		/// <exception cref="NullReferenceException">When option is null.</exception>
		public void RemoveOption(string option)
		{
			if (option == null)
			{
				throw new ArgumentNullException("option");
			}

			if (options.Remove(option))
			{
				RecreateUiBlock();
				foreach (string optionsKey in options.Keys)
				{
					BlockDefinition.AddCheckBoxListOption(optionsKey);
				}
			}
		}

		/// <summary>
		///     Clears an option.
		/// </summary>
		/// <param name="option">Option to be cleared.</param>
		/// <exception cref="ArgumentNullException">When option is null.</exception>
		/// <exception cref="ArgumentException">When the option does not exist.</exception>
		public void Uncheck(string option)
		{
			if (option == null)
			{
				throw new ArgumentNullException("option");
			}

			if (!options.ContainsKey(option))
			{
				throw new ArgumentException("CheckboxList does not have option: " + option, option);
			}

			if (options[option])
			{
				options[option] = false;
				BlockDefinition.InitialValue = String.Join(";", Checked);
			}
		}

		/// <summary>
		///     Clears all options.
		/// </summary>
		public void UncheckAll()
		{
			foreach (string option in options.Keys.ToList())
			{
				options[option] = false;
			}

			BlockDefinition.InitialValue = null;
		}

		internal override void LoadResult(UIResults uiResults)
		{
			string results = uiResults.GetString(this);
			if (results == null)
			{
				// Results could be null when you have an empty list of options
				BlockDefinition.InitialValue = String.Empty;
				return;
			}
				
			var checkedOptions = new HashSet<string>(results.Split(';'));
			foreach (string option in options.Keys.ToList())
			{
				bool isChecked = checkedOptions.Contains(option);
				bool hasChanged = options[option] != isChecked;

				options[option] = isChecked;

				if (hasChanged && WantsOnChange)
				{
					changed = true;
					changedOption = option;
					changedValue = isChecked;

					// only a single checkbox can be toggled when WantsOnChange is true
					break;
				}
			}

			BlockDefinition.InitialValue = String.Join(";", Checked);
		}

		/// <inheritdoc />
		internal override void RaiseResultEvents()
		{
			if (changed && (OnChanged != null))
			{
				OnChanged(this, new CheckBoxListChangedEventArgs(changedOption, changedValue));
			}

			changed = false;
		}

		private void ClearOptions()
		{
			options.Clear();
			RecreateUiBlock();
			BlockDefinition.InitialValue = null;
		}

		/// <summary>
		///     Provides data for the <see cref="Changed" /> event.
		/// </summary>
		public class CheckBoxListChangedEventArgs : EventArgs
		{
			internal CheckBoxListChangedEventArgs(string option, bool isChecked)
			{
				Option = option;
				IsChecked = isChecked;
			}

			/// <summary>
			///     Gets a value indicating whether the checkbox has been selected.
			/// </summary>
			public bool IsChecked { get; private set; }

			/// <summary>
			///     Gets the option of which the state has changed.
			/// </summary>
			public string Option { get; private set; }
		}
	}

///<summary>
///A button that can be used to show/hide a collection of widgets.
///</summary>
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

///<summary>
///Custom control used to display a date. Make sure that <see cref="Dialog.AllowOverlappingWidgets"/> is set to true when you use this control.
///</summary>
public class DatePicker : Section
{
		private readonly Dictionary<int, string> months = new Dictionary<int, string>()
		                                                  {
			                                                  { 1, "Jan" },
			                                                  { 2, "Feb" },
			                                                  { 3, "Mar" },
			                                                  { 4, "Apr" },
			                                                  { 5, "May" },
			                                                  { 6, "Jun" },
			                                                  { 7, "Jul" },
			                                                  { 8, "Aug" },
			                                                  { 9, "Sep" },
			                                                  { 10, "Oct" },
			                                                  { 11, "Nov" },
			                                                  { 12, "Dec" },
		                                                  };

		private readonly Numeric dayNumeric;

		private readonly DropDown monthDropDown;

		private readonly Numeric yearNumeric;

		private DateTime previous;

		/// <summary>
		/// Initializes a new instance of the DatePicker class.
		/// </summary>
		public DatePicker() : this(DateTime.Now)
		{
		}

		/// <summary>
		/// Initializes a new instance of the DatePicker class.
		/// </summary>
		/// <param name="dateTime"></param>
		public DatePicker(DateTime dateTime)
		{
			previous = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);

			dayNumeric = new Numeric(dateTime.Day)
			             {
				             Decimals = 0,
				             Minimum = 1,
				             Maximum = DateTime.DaysInMonth(dateTime.Year, dateTime.Month),
				             Width = 70,
				             Margin = new Margin(3,3,3,3),
			             };

			monthDropDown = new DropDown(months.Select(x => x.Value), months[dateTime.Month])
			                {
				                Width = 80,
				                Margin = new Margin(3,3,3,3)
			                };

			yearNumeric = new Numeric(dateTime.Year)
			              {
				              Decimals = 0,
				              Minimum = 1800,
				              Maximum = 9999,
				              Width = 80,
				              Margin = new Margin(3,3,3,3)
			              };

			dayNumeric.Changed += DayNumeric_OnChanged;
			monthDropDown.Changed += MonthDropDown_OnChanged;
			yearNumeric.Changed += YearNumeric_OnChanged;

			AddWidget(dayNumeric, 0, 0);
			AddWidget(monthDropDown, 0, 0);
			AddWidget(yearNumeric, 0, 0);
		}

		/// <summary>
		///     Triggered when a different date is picked.
		/// </summary>
		public event EventHandler<DatePickerChangedEventArgs> Changed
		{
			add
			{
				OnChanged += value;
			}

			remove
			{
				OnChanged -= value;
			}
		}

		private event EventHandler<DatePickerChangedEventArgs> OnChanged;

		/// <summary>
		/// Date that is displayed by the control.
		/// </summary>
		public DateTime Date
		{
			get
			{
				return new DateTime((int)yearNumeric.Value, months.First(x => x.Value == monthDropDown.Selected).Key, (int)dayNumeric.Value);
			}

			set
			{
				dayNumeric.Value = value.Day;
				dayNumeric.Maximum = DateTime.DaysInMonth(value.Year, value.Month);
				monthDropDown.Selected = months[value.Month];
				yearNumeric.Value = value.Year;

				previous = new DateTime(value.Year, value.Month, value.Day);
			}
		}

		/// <summary>
		/// Day that is displayed by the control.
		/// </summary>
		public int Day
		{
			get
			{
				return (int)dayNumeric.Value;
			}
		}

		/// <summary>
		/// Month that is displayed by the control.
		/// </summary>
		public int Month
		{
			get
			{
				return months.First(x => x.Value == monthDropDown.Selected).Key;
			}
		}

		/// <summary>
		/// Year that is displayed by the control.
		/// </summary>
		public int Year
		{
			get
			{
				return (int)yearNumeric.Value;
			}
		}

		/// <summary>
		/// Numeric widget that holds the day in this control.
		/// </summary>
		public Numeric DayNumeric
		{
			get
			{
				return dayNumeric;
			}
		}

		/// <summary>
		/// Drop down widget that holds the month in this control.
		/// </summary>
		public DropDown MonthDropDown
		{
			get
			{
				return monthDropDown;
			}
		}

		/// <summary>
		/// Numeric widget that holds the year in this control.
		/// </summary>
		public Numeric YearNumeric
		{
			get
			{
				return yearNumeric;
			}
		}

		private void DayNumeric_OnChanged(object sender, Numeric.NumericChangedEventArgs e)
		{
			RaiseEventResults();
		}

		private void MonthDropDown_OnChanged(object sender, DropDown.DropDownChangedEventArgs e)
		{
			dayNumeric.Maximum = DateTime.DaysInMonth(Year, Month);
			RaiseEventResults();
		}

		private void YearNumeric_OnChanged(object sender, Numeric.NumericChangedEventArgs e)
		{
			dayNumeric.Maximum = DateTime.DaysInMonth(Year, Month);
			RaiseEventResults();
		}

		private void RaiseEventResults()
		{
			if (Date == previous) return;

			if (OnChanged != null)
			{
				OnChanged(this, new DatePickerChangedEventArgs(Date, previous));
			}

			previous = Date;
		}

		/// <summary>
		///     Provides data for the <see cref="Changed" /> event.
		/// </summary>
		public class DatePickerChangedEventArgs : EventArgs
		{
			internal DatePickerChangedEventArgs(DateTime dateTime, DateTime previous)
			{
				DateTime = dateTime;
				Previous = previous;
			}

			/// <summary>
			///     Gets the new date value.
			/// </summary>
			public DateTime DateTime { get; private set; }

			/// <summary>
			///     Gets the previous date value.
			/// </summary>
			public DateTime Previous { get; private set; }
		}
	}

///<summary>
///Widget to show/edit a datetime.
///</summary>
public class DateTimePicker : TimePickerBase
{
		private readonly AutomationDateTimePickerOptions dateTimePickerOptions;

		private bool changed;
		private DateTime dateTime;
		private DateTime previous;
		private bool displayServerTime = false;

		/// <summary>
		///     Initializes a new instance of the <see cref="DateTimePicker" /> class.
		/// </summary>
		/// <param name="dateTime">Value displayed in the datetime picker.</param>
		public DateTimePicker(DateTime dateTime) : base(new AutomationDateTimePickerOptions())
		{
			Type = UIBlockType.Time;
			DateTime = dateTime;
			dateTimePickerOptions = (AutomationDateTimePickerOptions)DateTimeUpDownOptions;
			ValidationText = "Invalid Input";
			ValidationState = UIValidationState.NotValidated;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="DateTimePicker" /> class.
		/// </summary>
		public DateTimePicker() : this(DateTime.Now)
		{
		}

		/// <summary>
		///     Triggered when a different datetime is picked.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<DateTimePickerChangedEventArgs> Changed
		{
			add
			{
				OnChanged += value;
				WantsOnChange = true;
			}

			remove
			{
				OnChanged -= value;
				if(OnChanged == null || !OnChanged.GetInvocationList().Any())
				{
					WantsOnChange = false;
				}
			}
		}

		private event EventHandler<DateTimePickerChangedEventArgs> OnChanged;

		/// <summary>
		///		Gets or sets whether the displayed time is the server time or local time.
		/// </summary>
		public bool DisplayServerTime
		{
			get
			{
				return displayServerTime;
			}

			set
			{
				displayServerTime = value;
				DateTime = dateTime;
			}
		}

		/// <summary>
		///     Gets or sets the datetime displayed in the datetime picker.
		/// </summary>
		public DateTime DateTime
		{
			get
			{
				return dateTime;
			}

			set
			{
				dateTime = value;
				if (DisplayServerTime)
				{
					BlockDefinition.InitialValue = value.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
				}
				else
				{
					BlockDefinition.InitialValue = value.ToString(AutomationConfigOptions.GlobalDateTimeFormat, CultureInfo.InvariantCulture);
				}
			}
		}

		/// <summary>
		///     Gets or sets a value indicating whether the calendar pop-up will close when the user clicks a new date.
		/// </summary>
		public bool AutoCloseCalendar
		{
			get
			{
				return dateTimePickerOptions.AutoCloseCalendar;
			}

			set
			{
				dateTimePickerOptions.AutoCloseCalendar = value;
			}
		}

		/// <summary>
		///     Gets or sets the maximum timestamp.
		/// </summary>
		public DateTime Maximum
		{
			get
			{
				return DateTimeUpDownOptions.Maximum ?? DateTime.MaxValue;
			}

			set
			{
				DateTimeUpDownOptions.Maximum = value;
			}
		}

		/// <summary>
		///     Gets or sets the minimum timestamp.
		/// </summary>
		public DateTime Minimum
		{
			get
			{
				return DateTimeUpDownOptions.Minimum ?? DateTime.MinValue;
			}

			set
			{
				DateTimeUpDownOptions.Minimum = value;
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
		///     Gets or sets the display mode of the calendar inside the date-time picker control.
		///     Default: <c>CalendarMode.Month</c>
		/// </summary>
		public CalendarMode CalendarDisplayMode
		{
			get
			{
				return dateTimePickerOptions.CalendarDisplayMode;
			}

			set
			{
				dateTimePickerOptions.CalendarDisplayMode = value;
			}
		}

		/// <summary>
		///     Gets or sets a value indicating whether the calendar control drop-down button is shown.
		///     Default: <c>true</c>
		/// </summary>
		public bool HasDropDownButton
		{
			get
			{
				return dateTimePickerOptions.ShowDropDownButton;
			}

			set
			{
				dateTimePickerOptions.ShowDropDownButton = value;
			}
		}

		/// <summary>
		///     Gets or sets a value indicating whether the time picker is shown within the calender control.
		///     Default: <c>true</c>
		/// </summary>
		public bool IsTimePickerVisible
		{
			get
			{
				return dateTimePickerOptions.TimePickerVisible;
			}

			set
			{
				dateTimePickerOptions.TimePickerVisible = value;
			}
		}

		/// <summary>
		///     Gets or sets a value indicating whether the spin box of the calender control is shown.
		///     Default: <c>true</c>
		/// </summary>
		public bool HasTimePickerSpinnerButton
		{
			get
			{
				return dateTimePickerOptions.TimePickerShowButtonSpinner;
			}

			set
			{
				dateTimePickerOptions.TimePickerShowButtonSpinner = value;
			}
		}

		/// <summary>
		///     Gets or sets a value indicating whether the spin box of the calender is enabled.
		///     Default: <c>true</c>
		/// </summary>
		public bool IsTimePickerSpinnerButtonEnabled
		{
			get
			{
				return dateTimePickerOptions.TimePickerAllowSpin;
			}

			set
			{
				dateTimePickerOptions.TimePickerAllowSpin = value;
			}
		}

		/// <summary>
		///     Gets or sets the time format of the time picker.
		///     Default: <c>DateTimeFormat.ShortTime</c>
		/// </summary>
		public DateTimeFormat TimeFormat
		{
			get
			{
				return dateTimePickerOptions.TimeFormat;
			}

			set
			{
				dateTimePickerOptions.TimeFormat = value;
			}
		}

		/// <summary>
		///     Gets or sets the time format string used when TimeFormat is set to <c>DateTimeFormat.Custom</c>.
		/// </summary>
		/// <remarks>Sets <see cref="TimeFormat" /> to <c>DateTimeFormat.Custom</c></remarks>
		public string CustomTimeFormat
		{
			get
			{
				return dateTimePickerOptions.TimeFormatString;
			}

			set
			{
				TimeFormat = DateTimeFormat.Custom;
				dateTimePickerOptions.TimeFormatString = value;
			}
		}

		/// <summary>
		///		Gets or sets the state indicating if a given input field was validated or not and if the validation was valid.
		///		This should be used by the client to add a visual marker on the input field.
		/// </summary>
		/// <remarks>Available from DataMiner 10.0.5 onwards.</remarks>
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
		/// <remarks>Available from DataMiner 10.0.5 onwards.</remarks>
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
			string isoString = uiResults.GetString(DestVar);
			DateTime result = DateTime.Parse(isoString);

			if (WantsOnChange && (result != DateTime))
			{
				changed = true;
				previous = DateTime;
			}

			DateTime = result;
		}

		/// <inheritdoc />
		internal override void RaiseResultEvents()
		{
			if (changed && OnChanged != null)
			{
				OnChanged(this, new DateTimePickerChangedEventArgs(DateTime, previous));
			}

			changed = false;
		}

		/// <summary>
		///     Provides data for the <see cref="Changed" /> event.
		/// </summary>
		public class DateTimePickerChangedEventArgs : EventArgs
		{
			internal DateTimePickerChangedEventArgs(DateTime dateTime, DateTime previous)
			{
				DateTime = dateTime;
				Previous = previous;
			}

			/// <summary>
			///     Gets the new datetime value.
			/// </summary>
			public DateTime DateTime { get; private set; }

			/// <summary>
			///     Gets the previous datetime value.
			/// </summary>
			public DateTime Previous { get; private set; }
		}
	}

///<summary>
///A dialog represents a single window that can be shown.
///You can show widgets in the window by adding them to the dialog.
///The dialog uses a grid to determine the layout of its widgets.
///</summary>
public abstract class Dialog
{
		private const string Auto = "auto";
		private const string Stretch = "*";

		private readonly Dictionary<Widget, IWidgetLayout> widgetLayouts = new Dictionary<Widget, IWidgetLayout>();

		private readonly Dictionary<int, string> columnDefinitions = new Dictionary<int, string>();
		private readonly Dictionary<int, string> rowDefinitions = new Dictionary<int, string>();

		private int height;
		private int maxHeight;
		private int maxWidth;
		private int minHeight;
		private int minWidth;
		private int width;
		private bool isEnabled = true;

		/// <summary>
		/// Initializes a new instance of the <see cref="Dialog" /> class.
		/// </summary>
		/// <param name="engine"></param>
		protected Dialog(IEngine engine)
		{
			if (engine == null)
			{
				throw new ArgumentNullException("engine");
			}

			Engine = engine;
			width = -1;
			height = -1;
			MaxHeight = Int32.MaxValue;
			MinHeight = 1;
			MaxWidth = Int32.MaxValue;
			MinWidth = 1;
			RowCount = 0;
			ColumnCount = 0;
			Title = "Dialog";
			AllowOverlappingWidgets = false;
		}

		/// <summary>
		/// Gets or sets a value indicating whether overlapping widgets are allowed or not.
		/// Can be used in case you want to add multiple widgets to the same cell in the dialog.
		/// You can use the Margin property on the widgets to place them apart.
		/// </summary>
		public bool AllowOverlappingWidgets { get; set; }

		/// <summary>
		///     Triggered when the back button of the dialog is pressed.
		/// </summary>
		public event EventHandler<EventArgs> Back;

		/// <summary>
		///     Triggered when the forward button of the dialog is pressed.
		/// </summary>
		public event EventHandler<EventArgs> Forward;

		/// <summary>
		///     Triggered when there is any user interaction.
		/// </summary>
		public event EventHandler<EventArgs> Interacted;

		/// <summary>
		///     Gets the number of columns of the grid layout.
		/// </summary>
		public int ColumnCount { get; private set; }

		/// <summary>
		///     Gets the link to the SLAutomation process.
		/// </summary>
		public IEngine Engine { get; private set; }

		/// <summary>
		///     Gets or sets the fixed height (in pixels) of the dialog.
		/// </summary>
		/// <remarks>
		///     The user will still be able to resize the window,
		///     but scrollbars will appear immediately.
		///     <see cref="MinHeight" /> should be used instead as it has a more desired effect.
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
		public int Height
		{
			get
			{
				return height;
			}

			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				height = value;
			}
		}

		/// <summary>
		///     Gets or sets the maximum height (in pixels) of the dialog.
		/// </summary>
		/// <remarks>
		///     The user will still be able to resize the window past this limit.
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
		public int MaxHeight
		{
			get
			{
				return maxHeight;
			}

			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				maxHeight = value;
			}
		}

		/// <summary>
		///     Gets or sets the maximum width (in pixels) of the dialog.
		/// </summary>
		/// <remarks>
		///     The user will still be able to resize the window past this limit.
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
		public int MaxWidth
		{
			get
			{
				return maxWidth;
			}

			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				maxWidth = value;
			}
		}

		/// <summary>
		///     Gets or sets the minimum height (in pixels) of the dialog.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
		public int MinHeight
		{
			get
			{
				return minHeight;
			}

			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				minHeight = value;
			}
		}

		/// <summary>
		///     Gets or sets the minimum width (in pixels) of the dialog.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
		public int MinWidth
		{
			get
			{
				return minWidth;
			}

			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				minWidth = value;
			}
		}

		/// <summary>
		///     Gets the number of rows in the grid layout.
		/// </summary>
		public int RowCount { get; private set; }

		/// <summary>
		///		Gets or sets a value indicating whether the interactive widgets within the dialog are enabled or not.
		/// </summary>
		public bool IsEnabled
		{
			get
			{
				return isEnabled;
			}

			set
			{
				isEnabled = value;
				foreach (Widget widget in Widgets)
				{
					InteractiveWidget interactiveWidget = widget as InteractiveWidget;
					if (interactiveWidget != null && !(interactiveWidget is CollapseButton))
					{
						interactiveWidget.IsEnabled = isEnabled;
					}
				}
			}
		}

		/// <summary>
		///     Gets or sets the title at the top of the window.
		/// </summary>
		/// <remarks>Available from DataMiner 9.6.6 onwards.</remarks>
		public string Title { get; set; }

		/// <summary>
		///     Gets widgets that are added to the dialog.
		/// </summary>
		public IEnumerable<Widget> Widgets
		{
			get
			{
				return widgetLayouts.Keys;
			}
		}

		/// <summary>
		///     Gets or sets the fixed width (in pixels) of the dialog.
		/// </summary>
		/// <remarks>
		///     The user will still be able to resize the window,
		///     but scrollbars will appear immediately.
		///     <see cref="MinWidth" /> should be used instead as it has a more desired effect.
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
		public int Width
		{
			get
			{
				return width;
			}

			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				width = value;
			}
		}

		/// <summary>
		///     Adds a widget to the dialog.
		/// </summary>
		/// <param name="widget">Widget to add to the dialog.</param>
		/// <param name="widgetLayout">Location of the widget on the grid layout.</param>
		/// <returns>The dialog.</returns>
		/// <exception cref="ArgumentNullException">When the widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget has already been added to the dialog.</exception>
		public Dialog AddWidget(Widget widget, IWidgetLayout widgetLayout)
		{
			if (widget == null)
			{
				throw new ArgumentNullException("widget");
			}

			if (widgetLayouts.ContainsKey(widget))
			{
				throw new ArgumentException("Widget is already added to the dialog");
			}

			widgetLayouts.Add(widget, widgetLayout);

			SortedSet<int> rowsInUse;
			SortedSet<int> columnsInUse;
			this.FillRowsAndColumnsInUse(out rowsInUse, out columnsInUse);

			return this;
		}

		/// <summary>
		///     Adds a widget to the dialog.
		/// </summary>
		/// <param name="widget">Widget to add to the dialog.</param>
		/// <param name="row">Row location of widget on the grid.</param>
		/// <param name="column">Column location of the widget on the grid.</param>
		/// <param name="horizontalAlignment">Horizontal alignment of the widget.</param>
		/// <param name="verticalAlignment">Vertical alignment of the widget.</param>
		/// <returns>The dialog.</returns>
		/// <exception cref="ArgumentNullException">When the widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget has already been added to the dialog.</exception>
		public Dialog AddWidget(
			Widget widget,
			int row,
			int column,
			HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left,
			VerticalAlignment verticalAlignment = VerticalAlignment.Center)
		{
			AddWidget(widget, new WidgetLayout(row, column, horizontalAlignment, verticalAlignment));
			return this;
		}

		/// <summary>
		///     Adds a widget to the dialog.
		/// </summary>
		/// <param name="widget">Widget to add to the dialog.</param>
		/// <param name="fromRow">Row location of widget on the grid.</param>
		/// <param name="fromColumn">Column location of the widget on the grid.</param>
		/// <param name="rowSpan">Number of rows the widget will use.</param>
		/// <param name="colSpan">Number of columns the widget will use.</param>
		/// <param name="horizontalAlignment">Horizontal alignment of the widget.</param>
		/// <param name="verticalAlignment">Vertical alignment of the widget.</param>
		/// <returns>The dialog.</returns>
		/// <exception cref="ArgumentNullException">When the widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget has already been added to the dialog.</exception>
		public Dialog AddWidget(
			Widget widget,
			int fromRow,
			int fromColumn,
			int rowSpan,
			int colSpan,
			HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left,
			VerticalAlignment verticalAlignment = VerticalAlignment.Center)
		{
			AddWidget(
				widget,
				new WidgetLayout(fromRow, fromColumn, rowSpan, colSpan, horizontalAlignment, verticalAlignment));
			return this;
		}

		/// <summary>
		///     Gets the layout of the widget in the dialog.
		/// </summary>
		/// <param name="widget">A widget that is part of the dialog.</param>
		/// <returns>The widget layout in the dialog.</returns>
		/// <exception cref="NullReferenceException">When the widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget is not part of the dialog.</exception>
		public IWidgetLayout GetWidgetLayout(Widget widget)
		{
			CheckWidgetExists(widget);
			return widgetLayouts[widget];
		}

		/// <summary>
		///     Removes a widget from the dialog.
		/// </summary>
		/// <param name="widget">Widget to remove.</param>
		/// <exception cref="ArgumentNullException">When the widget is null.</exception>
		public void RemoveWidget(Widget widget)
		{
			if (widget == null)
			{
				throw new ArgumentNullException("widget");
			}

			widgetLayouts.Remove(widget);

			SortedSet<int> rowsInUse;
			SortedSet<int> columnsInUse;
			this.FillRowsAndColumnsInUse(out rowsInUse, out columnsInUse);
		}

		/// <summary>
		/// Adds the widgets from the section to the dialog.
		/// </summary>
		/// <param name="section">Section to be added to the dialog.</param>
		/// <param name="layout">Left top position of the section within the dialog.</param>
		/// <returns>Updated dialog.</returns>
		public Dialog AddSection(Section section, SectionLayout layout)
		{
			foreach(Widget widget in section.Widgets)
			{
				IWidgetLayout widgetLayout = section.GetWidgetLayout(widget);
				AddWidget(
					widget,
					new WidgetLayout(
						widgetLayout.Row + layout.Row,
						widgetLayout.Column + layout.Column,
						widgetLayout.RowSpan,
						widgetLayout.ColumnSpan,
						widgetLayout.HorizontalAlignment,
						widgetLayout.VerticalAlignment));
			}

			return this;
		}

		/// <summary>
		/// Adds the widgets from the section to the dialog.
		/// </summary>
		/// <param name="section">Section to be added to the dialog.</param>
		/// <param name="fromRow">Row in the dialog where the section should be added.</param>
		/// <param name="fromColumn">Column in the dialog where the section should be added.</param>
		/// <returns>Updated dialog.</returns>
		public Dialog AddSection(Section section, int fromRow, int fromColumn)
		{
			return AddSection(section, new SectionLayout(fromRow, fromColumn));
		}

		/// <summary>
		///     Applies a fixed width (in pixels) to a column.
		/// </summary>
		/// <param name="column">The index of the column on the grid.</param>
		/// <param name="columnWidth">The width of the column.</param>
		/// <exception cref="ArgumentOutOfRangeException">When the column index does not exist.</exception>
		/// <exception cref="ArgumentOutOfRangeException">When the column width is smaller than 0.</exception>
		public void SetColumnWidth(int column, int columnWidth)
		{
			if (column < 0) throw new ArgumentOutOfRangeException("column");
			if (columnWidth < 0) throw new ArgumentOutOfRangeException("columnWidth");

			if (columnDefinitions.ContainsKey(column)) columnDefinitions[column] = columnWidth.ToString();
			else columnDefinitions.Add(column, columnWidth.ToString());
		}

		/// <summary>
		///     The width of the column will be automatically adapted to the widest widget in that column.
		/// </summary>
		/// <param name="column">The index of the column on the grid.</param>
		/// <exception cref="ArgumentOutOfRangeException">When the column index does not exist.</exception>
		public void SetColumnWidthAuto(int column)
		{
			if (column < 0) throw new ArgumentOutOfRangeException("column");

			if (columnDefinitions.ContainsKey(column)) columnDefinitions[column] = Auto;
			else columnDefinitions.Add(column, Auto);
		}

		/// <summary>
		///     The column will have the largest possible width, depending on the width of the other columns.
		/// </summary>
		/// <param name="column">The index of the column on the grid.</param>
		/// <exception cref="ArgumentOutOfRangeException">When the column index does not exist.</exception>
		public void SetColumnWidthStretch(int column)
		{
			if (column < 0) throw new ArgumentOutOfRangeException("column");

			if (columnDefinitions.ContainsKey(column)) columnDefinitions[column] = Stretch;
			else columnDefinitions.Add(column, Stretch);
		}

		/// <summary>
		///     Applies a fixed height (in pixels) to a row.
		/// </summary>
		/// <param name="row">The index of the row on the grid.</param>
		/// <param name="rowHeight">The height of the column.</param>
		/// <exception cref="ArgumentOutOfRangeException">When the row index is smaller than 0.</exception>
		/// <exception cref="ArgumentOutOfRangeException">When the row height is smaller than 0.</exception>
		public void SetRowHeight(int row, int rowHeight)
		{
			if (row < 0) throw new ArgumentOutOfRangeException("row");
			if (rowHeight <= 0) throw new ArgumentOutOfRangeException("rowHeight");

			if (rowDefinitions.ContainsKey(row)) rowDefinitions[row] = rowHeight.ToString();
			else rowDefinitions.Add(row, rowHeight.ToString());
		}

		/// <summary>
		///     The height of the row will be automatically adapted to the highest widget in that row.
		/// </summary>
		/// <param name="row">The index of the row on the grid.</param>
		/// <exception cref="ArgumentOutOfRangeException">When the row index is smaller than 0.</exception>
		public void SetRowHeightAuto(int row)
		{
			if (row < 0) throw new ArgumentOutOfRangeException("row");

			if (rowDefinitions.ContainsKey(row)) rowDefinitions[row] = Auto;
			else rowDefinitions.Add(row, Auto);
		}

		/// <summary>
		///     The row will have the largest possible height, depending on the height of the other rows.
		/// </summary>
		/// <param name="row">The index of the row on the grid.</param>
		/// <exception cref="ArgumentOutOfRangeException">When the row index is smaller than 0.</exception>
		public void SetRowHeightStretch(int row)
		{
			if (row < 0) throw new ArgumentOutOfRangeException("row");

			if (rowDefinitions.ContainsKey(row)) rowDefinitions[row] = Stretch;
			else rowDefinitions.Add(row, Stretch);
		}

		/// <summary>
		///     Sets the layout of the widget in the dialog.
		/// </summary>
		/// <param name="widget">A widget that is part of the dialog.</param>
		/// <param name="widgetLayout">The layout to apply to the widget.</param>
		/// <exception cref="NullReferenceException">When widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget is not part of the dialog.</exception>
		public void SetWidgetLayout(Widget widget, IWidgetLayout widgetLayout)
		{
			CheckWidgetExists(widget);
			widgetLayouts[widget] = widgetLayout;
		}

		/// <summary>
		///     Shows the dialog window.
		///     Also loads changes and triggers events when <paramref name="requireResponse" /> is <c>true</c>.
		/// </summary>
		/// <param name="requireResponse">If the dialog expects user interaction.</param>
		/// <remarks>Should only be used when you create your own event loop.</remarks>
		public void Show(bool requireResponse = true)
		{
			UIBuilder uib = Build();
			uib.RequireResponse = requireResponse;

			UIResults uir = Engine.ShowUI(uib);

			if (requireResponse)
			{
				LoadChanges(uir);
				RaiseResultEvents(uir);
			}
		}

		/// <summary>
		/// Removes all widgets from the dialog.
		/// </summary>
		public void Clear()
		{
			widgetLayouts.Clear();
			RowCount = 0;
			ColumnCount = 0;
		}

		private static string AlignmentToUiString(HorizontalAlignment horizontalAlignment)
		{
			switch (horizontalAlignment)
			{
				case HorizontalAlignment.Center:
					return "Center";
				case HorizontalAlignment.Left:
					return "Left";
				case HorizontalAlignment.Right:
					return "Right";
				case HorizontalAlignment.Stretch:
					return "Stretch";
				default:
					throw new InvalidEnumArgumentException(
						"horizontalAlignment",
						(int)horizontalAlignment,
						typeof(HorizontalAlignment));
			}
		}

		private static string AlignmentToUiString(VerticalAlignment verticalAlignment)
		{
			switch (verticalAlignment)
			{
				case VerticalAlignment.Center:
					return "Center";
				case VerticalAlignment.Top:
					return "Top";
				case VerticalAlignment.Bottom:
					return "Bottom";
				case VerticalAlignment.Stretch:
					return "Stretch";
				default:
					throw new InvalidEnumArgumentException(
						"verticalAlignment",
						(int)verticalAlignment,
						typeof(VerticalAlignment));
			}
		}

		/// <summary>
		/// Checks if any visible widgets in the Dialog overlap.
		/// </summary>
		/// <exception cref="OverlappingWidgetsException">Thrown when two visible widgets overlap with each other.</exception>
		private void CheckIfVisibleWidgetsOverlap()
		{
			if (AllowOverlappingWidgets) return;

			foreach(Widget widget in widgetLayouts.Keys)
			{
				if (!widget.IsVisible) continue;

				IWidgetLayout widgetLayout = widgetLayouts[widget];
				for (int column = widgetLayout.Column; column < widgetLayout.Column + widgetLayout.ColumnSpan; column++)
				{
					for (int row = widgetLayout.Row; row < widgetLayout.Row + widgetLayout.RowSpan; row++)
					{
						foreach(Widget otherWidget in widgetLayouts.Keys)
						{
							if (!otherWidget.IsVisible || widget.Equals(otherWidget)) continue;

							IWidgetLayout otherWidgetLayout = widgetLayouts[otherWidget];
							if (column >= otherWidgetLayout.Column && column < otherWidgetLayout.Column + otherWidgetLayout.ColumnSpan && row >= otherWidgetLayout.Row && row < otherWidgetLayout.Row + otherWidgetLayout.RowSpan)
							{
								throw new OverlappingWidgetsException(String.Format("The widget overlaps with another widget in the Dialog on Row {0}, Column {1}, RowSpan {2}, ColumnSpan {3}", widgetLayout.Row, widgetLayout.Column, widgetLayout.RowSpan, widgetLayout.ColumnSpan));
							}
						}
					}
				}
			}
		}

		private string GetRowDefinitions(SortedSet<int> rowsInUse)
		{
			string[] definitions = new string[rowsInUse.Count];
			int currentIndex = 0;
			foreach (int rowInUse in rowsInUse)
			{
				string value;
				if (rowDefinitions.TryGetValue(rowInUse, out value))
				{
					definitions[currentIndex] = value;
				}
				else
				{
					definitions[currentIndex] = Auto;
				}

				currentIndex++;
			}

			return String.Join(";", definitions);
		}

		private string GetColumnDefinitions(SortedSet<int> columnsInUse)
		{
			string[] definitions = new string[columnsInUse.Count];
			int currentIndex = 0;
			foreach (int columnInUse in columnsInUse)
			{
				string value;
				if (columnDefinitions.TryGetValue(columnInUse, out value))
				{
					definitions[currentIndex] = value;
				}
				else
				{
					definitions[currentIndex] = Auto;
				}

				currentIndex++;
			}

			return String.Join(";", definitions);
		}

		private UIBuilder Build()
		{
			// Check rows and columns in use
			SortedSet<int> rowsInUse;
			SortedSet<int> columnsInUse;
			this.FillRowsAndColumnsInUse(out rowsInUse, out columnsInUse);

			// Check if visible widgets overlap and throw exception if this is the case
			CheckIfVisibleWidgetsOverlap();

			// Initialize UI Builder
			var uiBuilder = new UIBuilder
			{
				Height = Height,
				MinHeight = MinHeight,
				Width = Width,
				MinWidth = MinWidth,
				RowDefs = GetRowDefinitions(rowsInUse),
				ColumnDefs = GetColumnDefinitions(columnsInUse),
				Title = Title
			};

			KeyValuePair<Widget, IWidgetLayout> defaultKeyValuePair = default(KeyValuePair<Widget, IWidgetLayout>);
			int rowIndex = 0;
			int columnIndex = 0;
			foreach (int rowInUse in rowsInUse)
			{
				columnIndex = 0;
				foreach (int columnInUse in columnsInUse)
				{
					foreach (KeyValuePair<Widget, IWidgetLayout> keyValuePair in widgetLayouts.Where(x => x.Key.IsVisible && x.Key.Type != UIBlockType.Undefined && x.Value.Row.Equals(rowInUse) && x.Value.Column.Equals(columnInUse)))
					{
						if (keyValuePair.Equals(defaultKeyValuePair)) continue;

						// Can be removed once we retrieve all collapsed states from the UI
						TreeView treeView = keyValuePair.Key as TreeView;
						if (treeView != null) treeView.UpdateItemCache();

						UIBlockDefinition widgetBlockDefinition = keyValuePair.Key.BlockDefinition;
						IWidgetLayout widgetLayout = keyValuePair.Value;

						widgetBlockDefinition.Column = columnIndex;
						widgetBlockDefinition.ColumnSpan = widgetLayout.ColumnSpan;
						widgetBlockDefinition.Row = rowIndex;
						widgetBlockDefinition.RowSpan = widgetLayout.RowSpan;
						widgetBlockDefinition.HorizontalAlignment = AlignmentToUiString(widgetLayout.HorizontalAlignment);
						widgetBlockDefinition.VerticalAlignment = AlignmentToUiString(widgetLayout.VerticalAlignment);
						widgetBlockDefinition.Margin = widgetLayout.Margin.ToString();

						uiBuilder.AppendBlock(widgetBlockDefinition);
					}

					columnIndex++;
				}

				rowIndex++;
			}

			return uiBuilder;
		}

		/// <summary>
		/// Used to retrieve the rows and columns that are being used and updates the RowCount and ColumnCount properties based on the Widgets added to the dialog.
		/// </summary>
		/// <param name="rowsInUse">Collection containing the rows that are defined by the Widgets in the Dialog.</param>
		/// <param name="columnsInUse">Collection containing the columns that are defined by the Widgets in the Dialog.</param>
		private void FillRowsAndColumnsInUse(out SortedSet<int> rowsInUse, out SortedSet<int> columnsInUse)
		{
			rowsInUse = new SortedSet<int>();
			columnsInUse = new SortedSet<int>();
			foreach (KeyValuePair<Widget, IWidgetLayout> keyValuePair in this.widgetLayouts)
			{
				if (keyValuePair.Key.IsVisible && keyValuePair.Key.Type != UIBlockType.Undefined)
				{
					for (int i = keyValuePair.Value.Row; i < keyValuePair.Value.Row + keyValuePair.Value.RowSpan; i++)
					{
						rowsInUse.Add(i);
					}

					for (int i = keyValuePair.Value.Column; i < keyValuePair.Value.Column + keyValuePair.Value.ColumnSpan; i++)
					{
						columnsInUse.Add(i);
					}
				}
			}

			this.RowCount = rowsInUse.Count;
			this.ColumnCount = columnsInUse.Count;
		}

		// ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
		private void CheckWidgetExists(Widget widget)
		{
			if (widget == null)
			{
				throw new ArgumentNullException("widget");
			}

			if (!widgetLayouts.ContainsKey(widget))
			{
				throw new ArgumentException("Widget is not part of this dialog");
			}
		}

		private void LoadChanges(UIResults uir)
		{
			foreach (InteractiveWidget interactiveWidget in Widgets.OfType<InteractiveWidget>())
			{
				if (interactiveWidget.IsVisible)
				{
					interactiveWidget.LoadResult(uir);
				}
			}
		}

		private void RaiseResultEvents(UIResults uir)
		{
			if (Interacted != null)
			{
				Interacted(this, EventArgs.Empty);
			}

			if (uir.WasBack() && (Back != null))
			{
				Back(this, EventArgs.Empty);
				return;
			}

			if (uir.WasForward() && (Forward != null))
			{
				Forward(this, EventArgs.Empty);
				return;
			}

			// ToList is necessary to prevent InvalidOperationException when adding or removing widgets from a event handler.
			List<InteractiveWidget> intractableWidgets = Widgets.OfType<InteractiveWidget>()
				.Where(widget => widget.WantsOnChange).ToList();

			foreach (InteractiveWidget intractable in intractableWidgets)
			{
				intractable.RaiseResultEvents();
			}
		}
	}

///<summary>
///A drop-down list.
///</summary>
public class DropDown : InteractiveWidget
{
		private readonly HashSet<string> options = new HashSet<string>();
		private bool changed;
		private string previous;

		/// <summary>
		///     Initializes a new instance of the <see cref="DropDown" /> class.
		/// </summary>
		public DropDown() : this(Enumerable.Empty<string>())
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="DropDown" /> class.
		/// </summary>
		/// <param name="options">Options to be displayed in the list.</param>
		/// <param name="selected">The selected item in the list.</param>
		/// <exception cref="ArgumentNullException">When options is null.</exception>
		public DropDown(IEnumerable<string> options, string selected = null)
		{
			Type = UIBlockType.DropDown;
			SetOptions(options);
			if (selected != null) Selected = selected;
			ValidationText = "Invalid Input";
			ValidationState = UIValidationState.NotValidated;
		}

		/// <summary>
		///     Triggered when a different option is selected.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<DropDownChangedEventArgs> Changed
		{
			add
			{
				OnChanged += value;
				WantsOnChange = true;
			}

			remove
			{
				OnChanged -= value;
				if (OnChanged == null || !OnChanged.GetInvocationList().Any())
				{
					WantsOnChange = false;
				}
			}
		}

		private event EventHandler<DropDownChangedEventArgs> OnChanged;

		/// <summary>
		///     Gets or sets the possible options.
		/// </summary>
		public IEnumerable<string> Options
		{
			get
			{
				return options;
			}

			set
			{
				SetOptions(value);
			}
		}

		/// <summary>
		///     Gets or sets the selected option.
		/// </summary>
		public string Selected
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
		///		Gets or sets the state indicating if a given input field was validated or not and if the validation was valid.
		///		This should be used by the client to add a visual marker on the input field.
		/// </summary>
		/// <remarks>Available from DataMiner Feature Release 10.0.5 and 10.0.1.0 Main Release.</remarks>
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

		/// <summary>
		///     Gets or sets a value indicating whether a filter box is available for the drop-down list.
		/// </summary>
		/// <remarks>Available from DataMiner 9.5.6 onwards.</remarks>
		public bool IsDisplayFilterShown
		{
			get
			{
				return BlockDefinition.DisplayFilter;
			}

			set
			{
				BlockDefinition.DisplayFilter = value;
			}
		}

		/// <summary>
		///     Gets or sets a value indicating whether the options are sorted naturally.
		/// </summary>
		/// <remarks>Available from DataMiner 9.5.6 onwards.</remarks>
		public bool IsSorted
		{
			get
			{
				return BlockDefinition.IsSorted;
			}

			set
			{
				BlockDefinition.IsSorted = value;
			}
		}

		/// <summary>
		///     Adds an option to the drop-down list.
		/// </summary>
		/// <param name="option">Option to add.</param>
		/// <exception cref="ArgumentNullException">When option is null.</exception>
		public void AddOption(string option)
		{
			if (option == null)
			{
				throw new ArgumentNullException("option");
			}

			if (!options.Contains(option))
			{
				options.Add(option);
				BlockDefinition.AddDropDownOption(option);
			}
		}

		/// <summary>
		///     Sets the displayed options.
		///     Replaces existing options.
		/// </summary>
		/// <param name="optionsToSet">Options to set.</param>
		/// <exception cref="ArgumentNullException">When optionsToSet is null.</exception>
		public void SetOptions(IEnumerable<string> optionsToSet)
		{
			if (optionsToSet == null)
			{
				throw new ArgumentNullException("optionsToSet");
			}

			ClearOptions();
			foreach (string option in optionsToSet)
			{
				AddOption(option);
			}

			if (Selected == null || !optionsToSet.Contains(Selected))
			{
				Selected = optionsToSet.FirstOrDefault();
			}
		}

		/// <summary>
		/// 	Removes an option from the drop-down list.
		/// </summary>
		/// <param name="option">Option to remove.</param>
		/// <exception cref="ArgumentNullException">When option is null.</exception>
		public void RemoveOption(string option)
		{
			if (option == null)
			{
				throw new ArgumentNullException("option");
			}

			if (options.Remove(option))
			{
				RecreateUiBlock();
				foreach (string optionToAdd in options)
				{
					BlockDefinition.AddDropDownOption(optionToAdd);
				}

				if (Selected == option)
				{
					Selected = options.FirstOrDefault();
				}
			}
		}

		internal override void LoadResult(UIResults uiResults)
		{
			string selectedValue = uiResults.GetString(this);
			if (WantsOnChange)
			{
				changed = selectedValue != Selected;
			}

			previous = Selected;
			Selected = selectedValue;
		}

		/// <inheritdoc />
		internal override void RaiseResultEvents()
		{
			if (changed && (OnChanged != null))
			{
				OnChanged(this, new DropDownChangedEventArgs(Selected, previous));
			}

			changed = false;
		}

		private void ClearOptions()
		{
			options.Clear();
			RecreateUiBlock();
		}

		/// <summary>
		///     Provides data for the <see cref="Changed" /> event.
		/// </summary>
		public class DropDownChangedEventArgs : EventArgs
		{
			internal DropDownChangedEventArgs(string selected, string previous)
			{
				Selected = selected;
				Previous = previous;
			}

			/// <summary>
			///     Gets the previously selected option.
			/// </summary>
			public string Previous { get; private set; }

			/// <summary>
			///     Gets the option that has been selected.
			/// </summary>
			public string Selected { get; private set; }
		}
	}

///<summary>
///Dialog used to display an exception.
///</summary>
public class ExceptionDialog : Dialog
{
		private readonly Label exceptionLabel = new Label();
		private Exception exception;

		/// <summary>
		/// Initializes a new instance of the ExceptionDialog class.
		/// </summary>
		/// <param name="engine">Link with DataMiner.</param>
		public ExceptionDialog(IEngine engine) : base(engine)
		{
			Title = "Exception Occurred";
			OkButton = new Button("OK");

			AddWidget(exceptionLabel, 0, 0);
			AddWidget(OkButton, 1, 0);
		}

		/// <summary>
		/// Initializes a new instance of the ExceptionDialog class with a specific exception to be displayed.
		/// </summary>
		/// <param name="engine">Link with DataMiner.</param>
		/// <param name="exception">Exception to be displayed by the exception dialog.</param>
		public ExceptionDialog(IEngine engine, Exception exception) : this(engine)
		{
			Exception = exception;
		}

		/// <summary>
		/// Exception to be displayed by the exception dialog.
		/// </summary>
		public Exception Exception
		{
			get
			{
				return exception;
			}

			set
			{
				exception = value;
				exceptionLabel.Text = exception.ToString();
			}
		}

		/// <summary>
		/// Button that is displayed below the exception.
		/// </summary>
		public Button OkButton { get; private set; }
	}

///<summary>
///Event loop of the interactive Automation script.
///</summary>
public class InteractiveController
{
		private bool isManualModeRequested;
		private Action manualAction;
		private Dialog nextDialog;

		/// <summary>
		///     Initializes a new instance of the <see cref="InteractiveController" /> class.
		///     This object will manage the event loop of the interactive Automation script.
		/// </summary>
		/// <param name="engine">Link with the SLAutomation process.</param>
		/// <exception cref="ArgumentNullException">When engine is null.</exception>
		public InteractiveController(IEngine engine)
		{
			if (engine == null)
			{
				throw new ArgumentNullException("engine");
			}

			Engine = engine;
		}

		/// <summary>
		///     Gets the dialog that is shown to the user.
		/// </summary>
		public Dialog CurrentDialog { get; private set; }

		/// <summary>
		///     Gets the link to the SLManagedAutomation process.
		/// </summary>
		public IEngine Engine { get; private set; }

		/// <summary>
		///     Gets a value indicating whether the event loop is updated manually or automatically.
		/// </summary>
		public bool IsManualMode { get; private set; }

		/// <summary>
		///     Gets a value indicating whether the event loop has been started.
		/// </summary>
		public bool IsRunning { get; private set; }

		/// <summary>
		///     Switches the event loop to manual control.
		///     This mode allows the dialog to be updated without user interaction using <see cref="Update" />.
		///     The passed action method will be called when all events have been processed.
		///     The app returns to automatic user interaction mode when the method is exited.
		/// </summary>
		/// <param name="action">Method that will control the event loop manually.</param>
		public void RequestManualMode(Action action)
		{
			isManualModeRequested = true;
			manualAction = action;
		}

		/// <summary>
		///     Starts the application event loop.
		///     Updates the displayed dialog after each user interaction.
		///     Only user interaction on widgets with the WantsOnChange property set to true will cause updates.
		///     Use <see cref="RequestManualMode" /> if you want to manually control when the dialog is updated.
		/// </summary>
		/// <param name="startDialog">Dialog to be shown first.</param>
		public void Run(Dialog startDialog)
		{
			if (startDialog == null)
			{
				throw new ArgumentNullException("startDialog");
			}

			nextDialog = startDialog;

			if (IsRunning)
			{
				throw new InvalidOperationException("Already running");
			}

			IsRunning = true;
			while (true)
			{
				try
				{
					if (isManualModeRequested)
					{
						RunManualAction();
					}
					else
					{
						CurrentDialog = nextDialog;
						CurrentDialog.Show();
					}
				}
				catch (Exception)
				{
					IsRunning = false;
					IsManualMode = false;
					throw;
				}
			}
		}

		/// <summary>
		///     Sets the dialog that will be shown after user interaction events are processed,
		///     or when <see cref="Update" /> is called in manual mode.
		/// </summary>
		/// <param name="dialog">The next dialog to be shown.</param>
		/// <exception cref="ArgumentNullException">When dialog is null.</exception>
		public void ShowDialog(Dialog dialog)
		{
			if (dialog == null)
			{
				throw new ArgumentNullException("dialog");
			}

			nextDialog = dialog;
		}

		/// <summary>
		///     Manually updates the dialog.
		///     Use this method when you want to update the dialog without user interaction.
		///     Note that no events will be raised.
		/// </summary>
		/// <exception cref="InvalidOperationException">When not in manual mode.</exception>
		/// <exception cref="InvalidOperationException">When no dialog has been set.</exception>
		public void Update()
		{
			if (!IsManualMode)
			{
				throw new InvalidOperationException("Not allowed in automatic mode");
			}

			if (CurrentDialog == null)
			{
				throw new InvalidOperationException("No dialog has been set");
			}

			CurrentDialog = nextDialog;
			CurrentDialog.Show(false);
		}

		private void RunManualAction()
		{
			isManualModeRequested = false;
			IsManualMode = true;
			manualAction();
			IsManualMode = false;
		}
	}

///<summary>
///A widget that requires user input.
///</summary>
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

///<summary>
///A label is used to display text.
///Text can have different styles.
///</summary>
public class Label : Widget
{
		private TextStyle style;

		/// <summary>
		///     Initializes a new instance of the <see cref="Label" /> class.
		/// </summary>
		/// <param name="text">The text that is displayed by the label.</param>
		public Label(string text)
		{
			Type = UIBlockType.StaticText;
			Style = TextStyle.None;
			Text = text;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Label" /> class.
		/// </summary>
		public Label() : this("Label")
		{
		}

		/// <summary>
		///     Gets or sets the text style of the label.
		/// </summary>
		public TextStyle Style
		{
			get
			{
				return style;
			}

			set
			{
				style = value;
				BlockDefinition.Style = StyleToUiString(value);
			}
		}

		/// <summary>
		///     Gets or sets the displayed text.
		/// </summary>
		public string Text
		{
			get
			{
				return BlockDefinition.Text;
			}

			set
			{
				BlockDefinition.Text = value;
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


		private static string StyleToUiString(TextStyle textStyle)
		{
			switch (textStyle)
			{
				case TextStyle.None:
					return null;
				case TextStyle.Title:
					return "Title1";
				case TextStyle.Bold:
					return "Title2";
				case TextStyle.Heading:
					return "Title3";
				default:
					throw new ArgumentOutOfRangeException("textStyle", textStyle, null);
			}
		}
	}

///<summary>
///Defines the whitespace that is displayed around a widget.
///</summary>
public class Margin
{
		private int bottom;
		private int left;
		private int right;
		private int top;

		/// <summary>
		/// Initializes a new instance of the Margin class.
		/// </summary>
		/// <param name="left">Amount of margin on the left-hand side of the widget in pixels.</param>
		/// <param name="top">Amount of margin at the top of the widget in pixels.</param>
		/// <param name="right">Amount of margin on the right-hand side of the widget in pixels.</param>
		/// <param name="bottom">Amount of margin at the bottom of the widget in pixels.</param>
		public Margin(int left, int top, int right, int bottom)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}

		/// <summary>
		/// Initializes a new instance of the Margin class.
		/// A margin is by default 3 pixels wide.
		/// </summary>
		public Margin() : this(3, 3, 3, 3)
		{
		}

		/// <summary>
		/// Initializes a new instance of the Margin class based on a string.
		/// This string should have the following syntax: left;top;right;bottom
		/// </summary>
		/// <exception cref="ArgumentException">If the string does not match the predefined syntax, or if any of the margins is not a number.</exception>
		/// <param name="margin">Margin in string format.</param>
		public Margin(string margin)
		{
			if(String.IsNullOrWhiteSpace(margin))
			{
				left = 0;
				top = 0;
				right = 0;
				bottom = 0;
				return;
			}

			string[] splitMargin = margin.Split(';');
			if (splitMargin.Length != 4) throw new ArgumentException("Margin should have the following format: left;top;right;bottom");

			if (!Int32.TryParse(splitMargin[0], out left)) throw new ArgumentException("Left margin is not a number");
			if (!Int32.TryParse(splitMargin[1], out top)) throw new ArgumentException("Top margin is not a number");
			if (!Int32.TryParse(splitMargin[2], out right)) throw new ArgumentException("Right margin is not a number");
			if (!Int32.TryParse(splitMargin[3], out bottom)) throw new ArgumentException("Bottom margin is not a number");
		}

		/// <summary>
		/// Amount of margin in pixels at the bottom of the widget.
		/// </summary>
		public int Bottom
		{
			get
			{
				return bottom;
			}

			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				bottom = value;
			}
		}

		/// <summary>
		/// Amount of margin in pixels at the left-hand side of the widget.
		/// </summary>
		public int Left
		{
			get
			{
				return left;
			}

			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				left = value;
			}
		}

		/// <summary>
		/// Amount of margin in pixels at the right-hand side of the widget.
		/// </summary>
		public int Right
		{
			get
			{
				return right;
			}

			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				right = value;
			}
		}

		/// <summary>
		/// Amount of margin in pixels at the top of the widget.
		/// </summary>
		public int Top
		{
			get
			{
				return top;
			}

			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				top = value;
			}
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return String.Join(";", new object[] { left, top, right, bottom });
		}
	}

///<summary>
///Dialog used to display a message.
///</summary>
public class MessageDialog : Dialog
{
		private readonly Label messageLabel = new Label();

		/// <summary>
		/// Initializes a new instance of the <see cref="MessageDialog" /> class without a message.
		/// </summary>
		/// <param name="engine">Link with DataMiner.</param>
		public MessageDialog(IEngine engine) : base(engine)
		{
			OkButton = new Button("OK") { Width = 150 };

			AddWidget(messageLabel, 0, 0);
			AddWidget(OkButton, 1, 0);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MessageDialog" /> class with a specific message.
		/// </summary>
		/// <param name="engine">Link with DataMiner.</param>
		/// <param name="message">Message to be displayed in the dialog.</param>
		public MessageDialog(IEngine engine, String message) : this(engine)
		{
			Message = message;
		}

		/// <summary>
		/// Message to be displayed in the dialog.
		/// </summary>
		public string Message
		{
			get
			{
				return messageLabel.Text;
			}

			set
			{
				messageLabel.Text = value;
			}
		}

		/// <summary>
		/// Button that is displayed below the message.
		/// </summary>
		public Button OkButton { get; private set; }
	}

///<summary>
///A spinner or numeric up-down control.
///Has a slider when the range is limited.
///</summary>
public class Numeric : InteractiveWidget
{
		private bool changed;
		private double previous;
		private double value;

		/// <summary>
		///     Initializes a new instance of the <see cref="Numeric" /> class.
		/// </summary>
		/// <param name="value">Current value of the numeric.</param>
		public Numeric(double value)
		{
			Type = UIBlockType.Numeric;
			Maximum = Double.MaxValue;
			Minimum = Double.MinValue;
			Decimals = 0;
			StepSize = 1;
			Value = value;
			ValidationText = "Invalid Input";
			ValidationState = UIValidationState.NotValidated;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Numeric" /> class.
		/// </summary>
		public Numeric() : this(0)
		{
		}

		/// <summary>
		///     Triggered when the value of the numeric changes.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<NumericChangedEventArgs> Changed
		{
			add
			{
				OnChanged += value;
				WantsOnChange = true;
			}

			remove
			{
				OnChanged -= value;
				if(OnChanged == null || !OnChanged.GetInvocationList().Any())
				{
					WantsOnChange = false;
				}
			}
		}

		private event EventHandler<NumericChangedEventArgs> OnChanged;

		/// <summary>
		///     Gets or sets the number of decimals to show.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">When the value is smaller than 0.</exception>
		public int Decimals
		{
			get
			{
				return BlockDefinition.Decimals;
			}

			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				BlockDefinition.Decimals = value;
			}
		}

		/// <summary>
		///     Gets or sets the maximum value of the range.
		/// </summary>
		/// <exception cref="ArgumentException">When the value is smaller than the minimum.</exception>
		/// <exception cref="ArgumentException">When the value is <c>Double.NaN</c> or infinity.</exception>
		public double Maximum
		{
			get
			{
				return BlockDefinition.RangeHigh;
			}

			set
			{
				if (value < Minimum)
				{
					throw new ArgumentException("Maximum can't be smaller than Minimum", "value");
				}

				CheckDouble(value);

				BlockDefinition.RangeHigh = value;
				Value = ClipToRange(Value);
			}
		}

		/// <summary>
		///     Gets or sets the minimum value of the range.
		/// </summary>
		/// <exception cref="ArgumentException">When the value is larger than the maximum.</exception>
		/// <exception cref="ArgumentException">When the value is <c>Double.NaN</c> or infinity.</exception>
		public double Minimum
		{
			get
			{
				return BlockDefinition.RangeLow;
			}

			set
			{
				if (value > Maximum)
				{
					throw new ArgumentException("Minimum can't be larger than Maximum", "value");
				}

				CheckDouble(value);

				BlockDefinition.RangeLow = value;
				Value = ClipToRange(Value);
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
		///     Gets or sets the step size.
		/// </summary>
		/// <exception cref="ArgumentException">When the value is <c>Double.NaN</c> or infinity.</exception>
		public double StepSize
		{
			get
			{
				return BlockDefinition.RangeStep;
			}

			set
			{
				CheckDouble(value);
				BlockDefinition.RangeStep = value;
			}
		}

		/// <summary>
		///     Gets or sets the value of the numeric.
		/// </summary>
		public double Value
		{
			get
			{
				return value;
			}

			set
			{
				value = ClipToRange(value);
				this.value = value;
				BlockDefinition.InitialValue = value.ToString(CultureInfo.InvariantCulture);
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
			double result;
			if (!Double.TryParse(
				uiResults.GetString(this),
				NumberStyles.Float,
				CultureInfo.InvariantCulture,
				out result))
			{
				return;
			}

			result = ClipToRange(result);
			bool isNotEqual = !IsEqualWithinDecimalMargin(result, value);
			if (isNotEqual && WantsOnChange)
			{
				changed = true;
				previous = result;
			}

			Value = result;
		}

		/// <inheritdoc />
		internal override void RaiseResultEvents()
		{
			if (changed && (OnChanged != null))
			{
				OnChanged(this, new NumericChangedEventArgs(Value, previous));
			}

			changed = false;
		}

		// ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
		private static void CheckDouble(double value)
		{
			if (Double.IsNaN(value))
			{
				throw new ArgumentException("NAN is not allowed", "value");
			}

			if (Double.IsInfinity(value))
			{
				throw new ArgumentException("Infinity is not allowed", "value");
			}
		}

		private double ClipToRange(double number)
		{
			number = Math.Min(Maximum, number);
			number = Math.Max(Minimum, number);
			return number;
		}

		private bool IsEqualWithinDecimalMargin(double a, double b)
		{
			return Math.Abs(a - b) < Math.Pow(10, -Decimals);
		}

		/// <summary>
		///     Provides data for the <see cref="Changed" /> event.
		/// </summary>
		public class NumericChangedEventArgs : EventArgs
		{
			internal NumericChangedEventArgs(double value, double previous)
			{
				Value = value;
				Previous = previous;
			}

			/// <summary>
			///     Gets the previous value of the numeric.
			/// </summary>
			public double Previous { get; private set; }

			/// <summary>
			///     Gets the new value of the numeric.
			/// </summary>
			public double Value { get; private set; }
		}
	}

///<summary>
///This exception is used to indicate that two widgets have overlapping positions on the same dialog.
///</summary>
public class OverlappingWidgetsException : Exception
{
		/// <summary>
		/// Initializes a new instance of the <see cref="OverlappingWidgetsException" /> class.
		/// </summary>
		public OverlappingWidgetsException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OverlappingWidgetsException" /> class with a specified error message.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public OverlappingWidgetsException(string message) : base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OverlappingWidgetsException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="inner">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
		public OverlappingWidgetsException(string message, Exception inner) : base(message, inner)
		{
		}

		/// <summary>
		/// Initializes a new instance of the OverlappingWidgetException class with the serialized data.
		/// </summary>
		/// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual information about the source or destination.</param>
		protected OverlappingWidgetsException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}

///<summary>
///Displays the value of a protocol parameter.
///</summary>
public class Parameter : Widget
{
		private int dmaId;
		private int elementId;
		private string index;
		private int parameterId;

		/// <summary>
		///     Initializes a new instance of the <see cref="Parameter" /> class.
		/// </summary>
		/// <param name="dmaId">ID of the DataMiner Agent.</param>
		/// <param name="elementId">ID of the element.</param>
		/// <param name="parameterId">ID of the parameter.</param>
		/// <param name="index">Primary key of the table entry. Is null for standalone parameters.</param>
		public Parameter(int dmaId, int elementId, int parameterId, string index = null)
		{
			BlockDefinition.Type = UIBlockType.Parameter;
			DmaId = dmaId;
			ElementId = elementId;
			ParameterId = parameterId;
			Index = index;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Parameter" /> class.
		/// </summary>
		/// <param name="element">Element that has the parameter.</param>
		/// <param name="parameterId">ID of the parameter.</param>
		/// <param name="index">Primary key of the table entry. Is null for standalone parameters.</param>
		public Parameter(Element element, int parameterId, string index = null) : this(
			element.DmaId,
			element.ElementId,
			parameterId,
			index)
		{
		}

		/// <summary>
		///     Gets or sets the ID of the DataMiner Agent that has the parameter.
		/// </summary>
		public int DmaId
		{
			get
			{
				return dmaId;
			}

			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				dmaId = value;
				BlockDefinition.Extra = GenerateExtra();
			}
		}

		/// <summary>
		///     Gets or sets the ID of the element that has the parameter.
		/// </summary>
		public int ElementId
		{
			get
			{
				return elementId;
			}

			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				elementId = value;
				BlockDefinition.Extra = GenerateExtra();
			}
		}

		/// <summary>
		///     Gets or sets the primary key of the table entry.
		/// </summary>
		/// <remarks>Should be <c>null</c> for standalone parameters.</remarks>
		public string Index
		{
			get
			{
				return index;
			}

			set
			{
				index = value;
				BlockDefinition.Extra = GenerateExtra();
			}
		}

		/// <summary>
		///     Gets or sets the ID of the parameter.
		/// </summary>
		public int ParameterId
		{
			get
			{
				return parameterId;
			}

			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				parameterId = value;
				BlockDefinition.Extra = GenerateExtra();
			}
		}

		private string GenerateExtra()
		{
			return String.Format("{0}/{1}:{2}:{3}", dmaId, elementId, parameterId, index);
		}
	}

///<summary>
///A text box for passwords.
///</summary>
///<remarks>Available from DataMiner 9.6.6 onwards.</remarks>
public class PasswordBox : InteractiveWidget
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

		/// <summary>
		///     Triggered when the password changes.
		/// </summary>
		public event EventHandler<PasswordBoxChangedEventArgs> Changed;

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
			if (WantsOnChange && (result != Password))
			{
				changed = true;
				previous = Password;
			}

			Password = result;
		}

		/// <inheritdoc />
		internal override void RaiseResultEvents()
		{
			if (changed && (Changed != null))
			{
				Changed(this, new PasswordBoxChangedEventArgs(Password, previous));
			}

			changed = false;
		}

		/// <summary>
		///     Provides data for the <see cref="Changed" /> event.
		/// </summary>
		public class PasswordBoxChangedEventArgs : EventArgs
		{
			internal PasswordBoxChangedEventArgs(string password, string previous)
			{
				Password = password;
				Previous = previous;
			}

			/// <summary>
			///     Gets the password.
			/// </summary>
			public string Password { get; private set; }

			/// <summary>
			///     Gets the previous password.
			/// </summary>
			public string Previous { get; private set; }
		}
	}

///<summary>
///When progress is displayed, this dialog has to be shown without requiring user interaction.
///When you are done displaying progress, call the Finish method and show the dialog with user interaction required.
///</summary>
public class ProgressDialog : Dialog
{
		private readonly StringBuilder progress = new StringBuilder();
		private readonly Label progressLabel = new Label();

		/// <summary>
		/// Used to instantiate a new instance of the <see cref="ProgressDialog" /> class.
		/// </summary>
		/// <param name="engine">Link with DataMiner.</param>
		public ProgressDialog(IEngine engine) : base(engine)
		{
			OkButton = new Button("OK") { IsEnabled = true, Width = 150 };
		}

		/// <summary>
		/// Button that is displayed after the Finish method is called.
		/// </summary>
		public Button OkButton { get; private set; }

		/// <summary>
		/// Clears the current progress and displays the provided text.
		/// </summary>
		/// <param name="text">Indication of the progress made.</param>
		public void SetProgress(string text)
		{
			progress.Clear();
			progress.AppendLine(text);
			Engine.ShowProgress(progress.ToString());
		}

		/// <summary>
		/// Adds the provided text to the current progress.
		/// </summary>
		/// <param name="text">Text to add to the current line of progress.</param>
		public void AddProgress(string text)
		{
			progress.Append(text);
			Engine.ShowProgress(progress.ToString());
		}

		/// <summary>
		/// Adds the provided text on a new line to the current progress.
		/// </summary>
		/// <param name="text">Indication of the progress made. This will be placed on a separate line.</param>
		public void AddProgressLine(string text)
		{
			progress.AppendLine(text);
			Engine.ShowProgress(progress.ToString());
		}

		/// <summary>
		/// Clears the progress.
		/// </summary>
		public void ClearProgress()
		{
			progress.Clear();
			Engine.ShowProgress(progress.ToString());
		}

		/// <summary>
		/// Call this method when you are done updating the progress through this dialog.
		/// This will cause the OK button to appear.
		/// Display this form with user interactivity required after this method is called.
		/// </summary>
		public void Finish() // TODO: ShowConfirmation
		{
			progressLabel.Text = progress.ToString();

			if (!Widgets.Contains(progressLabel)) AddWidget(progressLabel, 0, 0);
			if (!Widgets.Contains(OkButton)) AddWidget(OkButton, 1, 0);
		}
	}

///<summary>
///A group of radio buttons.
///</summary>
public class RadioButtonList : InteractiveWidget
{
		private readonly HashSet<string> options = new HashSet<string>();
		private bool changed;
		private string previous;

		/// <summary>
		///     Initializes a new instance of the <see cref="RadioButtonList" /> class.
		/// </summary>
		public RadioButtonList() : this(Enumerable.Empty<string>())
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="RadioButtonList" /> class.
		/// </summary>
		/// <param name="options">Name of options that can be selected.</param>
		/// <param name="selected">Selected option.</param>
		public RadioButtonList(IEnumerable<string> options, string selected = null)
		{
			Type = UIBlockType.RadioButtonList;
			SetOptions(options);
			Selected = selected;
		}

		/// <summary>
		///     Triggered when a different option is selected.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<RadioButtonChangedEventArgs> Changed
		{
			add
			{
				OnChanged += value;
				WantsOnChange = true;
			}

			remove
			{
				OnChanged -= value;
				if (OnChanged == null || !OnChanged.GetInvocationList().Any())
				{
					WantsOnChange = false;
				}
			}
		}

		private event EventHandler<RadioButtonChangedEventArgs> OnChanged;

		/// <summary>
		///     Gets or sets a value indicating whether the options are sorted naturally.
		/// </summary>
		/// <remarks>Available from DataMiner 9.5.6 onwards.</remarks>
		public bool IsSorted
		{
			get
			{
				return BlockDefinition.IsSorted;
			}

			set
			{
				BlockDefinition.IsSorted = value;
			}
		}

		/// <summary>
		///     Gets all options.
		/// </summary>
		public IEnumerable<string> Options
		{
			get
			{
				return options;
			}

			set
			{
				SetOptions(value);
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
		///     Gets or sets the selected option.
		/// </summary>
		public string Selected
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
		///     Adds a radio button to the group.
		/// </summary>
		/// <param name="option">Option to add.</param>
		/// <exception cref="ArgumentNullException">When option is null.</exception>
		public void AddOption(string option)
		{
			if (option == null)
			{
				throw new ArgumentNullException("option");
			}

			if (!options.Contains(option))
			{
				options.Add(option);
				BlockDefinition.AddCheckBoxListOption(option);
			}
		}

		/// <summary>
		/// 	Removes an option from the radio button list.
		/// </summary>
		/// <param name="option">Option to remove.</param>
		/// <exception cref="ArgumentNullException">When option is null.</exception>
		public void RemoveOption(string option)
		{
			if (option == null)
			{
				throw new ArgumentNullException("option");
			}

			if (options.Remove(option))
			{
				RecreateUiBlock();
				foreach (string optionToAdd in options)
				{
					BlockDefinition.AddCheckBoxListOption(optionToAdd);
				}

				if (Selected == option)
				{
					Selected = options.FirstOrDefault();
				}
			}
		}

		/// <summary>
		///     Sets the displayed options.
		///     Replaces existing options.
		/// </summary>
		/// <param name="optionsToSet">Options to set.</param>
		/// <exception cref="ArgumentNullException">When optionsToSet is null.</exception>
		public void SetOptions(IEnumerable<string> optionsToSet)
		{
			if (optionsToSet == null)
			{
				throw new ArgumentNullException("optionsToSet");
			}

			ClearOptions();
			foreach (string option in optionsToSet)
			{
				AddOption(option);
			}

			if (Selected == null || !optionsToSet.Contains(Selected))
			{
				Selected = optionsToSet.FirstOrDefault();
			}
		}

		internal override void LoadResult(UIResults uiResults)
		{
			string result = uiResults.GetString(this);
			if (String.IsNullOrWhiteSpace(result)) return;

			string[] checkedOptions = result.Split(';');
			foreach (string checkedOption in checkedOptions)
			{
				if (!String.IsNullOrEmpty(checkedOption) && (checkedOption != Selected))
				{
					previous = Selected;
					Selected = checkedOption;
					changed = true;
					break;
				}
			}
		}

		/// <inheritdoc />
		internal override void RaiseResultEvents()
		{
			if (changed && OnChanged != null)
			{
				OnChanged(this, new RadioButtonChangedEventArgs(Selected, previous));
			}

			changed = false;
		}

		private void ClearOptions()
		{
			options.Clear();
			RecreateUiBlock();
		}

		/// <summary>
		///     Provides data for the <see cref="Changed" /> event.
		/// </summary>
		public class RadioButtonChangedEventArgs : EventArgs
		{
			internal RadioButtonChangedEventArgs(string selectedValue, string previous)
			{
				SelectedValue = selectedValue;
				Previous = previous;
			}

			/// <summary>
			///     Gets the previously selected option.
			/// </summary>
			public string Previous { get; private set; }

			/// <summary>
			///     Gets the option that has been selected.
			/// </summary>
			public string SelectedValue { get; private set; }
		}
	}

///<summary>
///A section is a special component that can be used to group widgets together.
///</summary>
public class Section
{
		private readonly Dictionary<Widget, IWidgetLayout> widgetLayouts = new Dictionary<Widget, IWidgetLayout>();

		private bool isEnabled = true;
		private bool isVisible = true;

		/// <summary>
		/// Number of columns that are currently defined by the widgets that have been added to this section.
		/// </summary>
		public int ColumnCount { get; private set; }

		/// <summary>
		/// Number of rows that are currently defined by the widgets that have been added to this section.
		/// </summary>
		public int RowCount { get; private set; }

		/// <summary>
		///		Gets or sets a value indicating whether the widgets within the section are visible or not.
		/// </summary>
		public bool IsVisible
		{
			get
			{
				return isVisible;
			}

			set
			{
				isVisible = value;
				foreach (Widget widget in Widgets)
				{
					widget.IsVisible = isVisible;
				}
			}
		}

		/// <summary>
		///		Gets or sets a value indicating whether the interactive widgets within the section are enabled or not.
		/// </summary>
		public bool IsEnabled
		{
			get
			{
				return isEnabled;
			}

			set
			{
				isEnabled = value;
				foreach (Widget widget in Widgets)
				{
					InteractiveWidget interactiveWidget = widget as InteractiveWidget;
					if (interactiveWidget != null)
					{
						interactiveWidget.IsEnabled = isEnabled;
					}
				}
			}
		}

		/// <summary>
		///     Gets widgets that have been added to the section.
		/// </summary>
		public IEnumerable<Widget> Widgets
		{
			get
			{
				return widgetLayouts.Keys;
			}
		}

		/// <summary>
		///     Adds a widget to the section.
		/// </summary>
		/// <param name="widget">Widget to add to the <see cref="Section" />.</param>
		/// <param name="widgetLayout">Location of the widget in the grid layout.</param>
		/// <returns>The dialog.</returns>
		/// <exception cref="ArgumentNullException">When the widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget has already been added to the <see cref="Section" />.</exception>
		public Section AddWidget(Widget widget, IWidgetLayout widgetLayout)
		{
			if (widget == null)
			{
				throw new ArgumentNullException("widget");
			}

			if (widgetLayouts.ContainsKey(widget))
			{
				throw new ArgumentException("Widget is already added to the section");
			}

			widgetLayouts.Add(widget, widgetLayout);
			UpdateRowAndColumnCount();

			return this;
		}

		/// <summary>
		///     Adds a widget to the section.
		/// </summary>
		/// <param name="widget">Widget to add to the section.</param>
		/// <param name="row">Row location of the widget on the grid.</param>
		/// <param name="column">Column location of the widget on the grid.</param>
		/// <param name="horizontalAlignment">Horizontal alignment of the widget.</param>
		/// <param name="verticalAlignment">Vertical alignment of the widget.</param>
		/// <returns>The updated section.</returns>
		/// <exception cref="ArgumentNullException">When the widget is null.</exception>
		/// <exception cref="ArgumentException">When the location is out of bounds of the grid.</exception>
		/// <exception cref="ArgumentException">When the widget has already been added to the dialog.</exception>
		public Section AddWidget(
			Widget widget,
			int row,
			int column,
			HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left,
			VerticalAlignment verticalAlignment = VerticalAlignment.Center)
		{
			AddWidget(widget, new WidgetLayout(row, column, horizontalAlignment, verticalAlignment));
			return this;
		}

		/// <summary>
		///     Adds a widget to the section.
		/// </summary>
		/// <param name="widget">Widget to add to the section.</param>
		/// <param name="fromRow">Row location of the widget on the grid.</param>
		/// <param name="fromColumn">Column location of the widget on the grid.</param>
		/// <param name="rowSpan">Number of rows the widget will use.</param>
		/// <param name="colSpan">Number of columns the widget will use.</param>
		/// <param name="horizontalAlignment">Horizontal alignment of the widget.</param>
		/// <param name="verticalAlignment">Vertical alignment of the widget.</param>
		/// <returns>The updated section.</returns>
		/// <exception cref="ArgumentNullException">When the widget is null.</exception>
		/// <exception cref="ArgumentException">When the location is out of bounds of the grid.</exception>
		/// <exception cref="ArgumentException">When the widget has already been added to the dialog.</exception>
		public Section AddWidget(
			Widget widget,
			int fromRow,
			int fromColumn,
			int rowSpan,
			int colSpan,
			HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left,
			VerticalAlignment verticalAlignment = VerticalAlignment.Center)
		{
			AddWidget(
				widget,
				new WidgetLayout(fromRow, fromColumn, rowSpan, colSpan, horizontalAlignment, verticalAlignment));
			return this;
		}

		/// <summary>
		/// Adds the widgets from the section to the section.
		/// </summary>
		/// <param name="section">Section to be added to the section.</param>
		/// <param name="layout">Left-top position of the section within the parent section.</param>
		/// <returns>The updated section.</returns>
		public Section AddSection(Section section, ILayout layout)
		{
			foreach (Widget widget in section.Widgets)
			{
				IWidgetLayout widgetLayout = section.GetWidgetLayout(widget);
				AddWidget(
					widget,
					new WidgetLayout(
						widgetLayout.Row + layout.Row,
						widgetLayout.Column + layout.Column,
						widgetLayout.RowSpan,
						widgetLayout.ColumnSpan,
						widgetLayout.HorizontalAlignment,
						widgetLayout.VerticalAlignment));
			}

			return this;
		}

		/// <summary>
		///     Gets the layout of the widget in the dialog.
		/// </summary>
		/// <param name="widget">A widget that is part of the dialog.</param>
		/// <returns>The widget layout in the dialog.</returns>
		/// <exception cref="NullReferenceException">When the widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget is not part of the dialog.</exception>
		public IWidgetLayout GetWidgetLayout(Widget widget)
		{
			CheckWidgetExits(widget);
			return widgetLayouts[widget];
		}

		/// <summary>
		///     Removes a widget from the dialog.
		/// </summary>
		/// <param name="widget">Widget to remove.</param>
		/// <exception cref="ArgumentNullException">When the widget is null.</exception>
		public void RemoveWidget(Widget widget)
		{
			if (widget == null)
			{
				throw new ArgumentNullException("widget");
			}

			widgetLayouts.Remove(widget);
			UpdateRowAndColumnCount();
		}

		/// <summary>
		///     Sets the layout of a widget in the section.
		/// </summary>
		/// <param name="widget">A widget that is part of the section.</param>
		/// <param name="widgetLayout">The layout to apply to the widget.</param>
		/// <exception cref="NullReferenceException">When widget is null.</exception>
		/// <exception cref="ArgumentException">When the widget is not part of the section.</exception>
		/// <exception cref="NullReferenceException">When widgetLayout is null.</exception>
		public void SetWidgetLayout(Widget widget, IWidgetLayout widgetLayout)
		{
			if (widgetLayout == null) throw new ArgumentNullException(nameof(widgetLayout));

			CheckWidgetExits(widget);
			widgetLayouts[widget] = widgetLayout;
		}

		/// <summary>
		/// Removes all widgets from the section.
		/// </summary>
		public void Clear()
		{
			widgetLayouts.Clear();
			RowCount = 0;
			ColumnCount = 0;
		}

		/// <summary>
		///		Checks if the widget layout overlaps with another widget in the Dialog.
		/// </summary>
		/// <param name="widgetLayout">Layout to be checked.</param>
		/// <returns>True if the layout overlaps with another layout, else false.</returns>
		private bool Overlaps(IWidgetLayout widgetLayout)
		{
			for (int column = widgetLayout.Column; column < widgetLayout.Column + widgetLayout.ColumnSpan; column++)
			{
				for (int row = widgetLayout.Row; row < widgetLayout.Row + widgetLayout.RowSpan; row++)
				{
					foreach (IWidgetLayout existingWidgetLayout in widgetLayouts.Values)
					{
						if (column >= existingWidgetLayout.Column && column < existingWidgetLayout.Column + existingWidgetLayout.ColumnSpan && row >= existingWidgetLayout.Row && row < existingWidgetLayout.Row + existingWidgetLayout.RowSpan)
						{
							return true;
						}
					}
				}
			}

			return false;
		}

		private void CheckWidgetExits(Widget widget)
		{
			if (widget == null)
			{
				throw new ArgumentNullException("widget");
			}

			if (!widgetLayouts.ContainsKey(widget))
			{
				throw new ArgumentException("Widget is not part of this dialog");
			}
		}

		/// <summary>
		///		Used to update the RowCount and ColumnCount properties based on the Widgets added to the section.
		/// </summary>
		private void UpdateRowAndColumnCount()
		{
			if(widgetLayouts.Any())
			{
				RowCount = widgetLayouts.Values.Max(w => w.Row + w.RowSpan);
				ColumnCount = widgetLayouts.Values.Max(w => w.Column + w.ColumnSpan);
			}
			else
			{
				RowCount = 0;
				ColumnCount = 0;
			}
		}
	}

///<summary>
///Used to define the position of a section in another section or dialog.
///</summary>
public class SectionLayout : ILayout
{
		private int column;
		private int row;

		/// <summary>
		/// Initializes a new instance of the <see cref="SectionLayout"/> class.
		/// </summary>
		/// <param name="row">Row index of the cell that the top-left cell of the section will be mapped to.</param>
		/// <param name="column">Column index of the cell that the top-left cell of the section will be mapped to.</param>
		public SectionLayout(int row, int column)
		{
			this.row = row;
			this.column = column;
		}

		/// <summary>
		///     Gets or sets the column location of the section on the dialog grid.
		/// </summary>
		/// <remarks>The top-left position is (0, 0) by default.</remarks>
		public int Column
		{
			get
			{
				return column;
			}

			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				column = value;
			}
		}

		/// <summary>
		///     Gets or sets the row location of the section on the dialog grid.
		/// </summary>
		/// <remarks>The top-left position is (0, 0) by default.</remarks>
		public int Row
		{
			get
			{
				return row;
			}

			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				row = value;
			}
		}
	}

///<summary>
///Widget that is used to edit and display text.
///</summary>
public class TextBox : InteractiveWidget
{
		private bool changed;
		private string previous;

		/// <summary>
		///     Initializes a new instance of the <see cref="TextBox" /> class.
		/// </summary>
		/// <param name="text">The text displayed in the text box.</param>
		public TextBox(string text)
		{
			Type = UIBlockType.TextBox;
			Text = text;
			PlaceHolder = String.Empty;
			ValidationText = "Invalid Input";
			ValidationState = UIValidationState.NotValidated;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="TextBox" /> class.
		/// </summary>
		public TextBox() : this(String.Empty)
		{
		}

		/// <summary>
		///     Triggered when the text in the text box changes.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<TextBoxChangedEventArgs> Changed
		{
			add
			{
				OnChanged += value;
				WantsOnChange = true;
			}

			remove
			{
				OnChanged -= value;
				if(OnChanged == null || !OnChanged.GetInvocationList().Any())
				{
					WantsOnChange = false;
				}
			}
		}

		private event EventHandler<TextBoxChangedEventArgs> OnChanged;

		/// <summary>
		///     Gets or sets a value indicating whether users are able to enter multiple lines of text.
		/// </summary>
		public bool IsMultiline
		{
			get
			{
				return BlockDefinition.IsMultiline;
			}

			set
			{
				BlockDefinition.IsMultiline = value;
			}
		}

		/// <summary>
		///     Gets or sets the text displayed in the text box.
		/// </summary>
		public string Text
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

		internal override void LoadResult(UIResults uiResults)
		{
			string value = uiResults.GetString(this);
			if (WantsOnChange)
			{
				changed = value != Text;
				previous = Text;
			}

			Text = value;
		}

		/// <inheritdoc />
		internal override void RaiseResultEvents()
		{
			if (changed && OnChanged != null)
			{
				OnChanged(this, new TextBoxChangedEventArgs(Text, previous));
			}

			changed = false;
		}

		/// <summary>
		///     Provides data for the <see cref="Changed" /> event.
		/// </summary>
		public class TextBoxChangedEventArgs : EventArgs
		{
			internal TextBoxChangedEventArgs(string value, string previous)
			{
				Value = value;
				Previous = previous;
			}

			/// <summary>
			///     Gets the text before the change.
			/// </summary>
			public string Previous { get; private set; }

			/// <summary>
			///     Gets the changed text.
			/// </summary>
			public string Value { get; private set; }
		}
	}

///<summary>
///Widget to show/edit a time duration.
///</summary>
public class Time : InteractiveWidget
{
		private bool changed;
		private TimeSpan previous;
		private TimeSpan timeSpan;
		private AutomationTimeUpDownOptions timeUpDownOptions;

		/// <summary>
		///     Initializes a new instance of the <see cref="Time" /> class.
		/// </summary>
		/// <param name="timeSpan">The timespan displayed in the time widget.</param>
		public Time(TimeSpan timeSpan)
		{
			Type = UIBlockType.Time;
			TimeUpDownOptions = new AutomationTimeUpDownOptions { UpdateValueOnEnterKey = false };
			TimeSpan = timeSpan;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Time" /> class.
		/// </summary>
		public Time() : this(new TimeSpan())
		{
		}

		/// <summary>
		///     Triggered when the timespan changes.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<TimeChangedEventArgs> Changed
		{
			add
			{
				OnChanged += value;
				WantsOnChange = true;
			}

			remove
			{
				OnChanged -= value;
				if(OnChanged == null || !OnChanged.GetInvocationList().Any())
				{
					WantsOnChange = false;
				}
			}
		}

		private event EventHandler<TimeChangedEventArgs> OnChanged;

		/// <summary>
		///     Gets or sets a value indicating whether the value is clipped to the range.
		///     Default: <c>false</c>
		/// </summary>
		public bool ClipValueToRange
		{
			get
			{
				return TimeUpDownOptions.ClipValueToMinMax;
			}

			set
			{
				TimeUpDownOptions.ClipValueToMinMax = value;
			}
		}

		/// <summary>
		///     Gets or sets the number of digits to be used in order to represent the fractions of seconds.
		///     Default: <c>0</c>
		/// </summary>
		public int Decimals
		{
			get
			{
				return TimeUpDownOptions.FractionalSecondsDigitsCount;
			}

			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				TimeUpDownOptions.FractionalSecondsDigitsCount = value;
			}
		}

		/// <summary>
		///     Gets or sets a value indicating whether seconds are displayed in the time widget.
		///     Default: <c>true</c>
		/// </summary>
		public bool HasSeconds
		{
			get
			{
				return TimeUpDownOptions.ShowSeconds;
			}

			set
			{
				TimeUpDownOptions.ShowSeconds = value;
			}
		}

		/// <summary>
		///     Gets or sets a value indicating whether a spinner button is shown.
		///     Default: <c>true</c>
		/// </summary>
		public bool HasSpinnerButton
		{
			get
			{
				return TimeUpDownOptions.ShowButtonSpinner;
			}

			set
			{
				TimeUpDownOptions.ShowButtonSpinner = value;
			}
		}

		/// <summary>
		///     Gets or sets a value indicating whether the spinner button is enabled.
		///     Default: <c>true</c>
		/// </summary>
		public bool IsSpinnerButtonEnabled
		{
			get
			{
				return TimeUpDownOptions.AllowSpin;
			}

			set
			{
				TimeUpDownOptions.AllowSpin = value;
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
		///     Gets or sets the maximum timespan.
		///     Default: <c>TimeSpan.MaxValue</c>
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">When the maximum is smaller than the minimum.</exception>
		public TimeSpan Maximum
		{
			get
			{
				return TimeUpDownOptions.Maximum ?? TimeSpan.MaxValue;
			}

			set
			{
				if (value < Minimum)
				{
					throw new ArgumentOutOfRangeException("value", "Maximum can't be smaller than Minimum");
				}

				TimeUpDownOptions.Maximum = value;
			}
		}

		/// <summary>
		///     Gets or sets the minimum timespan.
		///     Default: <c>TimeSpan.MinValue</c>
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">When the minimum is larger than the maximum.</exception>
		public TimeSpan Minimum
		{
			get
			{
				return TimeUpDownOptions.Minimum ?? TimeSpan.MinValue;
			}

			set
			{
				if (value > Maximum)
				{
					throw new ArgumentOutOfRangeException("value", "Minimum can't be larger than Maximum");
				}

				TimeUpDownOptions.Minimum = value;
			}
		}

		/// <summary>
		///     Gets or sets the timespan displayed in the time widget.
		/// </summary>
		public TimeSpan TimeSpan
		{
			get
			{
				return timeSpan;
			}

			set
			{
				timeSpan = value;
				BlockDefinition.InitialValue = timeSpan.ToString(
					AutomationConfigOptions.GlobalTimeSpanFormat,
					CultureInfo.InvariantCulture);
			}
		}

		/// <summary>
		///     Gets or sets a value indicating whether the widget will only trigger an event when the enter key is pressed.
		///     Default: <c>false</c>
		/// </summary>
		public bool UpdateOnEnter
		{
			get
			{
				return TimeUpDownOptions.UpdateValueOnEnterKey;
			}

			set
			{
				TimeUpDownOptions.UpdateValueOnEnterKey = value;
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

		private AutomationTimeUpDownOptions TimeUpDownOptions
		{
			get
			{
				return timeUpDownOptions;
			}

			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}

				timeUpDownOptions = value;
				BlockDefinition.ConfigOptions = value;
			}
		}

		/// <inheritdoc />
		internal override void LoadResult(UIResults uiResults)
		{
			TimeSpan result = uiResults.GetTime(this);
			if ((result != TimeSpan) && WantsOnChange)
			{
				changed = true;
				previous = TimeSpan;
			}

			TimeSpan = result;
		}

		/// <inheritdoc />
		internal override void RaiseResultEvents()
		{
			if (changed && (OnChanged != null))
			{
				OnChanged(this, new TimeChangedEventArgs(TimeSpan, previous));
			}

			changed = false;
		}

		/// <summary>
		///     Provides data for the <see cref="Changed" /> event.
		/// </summary>
		public class TimeChangedEventArgs : EventArgs
		{
			internal TimeChangedEventArgs(TimeSpan timeSpan, TimeSpan previous)
			{
				TimeSpan = timeSpan;
				Previous = previous;
			}

			/// <summary>
			///     Gets the previous timespan.
			/// </summary>
			public TimeSpan Previous { get; private set; }

			/// <summary>
			///     Gets the new timespan.
			/// </summary>
			public TimeSpan TimeSpan { get; private set; }
		}
	}

///<summary>
///Widget to show/edit a time of day.
///</summary>
public class TimePicker : TimePickerBase
{
		private bool changed;
		private int maxDropDownHeight;
		private TimeSpan maximum;
		private TimeSpan minimum;
		private TimeSpan previous;
		private TimeSpan time;
		private AutomationTimePickerOptions timePickerOptions;

		/// <summary>
		///     Initializes a new instance of the <see cref="TimePicker" /> class.
		/// </summary>
		/// <param name="time">Time displayed in the time picker.</param>
		public TimePicker(TimeSpan time) : base(new AutomationTimePickerOptions())
		{
			Type = UIBlockType.Time;
			Time = time;
			TimePickerOptions = (AutomationTimePickerOptions)DateTimeUpDownOptions;
			ValidationText = "Invalid Input";
			ValidationState = UIValidationState.NotValidated;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="TimePicker" /> class.
		/// </summary>
		public TimePicker() : this(DateTime.Now.TimeOfDay)
		{
		}

		/// <summary>
		///     Triggered when a different time is picked.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<TimePickerChangedEventArgs> Changed
		{
			add
			{
				OnChanged += value;
				WantsOnChange = true;
			}

			remove
			{
				OnChanged -= value;
				if(OnChanged == null || !OnChanged.GetInvocationList().Any())
				{
					WantsOnChange = false;
				}
			}
		}

		private event EventHandler<TimePickerChangedEventArgs> OnChanged;

		/// <summary>
		///     Gets or sets the last time listed in the time picker control.
		///     Default: <c>TimeSpan.FromMinutes(1439)</c> (1 day - 1 minute).
		/// </summary>
		public TimeSpan EndTime
		{
			get
			{
				return TimePickerOptions.EndTime;
			}

			set
			{
				CheckTimeOfDay(value);
				TimePickerOptions.EndTime = value;
			}
		}

		/// <summary>
		///     Gets or sets a value indicating whether the drop-down button of the time picker control is shown.
		///     Default: <c>true</c>
		/// </summary>
		public bool HasDropDownButton
		{
			get
			{
				return TimePickerOptions.ShowDropDownButton;
			}

			set
			{
				TimePickerOptions.ShowDropDownButton = value;
			}
		}

		/// <summary>
		///     Gets or sets the height of the time picker control.
		///     Default: 130.
		/// </summary>
		public int MaxDropDownHeight
		{
			get
			{
				return maxDropDownHeight;
			}

			set
			{
				maxDropDownHeight = value;
				TimePickerOptions.MaxDropDownHeight = value;
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
		///     Gets or sets the maximum time of day.
		/// </summary>
		public TimeSpan Maximum
		{
			get
			{
				return maximum;
			}

			set
			{
				CheckTimeOfDay(value);
				maximum = value;
				DateTimeUpDownOptions.Maximum = new DateTime() + value;
			}
		}

		/// <summary>
		///     Gets or sets the minimum time of day.
		/// </summary>
		public TimeSpan Minimum
		{
			get
			{
				return minimum;
			}

			set
			{
				CheckTimeOfDay(value);
				minimum = value;
				DateTimeUpDownOptions.Minimum = new DateTime() + value;
			}
		}

		/// <summary>
		///     Gets or sets the earliest time listed in the time picker control.
		///     Default: <c>TimeSpan.Zero</c>
		/// </summary>
		public TimeSpan StartTime
		{
			get
			{
				return TimePickerOptions.StartTime;
			}

			set
			{
				CheckTimeOfDay(value);
				TimePickerOptions.StartTime = value;
			}
		}

		/// <summary>
		///     Gets or sets the time of day displayed in the time picker.
		/// </summary>
		public TimeSpan Time
		{
			get
			{
				return time;
			}

			set
			{
				CheckTimeOfDay(value);
				time = value;
				BlockDefinition.InitialValue = value.ToString(
					AutomationConfigOptions.GlobalTimeSpanFormat,
					CultureInfo.InvariantCulture);
			}
		}

		/// <summary>
		///     Gets or sets the time interval between two time items in the time picker control.
		///     Default: <c>TimeSpan.FromHours(1)</c>
		/// </summary>
		public TimeSpan TimeInterval
		{
			get
			{
				return TimePickerOptions.TimeInterval;
			}

			set
			{
				CheckTimeOfDay(value);
				TimePickerOptions.TimeInterval = value;
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

		private AutomationTimePickerOptions TimePickerOptions
		{
			get
			{
				return timePickerOptions;
			}

			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}

				timePickerOptions = value;
				BlockDefinition.ConfigOptions = value;
			}
		}

		/// <inheritdoc />
		internal override void LoadResult(UIResults uiResults)
		{
			TimeSpan result = uiResults.GetTime(this);
			if ((result != Time) && WantsOnChange)
			{
				changed = true;
				previous = Time;
			}

			Time = result;
		}

		/// <inheritdoc />
		internal override void RaiseResultEvents()
		{
			if (changed && (OnChanged != null))
			{
				OnChanged(this, new TimePickerChangedEventArgs(Time, previous));
			}

			changed = false;
		}

		private static void CheckTimeOfDay(TimeSpan value)
		{
			if ((value.Ticks < 0) && (value.Days >= 1))
			{
				throw new ArgumentOutOfRangeException("value", "TimeSpan must represent time of day");
			}
		}

		/// <summary>
		///     Provides data for the <see cref="Changed" /> event.
		/// </summary>
		public class TimePickerChangedEventArgs : EventArgs
		{
			internal TimePickerChangedEventArgs(TimeSpan timeSpan, TimeSpan previous)
			{
				TimeSpan = timeSpan;
				Previous = previous;
			}

			/// <summary>
			///     Gets the previous time of day.
			/// </summary>
			public TimeSpan Previous { get; private set; }

			/// <summary>
			///     Gets the new time of day.
			/// </summary>
			public TimeSpan TimeSpan { get; private set; }
		}
	}

///<summary>
///Base class for time-based widgets that rely on the <see cref="AutomationDateTimeUpDownOptions" />.
///</summary>
public abstract class TimePickerBase : InteractiveWidget
{
		private AutomationDateTimeUpDownOptions dateTimeUpDownOptions;

		/// <summary>
		/// Initializes a new instance of the <see cref="TimePickerBase" />
		/// </summary>
		/// <param name="dateTimeUpDownOptions">Configuration for the new TimePickerBase instance.</param>
		protected TimePickerBase(AutomationDateTimeUpDownOptions dateTimeUpDownOptions)
		{
			DateTimeUpDownOptions = dateTimeUpDownOptions;
			UpdateOnEnter = false;
			Kind = DateTimeKind.Local;
		}

		/// <summary>
		///     Gets or sets a value indicating whether the spinner button is enabled.
		///     Default <c>true</c>.
		/// </summary>
		public bool IsSpinnerButtonEnabled
		{
			get
			{
				return DateTimeUpDownOptions.AllowSpin;
			}

			set
			{
				DateTimeUpDownOptions.AllowSpin = value;
			}
		}

		/// <summary>
		///     Gets or sets a value indicating whether the widget has a spinner button.
		///     Default <c>true</c>.
		/// </summary>
		public bool HasSpinnerButton
		{
			get
			{
				return DateTimeUpDownOptions.ShowButtonSpinner;
			}

			set
			{
				DateTimeUpDownOptions.ShowButtonSpinner = value;
			}
		}

		/// <summary>
		///     Gets or sets a value indicating whether the widget will only trigger an event when the enter key is pressed.
		///     Default: <c>false</c>
		/// </summary>
		public bool UpdateOnEnter
		{
			get
			{
				return DateTimeUpDownOptions.UpdateValueOnEnterKey;
			}

			set
			{
				DateTimeUpDownOptions.UpdateValueOnEnterKey = value;
			}
		}

		/// <summary>
		///     Gets or sets the date and time format used by the up-down control.
		///     Default: FullDateTime in DataMiner 9.5.3, general dateTime from DataMiner 9.5.4 onwards (Format = Custom, CustomFormat = "G").
		/// </summary>
		public DateTimeFormat DateTimeFormat
		{
			get
			{
				return DateTimeUpDownOptions.Format;
			}

			set
			{
				DateTimeUpDownOptions.Format = value;
			}
		}

		/// <summary>
		///     Gets or sets the date-time format to be used by the control when DateTimeFormat is set to
		///     <c>DateTimeFormat.Custom</c>.
		///     Default: G (from DataMiner 9.5.4 onwards; previously the default value was null).
		/// </summary>
		/// <remarks>Sets <see cref="DateTimeFormat" /> to <c>DateTimeFormat.Custom</c></remarks>
		public string CustomDateTimeFormat
		{
			get
			{
				return DateTimeUpDownOptions.FormatString;
			}

			set
			{
				DateTimeUpDownOptions.FormatString = value;
				DateTimeFormat = DateTimeFormat.Custom;
			}
		}

		/// <summary>
		///     Gets or sets the DateTimeKind (.NET) used by the datetime up-down control.
		///     Default: <c>DateTimeKind.Unspecified</c>
		/// </summary>
		public DateTimeKind Kind
		{
			get
			{
				return DateTimeUpDownOptions.Kind;
			}

			set
			{
				DateTimeUpDownOptions.Kind = value;
			}
		}

		/// <summary>
		///     Gets or sets a value indicating whether the value is clipped to the range.
		///     Default: <c>false</c>
		/// </summary>
		public bool ClipValueToRange
		{
			get
			{
				return DateTimeUpDownOptions.ClipValueToMinMax;
			}

			set
			{
				DateTimeUpDownOptions.ClipValueToMinMax = value;
			}
		}

		/// <summary>
		/// Configuration of this <see cref="TimePickerBase" /> instance.
		/// </summary>
		protected AutomationDateTimeUpDownOptions DateTimeUpDownOptions
		{
			get
			{
				return dateTimeUpDownOptions;
			}

			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}

				dateTimeUpDownOptions = value;
				BlockDefinition.ConfigOptions = value;
			}
		}
	}

///<summary>
///A tree view structure.
///</summary>
public class TreeView : InteractiveWidget
{
		private Dictionary<string, bool> checkedItemCache;
		private Dictionary<string, bool> collapsedItemCache; // TODO: should only contain Items with LazyLoading set to true
		private Dictionary<string, TreeViewItem> lookupTable;

		private bool itemsChanged = false;
		private List<TreeViewItem> changedItems = new List<TreeViewItem>();

		private bool itemsChecked = false;
		private List<TreeViewItem> checkedItems = new List<TreeViewItem>();

		private bool itemsUnchecked = false;
		private List<TreeViewItem> uncheckedItems = new List<TreeViewItem>();

		private bool itemsExpanded = false;
		private List<TreeViewItem> expandedItems = new List<TreeViewItem>();

		private bool itemsCollapsed = false;
		private List<TreeViewItem> collapsedItems = new List<TreeViewItem>();

		/// <summary>
		///		Initializes a new instance of the <see cref="TreeView" /> class.
		/// </summary>
		/// <param name="treeViewItems"></param>
		public TreeView(IEnumerable<TreeViewItem> treeViewItems)
		{
			Type = UIBlockType.TreeView;
			Items = treeViewItems;
		}

		/// <summary>
		///     Triggered when a different item is selected or no longer selected.
		///     WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<IEnumerable<TreeViewItem>> Changed
		{
			add
			{
				OnChanged += value;
				WantsOnChange = true;
			}

			remove
			{
				OnChanged -= value;
				if (OnChanged == null || !OnChanged.GetInvocationList().Any())
				{
					WantsOnChange = false;
				}
			}
		}

		private event EventHandler<IEnumerable<TreeViewItem>> OnChanged;

		/// <summary>
		///  Triggered whenever an item is selected.
		///  WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<IEnumerable<TreeViewItem>> Checked
		{
			add
			{
				OnChecked += value;
				WantsOnChange = true;
			}

			remove
			{
				OnChecked -= value;
				if (OnChecked == null || !OnChecked.GetInvocationList().Any())
				{
					WantsOnChange = false;
				}
			}
		}

		private event EventHandler<IEnumerable<TreeViewItem>> OnChecked;

		/// <summary>
		///  Triggered whenever an item is no longer selected.
		///  WantsOnChange will be set to true when this event is subscribed to.
		/// </summary>
		public event EventHandler<IEnumerable<TreeViewItem>> Unchecked
		{
			add
			{
				OnUnchecked += value;
				WantsOnChange = true;
			}

			remove
			{
				OnUnchecked -= value;
				if (OnUnchecked == null || !OnUnchecked.GetInvocationList().Any())
				{
					WantsOnChange = false;
				}
			}
		}

		private event EventHandler<IEnumerable<TreeViewItem>> OnUnchecked;

		/// <summary>
		///  Triggered whenever an item is expanded.
		///  Can be used for lazy loading.
		///  Will be triggered whenever a node with SupportsLazyLoading set to true is expanded.
		/// </summary>
		public event EventHandler<IEnumerable<TreeViewItem>> Expanded
		{
			add
			{
				OnExpanded += value;
			}

			remove
			{
				OnExpanded -= value;
			}
		}

		private event EventHandler<IEnumerable<TreeViewItem>> OnExpanded;

		/// <summary>
		///  Triggered whenever an item is collapsed.
		///  Will be triggered whenever a node with SupportsLazyLoading set to true is collapsed.
		/// </summary>
		public event EventHandler<IEnumerable<TreeViewItem>> Collapsed
		{
			add
			{
				OnCollapsed += value;
			}

			remove
			{
				OnCollapsed -= value;
			}
		}

		private event EventHandler<IEnumerable<TreeViewItem>> OnCollapsed;

		/// <summary>
		/// Sets the IsCollapsed state for all items in the tree view to true, causing the entire tree view to be collapsed.
		/// </summary>
		public void Collapse()
		{
			foreach (var item in GetAllItems())
			{
				item.IsCollapsed = true;
			}
		}

		/// <summary>
		/// Sets the IsCollapsed state for all items in the tree view to false, causing the entire tree view to be expanded.
		/// </summary>
		public void Expand()
		{
			foreach(var item in GetAllItems())
			{
				item.IsCollapsed = false;
			}
		}

		/// <summary>
		/// Returns the top-level items in the tree view.
		/// The TreeViewItem.ChildItems property can be used to navigate further down the tree.
		/// </summary>
		public IEnumerable<TreeViewItem> Items
		{
			get
			{
				return BlockDefinition.TreeViewItems;
			}

			set
			{
				if (value == null) throw new ArgumentNullException("value");
				BlockDefinition.TreeViewItems = new List<TreeViewItem>(value);
				UpdateItemCache();
			}
		}

		/// <summary>
		/// Returns all items in the tree view that are selected.
		/// </summary>
		public IEnumerable<TreeViewItem> CheckedItems
		{
			get
			{
				return GetCheckedItems();
			}
		}

		/// <summary>
		/// Returns all leaves (= items without children) in the tree view that are selected.
		/// </summary>
		public IEnumerable<TreeViewItem> CheckedLeaves
		{
			get
			{
				return GetCheckedItems().Where(x => !x.ChildItems.Any());
			}
		}

		/// <summary>
		/// Returns all nodes (= items with children) in the tree view that are selected.
		/// </summary>
		public IEnumerable<TreeViewItem> CheckedNodes
		{
			get
			{
				return GetCheckedItems().Where(x => x.ChildItems.Any());
			}
		}

		/// <summary>
		/// Can be used to retrieve an item from the tree view based on its key value.
		/// </summary>
		/// <param name="key">Key used to search for the item.</param>
		/// <param name="item">Item in the tree that matches the provided key.</param>
		/// <returns>True if the item was found, otherwise false.</returns>
		public bool TryFindTreeViewItem(string key, out TreeViewItem item)
		{
			item = GetAllItems().FirstOrDefault(x => x.KeyValue.Equals(key));
			return item != null;
		}

		/// <summary>
		/// This method is used to update the cached TreeViewItems and lookup table.
		/// </summary>
		internal void UpdateItemCache()
		{
			checkedItemCache = new Dictionary<string, bool>();
			collapsedItemCache = new Dictionary<string, bool>();
			lookupTable = new Dictionary<string, TreeViewItem>();

			foreach (var item in GetAllItems())
			{
				try
				{
					checkedItemCache.Add(item.KeyValue, item.IsChecked);
					if (item.SupportsLazyLoading) collapsedItemCache.Add(item.KeyValue, item.IsCollapsed);
					lookupTable.Add(item.KeyValue, item);
				}
				catch(Exception e)
				{
					throw new TreeViewDuplicateItemsException(item.KeyValue, e);
				}
			}
		}

		/// <summary>
		/// Returns all items in the TreeView that are checked.
		/// </summary>
		/// <returns>All checked TreeViewItems in the TreeView.</returns>
		private IEnumerable<TreeViewItem> GetCheckedItems()
		{
			return lookupTable.Values.Where(x => x.ItemType == TreeViewItem.TreeViewItemType.CheckBox && x.IsChecked);
		}

		/// <summary>
		/// Iterates over all items in the tree and returns them in a flat collection.
		/// </summary>
		/// <returns>A flat collection containing all items in the tree view.</returns>
		public IEnumerable<TreeViewItem> GetAllItems()
		{
			List<TreeViewItem> allItems = new List<TreeViewItem>();
			foreach(var item in Items)
			{
				allItems.Add(item);
				allItems.AddRange(GetAllItems(item.ChildItems));
			}

			return allItems;
		}

		/// <summary>
		/// This method is used to recursively go through all the items in the TreeView.
		/// </summary>
		/// <param name="children">List of TreeViewItems to be visited.</param>
		/// <returns>Flat collection containing every item in the provided children collection and all underlying items.</returns>
		private IEnumerable<TreeViewItem> GetAllItems(IEnumerable<TreeViewItem> children)
		{
			List<TreeViewItem> allItems = new List<TreeViewItem>();
			foreach(var item in children)
			{
				allItems.Add(item);
				allItems.AddRange(GetAllItems(item.ChildItems));
			}

			return allItems;
		}

		/// <summary>
		/// Returns all items in the tree view that are located at the provided depth.
		/// Whenever the requested depth is greater than the longest branch in the tree, an empty collection will be returned.
		/// </summary>
		/// <param name="depth">Depth of the requested items.</param>
		/// <returns>All items in the tree view that are located at the provided depth.</returns>
		public IEnumerable<TreeViewItem> GetItems(int depth)
		{
			return GetItems(Items, depth, 0);
		}

		/// <summary>
		/// Returns all TreeViewItems in the TreeView that are located on the provided depth.
		/// </summary>
		/// <param name="children">Items to be checked.</param>
		/// <param name="requestedDepth">Depth that was requested.</param>
		/// <param name="currentDepth">Current depth in the tree.</param>
		/// <returns>All TreeViewItems in the TreeView that are located on the provided depth.</returns>
		private IEnumerable<TreeViewItem> GetItems(IEnumerable<TreeViewItem> children, int requestedDepth, int currentDepth)
		{
			List<TreeViewItem> requestedItems = new List<TreeViewItem>();
			bool depthReached = requestedDepth == currentDepth;
			foreach (TreeViewItem item in children)
			{
				if (depthReached)
				{
					requestedItems.Add(item);
				}
				else
				{
					int newDepth = currentDepth + 1;
					requestedItems.AddRange(GetItems(item.ChildItems, requestedDepth, newDepth));
				}
			}

			return requestedItems;
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

		internal override void LoadResult(UIResults uiResults)
		{
			var checkedItemKeys = uiResults.GetCheckedItemKeys(this); // this includes all checked items
			var expandedItemKeys = uiResults.GetExpandedItemKeys(this); // this includes all expanded items with LazyLoading set to true

			// Check for changes
			// Expanded Items
			List<string> newlyExpandedItems = collapsedItemCache.Where(x => expandedItemKeys.Contains(x.Key) && x.Value).Select(x => x.Key).ToList();
			if (newlyExpandedItems.Any() && OnExpanded != null)
			{
				itemsExpanded = true;
				expandedItems = new List<TreeViewItem>();

				foreach (string newlyExpandedItemKey in newlyExpandedItems)
				{
					expandedItems.Add(lookupTable[newlyExpandedItemKey]);
				}
			}

			// Collapsed Items
			List<string> newlyCollapsedItems = collapsedItemCache.Where(x => !expandedItemKeys.Contains(x.Key) && !x.Value).Select(x => x.Key).ToList();
			if (newlyCollapsedItems.Any() && OnCollapsed != null)
			{
				itemsCollapsed = true;
				collapsedItems = new List<TreeViewItem>();

				foreach (string newyCollapsedItemKey in newlyCollapsedItems)
				{
					collapsedItems.Add(lookupTable[newyCollapsedItemKey]);
				}
			}

			// Checked Items
			List<string> newlyCheckedItemKeys = checkedItemCache.Where(x => checkedItemKeys.Contains(x.Key) && !x.Value).Select(x => x.Key).ToList();
			if (newlyCheckedItemKeys.Any() && OnChecked != null)
			{
				itemsChecked = true;
				checkedItems = new List<TreeViewItem>();

				foreach (string newlyCheckedItemKey in newlyCheckedItemKeys)
				{
					checkedItems.Add(lookupTable[newlyCheckedItemKey]);
				}
			}
			
			// Unchecked Items
			List<string> newlyUncheckedItemKeys = checkedItemCache.Where(x => !checkedItemKeys.Contains(x.Key) && x.Value).Select(x => x.Key).ToList();
			if (newlyUncheckedItemKeys.Any() && OnUnchecked != null)
			{
				itemsUnchecked = true;
				uncheckedItems = new List<TreeViewItem>();

				foreach (string newlyUncheckedItemKey in newlyUncheckedItemKeys)
				{
					uncheckedItems.Add(lookupTable[newlyUncheckedItemKey]);
				}
			}

			// Changed Items
			List<string> changedItemKeys = new List<string>();
			changedItemKeys.AddRange(newlyCheckedItemKeys);
			changedItemKeys.AddRange(newlyUncheckedItemKeys);
			if(changedItemKeys.Any() && OnChanged != null)
			{
				itemsChanged = true;
				changedItems = new List<TreeViewItem>();

				foreach (string changedItemKey in changedItemKeys)
				{
					changedItems.Add(lookupTable[changedItemKey]);
				}
			}
			
			// Persist states
			foreach (TreeViewItem item in lookupTable.Values)
			{
				item.IsChecked = checkedItemKeys.Contains(item.KeyValue);
				item.IsCollapsed = !expandedItemKeys.Contains(item.KeyValue);
			}

			UpdateItemCache();
		}

		/// <inheritdoc />
		internal override void RaiseResultEvents()
		{
			// Expanded items
			if (itemsExpanded && OnExpanded != null) OnExpanded(this, expandedItems);

			// Collapsed items
			if (itemsCollapsed && OnCollapsed != null) OnCollapsed(this, collapsedItems);

			// Checked items
			if (itemsChecked && OnChecked != null) OnChecked(this, checkedItems);

			// Unchecked items
			if (itemsUnchecked && OnUnchecked != null) OnUnchecked(this, uncheckedItems);

			// Changed items
			if (itemsChanged && OnChanged != null) OnChanged(this, changedItems);

			itemsExpanded = false;
			itemsCollapsed = false;
			itemsChecked = false;
			itemsUnchecked = false;
			itemsChanged = false;

			UpdateItemCache();
		}
	}

///<summary>
///This exception is used to indicate that a tree view contains multiple items with the same key.
///</summary>
public class TreeViewDuplicateItemsException : Exception
{
		/// <summary>
		/// Initializes a new instance of the <see cref="TreeViewDuplicateItemsException"/> class.
		/// </summary>
		public TreeViewDuplicateItemsException()
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TreeViewDuplicateItemsException"/> class with a specified error message.
		/// </summary>
		/// <param name="key">The key of the duplicate tree view items.</param>
		public TreeViewDuplicateItemsException(string key) : base(String.Format("An item with key {0} is already present in the TreeView", key))
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TreeViewDuplicateItemsException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="key">The key of the duplicate tree view items.</param>
		/// <param name="inner">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
		public TreeViewDuplicateItemsException(string key, Exception inner) : base(String.Format("An item with key {0} is already present in the TreeView", key), inner)
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TreeViewDuplicateItemsException"/> class with the serialized data.
		/// </summary>
		/// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual information about the source or destination.</param>
		protected TreeViewDuplicateItemsException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}

internal static class UiResultsExtensions
{
		public static bool GetChecked(this UIResults uiResults, CheckBox checkBox)
		{
			return uiResults.GetChecked(checkBox.DestVar);
		}

		public static DateTime GetDateTime(this UIResults uiResults, DateTimePicker dateTimePicker)
		{
			return uiResults.GetDateTime(dateTimePicker.DestVar);
		}

		public static string GetString(this UIResults uiResults, InteractiveWidget interactiveWidget)
		{
			return uiResults.GetString(interactiveWidget.DestVar);
		}

		public static string GetUploadedFilePath(this UIResults uiResults, InteractiveWidget interactiveWidget)
		{
			return uiResults.GetUploadedFilePath(interactiveWidget.DestVar);
		}

		public static bool WasButtonPressed(this UIResults uiResults, Button button)
		{
			return uiResults.WasButtonPressed(button.DestVar);
		}

		public static bool WasCollapseButtonPressed(this UIResults uiResults, CollapseButton button)
		{
			return uiResults.WasButtonPressed(button.DestVar);
		}

		public static bool WasOnChange(this UIResults uiResults, InteractiveWidget interactiveWidget)
		{
			return uiResults.WasOnChange(interactiveWidget.DestVar);
		}

		public static TimeSpan GetTime(this UIResults uiResults, Time time)
		{
			string receivedTime = uiResults.GetString(time);
			TimeSpan result;

			// This try catch is here because of a bug in Dashboards
			// The string that is received from Dashboards is a DateTime (e.g. 2021-11-16T00:00:16.0000000Z), while the string from Cube is an actual TimeSpan (e.g. 1.06:00:03).
			// This means that when using the Time component from Dashboards, you are restricted to 24h and can only enter HH:mm times.
			// See task: 171211
			if (TimeSpan.TryParse(receivedTime, out result))
			{
				return result;
			}
			else
			{
				return DateTime.Parse(receivedTime, CultureInfo.InvariantCulture).TimeOfDay;
			}
		}

		public static TimeSpan GetTime(this UIResults uiResults, TimePicker time)
		{
			return DateTime.Parse(uiResults.GetString(time), CultureInfo.InvariantCulture).TimeOfDay;
		}

		public static IEnumerable<string> GetExpandedItemKeys(this UIResults uiResults, TreeView treeView)
		{
			string[] expandedItems = uiResults.GetExpanded(treeView.DestVar);
			if (expandedItems == null) return new string[0];
			return expandedItems.Where(x => !String.IsNullOrWhiteSpace(x)).ToList();
		}

		public static IEnumerable<string> GetCheckedItemKeys(this UIResults uiResults, TreeView treeView)
		{
			string result = uiResults.GetString(treeView.DestVar);
			if (String.IsNullOrEmpty(result)) return new string[0];
			return result.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
		}
	}

///<summary>
///A whitespace.
///</summary>
public class WhiteSpace : Widget
{
		/// <summary>
		/// Initializes a new instance of the <see cref="WhiteSpace"/> class.
		/// </summary>
		public WhiteSpace()
		{
			Type = UIBlockType.StaticText;
			BlockDefinition.Style = null;
			BlockDefinition.Text = String.Empty;
		}
	}

///<summary>
///Base class for widgets.
///</summary>
public class Widget
{
		private UIBlockDefinition blockDefinition = new UIBlockDefinition();

		/// <summary>
		/// Initializes a new instance of the Widget class.
		/// </summary>
		protected Widget()
		{
			Type = UIBlockType.Undefined;
			IsVisible = true;
			SetHeightAuto();
			SetWidthAuto();
		}

		/// <summary>
		///     Gets or sets the fixed height (in pixels) of the widget.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
		public int Height
		{
			get
			{
				return BlockDefinition.Height;
			}

			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				BlockDefinition.Height = value;
			}
		}

		/// <summary>
		///     Gets or sets a value indicating whether the widget is visible in the dialog.
		/// </summary>
		public bool IsVisible { get; set; }

		/// <summary>
		///     Gets or sets the maximum height (in pixels) of the widget.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
		public int MaxHeight
		{
			get
			{
				return BlockDefinition.MaxHeight;
			}

			set
			{
				if (value <= -2)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				BlockDefinition.MaxHeight = value;
			}
		}

		/// <summary>
		///     Gets or sets the maximum width (in pixels) of the widget.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
		public int MaxWidth
		{
			get
			{
				return BlockDefinition.MaxWidth;
			}

			set
			{
				if (value <= -2)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				BlockDefinition.MaxWidth = value;
			}
		}

		/// <summary>
		///     Gets or sets the minimum height (in pixels) of the widget.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
		public int MinHeight
		{
			get
			{
				return BlockDefinition.MinHeight;
			}

			set
			{
				if (value <= -2)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				BlockDefinition.MinHeight = value;
			}
		}

		/// <summary>
		///     Gets or sets the minimum width (in pixels) of the widget.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
		public int MinWidth
		{
			get
			{
				return BlockDefinition.MinWidth;
			}

			set
			{
				if (value <= -2)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				BlockDefinition.MinWidth = value;
			}
		}

		/// <summary>
		///     Gets or sets the UIBlockType of the widget.
		/// </summary>
		public UIBlockType Type
		{
			get
			{
				return BlockDefinition.Type;
			}

			protected set
			{
				BlockDefinition.Type = value;
			}
		}

		/// <summary>
		///     Gets or sets the fixed width (in pixels) of the widget.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">When the value is smaller than 1.</exception>
		public int Width
		{
			get
			{
				return BlockDefinition.Width;
			}

			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				BlockDefinition.Width = value;
			}
		}

		/// <summary>
		/// Margin of the widget.
		/// </summary>
		public Margin Margin
		{
			get
			{
				return new Margin(BlockDefinition.Margin);
			}

			set
			{
				BlockDefinition.Margin = value.ToString();
			}
		}

		internal UIBlockDefinition BlockDefinition
		{
			get
			{
				return blockDefinition;
			}
		}

		/// <summary>
		///     Set the height of the widget based on its content.
		/// </summary>
		public void SetHeightAuto()
		{
			BlockDefinition.Height = -1;
			BlockDefinition.MaxHeight = -1;
			BlockDefinition.MinHeight = -1;
		}

		/// <summary>
		///     Set the width of the widget based on its content.
		/// </summary>
		public void SetWidthAuto()
		{
			BlockDefinition.Width = -1;
			BlockDefinition.MaxWidth = -1;
			BlockDefinition.MinWidth = -1;
		}

		/// <summary>
		/// Ugly method to clear the internal list of DropDown items that can't be accessed.
		/// </summary>
		protected void RecreateUiBlock()
		{
			UIBlockDefinition newUiBlockDefinition = new UIBlockDefinition();
			PropertyInfo[] propertyInfo = typeof(UIBlockDefinition).GetProperties();

			foreach (PropertyInfo property in propertyInfo)
			{
				if (property.CanWrite)
				{
					property.SetValue(newUiBlockDefinition, property.GetValue(blockDefinition));
				}
			}

			blockDefinition = newUiBlockDefinition;
		}
	}

///<inheritdoc />
public class WidgetLayout : IWidgetLayout
{
		private int column;
		private int columnSpan;
		private Margin margin;
		private int row;
		private int rowSpan;

		/// <summary>
		/// Initializes a new instance of the <see cref="WidgetLayout"/> class.
		/// </summary>
		/// <param name="fromRow">Row index of top-left cell.</param>
		/// <param name="fromColumn">Column index of the top-left cell.</param>
		/// <param name="rowSpan">Number of vertical cells the widget spans across.</param>
		/// <param name="columnSpan">Number of horizontal cells the widget spans across.</param>
		/// <param name="horizontalAlignment">Horizontal alignment of the widget.</param>
		/// <param name="verticalAlignment">Vertical alignment of the widget.</param>
		public WidgetLayout(
			int fromRow,
			int fromColumn,
			int rowSpan,
			int columnSpan,
			HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left,
			VerticalAlignment verticalAlignment = VerticalAlignment.Top)
		{
			Row = fromRow;
			Column = fromColumn;
			RowSpan = rowSpan;
			ColumnSpan = columnSpan;
			HorizontalAlignment = horizontalAlignment;
			VerticalAlignment = verticalAlignment;
			Margin = new Margin();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="WidgetLayout"/> class.
		/// </summary>
		/// <param name="row">Row index of the cell where the widget is placed.</param>
		/// <param name="column">Column index of the cell where the widget is placed.</param>
		/// <param name="horizontalAlignment">Horizontal alignment of the widget.</param>
		/// <param name="verticalAlignment">Vertical alignment of the widget.</param>
		public WidgetLayout(
			int row,
			int column,
			HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left,
			VerticalAlignment verticalAlignment = VerticalAlignment.Top) : this(
			row,
			column,
			1,
			1,
			horizontalAlignment,
			verticalAlignment)
		{
		}

		/// <summary>
		///     Gets or sets the column location of the widget on the grid.
		/// </summary>
		public int Column
		{
			get
			{
				return column;
			}

			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				column = value;
			}
		}

		/// <summary>
		///     Gets or sets how many columns the widget spans on the grid.
		/// </summary>
		public int ColumnSpan
		{
			get
			{
				return columnSpan;
			}

			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				columnSpan = value;
			}
		}

		/// <inheritdoc />
		public HorizontalAlignment HorizontalAlignment { get; set; }

		/// <inheritdoc />
		public Margin Margin
		{
			get
			{
				return margin;
			}

			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}

				margin = value;
			}
		}

		/// <summary>
		///     Gets or sets the row location of the widget on the grid.
		/// </summary>
		public int Row
		{
			get
			{
				return row;
			}

			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				row = value;
			}
		}

		/// <summary>
		///     Gets or sets how many rows the widget spans on the grid.
		/// </summary>
		public int RowSpan
		{
			get
			{
				return rowSpan;
			}

			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				rowSpan = value;
			}
		}

		/// <inheritdoc />
		public VerticalAlignment VerticalAlignment { get; set; }

		/// <inheritdoc />
		public override bool Equals(object obj)
		{
			WidgetLayout other = obj as WidgetLayout;
			if (other == null) return false;

			bool rowMatch = Row.Equals(other.Row);
			bool columnMatch = Column.Equals(other.Column);
			bool rowSpanMatch = RowSpan.Equals(other.RowSpan);
			bool columnSpanMatch = ColumnSpan.Equals(other.ColumnSpan);
			bool horizontalAlignmentMatch = HorizontalAlignment.Equals(other.HorizontalAlignment);
			bool verticalAlignmentMatch = VerticalAlignment.Equals(other.VerticalAlignment);

			bool rowParamsMatch = rowMatch && rowSpanMatch;
			bool columnParamsMatch = columnMatch && columnSpanMatch;
			bool alignmentParamsMatch = horizontalAlignmentMatch && verticalAlignmentMatch;

			return rowParamsMatch && columnParamsMatch && alignmentParamsMatch;
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			return Row ^ Column ^ RowSpan ^ ColumnSpan ^ (int)HorizontalAlignment ^ (int)VerticalAlignment;
		}
	}

}

