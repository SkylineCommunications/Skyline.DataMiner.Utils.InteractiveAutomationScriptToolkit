namespace InteractiveAutomationToolkitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit;
    using Skyline.DataMiner.Net.AutomationUI.Objects;

    [TestClass]
    public class FocusLostTests
    {
        [TestMethod]
        public void CalendarFocusLost()
        {
            Calendar calendar = new Calendar();
            calendar.FocusLost += Calendar_FocusLost;

            Assert.IsFalse(calendar.BlockDefinition.WantsOnChange);
            Assert.IsTrue(calendar.BlockDefinition.WantsOnFocusLost);

            calendar.FocusLost -= Calendar_FocusLost;

            Assert.IsFalse(calendar.BlockDefinition.WantsOnChange);
            Assert.IsFalse(calendar.BlockDefinition.WantsOnFocusLost);

            calendar.FocusLost += Calendar_FocusLost;
            calendar.Changed += Calendar_Changed;

            Assert.IsTrue(calendar.BlockDefinition.WantsOnChange);
            Assert.IsTrue(calendar.BlockDefinition.WantsOnFocusLost);

            calendar.Changed -= Calendar_Changed;

            Assert.IsFalse(calendar.BlockDefinition.WantsOnChange);
            Assert.IsTrue(calendar.BlockDefinition.WantsOnFocusLost);

            calendar.Changed += Calendar_Changed;
            calendar.FocusLost -= Calendar_FocusLost;

            Assert.IsTrue(calendar.BlockDefinition.WantsOnChange);
            Assert.IsFalse(calendar.BlockDefinition.WantsOnFocusLost);
        }

        private void Calendar_Changed(object sender, Calendar.CalendarChangedEventArgs e)
        {
            // Nothing to do
        }

        private void Calendar_FocusLost(object sender, EventArgs e)
        {
            // Nothing to do
        }

        [TestMethod]
        public void CheckBoxFocusLost()
        {
            CheckBox checkBox = new CheckBox();
            checkBox.FocusLost += CheckBox_FocusLost;

            Assert.IsFalse(checkBox.BlockDefinition.WantsOnChange);
            Assert.IsTrue(checkBox.BlockDefinition.WantsOnFocusLost);

            checkBox.FocusLost -= CheckBox_FocusLost;

            Assert.IsFalse(checkBox.BlockDefinition.WantsOnChange);
            Assert.IsFalse(checkBox.BlockDefinition.WantsOnFocusLost);

            checkBox.FocusLost += CheckBox_FocusLost;
            checkBox.Changed += CheckBox_Changed;

            Assert.IsTrue(checkBox.BlockDefinition.WantsOnChange);
            Assert.IsTrue(checkBox.BlockDefinition.WantsOnFocusLost);

            checkBox.Changed -= CheckBox_Changed;

            Assert.IsFalse(checkBox.BlockDefinition.WantsOnChange);
            Assert.IsTrue(checkBox.BlockDefinition.WantsOnFocusLost);

            checkBox.Changed += CheckBox_Changed;
            checkBox.FocusLost -= CheckBox_FocusLost;

            Assert.IsTrue(checkBox.BlockDefinition.WantsOnChange);
            Assert.IsFalse(checkBox.BlockDefinition.WantsOnFocusLost);
        }

        private void CheckBox_Changed(object sender, CheckBox.CheckBoxChangedEventArgs e)
        {
            // Nothing to do
        }

        private void CheckBox_FocusLost(object sender, EventArgs e)
        {
            // Nothing to do
        }

        [TestMethod]
        public void DateTimePickerFocusLost()
        {
            DateTimePicker dateTimePicker = new DateTimePicker();
            dateTimePicker.FocusLost += DateTimePicker_FocusLost;

            Assert.IsFalse(dateTimePicker.BlockDefinition.WantsOnChange);
            Assert.IsTrue(dateTimePicker.BlockDefinition.WantsOnFocusLost);

            dateTimePicker.FocusLost -= DateTimePicker_FocusLost;

            Assert.IsFalse(dateTimePicker.BlockDefinition.WantsOnChange);
            Assert.IsFalse(dateTimePicker.BlockDefinition.WantsOnFocusLost);

            dateTimePicker.FocusLost += DateTimePicker_FocusLost;
            dateTimePicker.Changed += DateTimePicker_Changed;

            Assert.IsTrue(dateTimePicker.BlockDefinition.WantsOnChange);
            Assert.IsTrue(dateTimePicker.BlockDefinition.WantsOnFocusLost);

            dateTimePicker.Changed -= DateTimePicker_Changed;

            Assert.IsFalse(dateTimePicker.BlockDefinition.WantsOnChange);
            Assert.IsTrue(dateTimePicker.BlockDefinition.WantsOnFocusLost);

            dateTimePicker.Changed += DateTimePicker_Changed;
            dateTimePicker.FocusLost -= DateTimePicker_FocusLost;

            Assert.IsTrue(dateTimePicker.BlockDefinition.WantsOnChange);
            Assert.IsFalse(dateTimePicker.BlockDefinition.WantsOnFocusLost);
        }

        private void DateTimePicker_Changed(object sender, DateTimePicker.DateTimePickerChangedEventArgs e)
        {
            // Nothing to do
        }

        private void DateTimePicker_FocusLost(object sender, EventArgs e)
        {
            // Nothing to do
        }

        [TestMethod]
        public void NumericFocusLost()
        {
            Numeric numeric = new Numeric();
            numeric.FocusLost += Numeric_FocusLost;

            Assert.IsFalse(numeric.BlockDefinition.WantsOnChange);
            Assert.IsTrue(numeric.BlockDefinition.WantsOnFocusLost);

            numeric.FocusLost -= Numeric_FocusLost;

            Assert.IsFalse(numeric.BlockDefinition.WantsOnChange);
            Assert.IsFalse(numeric.BlockDefinition.WantsOnFocusLost);

            numeric.FocusLost += Numeric_FocusLost;
            numeric.Changed += Numeric_Changed;

            Assert.IsTrue(numeric.BlockDefinition.WantsOnChange);
            Assert.IsTrue(numeric.BlockDefinition.WantsOnFocusLost);

            numeric.Changed -= Numeric_Changed;

            Assert.IsFalse(numeric.BlockDefinition.WantsOnChange);
            Assert.IsTrue(numeric.BlockDefinition.WantsOnFocusLost);

            numeric.Changed += Numeric_Changed;
            numeric.FocusLost -= Numeric_FocusLost;

            Assert.IsTrue(numeric.BlockDefinition.WantsOnChange);
            Assert.IsFalse(numeric.BlockDefinition.WantsOnFocusLost);
        }

        private void Numeric_Changed(object sender, Numeric.NumericChangedEventArgs e)
        {
            // Nothing to do
        }

        private void Numeric_FocusLost(object sender, EventArgs e)
        {
            // Nothing to do
        }

        [TestMethod]
        public void TextBoxFocusLost()
        {
            TextBox textBox = new TextBox();
            textBox.FocusLost += TextBox_FocusLost;

            Assert.IsFalse(textBox.BlockDefinition.WantsOnChange);
            Assert.IsTrue(textBox.BlockDefinition.WantsOnFocusLost);

            textBox.FocusLost -= TextBox_FocusLost;

            Assert.IsFalse(textBox.BlockDefinition.WantsOnChange);
            Assert.IsFalse(textBox.BlockDefinition.WantsOnFocusLost);

            textBox.FocusLost += TextBox_FocusLost;
            textBox.Changed += TextBox_Changed;

            Assert.IsTrue(textBox.BlockDefinition.WantsOnChange);
            Assert.IsTrue(textBox.BlockDefinition.WantsOnFocusLost);

            textBox.Changed -= TextBox_Changed;

            Assert.IsFalse(textBox.BlockDefinition.WantsOnChange);
            Assert.IsTrue(textBox.BlockDefinition.WantsOnFocusLost);

            textBox.Changed += TextBox_Changed;
            textBox.FocusLost -= TextBox_FocusLost;

            Assert.IsTrue(textBox.BlockDefinition.WantsOnChange);
            Assert.IsFalse(textBox.BlockDefinition.WantsOnFocusLost);
        }

        private void TextBox_Changed(object sender, TextBox.TextBoxChangedEventArgs e)
        {
            // Nothing to do
        }

        private void TextBox_FocusLost(object sender, EventArgs e)
        {
            // Nothing to do
        }

        [TestMethod]
        public void TimeFocusLost()
        {
            Time time = new Time();
            time.FocusLost += Time_FocusLost;

            Assert.IsFalse(time.BlockDefinition.WantsOnChange);
            Assert.IsTrue(time.BlockDefinition.WantsOnFocusLost);

            time.FocusLost -= Time_FocusLost;

            Assert.IsFalse(time.BlockDefinition.WantsOnChange);
            Assert.IsFalse(time.BlockDefinition.WantsOnFocusLost);

            time.FocusLost += Time_FocusLost;
            time.Changed += Time_Changed;

            Assert.IsTrue(time.BlockDefinition.WantsOnChange);
            Assert.IsTrue(time.BlockDefinition.WantsOnFocusLost);

            time.Changed -= Time_Changed;

            Assert.IsFalse(time.BlockDefinition.WantsOnChange);
            Assert.IsTrue(time.BlockDefinition.WantsOnFocusLost);

            time.Changed += Time_Changed;
            time.FocusLost -= Time_FocusLost;

            Assert.IsTrue(time.BlockDefinition.WantsOnChange);
            Assert.IsFalse(time.BlockDefinition.WantsOnFocusLost);
        }

        private void Time_Changed(object sender, Time.TimeChangedEventArgs e)
        {
            // Nothing to do
        }

        private void Time_FocusLost(object sender, EventArgs e)
        {
            // Nothing to do
        }

        [TestMethod]
        public void TimePickerFocusLost()
        {
            TimePicker timePicker = new TimePicker();
            timePicker.FocusLost += TimePicker_FocusLost;

            Assert.IsFalse(timePicker.BlockDefinition.WantsOnChange);
            Assert.IsTrue(timePicker.BlockDefinition.WantsOnFocusLost);

            timePicker.FocusLost -= TimePicker_FocusLost;

            Assert.IsFalse(timePicker.BlockDefinition.WantsOnChange);
            Assert.IsFalse(timePicker.BlockDefinition.WantsOnFocusLost);

            timePicker.FocusLost += TimePicker_FocusLost;
            timePicker.Changed += TimePicker_Changed;

            Assert.IsTrue(timePicker.BlockDefinition.WantsOnChange);
            Assert.IsTrue(timePicker.BlockDefinition.WantsOnFocusLost);

            timePicker.Changed -= TimePicker_Changed;

            Assert.IsFalse(timePicker.BlockDefinition.WantsOnChange);
            Assert.IsTrue(timePicker.BlockDefinition.WantsOnFocusLost);

            timePicker.Changed += TimePicker_Changed;
            timePicker.FocusLost -= TimePicker_FocusLost;

            Assert.IsTrue(timePicker.BlockDefinition.WantsOnChange);
            Assert.IsFalse(timePicker.BlockDefinition.WantsOnFocusLost);
        }

        private void TimePicker_Changed(object sender, TimePicker.TimePickerChangedEventArgs e)
        {
            // Nothing to do
        }

        private void TimePicker_FocusLost(object sender, EventArgs e)
        {
            // Nothing to do
        }
    }
}
