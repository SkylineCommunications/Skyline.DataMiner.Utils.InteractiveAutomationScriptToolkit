namespace InteractiveAutomationToolkitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Skyline.DataMiner.Net.AutomationUI.Objects;
    using Skyline.DataMiner.Utils.InteractiveAutomationScript;

    [TestClass]
    public class ComponentTests
    {
        /// <summary>
        /// Checks if the methods to manipulate the list of options on a dropdown are working as expected.
        /// </summary>
        [TestMethod]
        public void DropDownSetOptionsSelected()
        {
            string[] options = new string[] { "option1", "option2", "option3" };

            DropDown dropDown1 = new DropDown(options);
            Assert.AreEqual("option1", dropDown1.Selected);

            DropDown dropDown2 = new DropDown();
            dropDown2.SetOptions(options);
            Assert.AreEqual("option1", dropDown2.Selected);

            dropDown1.RemoveOption("option1");
            Assert.AreNotEqual("option1", dropDown1.Selected);

            dropDown1.SetOptions(options);
            Assert.AreEqual("option2", dropDown1.Selected);
        }

        [TestMethod]
        public void TestWidgetMargins()
        {
            Button button = new Button("Button");
            Assert.AreEqual(0, button.Margin.Left);

            button.Margin = new Margin(10, 5, 2, 1);
            Assert.AreEqual(2, button.Margin.Right);
        }

        [TestMethod]
        public void TestSection()
        {
            TestSection section = new TestSection();
            section.AddWidget(new Label("Label 1"), 0, 0);
            section.AddWidget(new Label("Label 2"), 1, 0);

            Assert.AreEqual(2, section.RowCount);
            Assert.AreEqual(1, section.ColumnCount);

            section.AddWidget(new Label("Label 3"), 3, 1);

            Assert.AreEqual(4, section.RowCount);
            Assert.AreEqual(2, section.ColumnCount);

            Assert.AreEqual(3, section.Widgets.Count());

            section.Clear();

            Assert.AreEqual(0, section.Widgets.Count());
        }

        [TestMethod]
        public void RemoveWidgetsFromSection()
        {
            TestSection section = new TestSection();
            Label label1 = new Label("Label 1");
            Label label2 = new Label("Label 2");

            section.AddWidget(label1, 0, 0);
            section.AddWidget(label2, 1, 0);

            Assert.AreEqual(2, section.Widgets.Count());
            Assert.AreEqual(2, section.RowCount);
            Assert.AreEqual(1, section.ColumnCount);

            section.RemoveWidget(label2);
            Assert.AreEqual(1, section.Widgets.Count());
            Assert.AreEqual(1, section.RowCount);
            Assert.AreEqual(1, section.ColumnCount);

            section.RemoveWidget(label1);
            Assert.AreEqual(0, section.Widgets.Count());
            Assert.AreEqual(0, section.RowCount);
            Assert.AreEqual(0, section.ColumnCount);
        }

        [TestMethod]
        public void RecreateUiBlockTest()
        {
            Exception exception = null;
            try
            {
                string[] options = new string[] { "option 1", "option 2", "option 3" };
                DropDown dropDown = new DropDown();
                dropDown.RemoveOption(options.First());

                dropDown.SetOptions(new string[] { "option 4", "option 5", "option 6" });
            }
            catch (Exception e)
            {
                exception = e;
            }

            Assert.IsNull(exception);
        }

        [TestMethod]
        public void FindTreeViewItem()
        {
            TreeView treeView = new TreeView(new[] {
                new TreeViewItem("thomas", "thomasKey", new List<TreeViewItem>(new [] {
                    new TreeViewItem("thomasItem1", "thomasItem1Key", new List<TreeViewItem>(new [] {
                        new TreeViewItem("thomasItem11", "thomasItem11Key"),
                        new TreeViewItem("thomasItem12", "thomasItem12Key") }
                    )) })),
                new TreeViewItem("brian", "brianKey", new List<TreeViewItem>(new [] {
                    new TreeViewItem("brianItem1", "brianItem1Key")}))});

            TreeViewItem brianItem1;
            bool brianItem1Found = treeView.TryFindTreeViewItem("brianItem1Key", out brianItem1);
            Assert.IsNotNull(brianItem1);
            Assert.IsTrue(brianItem1Found);

            TreeViewItem thomasItem12;
            bool thomasItem12Found = treeView.TryFindTreeViewItem("thomasItem12Key", out thomasItem12);
            Assert.IsNotNull(thomasItem12);
            Assert.IsTrue(thomasItem12Found);

            TreeViewItem thomasItem1;
            bool thomasItem1Found = treeView.TryFindTreeViewItem("thomasItem1Key", out thomasItem1);
            Assert.IsNotNull(thomasItem1);
            Assert.IsTrue(thomasItem1Found);

            TreeViewItem thomasItem;
            bool thomasItemFound = treeView.TryFindTreeViewItem("thomasKey", out thomasItem);
            Assert.IsNotNull(thomasItem);
            Assert.IsTrue(thomasItemFound);

            TreeViewItem randomItem;
            bool randomItemFound = treeView.TryFindTreeViewItem("randomItemKey", out randomItem);
            Assert.IsNull(randomItem);
            Assert.IsFalse(randomItemFound);
        }

        [TestMethod]
        public void FindTreeViewItemDepth()
        {
            TreeView treeView = new TreeView(new[] {
                new TreeViewItem("thomas", "thomasKey", new List<TreeViewItem>(new [] {
                    new TreeViewItem("thomasItem1", "thomasItem1Key", new List<TreeViewItem>(new [] {
                        new TreeViewItem("thomasItem11", "thomasItem11Key"),
                        new TreeViewItem("thomasItem12", "thomasItem12Key") }
                    )) })),
                new TreeViewItem("brian", "brianKey", new List<TreeViewItem>(new [] {
                    new TreeViewItem("brianItem1", "brianItem1Key")}))});

            List<TreeViewItem> itemsOnDepth0 = new List<TreeViewItem>(treeView.GetItems(0));
            Assert.AreEqual(2, itemsOnDepth0.Count);

            List<TreeViewItem> itemsOnDepth1 = new List<TreeViewItem>(treeView.GetItems(1));
            Assert.AreEqual(2, itemsOnDepth1.Count);

            List<TreeViewItem> itemsOnDepth2 = new List<TreeViewItem>(treeView.GetItems(2));
            Assert.AreEqual(2, itemsOnDepth2.Count);

            List<TreeViewItem> itemsOnDepth3 = new List<TreeViewItem>(treeView.GetItems(3));
            Assert.AreEqual(0, itemsOnDepth3.Count);
        }
    }

    public class TestSection : Section
    {
    }
}
