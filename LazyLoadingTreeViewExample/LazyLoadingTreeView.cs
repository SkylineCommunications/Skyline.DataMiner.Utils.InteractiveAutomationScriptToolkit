using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Skyline.DataMiner.Automation;
using Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit;
using Skyline.DataMiner.Net.AutomationUI.Objects;

/// <summary>
/// DataMiner Script Class.
/// Engine.ShowUI();
/// </summary>
public class Script
{
	private InteractiveController app;

	/// <summary>
	/// The Script entry point.
	/// </summary>
	/// <param name="engine">Link with SLScripting process.</param>
	public void Run(Engine engine)
	{
		try
		{
			engine.SetFlag(RunTimeFlags.NoKeyCaching);
			RunSafe(engine);
		}
		catch (ScriptAbortException)
		{
			throw;
		}
		catch (Exception e)
		{
			engine.Log("Run|Something went wrong: " + e);
			ShowExceptionDialog(engine, e);
		}
	}

	private void RunSafe(Engine engine)
	{
		app = new InteractiveController(engine);

		FileSelectorDialog fileSelectorDialog = new FileSelectorDialog(engine, @"C:\Skyline DataMiner");
		fileSelectorDialog.TreeView.Changed += (sender, args) => { fileSelectorDialog.SelectedFilesTextBox.Text = String.Join(Environment.NewLine, fileSelectorDialog.TreeView.CheckedItems.Select(x => x.DisplayValue)); };
		fileSelectorDialog.TreeView.Expanded += TreeView_Expanded;
		app.Run(fileSelectorDialog);
	}

	private void TreeView_Expanded(object sender, IEnumerable<TreeViewItem> e)
	{
		foreach (var item in e)
		{
			string[] directories = Directory.GetDirectories(item.KeyValue);
			string[] files = Directory.GetFiles(item.KeyValue);

			foreach(string directory in directories)
			{
				item.ChildItems.Add(new TreeViewItem(directory.Split(Path.DirectorySeparatorChar).Last(), directory) { ItemType = TreeViewItem.TreeViewItemType.Empty, IsCollapsed = true, SupportsLazyLoading = true });
			}

			foreach(string file in files)
			{
				item.ChildItems.Add(new TreeViewItem(Path.GetFileName(file), file) { ItemType = TreeViewItem.TreeViewItemType.CheckBox });
			}

			item.SupportsLazyLoading = false;
		}
	}

	private void ShowExceptionDialog(Engine engine, Exception exception)
	{
		ExceptionDialog dialog = new ExceptionDialog(engine, exception);
		dialog.OkButton.Pressed += (sender, args) => engine.ExitSuccess("Something went wrong during the creation of the new event.");
		if (app.IsRunning) app.ShowDialog(dialog); else app.Run(dialog);
	}
}

public class FileSelectorDialog : Dialog
{
	public FileSelectorDialog(Engine engine, string rootPath) : base(engine)
	{
		TreeView = new TreeView(new [] { new TreeViewItem(rootPath, rootPath) { IsCollapsed = true, SupportsLazyLoading = true, ItemType = TreeViewItem.TreeViewItemType.Empty } } );
		SelectedFilesTextBox = new TextBox { IsMultiline = true, Height = 250, Width = 500 };
		ExitButton = new Button("Exit");

		AddWidget(TreeView, 0, 0);
		AddWidget(SelectedFilesTextBox, 1, 0);
		AddWidget(ExitButton, 2, 0);
	}

	public TreeView TreeView { get; private set; }

	public TextBox SelectedFilesTextBox { get; private set; }

	public Button ExitButton { get; private set; }
}