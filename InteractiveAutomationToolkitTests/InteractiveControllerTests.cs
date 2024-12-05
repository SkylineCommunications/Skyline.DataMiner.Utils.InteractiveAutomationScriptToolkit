using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Skyline.DataMiner.Automation;

namespace Skyline.DataMiner.Utils.InteractiveAutomationScript.Tests
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

            Assert.IsTrue(dialog.ShowScriptAbortPopup);
            Assert.IsFalse(uiBuilder.SkipAbortConfirmation);
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

        //[TestMethod]
        public void ShowDialogTest()
        {
            var mockedEngine = new Mock<IEngine>();
            mockedEngine.Setup(x => x.ShowUI(It.IsAny<UIBuilder>())).Returns(new UIResults());
            mockedEngine.Setup(x => x.ShowUI(It.IsAny<string>())).Returns(new UIResults());
            mockedEngine.Setup(x => x.ShowUI(It.IsAny<string>(), It.IsAny<bool>())).Returns(new UIResults());

            var dialog = new TestDialog(mockedEngine.Object);

            var controller = new InteractiveController(mockedEngine.Object);

            controller.ShowDialog(dialog);

            Assert.IsTrue(dialog.ShowScriptAbortPopup);
        }
    }

    public class TestDialog : Dialog
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