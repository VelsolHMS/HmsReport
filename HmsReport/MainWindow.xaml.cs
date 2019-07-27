using CrystalDecisions.CrystalReports.Engine;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace HmsReport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ReportDocument re = new ReportDocument();
            DataTable dd = Report();
            DataTable dd1 = report1();
            DataTable dd2 = report();
            re.Load("../../Reports/CheckoutSubReport2.rpt");
            re.Load("../../Reports/CheckoutSubReport.rpt");
            re.Load("../../Reports/CheckoutReport.rpt");
            re.Subreports[1].SetDataSource(dd1);
            re.Subreports[0].SetDataSource(dd);
            re.SetDataSource(dd2);
            re.PrintToPrinter(1, false, 0, 0);
            re.Refresh();
        }

        public DataTable Report()
        {
            DataTable d = new DataTable();
            d.Columns.Add("Name", typeof(string));
            d.Columns.Add("Address", typeof(string));
            d.Columns.Add("City", typeof(string));
            d.Columns.Add("State", typeof(string));
            d.Columns.Add("ResNo", typeof(string));
            d.Columns.Add("Room",typeof(int));
            d.Columns.Add("Pax", typeof(int));
            d.Columns.Add("Invoice",typeof(int));
            d.Columns.Add("Type",typeof(string));
            d.Columns.Add("Tarrif", typeof(decimal));
            d.Columns.Add("ArrivalDate", typeof(string));
            d.Columns.Add("ArrivalTime", typeof(string));
            d.Columns.Add("DepartureDate", typeof(string));
            d.Columns.Add("DepartureTime", typeof(string));
            DataTable dd = main();
                DataRow row = d.NewRow();
                row["Name"] = dd.Rows[0]["Name"];
                row["Address"] = dd.Rows[0]["Address"];
                row["City"] = dd.Rows[0]["City"];
                row["State"] = dd.Rows[0]["State"];
                row["ResNo"] = dd.Rows[0]["ResNo"];
                row["Room"] = dd.Rows[0]["Room"];
                row["Pax"] = dd.Rows[0]["Pax"];
                row["Invoice"] = dd.Rows[0]["Invoice"];
                row["Type"] = dd.Rows[0]["Type"];
                row["Tarrif"] = dd.Rows[0]["Tarrif"];
                row["ArrivalDate"] = dd.Rows[0]["ArrivalDate"];
                row["ArrivalTime"] = dd.Rows[0]["ArrivalTime"];
                row["DepartureDate"] = dd.Rows[0]["DepartureDate"];
                row["DepartureTime"] = dd.Rows[0]["DepartureTime"];
                d.Rows.Add(row);
            return d;
        }
        public DataTable main()
        {
            var list = new List<SqlParameter>();
            string s = "SELECT Name,[Address Line 1]+','+Area as Address,State as City,Country as State,[REG NO] as ResNo ,[Room No] as Room, Pax,[Invoice No] as Invoice," +
                "[Room Type] as Type, Tariff as Tarrif, Convert(VARCHAR(10),[Arrival Date], 103) As ArrivalDate, [Arrival Time] as ArrivalTime," +
                " Convert(VARCHAR(10),[Depature Date], 103) As DepartureDate,[Depature Time] as DepartureTime FROM Sheet1$ Where [Room No]=213 and [Invoice No]=1";
            DataTable d = DbFunctions.ExecuteCommand<DataTable>(s, list);
            return d;
        }

        public DataTable report1()
        {
            DataTable d1 = new DataTable();
            d1.Columns.Add("Date", typeof(string));
            d1.Columns.Add("advance", typeof(decimal));
            d1.Columns.Add("Tarrif", typeof(decimal));
            d1.Columns.Add("Luxtax", typeof(decimal));
            d1.Columns.Add("balance", typeof(decimal));
            d1.Columns.Add("totaltarrif", typeof(decimal));
            d1.Columns.Add("totalcredit", typeof(decimal));
            d1.Columns.Add("totalbalance", typeof(decimal));
            
            DataTable drow =sub();
            DataTable dd = subre();

            for (int i = 0; i < de; i++)
            {
                DataRow row = dd.NewRow();
                row["Date"] = dd.Rows[i]["Date"];
                if(i == 0)
                {
                    row["advance"] = dd.Rows[0]["advance"];
                    C = Convert.ToDecimal(dd.Rows[0]["advance"]);
                }
                else
                {
                    row["advance"] = 0.00;
                }
                A = Convert.ToDecimal(dd.Rows[i]["Tarrif"]);
                row["Tarrif"] = dd.Rows[i]["Tarrif"];
                row["Luxtax"] = dd.Rows[i]["Luxtax"];
                D = Convert.ToDecimal(dd.Rows[i]["Luxtax"]);
               
                if(i == 0)
                {
                    row["totaltarrif"] = A + D;
                    row["totalcredit"] = C;
                    row["totalbalance"] = A + D - C;
                }
                else
                {
                    row["totaltarrif"] = A + D;
                    C = 0;
                    row["totalbalance"] = A + D - C;
                }
                dd.Rows.Add(row);
                //if(dd.Rows.Count == de)
                //{
                //    break;
                //}
            }
            return dd;
        }
        public decimal de,A,B,C,D;
        public DataTable sub()
        {
            var list = new List<SqlParameter>();
            string s = "select [Stayed Days] as days from Sheet1$ where [Room No]=213 and [Invoice No]=1";
            DataTable d = DbFunctions.ExecuteCommand<DataTable>(s, list);
            de =decimal.Parse(d.Rows[0]["days"].ToString());
            return d;
        }
        public DataTable subre()
        {
            var list = new List<SqlParameter>();
            string s = "select  Convert(VARCHAR(10),[Arrival Date],103) As Date,"+
                "Advance as advance,[LUX Tax] as Luxtax,Tariff as Tarrif,'' as totaltarrif,'' as totalcredit, '' as totalbalance from Sheet1$ where [Room No]=213 and [Invoice No]=1";
            DataTable d = DbFunctions.ExecuteCommand<DataTable>(s, list);
            return d;
        }

        public DataTable rep()
        {
            var list = new List<SqlParameter>();
            string s = "select Total as GrandTotal  from Sheet1$ where [Room No]=213 and [Invoice No]=1";
            DataTable d = DbFunctions.ExecuteCommand<DataTable>(s, list);
            return d;
        }

        public DataTable report()
        {
            DataTable dd = new DataTable();
            dd.Columns.Add("GrandTotal", typeof(string));
            DataTable dq = rep();
            DataRow row = dd.NewRow();
            row["GrandTotal"] = dq.Rows[0]["GrandTotal"];
            dd.Rows.Add(row);
            return dd;
        }

    }
}
