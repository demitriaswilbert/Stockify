using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace crmLogins
{
    class Glb
    {
        public static SqlConnection KoneksiDB;
        public static SqlCommand CmdUserstock;
        public static void BukaDB()
        {

            KoneksiDB = new SqlConnection("Data Source = DESKTOP-FE780L7\\SQLEXPRESS; Initial Catalog = dbstock; Integrated Security = True");
            KoneksiDB.Open();
            CmdUserstock = KoneksiDB.CreateCommand();
        }

        public static void TutupDB()
        {
            CmdUserstock.Dispose();

            KoneksiDB.Close();
            KoneksiDB.Dispose();
        }
    }
}
