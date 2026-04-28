using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Skyline.DataMiner.Automation;
using Skyline.DataMiner.Utils.InteractiveAutomationScript;

namespace InteractiveAutomationToolkitTests
{
	[TestClass]
	public class InteractiveControllerTests
	{
		[TestMethod]
		public void DialogBuild_ShowScriptAbortPopup_DefaultSetting()
		{
			var mockedEngine = new Mock<IEngine>();
			var dialog = new TestDialog(mockedEngine.Object);

			var uiBuilder = dialog.Build();

			Assert.IsFalse(dialog.ShowScriptAbortPopup);
			Assert.IsTrue(uiBuilder.SkipAbortConfirmation);
		}

		[TestMethod]
		public void DialogBuild_ShowScriptAbortPopup_ShowAbortPopup()
		{
			var mockedEngine = new Mock<IEngine>();
			var dialog = new TestDialog(mockedEngine.Object) { ShowScriptAbortPopup = true };

			var uiBuilder = dialog.Build();

			Assert.IsTrue(dialog.ShowScriptAbortPopup);
			Assert.IsFalse(uiBuilder.SkipAbortConfirmation);
		}

		[TestMethod]
		public void DialogBuild_ShowScriptAbortPopup_HideAbortPopup()
		{
			var mockedEngine = new Mock<IEngine>();
			var dialog = new TestDialog(mockedEngine.Object) { ShowScriptAbortPopup = false };

			var uiBuilder = dialog.Build();

			Assert.IsFalse(dialog.ShowScriptAbortPopup);
			Assert.IsTrue(uiBuilder.SkipAbortConfirmation);
		}

		[TestMethod]
		public void ScriptPopupBehavior_Default()
		{
			var mockedEngine = new Mock<IEngine>();
			mockedEngine.Setup(x => x.ShowUI(It.IsAny<UIBuilder>())).Throws(new InvalidOperationException());
			mockedEngine.Setup(x => x.ShowUI(It.IsAny<string>())).Throws(new InvalidOperationException());
			mockedEngine.Setup(x => x.ShowUI(It.IsAny<string>(), It.IsAny<bool>())).Throws(new InvalidOperationException());

			var dialog = new TestDialog(mockedEngine.Object);

			var controller = new InteractiveController(mockedEngine.Object);

			Assert.ThrowsExactly<InvalidOperationException>(() => controller.ShowDialog(dialog));

			Assert.IsFalse(dialog.ShowScriptAbortPopup);
		}

		[TestMethod]
		public void ScriptPopupBehavior_OnDialogLevel_Enabled()
		{
			var mockedEngine = new Mock<IEngine>();
			mockedEngine.Setup(x => x.ShowUI(It.IsAny<UIBuilder>())).Throws(new InvalidOperationException());
			mockedEngine.Setup(x => x.ShowUI(It.IsAny<string>())).Throws(new InvalidOperationException());
			mockedEngine.Setup(x => x.ShowUI(It.IsAny<string>(), It.IsAny<bool>())).Throws(new InvalidOperationException());

			var dialog = new TestDialog(mockedEngine.Object) { ShowScriptAbortPopup = true };

			var controller = new InteractiveController(mockedEngine.Object) { ScriptAbortPopupBehavior = ScriptAbortPopupBehavior.OnDialogLevel };

			Assert.ThrowsExactly<InvalidOperationException>(() => controller.ShowDialog(dialog));

			Assert.IsTrue(dialog.ShowScriptAbortPopup);
		}

		[TestMethod]
		public void ScriptPopupBehavior_OnDialogLevel_Disabled()
		{
			var mockedEngine = new Mock<IEngine>();
			mockedEngine.Setup(x => x.ShowUI(It.IsAny<UIBuilder>())).Throws(new InvalidOperationException());
			mockedEngine.Setup(x => x.ShowUI(It.IsAny<string>())).Throws(new InvalidOperationException());
			mockedEngine.Setup(x => x.ShowUI(It.IsAny<string>(), It.IsAny<bool>())).Throws(new InvalidOperationException());

			var dialog = new TestDialog(mockedEngine.Object) { ShowScriptAbortPopup = false };

			var controller = new InteractiveController(mockedEngine.Object) { ScriptAbortPopupBehavior = ScriptAbortPopupBehavior.OnDialogLevel };

			Assert.ThrowsExactly<InvalidOperationException>(() => controller.ShowDialog(dialog));

			Assert.IsFalse(dialog.ShowScriptAbortPopup);
		}

