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
        public int ino,inv,invoice;

        public DataTable InvoiceCount()
        {
            var LIST = new List<SqlParameter>();
            string S = "SELECT Count([Invoice No]) AS Invoice FROM Final$";
            DataTable d = DbFunctions.ExecuteCommand<DataTable>(S, LIST);
            return d;
        }
        public DataTable TariffAmounts()
        {
            var LIST = new List<SqlParameter>();
            string S = "SELECT [Arrival Date],[Depature Date],Advance,Tariff,[Stayed Days],[LUX Tax] FROM Final$ where [Invoice No] = '" + ino+"'";
            DataTable d = DbFunctions.ExecuteCommand<DataTable>(S, LIST);
            return d;
        }
        public DataTable TotalAmounts()
        {
            var LIST = new List<SqlParameter>();
            string S = "SELECT Sum(Advance) AS Advance,Sum(Total) AS Tariff,Sum([Total After Discount w/o tax]) AS Total,Sum([LUX Tax]) AS Tax,Sum([Discount Amount]) AS Discount  FROM Final$ where [Invoice No] = '" + inv + "'";
            DataTable d = DbFunctions.ExecuteCommand<DataTable>(S, LIST);
            return d;
        }
        public DataTable InvoiceData()
        {
            var LIST = new List<SqlParameter>();
            string S = "SELECT Name,[Address Line 1],City,State,[Room No],"+
                "Pax,CONVERT(VARCHAR(10),[Arrival Date],103) AS [Arrival Date], CONVERT(VARCHAR(10),[Depature Date],103) AS [Depature Date],[Invoice No],[Room Type],Tariff,[Arrival Time],[Depature Time] FROM Final$ where [Invoice No] = '" + invoice + "'";
            DataTable d = DbFunctions.ExecuteCommand<DataTable>(S, LIST);
            return d;
        }
    }
}