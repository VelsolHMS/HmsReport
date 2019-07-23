using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HmsReport.Model
{
    class Checkout
    {
        public DataTable getdetails()
        {
            var list = new List<SqlParameter>();
            string s = "SELECT Name,[Address Line 1],State,Area,[REG NO],[Room No],Pax,[Arrival Date],[Arrival Time],[Depature Date],[Depature Time],[Invoice No],[Room No],Tariff FROM Sheet1$  WHERE [Invoice No]='" + MainWindow.invo + "'";
            DataTable d = DbFunctions.ExecuteCommand<DataTable>(s, list);
            return d;
        }
        public DataTable getcheckouts()
        {
            var list = new List<SqlParameter>();
            string s = "SELECT [Arrival Date],[Depature Date],Advance,Tariff,[LUX Tax],[Stayed Days] FROM Sheet1$ WHERE [Invoice No]='"+MainWindow.invo+"'";
            DataTable d = DbFunctions.ExecuteCommand<DataTable>(s, list);
            return d;
        }
        public DataTable getinvoice()
        {
            var list = new List<SqlParameter>();
            string s = "SELECT COUNT([Invoice No]) as InvoiceNo FROM Sheet1$ ";
            DataTable d = DbFunctions.ExecuteCommand<DataTable>(s, list);
            return d;
        }
    }
}
