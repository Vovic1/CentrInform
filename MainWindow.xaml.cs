using System;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Windows;
using System.Xml.Serialization;
using System.Windows.Data;
using System.Data.Entity;
using System.Windows.Media.TextFormatting;
using Microsoft.Win32;
using System.Reflection;
using System.Data;
using System.Windows.Controls;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public string fn1 = @"..\..\test1.xml";
        public string fn2 = @"..\..\test1.xml";
        public static string scon = "Data Source=V500GB\\SQLEXPRESS;Initial Catalog=CInfo;Integrated Security=True";

        public MainWindow()
        {
            //test_ef();
            //var dd = read_xml(fn1);
            //write_xml(fn2, dd);

            InitializeComponent();

            //this.employyesTab.
        }

        private void test_ef()
        {
            using (var db = new EmpContext(scon))
            {
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE [employees_db_2]");
                db.employees.Clear();
                db.SaveChanges();

                Console.WriteLine("count = {0}", db.employees.Count());

                db.employees.Add(new employee_db() { department = "d1", email = "e1", firstname = "f1" });
                db.employees.Add(new employee_db() { department = "d2", email = "e1", firstname = "f2" });
                db.SaveChanges();

                foreach (var employee in db.employees)
                {
                    Console.WriteLine(employee.firstname);
                }
            }
        }

        public EXPORT2 read_xml(string fname)
        {
            XmlSerializer reader = new XmlSerializer(typeof(EXPORT2));
            StreamReader xmlf = new StreamReader(fname);
            EXPORT2 dd = (EXPORT2)reader.Deserialize(xmlf);
            xmlf.Close();
            return dd;
        }

        public void write_xml(string fname, EXPORT2 dd)
        {
            var writer = new XmlSerializer(typeof(EXPORT2));
            var xmlf = new StreamWriter(fname);
            writer.Serialize(xmlf, dd);
            xmlf.Close();
        }

        private void load_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog fdlg = new OpenFileDialog();
            if (fdlg.ShowDialog() != true) return;

            fn1 = fdlg.FileName;
            var dd = read_xml(fn1);

            using (var db = new EmpContext(scon))
            {
                var elast = db.employees.OrderByDescending(u => u.DDD).FirstOrDefault();
                var lastDDD = elast.DDD;

                foreach (var emp in dd.EXPORT)
                {
                    var edb = new employee_db(emp);
                    edb.DDD = ++lastDDD;
                    db.employees.Add(edb);
                }
                
                this.employyesTab.ItemsSource= db.employees.Local;
                db.SaveChanges();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new EmpContext(scon))
            {
                db.employees.Load();
                this.employyesTab.ItemsSource = db.employees.Local;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            EditDlg dlg = new EditDlg();
            dlg.set_data(new employee_db());
            dlg.tip = 0;

            if (dlg.ShowDialog() == true)
            {
                using (var db = new EmpContext(scon))
                {
                    var emp = dlg.get_data();
                    var elast = db.employees.OrderByDescending(u => u.DDD).FirstOrDefault();
                    emp.DDD = elast.DDD + 1;
                    db.employees.Add(emp);
                    db.SaveChanges();

                    db.employees.Load();
                    this.employyesTab.ItemsSource = db.employees.Local;
                }
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            var emp = (employee_db)this.employyesTab.SelectedItem;
            using (var db = new EmpContext(scon))
            {
                var obj = db.employees.Find(emp.DDD);
                if (obj == null) return;
                db.employees.Remove(obj);
                db.SaveChanges();

                db.employees.Load();
                this.employyesTab.ItemsSource = db.employees.Local;
            }
        }

        private void btnUpd_Click(object sender, RoutedEventArgs e)
        {
            var row = (employee_db)this.employyesTab.SelectedItem;
            EditDlg dd = new EditDlg();
            dd.tip = 1;
            dd.set_data(row);

            if (dd.ShowDialog() == true)
            {
                using (var db = new EmpContext(scon))
                {
                    var ddd = row.DDD;
                    var d = dd.get_data();

                    var emp = db.employees.SingleOrDefault(b => b.DDD == ddd);
                    if (emp != null)
                    {
                        emp.department = d.department;
                        emp.firstname = d.firstname;
                        emp.middlename = d.middlename;
                        emp.lastname = d.lastname;
                        emp.position = d.position;
                        emp.email = d.email;
                        db.SaveChanges();
                    }

                    db.SaveChanges();
                    db.employees.Load();
                    this.employyesTab.ItemsSource = db.employees.Local;
                }
            }

        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new EmpContext(scon))
            {
                var dlg = new FindDlg();

                var flist = typeof(employee_db).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                            .Where(p => p.CanRead )
                            .Select(property => property.Name)
                            .ToList();
                dlg.set_fields(flist);
                //dlg.set_cols(employyesTab.Columns.ToList());

                if (dlg.ShowDialog() != true) return;

                var f = dlg.get_field();
                var t = dlg.get_text();

                PropertyInfo pr = typeof(employee_db).GetProperty(f);

                foreach (var r in employyesTab.Items)
                {
                    var h = r as employee_db;
                    if (h == null) continue;

                    var v = pr.GetValue(h, null);
                    if (v == null) continue;

                    if (v.ToString().Contains(t))
                    {
                        employyesTab.SelectedItem = r;
                        employyesTab.ScrollIntoView(r);
                    }
                }
            }
        }

        // save
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            SaveFileDialog fdlg = new SaveFileDialog();
            if (fdlg.ShowDialog() != true) return;

            fn1 = fdlg.FileName;

            using (var db = new EmpContext(scon))
            {
                var ex = new EXPORT2();
                var a1 = db.employees.ToList(); //.Select(o => new EMPLOYEE());
                var a2 = a1.Select(t => new EMPLOYEE(t));
                ex.EXPORT.AddRange(a2);
                this.write_xml(fn1, ex);

            }
        }
    }
}
