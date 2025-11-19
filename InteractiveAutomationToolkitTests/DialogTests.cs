namespace InteractiveAutomationToolkitTests
{
	using Microsoft.Extensions.Logging;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Moq;
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;
	using System;
	using System.Collections.Generic;
	using System.Linq;

	[TestClass]
	public class DialogTests
	{
		/// <summary>
		/// This test will assign a single label to a cell in the dialog.
		/// </summary>
		[TestMethod]
		public void AddSingleWidgetCheckColumnCount()
		{
			EmptyDialog testDialog = new EmptyDialog(new Engine());
			testDialog.AddWidget(new Label("Label1"), 0, 0);
			Assert.AreEqual(1, testDialog.ColumnCount);
		}

		/// <summary>
		/// This test will add two labels without spanning on the same row of the dialog and remove them one by one.
		/// </summary>
		[TestMethod]
		public void AddAndRemoveWidgetsCheckColumnCount()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2");

			EmptyDialog testDialog = new EmptyDialog(new Engine());
			testDialog.AddWidget(label1, 0, 0);
			testDialog.AddWidget(label2, 0, 1);

			Assert.AreEqual(2, testDialog.ColumnCount);

			testDialog.RemoveWidget(label1);
			Assert.AreEqual(1, testDialog.ColumnCount);

			testDialog.RemoveWidget(label2);
			Assert.AreEqual(0, testDialog.ColumnCount);
		}

		/// <summary>
		/// This test will add two labels that span multiple columns on different rows.
		/// </summary>
		[TestMethod]
		public void AddAndRemoveSpanningWidgetsCheckColumnCount()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2");

			EmptyDialog testDialog = new EmptyDialog(new Engine());
			testDialog.AddWidget(label1, 0, 0, 1, 3);
			testDialog.AddWidget(label2, 1, 0, 1, 2);
			Assert.AreEqual(3, testDialog.ColumnCount);

			testDialog.RemoveWidget(label1);
			Assert.AreEqual(2, testDialog.ColumnCount);
		}

		/// <summary>
		/// This test will add two labels that span multiple columns on the same row of the dialog with multiple "empty" columns between them.
		/// </summary>
		[TestMethod]
		public void AddAndRemoveSpanningWidgetsEmptyColumnsCheckColumnCount()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2");

			EmptyDialog testDialog = new EmptyDialog(new Engine());
			testDialog.AddWidget(label1, 0, 0, 1, 3);
			testDialog.AddWidget(label2, 0, 100, 1, 2);
			Assert.AreEqual(5, testDialog.ColumnCount);

			testDialog.RemoveWidget(label1);
			Assert.AreEqual(2, testDialog.ColumnCount);
		}

		/// <summary>
		/// This test will assign a single label to a cell in the dialog.
		/// </summary>
		[TestMethod]
		public void AddSingleWidgetCheckRowCount()
		{
			EmptyDialog testDialog = new EmptyDialog(new Engine());
			testDialog.AddWidget(new Label("Label1"), 0, 0);
			Assert.AreEqual(1, testDialog.RowCount);
		}

		/// <summary>
		/// This test will add two labels without spanning on different rows of the dialog and remove them one by one.
		/// </summary>
		[TestMethod]
		public void AddAndRemoveWidgetsCheckRowCount()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2");

			EmptyDialog testDialog = new EmptyDialog(new Engine());
			testDialog.AddWidget(label1, 0, 0);
			testDialog.AddWidget(label2, 1, 0);

			Assert.AreEqual(2, testDialog.RowCount);

			testDialog.RemoveWidget(label1);
			Assert.AreEqual(1, testDialog.RowCount);

			testDialog.RemoveWidget(label2);
			Assert.AreEqual(0, testDialog.RowCount);
		}

		/// <summary>
		/// This test will add two labels that span multiple rows on different columns.
		/// </summary>
		[TestMethod]
		public void AddAndRemoveSpanningWidgetsCheckRowCount()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2");

			EmptyDialog testDialog = new EmptyDialog(new Engine());
			testDialog.AddWidget(label1, 0, 0, 3, 1);
			testDialog.AddWidget(label2, 0, 1, 2, 1);
			Assert.AreEqual(3, testDialog.RowCount);

			testDialog.RemoveWidget(label1);
			Assert.AreEqual(2, testDialog.RowCount);
		}

		/// <summary>
		/// This test will add two labels that span multiple columns on the same row of the dialog with multiple "empty" rows between them.
		/// </summary>
		[TestMethod]
		public void AddAndRemoveSpanningWidgetsEmptyRowsCheckRowCount()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2");

			EmptyDialog testDialog = new EmptyDialog(new Engine());
			testDialog.AddWidget(label1, 0, 0, 3, 1);
			testDialog.AddWidget(label2, 100, 0, 2, 1);
			Assert.AreEqual(5, testDialog.RowCount);

			testDialog.RemoveWidget(label1);
			Assert.AreEqual(2, testDialog.RowCount);
		}

		/// <summary>
		/// This test will add the same label to a dialog twice and checks if an exception is thrown when the widget is being added for the second time.
		/// </summary>
		[TestMethod]
		public void TryAddSingleWidgetTwice()
		{
			Label label1 = new Label("Label 1");
			EmptyDialog testDialog = new EmptyDialog(new Engine());
			testDialog.AddWidget(label1, 0, 0);

			ArgumentException exception = null;
			try
			{
				testDialog.AddWidget(label1, 0, 1);
			}
			catch (ArgumentException e)
			{
				exception = e;
			}

			Assert.IsNotNull(exception);
		}

		/// <summary>
		/// This test will add two different labels (without spanning) on the same position of the dialog and check if an exception is thrown.
		/// </summary>
		[TestMethod]
		public void TryAddWidgetsSamePosition()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2");
			EmptyDialog testDialog = new EmptyDialog(new Engine());
			testDialog.AddWidget(label1, 0, 0);
			testDialog.AddWidget(label2, 0, 0);

			OverlappingWidgetsException exception = null;
			try
			{
				testDialog.Show();
			}
			catch (OverlappingWidgetsException e)
			{
				exception = e;
			}

			Assert.IsNotNull(exception);
		}

		/// <summary>
		/// This test will add two different overlapping labels (with column spanning) to the dialog and check if an exception is thrown.
		/// </summary>
		[TestMethod]
		public void TryAddOverlappingColumnSpanningWidgets()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2");
			EmptyDialog testDialog = new EmptyDialog(new Engine());
			testDialog.AddWidget(label1, 0, 0, 1, 3);
			testDialog.AddWidget(label2, 0, 2, 1, 2);

			OverlappingWidgetsException exception = null;
			try
			{
				testDialog.Show();
			}
			catch (OverlappingWidgetsException e)
			{
				exception = e;
			}

			Assert.IsNotNull(exception);
		}

		/// <summary>
		/// This test will add two different overlapping labels (with row spanning) to the dialog and check if an exception is thrown.
		/// </summary>
		[TestMethod]
		public void TryAddOverlappingRowSpanningWidgets()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2");
			EmptyDialog testDialog = new EmptyDialog(new Engine());
			testDialog.AddWidget(label1, 0, 0, 3, 1);
			testDialog.AddWidget(label2, 2, 0, 2, 1);

			OverlappingWidgetsException exception = null;
			try
			{
				testDialog.Show();
			}
			catch (OverlappingWidgetsException e)
			{
				exception = e;
			}

			Assert.IsNotNull(exception);
		}

		/// <summary>
		/// This test will add two different overlapping labels (with column and row spanning) to the dialog and check if an exception is thrown.
		/// </summary>
		[TestMethod]
		public void TryAddOverlappingColumnAndRowSpanningWidgets()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2");
			EmptyDialog testDialog = new EmptyDialog(new Engine());
			testDialog.AddWidget(label1, 0, 0, 2, 2);
			testDialog.AddWidget(label2, 1, 1, 2, 3);

			OverlappingWidgetsException exception = null;
			try
			{
				testDialog.Show();
			}
			catch (OverlappingWidgetsException e)
			{
				exception = e;
			}

			Assert.IsNotNull(exception);
		}

		/// <summary>
		/// This test will add two different overlapping invisble labels (with column and row spanning) to the dialog and check if no exception is thrown.
		/// </summary>
		[TestMethod]
		public void TryAddOverlappingInvisisbleColumnAndRowSpanningWidgets()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2") { IsVisible = false };
			EmptyDialog testDialog = new EmptyDialog(new Engine());
			testDialog.AddWidget(label1, 0, 0, 2, 2);
			testDialog.AddWidget(label2, 1, 1, 2, 3);

			OverlappingWidgetsException exception = null;
			try
			{
				testDialog.Show();
			}
			catch (OverlappingWidgetsException e)
			{
				exception = e;
			}
			catch (Exception)
			{
				// Expected exception as no source is available for the Engine provided to the Dialog.
			}

			Assert.IsNull(exception);
		}

		/// <summary>
		/// This test will check if all widgets are removed after the Clear method is called.
		/// </summary>
		[TestMethod]
		public void ClearDialogTest()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2") { IsVisible = false };

			EmptyDialog testDialog = new EmptyDialog(new Engine());
			testDialog.AddWidget(label1, 0, 0, 2, 2);
			testDialog.AddWidget(label2, 1, 1, 2, 3);

			Assert.AreEqual(2, testDialog.Widgets.Count());

			testDialog.Clear();

			Assert.AreEqual(0, testDialog.Widgets.Count());
		}

		[TestMethod]
		public void RequiresResponseTest_OnlyLabels()
		{
			Label label1 = new Label("Label 1");
			Label label2 = new Label("Label 2") { IsVisible = false };

			EmptyDialog testDialog = new EmptyDialog(new Engine());
			testDialog.AddWidget(label1, 0, 0, 2, 2);
			testDialog.AddWidget(label2, 1, 1, 2, 3);

			Assert.IsFalse(testDialog.RequiresResponse);
		}

		[TestMethod]
		public void RequiresResponseTest_InvisibleInteractiveWidgetWithoutEvent()
		{
			Label label = new Label("Label");
			Button button = new Button("Button") { IsVisible = false };

			EmptyDialog testDialog = new EmptyDialog(new Engine());
			testDialog.AddWidget(label, 0, 0, 2, 2);
			testDialog.AddWidget(button, 1, 1, 2, 3);

			Assert.IsFalse(testDialog.RequiresResponse);
		}

		[TestMethod]
		public void RequiresResponseTest_VisibleInteractiveWidgetWithoutEvent()
		{
			Label label = new Label("Label");
			Button button = new Button("Button");

			EmptyDialog testDialog = new EmptyDialog(new Engine());
			testDialog.AddWidget(label, 0, 0, 2, 2);
			testDialog.AddWidget(button, 1, 1, 2, 3);

			Assert.IsFalse(testDialog.RequiresResponse);
		}

		[TestMethod]
		public void RequiresResponseTest_VisibleInteractiveWidgetWithChanged()
		{
			Label label = new Label("Label");
			TextBox textBox = new TextBox();

			textBox.Changed += (s, e) => { /* do nothing */ };

			EmptyDialog testDialog = new EmptyDialog(new Engine());
			testDialog.AddWidget(label, 0, 0, 2, 2);
			testDialog.AddWidget(textBox, 1, 1, 2, 3);

			Assert.IsTrue(testDialog.RequiresResponse);
		}

		[TestMethod]
		public void RequiresResponseTest_VisibleInteractiveWidgetWithFocusLost()
		{
			Label label = new Label("Label");
			TextBox textBox = new TextBox();

			textBox.FocusLost += (s, e) => { /* do nothing */ };

			EmptyDialog testDialog = new EmptyDialog(new Engine());
			testDialog.AddWidget(label, 0, 0, 2, 2);
			testDialog.AddWidget(textBox, 1, 1, 2, 3);

			Assert.IsTrue(testDialog.RequiresResponse);
		}

		[TestMethod]
		public void RequiresResponseTest_VisibleInteractiveWidgetIsReadOnly()
		{
			Label label = new Label("Label");
			TextBox textBox = new TextBox { IsReadOnly = true };

			EmptyDialog testDialog = new EmptyDialog(new Engine());
			testDialog.AddWidget(label, 0, 0, 2, 2);
			testDialog.AddWidget(textBox, 1, 1, 2, 3);

			Assert.IsFalse(testDialog.RequiresResponse);
		}

		[TestMethod]
		public void RequiresResponseTest_VisibleInteractiveWidgetIsDisabled()
		{
			Label label = new Label("Label");
			TextBox textBox = new TextBox { IsEnabled = false };

			EmptyDialog testDialog = new EmptyDialog(new Engine());
			testDialog.AddWidget(label, 0, 0, 2, 2);
			testDialog.AddWidget(textBox, 1, 1, 2, 3);

			Assert.IsFalse(testDialog.RequiresResponse);
		}

		[TestMethod]
		public void RequiresResponseTest_MultipleInteractiveWidgetsWithoutResponse()
		{
			Label label = new Label("Label");
			TextBox textBox1 = new TextBox { IsEnabled = false };
			TextBox textBox2 = new TextBox { IsReadOnly = true };
			TextBox textBox3 = new TextBox { IsVisible = false };
			TextBox textBox4 = new TextBox();

			EmptyDialog testDialog = new EmptyDialog(new Engine());
			testDialog.AddWidget(label, 0, 0, 2, 2);
			testDialog.AddWidget(textBox1, 1, 1, 2, 3);
			testDialog.AddWidget(textBox2, 2, 1, 2, 3);
			testDialog.AddWidget(textBox3, 3, 1, 2, 3);
			testDialog.AddWidget(textBox4, 4, 1, 2, 3);

			Assert.IsFalse(testDialog.RequiresResponse);
		}

		[TestMethod]
		public void RequiresResponseTest_MultipleInteractiveWidgetsWithResponse()
		{
			Label label = new Label("Label");
			TextBox textBox1 = new TextBox { IsEnabled = false };
			TextBox textBox2 = new TextBox { IsReadOnly = true };
			TextBox textBox3 = new TextBox { IsVisible = false };
			TextBox textBox4 = new TextBox();

			textBox4.Changed += (s, e) => { /* do nothing */ };

			EmptyDialog testDialog = new EmptyDialog(new Engine());
			testDialog.AddWidget(label, 0, 0, 2, 2);
			testDialog.AddWidget(textBox1, 1, 1, 2, 3);
			testDialog.AddWidget(textBox2, 2, 1, 2, 3);
			testDialog.AddWidget(textBox3, 3, 1, 2, 3);
			testDialog.AddWidget(textBox4, 4, 1, 2, 3);

			Assert.IsTrue(testDialog.RequiresResponse);
		}

		[TestMethod]
		public void Dialog_ShowWithoutUserInteraction()
		{
			var mockedEngine = new Mock<IEngine>();
			var dialog = new TestDialog(mockedEngine.Object);

			int raisedEvents = 0;
			dialog.Button.Pressed += (s, e) => raisedEvents += 1;
			dialog.Calendar.Changed += (s, e) => raisedEvents += 1;
			dialog.CheckBox.Changed += (s, e) => raisedEvents += 1;
			dialog.CheckBox.Checked += (s, e) => raisedEvents += 1;
			dialog.CheckBox.UnChecked += (s, e) => raisedEvents += 1;
			dialog.CheckBoxList.Changed += (s, e) => raisedEvents += 1;
			dialog.GenericCheckBoxList.Changed += (s, e) => raisedEvents += 1;
			dialog.CollapseButton.Pressed += (s, e) => raisedEvents += 1;
			dialog.DateTimePicker.Changed += (s, e) => raisedEvents += 1;
			dialog.DownloadButton.DownloadStarted += (s, e) => raisedEvents += 1;
			dialog.DropDown.Changed += (s, e) => raisedEvents += 1;
			dialog.GenericDropDown.Changed += (s, e) => raisedEvents += 1;
			dialog.EnumDropDown.Changed += (s, e) => raisedEvents += 1;
			dialog.Numeric.Changed += (s, e) => raisedEvents += 1;
			dialog.RadioButtonList.Changed += (s, e) => raisedEvents += 1;
			dialog.GenericRadioButtonList.Changed += (s, e) => raisedEvents += 1;
			dialog.EnumRadioButtonList.Changed += (s, e) => raisedEvents += 1;
			dialog.TextBox.Changed += (s, e) => raisedEvents += 1;
			dialog.Time.Changed += (s, e) => raisedEvents += 1;
			dialog.TimePicker.Changed += (s, e) => raisedEvents += 1;
			dialog.TreeView.Changed += (s, e) => raisedEvents += 1;
			dialog.GenericTreeView.Changed += (s, e) => raisedEvents += 1;

			var mockedUiResults = new Mock<IUIResults>();
			mockedUiResults.Setup(x => x.WasButtonPressed(dialog.Button.DestVar)).Returns(false);
			mockedUiResults.Setup(x => x.GetDateTime(dialog.Calendar.DestVar)).Returns(dialog.Calendar.DateTime);
			mockedUiResults.Setup(x => x.GetChecked(dialog.CheckBox.DestVar)).Returns(dialog.CheckBox.IsChecked);
			mockedUiResults.Setup(x => x.GetChecked(dialog.CheckBoxList.DestVar, It.IsAny<string>())).Returns(false);
			mockedUiResults.Setup(x => x.WasButtonPressed(dialog.CollapseButton.DestVar)).Returns(false);
			mockedUiResults.Setup(x => x.GetString(dialog.DateTimePicker.DestVar)).Returns(dialog.DateTimePicker.DateTime.ToString("O"));
			mockedUiResults.Setup(x => x.WasOnDownloadStarted(dialog.DownloadButton.DestVar)).Returns(false);
			mockedUiResults.Setup(x => x.GetString(dialog.DropDown.DestVar)).Returns(dialog.DropDown.Selected);
			mockedUiResults.Setup(x => x.GetString(dialog.GenericDropDown.DestVar)).Returns(dialog.GenericDropDown.Selected);
			mockedUiResults.Setup(x => x.GetString(dialog.EnumDropDown.DestVar)).Returns(dialog.EnumDropDown.Selected.ToString());
			mockedUiResults.Setup(x => x.GetUploadedFilePaths(dialog.FileSelector.DestVar)).Returns<string[]>(null);
			mockedUiResults.Setup(x => x.WasOnDownloadStarted(dialog.Hyperlink.DestVar)).Returns(false);
			mockedUiResults.Setup(x => x.GetString(dialog.Numeric.DestVar)).Returns(dialog.Numeric.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
			mockedUiResults.Setup(x => x.GetString(dialog.PasswordBox.DestVar)).Returns(dialog.PasswordBox.Password);
			mockedUiResults.Setup(x => x.GetString(dialog.RadioButtonList.DestVar)).Returns(dialog.RadioButtonList.Selected);
			mockedUiResults.Setup(x => x.GetString(dialog.GenericRadioButtonList.DestVar)).Returns(dialog.GenericRadioButtonList.Selected);
			mockedUiResults.Setup(x => x.GetString(dialog.EnumRadioButtonList.DestVar)).Returns<string>(null);
			mockedUiResults.Setup(x => x.GetString(dialog.TextBox.DestVar)).Returns(dialog.TextBox.Text);
			mockedUiResults.Setup(x => x.GetString(dialog.Time.DestVar)).Returns(dialog.Time.TimeSpan.ToString());
			mockedUiResults.Setup(x => x.GetString(dialog.TimePicker.DestVar)).Returns(dialog.TimePicker.Time.ToString());
			mockedUiResults.Setup(x => x.GetString(dialog.TreeView.DestVar)).Returns<string>(null);
			mockedUiResults.Setup(x => x.GetExpanded(dialog.TreeView.DestVar)).Returns<string[]>(null);
			mockedUiResults.Setup(x => x.GetString(dialog.GenericTreeView.DestVar)).Returns<string>(null);
			mockedUiResults.Setup(x => x.GetExpanded(dialog.GenericTreeView.DestVar)).Returns<string[]>(null);

			dialog.LoadChanges(mockedUiResults.Object);
			dialog.RaiseResultEvents(mockedUiResults.Object);

			Assert.AreEqual(0, raisedEvents);
		}

		private sealed class TestDialog : Dialog
		{
			public TestDialog(IEngine engine)
				: base(engine)
			{
				var widgetsToAdd = new List<Widget>
				{
					Button,
					Calendar,
					CheckBox,
					CheckBoxList,
					GenericCheckBoxList,
					CollapseButton,
					DateTimePicker,
					DownloadButton,
					DropDown,
					GenericDropDown,
					EnumDropDown,
					FileSelector,
					Hyperlink,
					Label,
					Numeric,
					Parameter,
					PasswordBox,
					RadioButtonList,
					GenericRadioButtonList,
					EnumRadioButtonList,
					TextBox,
					Time,
					TimePicker,
					TreeView,
					GenericTreeView,
					WhiteSpace
				};

				for (int i = 0; i < widgetsToAdd.Count; i++)
				{
					AddWidget(widgetsToAdd[i], i, 0);
				}
			}

			public Button Button { get; } = new Button("Test");

			public Calendar Calendar { get; } = new Calendar();

			public CheckBox CheckBox { get; } = new CheckBox("Test");

			public CheckBoxList CheckBoxList { get; } = new CheckBoxList();

			public CheckBoxList<string> GenericCheckBoxList { get; } = new CheckBoxList<string>();

			public CollapseButton CollapseButton { get; } = new CollapseButton();

			public DateTimePicker DateTimePicker { get; } = new DateTimePicker();

			public DownloadButton DownloadButton { get; } = new DownloadButton("Test");

			public DropDown DropDown { get; } = new DropDown();

			public DropDown<string> GenericDropDown { get; } = new DropDown<string>();

			public EnumDropDown<DayOfWeek> EnumDropDown { get; } = new EnumDropDown<DayOfWeek>();

			public FileSelector FileSelector { get; } = new FileSelector();

			public Hyperlink Hyperlink { get; } = new Hyperlink("Test", new Uri("http://example.com"));

			public Label Label { get; } = new Label("Test");

			public Numeric Numeric { get; } = new Numeric();

			public Parameter Parameter { get; } = new Parameter(1, 1, 1);

			public PasswordBox PasswordBox { get; } = new PasswordBox();

			public RadioButtonList RadioButtonList { get; } = new RadioButtonList();

			public RadioButtonList<string> GenericRadioButtonList { get; } = new RadioButtonList<string>();

			public EnumRadioButtonList<DayOfWeek> EnumRadioButtonList { get; } = new EnumRadioButtonList<DayOfWeek>();

			public TextBox TextBox { get; } = new TextBox();

			public Time Time { get; } = new Time();

			public TimePicker TimePicker { get; } = new TimePicker();

			public TreeView TreeView { get; } = new TreeView();

			public TreeView<string> GenericTreeView { get; } = new TreeView<string>();

			public WhiteSpace WhiteSpace { get; } = new WhiteSpace();
		}
	}
}
