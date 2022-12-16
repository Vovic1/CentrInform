using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для FindDlg.xaml
    /// </summary>
    public partial class FindDlg : Window
    {
        public FindDlg()
        {
            InitializeComponent();
        }

        public string get_text() { return this.tbText.Text; }
        public string get_field() { return this.cmbFields.SelectedValue.ToString(); }

        public void set_fields(List<string> flist) { this.cmbFields.ItemsSource = flist; }
        public void set_cols(List<DataGridColumn> cols) { this.cmbFields.ItemsSource = cols; }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void tbText_TextChanged(object sender, TextChangedEventArgs e)
        {
            //
        }
    }
}