		[TestMethod]
		public void ScriptPopupBehavior_HideAlways_Enabled()
		{
			var mockedEngine = new Mock<IEngine>();
			mockedEngine.Setup(x => x.ShowUI(It.IsAny<UIBuilder>())).Throws(new InvalidOperationException());
			mockedEngine.Setup(x => x.ShowUI(It.IsAny<string>())).Throws(new InvalidOperationException());
			mockedEngine.Setup(x => x.ShowUI(It.IsAny<string>(), It.IsAny<bool>())).Throws(new InvalidOperationException());

			var dialog = new TestDialog(mockedEngine.Object) { ShowScriptAbortPopup = true };

			var controller = new InteractiveController(mockedEngine.Object) { ScriptAbortPopupBehavior = ScriptAbortPopupBehavior.HideAlways };

			Assert.ThrowsExactly<InvalidOperationException>(() => controller.ShowDialog(dialog));

			Assert.IsFalse(dialog.ShowScriptAbortPopup);
		}

		[TestMethod]
		public void ScriptPopupBehavior_HideAlways_Disabled()
		{
			var mockedEngine = new Mock<IEngine>();
			mockedEngine.Setup(x => x.ShowUI(It.IsAny<UIBuilder>())).Throws(new InvalidOperationException());
			mockedEngine.Setup(x => x.ShowUI(It.IsAny<string>())).Throws(new InvalidOperationException());
			mockedEngine.Setup(x => x.ShowUI(It.IsAny<string>(), It.IsAny<bool>())).Throws(new InvalidOperationException());

			var dialog = new TestDialog(mockedEngine.Object) { ShowScriptAbortPopup = false };

			var controller = new InteractiveController(mockedEngine.Object) { ScriptAbortPopupBehavior = ScriptAbortPopupBehavior.HideAlways };

			Assert.ThrowsExactly<InvalidOperationException>(() => controller.ShowDialog(dialog));

			Assert.IsFalse(dialog.ShowScriptAbortPopup);
		}

		[TestMethod]
		public void ScriptPopupBehavior_ShowAlways_Enabled()
		{
			var mockedEngine = new Mock<IEngine>();
			mockedEngine.Setup(x => x.ShowUI(It.IsAny<UIBuilder>())).Throws(new InvalidOperationException());
			mockedEngine.Setup(x => x.ShowUI(It.IsAny<string>())).Throws(new InvalidOperationException());
			mockedEngine.Setup(x => x.ShowUI(It.IsAny<string>(), It.IsAny<bool>())).Throws(new InvalidOperationException());

			var dialog = new TestDialog(mockedEngine.Object) { ShowScriptAbortPopup = true };

			var controller = new InteractiveController(mockedEngine.Object) { ScriptAbortPopupBehavior = ScriptAbortPopupBehavior.ShowAlways };

			Assert.ThrowsExactly<InvalidOperationException>(() => controller.ShowDialog(dialog));

			Assert.IsTrue(dialog.ShowScriptAbortPopup);
		}

		[TestMethod]
		public void ScriptPopupBehavior_ShowAlways_Disabled()
		{
			var mockedEngine = new Mock<IEngine>();
			mockedEngine.Setup(x => x.ShowUI(It.IsAny<UIBuilder>())).Throws(new InvalidOperationException());
			mockedEngine.Setup(x => x.ShowUI(It.IsAny<string>())).Throws(new InvalidOperationException());
			mockedEngine.Setup(x => x.ShowUI(It.IsAny<string>(), It.IsAny<bool>())).Throws(new InvalidOperationException());

			var dialog = new TestDialog(mockedEngine.Object) { ShowScriptAbortPopup = false };

			var controller = new InteractiveController(mockedEngine.Object) { ScriptAbortPopupBehavior = ScriptAbortPopupBehavior.ShowAlways };

			Assert.ThrowsExactly<InvalidOperationException>(() => controller.ShowDialog(dialog));

			Assert.IsTrue(dialog.ShowScriptAbortPopup);
		}

		private class TestDialog : Dialog
		{
			public TestDialog(IEngine engine) : base(engine)
			{
				ContinueButton = new Button("Continue");
				ContinueButton.Pressed += (s, e) => throw new System.Exception(); // Causes the InterActiveController.Run loop to be broken

				AddWidget(ContinueButton, 0, 0);
			}

			public Button ContinueButton { get; set; }
		}
	}
}