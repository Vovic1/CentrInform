using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Runtime.Remoting.Contexts;
using System.Xml.Serialization;

namespace WpfApp1
{
    public class cstring2
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }


    public class employee_db
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DDD { get; set; }
        public string department { get; set; }
        public string firstname { get; set; }
        public string middlename { get; set; }
        public string lastname { get; set; }
        public string datebirth { get; set; }
        public string position { get; set; }
        public string phone { get; set; }
        public string email { get; set; }

        public employee_db() { }

        public employee_db(EMPLOYEE e)
        {
            this.department = e.DEPARTMENT;
            firstname = e.DATA.FIRSTNAME;
            middlename = e.DATA.MIDDLENAME;
            lastname = e.DATA.LASTNAME;
            position = e.DATA.POSITION;
            datebirth = e.DATA.DATEBIRTH;
            phone = e.DATA.PHONE;
            email = e.DATA.EMAIL;
        }
    }

    public class EMPLOYEE
    {
        public string DEPARTMENT { get; set; }
        public DATA DATA { get; set; }

        public EMPLOYEE() { }

        public EMPLOYEE(employee_db d)
        {
            this.DATA = new DATA();
            DEPARTMENT = d.department;
            DATA.FIRSTNAME = d.firstname;
            DATA.MIDDLENAME = d.middlename;
            DATA.LASTNAME = d.lastname;
            DATA.DATEBIRTH = d.datebirth;
            DATA.POSITION = d.position;
            DATA.PHONE = d.phone;
            DATA.EMAIL = d.email;
        }
    }

    public class DATA
    {
        public string FIRSTNAME { get; set; }
        public string MIDDLENAME { get; set; }
        public string DATEBIRTH { get; set; }
        public string LASTNAME { get; set; }
        public string POSITION { get; set; }
        public string PHONE { get; set; }
        public string EMAIL { get; set; }
    }

    [XmlType("EXPORT")]
    public class EXPORT2
    {
        public EXPORT2()
        {
            EXPORT = new List<EMPLOYEE>();
        }
        [XmlElement("EMPLOYEE")]
        public List<EMPLOYEE> EXPORT { get; set; }
    }

    public class EmpContext : DbContext
    {
        //public DbSet<employee_db> employees { get; set; }
        public DbSet<employee_db> employees { get; set; }

        public EmpContext(string con) : base(con) { /*base.Configuration.ValidateOnSaveEnabled = false;*/ }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    //define ignores here
        //    modelBuilder.Entity<employee_db>().Ignore(t => t.DDD);
        //}
    }

    public static class EntityExtensions
    {
        public static void Clear<T>(this DbSet<T> dbSet) where T : class
        {
            dbSet.RemoveRange(dbSet);
        }
    }

}


