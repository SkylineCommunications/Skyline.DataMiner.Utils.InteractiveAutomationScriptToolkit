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

            Assert.IsTrue(calendar.WantsOnChange);
            Assert.IsTrue(calendar.WantsOnFocusLost);

            calendar.FocusLost -= Calendar_FocusLost;

            Assert.IsFalse(calendar.WantsOnChange);
            Assert.IsFalse(calendar.WantsOnFocusLost);

            calendar.FocusLost += Calendar_FocusLost;
            calendar.Changed += Calendar_Changed;

            Assert.IsTrue(calendar.WantsOnChange);
            Assert.IsTrue(calendar.WantsOnFocusLost);

            calendar.Changed -= Calendar_Changed;

            Assert.IsTrue(calendar.WantsOnChange);
            Assert.IsTrue(calendar.WantsOnFocusLost);

            calendar.Changed += Calendar_Changed;
            calendar.FocusLost -= Calendar_FocusLost;

            Assert.IsTrue(calendar.WantsOnChange);
            Assert.IsFalse(calendar.WantsOnFocusLost);
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

            Assert.IsTrue(checkBox.WantsOnChange);
            Assert.IsTrue(checkBox.WantsOnFocusLost);

            checkBox.FocusLost -= CheckBox_FocusLost;

            Assert.IsFalse(checkBox.WantsOnChange);
            Assert.IsFalse(checkBox.WantsOnFocusLost);

            checkBox.FocusLost += CheckBox_FocusLost;
            checkBox.Changed += CheckBox_Changed;

            Assert.IsTrue(checkBox.WantsOnChange);
            Assert.IsTrue(checkBox.WantsOnFocusLost);

            checkBox.Changed -= CheckBox_Changed;

            Assert.IsTrue(checkBox.WantsOnChange);
            Assert.IsTrue(checkBox.WantsOnFocusLost);

            checkBox.Changed += CheckBox_Changed;
            checkBox.FocusLost -= CheckBox_FocusLost;

            Assert.IsTrue(checkBox.WantsOnChange);
            Assert.IsFalse(checkBox.WantsOnFocusLost);
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

            Assert.IsTrue(dateTimePicker.WantsOnChange);
            Assert.IsTrue(dateTimePicker.WantsOnFocusLost);

            dateTimePicker.FocusLost -= DateTimePicker_FocusLost;

            Assert.IsFalse(dateTimePicker.WantsOnChange);
            Assert.IsFalse(dateTimePicker.WantsOnFocusLost);

            dateTimePicker.FocusLost += DateTimePicker_FocusLost;
            dateTimePicker.Changed += DateTimePicker_Changed;

            Assert.IsTrue(dateTimePicker.WantsOnChange);
            Assert.IsTrue(dateTimePicker.WantsOnFocusLost);

            dateTimePicker.Changed -= DateTimePicker_Changed;

            Assert.IsTrue(dateTimePicker.WantsOnChange);
            Assert.IsTrue(dateTimePicker.WantsOnFocusLost);

            dateTimePicker.Changed += DateTimePicker_Changed;
            dateTimePicker.FocusLost -= DateTimePicker_FocusLost;

            Assert.IsTrue(dateTimePicker.WantsOnChange);
            Assert.IsFalse(dateTimePicker.WantsOnFocusLost);
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

            Assert.IsTrue(numeric.WantsOnChange);
            Assert.IsTrue(numeric.WantsOnFocusLost);

            numeric.FocusLost -= Numeric_FocusLost;

            Assert.IsFalse(numeric.WantsOnChange);
            Assert.IsFalse(numeric.WantsOnFocusLost);

            numeric.FocusLost += Numeric_FocusLost;
            numeric.Changed += Numeric_Changed;

            Assert.IsTrue(numeric.WantsOnChange);
            Assert.IsTrue(numeric.WantsOnFocusLost);

            numeric.Changed -= Numeric_Changed;

            Assert.IsTrue(numeric.WantsOnChange);
            Assert.IsTrue(numeric.WantsOnFocusLost);

            numeric.Changed += Numeric_Changed;
            numeric.FocusLost -= Numeric_FocusLost;

            Assert.IsTrue(numeric.WantsOnChange);
            Assert.IsFalse(numeric.WantsOnFocusLost);
        }

        private void Numeric_Changed(object sender, Numeric.NumericChangedEventArgs e)
        {
            // Nothing to do
        }

        private void Numeric_FocusLost(object sender, EventArgs e)
        {
            // Nothing to do
        }

        //[TestMethod]
        //public void TextBoxFocusLost()
        //{
        //    TextBox textBox = new TextBox();
        //    textBox.FocusLost += TextBox_FocusLost;

        //    Assert.IsTrue(textBox.WantsOnChange);
        //    Assert.IsTrue(textBox.WantsOnFocusLost);

        //    textBox.FocusLost -= TextBox_FocusLost;

        //    Assert.IsFalse(textBox.WantsOnChange);
        //    Assert.IsFalse(textBox.WantsOnFocusLost);

        //    textBox.FocusLost += TextBox_FocusLost;
        //    textBox.Changed += TextBox_Changed;

        //    Assert.IsTrue(textBox.WantsOnChange);
        //    Assert.IsTrue(textBox.WantsOnFocusLost);

        //    textBox.Changed -= TextBox_Changed;

        //    Assert.IsTrue(textBox.WantsOnChange);
        //    Assert.IsTrue(textBox.WantsOnFocusLost);

        //    textBox.Changed += TextBox_Changed;
        //    textBox.FocusLost -= TextBox_FocusLost;

        //    Assert.IsTrue(textBox.WantsOnChange);
        //    Assert.IsFalse(textBox.WantsOnFocusLost);
        //}

        //private void TextBox_Changed(object sender, TextBox.TextBoxChangedEventArgs e)
        //{
        //    // Nothing to do
        //}

        //private void TextBox_FocusLost(object sender, EventArgs e)
        //{
        //    // Nothing to do
        //}

        [TestMethod]
        public void TimeFocusLost()
        {
            Time time = new Time();
            time.FocusLost += Time_FocusLost;

            Assert.IsTrue(time.WantsOnChange);
            Assert.IsTrue(time.WantsOnFocusLost);

            time.FocusLost -= Time_FocusLost;

            Assert.IsFalse(time.WantsOnChange);
            Assert.IsFalse(time.WantsOnFocusLost);

            time.FocusLost += Time_FocusLost;
            time.Changed += Time_Changed;

            Assert.IsTrue(time.WantsOnChange);
            Assert.IsTrue(time.WantsOnFocusLost);

            time.Changed -= Time_Changed;

            Assert.IsTrue(time.WantsOnChange);
            Assert.IsTrue(time.WantsOnFocusLost);

            time.Changed += Time_Changed;
            time.FocusLost -= Time_FocusLost;

            Assert.IsTrue(time.WantsOnChange);
            Assert.IsFalse(time.WantsOnFocusLost);
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

            Assert.IsTrue(timePicker.WantsOnChange);
            Assert.IsTrue(timePicker.WantsOnFocusLost);

            timePicker.FocusLost -= TimePicker_FocusLost;

            Assert.IsFalse(timePicker.WantsOnChange);
            Assert.IsFalse(timePicker.WantsOnFocusLost);

            timePicker.FocusLost += TimePicker_FocusLost;
            timePicker.Changed += TimePicker_Changed;

            Assert.IsTrue(timePicker.WantsOnChange);
            Assert.IsTrue(timePicker.WantsOnFocusLost);

            timePicker.Changed -= TimePicker_Changed;

            Assert.IsTrue(timePicker.WantsOnChange);
            Assert.IsTrue(timePicker.WantsOnFocusLost);

            timePicker.Changed += TimePicker_Changed;
            timePicker.FocusLost -= TimePicker_FocusLost;

            Assert.IsTrue(timePicker.WantsOnChange);
            Assert.IsFalse(timePicker.WantsOnFocusLost);
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
