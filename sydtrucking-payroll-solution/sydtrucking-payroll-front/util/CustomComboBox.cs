namespace sydtrucking_payroll_front.util
{
    using System.Collections;
    using System.Windows.Controls;

    public static class CustomComboBox
    {
        public static void LoadItems(this ComboBox comboBox, IList data, string textFirstItem)
        {
            data.Insert(0, textFirstItem);
            comboBox.ItemsSource = data;
        }
    }
}
