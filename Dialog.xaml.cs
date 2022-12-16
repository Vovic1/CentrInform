using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Логика взаимодействия для Dialog.xaml
    /// </summary>
    public partial class EditDlg : Window
    {
        private List<cstring2> rows = new List<cstring2>();

        public int tip { get; set; }        // add / update dialog
        //public employee_db data { get; set; }

        public EditDlg()
        {
            InitializeComponent();
            this.grid1.ItemsSource = rows;
            this.grid1.Columns[0].IsReadOnly = true;
        }

        public void set_data(employee_db d)
        {
            rows.Clear();
            rows.Add(new cstring2() { Key = "department", Value = d.department });
            rows.Add(new cstring2() { Key = "firstname", Value = d.firstname });
            rows.Add(new cstring2() { Key = "middlename", Value = d.middlename });
            rows.Add(new cstring2() { Key = "lastname", Value = d.lastname });
            rows.Add(new cstring2() { Key = "position", Value = d.position });
            rows.Add(new cstring2() { Key = "phone", Value = d.phone });
            rows.Add(new cstring2() { Key = "email", Value = d.email });
        }

        public employee_db get_data()
        {
            var items = this.grid1.Items;

            return new employee_db()
            {
                department = ((cstring2)items[0]).Value,
                firstname = ((cstring2)items[1]).Value,
                middlename = ((cstring2)items[2]).Value,
                lastname = ((cstring2)items[3]).Value,
                position = ((cstring2)items[4]).Value,
                phone = ((cstring2)items[5]).Value,
                email = ((cstring2)items[0]).Value
            };
        }

        private void OK_Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
