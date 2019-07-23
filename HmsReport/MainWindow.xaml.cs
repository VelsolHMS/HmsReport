using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using System.Windows;
using HmsReport.Model;
using System;

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
        Checkout ch = new Checkout();
        public DateTime arrdate, depdate;
        public int staydays;
        public DataTable s;
        public static int invo = 1;
        public int invoice;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ReportDocument re = new ReportDocument();
            DataTable di = ch.getinvoice();
            invoice =Convert.ToInt32(di.Rows[0]["InvoiceNo"]);
            for (int i=1;i<=invoice;i++)
            {
                s = hoteldetails();
                re.Load("../../Reports/CheckoutSubReport2.rpt");
                DataTable s1 = checkoutdata();
                re.Load("../../Reports/CheckoutSubReport.rpt");
                DataTable s2 = main();
                re.Load("../../Reports/CheckoutReport.rpt");
                re.Subreports[1].SetDataSource(s);
                re.Subreports[0].SetDataSource(s1);
                re.SetDataSource(s2);
                re.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
                re.PrintToPrinter(0, false, 0, 0);
                re.Refresh();
            }
        }
        public DataTable hoteldetails()
        {
            DataTable dd = new DataTable();
            dd.Columns.Add("Name", typeof(string));
            dd.Columns.Add("Address", typeof(string));
            dd.Columns.Add("State", typeof(string));
            dd.Columns.Add("City", typeof(string));
            DataTable dt = ch.getdetails();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dd.NewRow();
                row["Name"] = dt.Rows[i]["Name"].ToString();
                row["Address"] = dt.Rows[i]["Address Line 1"].ToString();
                row["State"] = dt.Rows[i]["State"].ToString();
                row["City"] = dt.Rows[i]["Area"].ToString();
                dd.Rows.Add(row);
            }
            return dd;
        }
        public DataTable main()
        {
            DataTable dd = new DataTable();
            dd.Columns.Add("Discount", typeof(decimal));
            dd.Columns.Add("Tax", typeof(decimal));
            dd.Columns.Add("GrandTotal", typeof(decimal));
            return dd;

        }
        public decimal tarrif,adva,tax;
        public DataTable checkoutdata()
        {
            DataTable dd = new DataTable();
            dd.Columns.Add("Date", typeof(DateTime));
            dd.Columns.Add("Bill/voucher", typeof(string));
            dd.Columns.Add("Debit", typeof(decimal));
            dd.Columns.Add("Credit", typeof(decimal));
            dd.Columns.Add("Balance", typeof(decimal));
            dd.Columns.Add("TDebit", typeof(decimal));
            dd.Columns.Add("TCredit", typeof(decimal));
            dd.Columns.Add("TBalance", typeof(decimal));
            DataTable dtt = ch.getcheckouts();
            arrdate = Convert.ToDateTime(dtt.Rows[0]["Arrival Date"]);
            staydays =Convert.ToInt32(dtt.Rows[0]["Stayed Days"]);
            for(int i=0;i<=staydays;i++)
            {
                DataTable dv = voucher();
                for (int k = 0; k < dv.Rows.Count; k++)
                {
                    DataRow row = dd.NewRow();
                    row["Date"] = arrdate.AddDays(i);
                    row["Bill/voucher"] = dv.Rows[k]["Vou"].ToString();
                    if (dv.Rows[k]["Vou"].ToString() == "Advance")
                    {
                        adva = Convert.ToDecimal(dtt.Rows[0]["Advance"]);
                        row["Credit"] = dtt.Rows[0]["Advance"].ToString();
                        row["Debit"] = "0.00";
                        row["Balance"] = "0.00";
                    }
                    else if (dv.Rows[k]["Vou"].ToString() == "Tarrif Amount")
                    {
                        row["Credit"] = "0.00";
                        tarrif =Convert.ToDecimal(dtt.Rows[0]["Tariff"]);
                        row["Debit"] = dtt.Rows[0]["Tariff"].ToString();
                        row["Balance"] = "0.00";
                    }
                    else if (dv.Rows[k]["Vou"].ToString() == "LUX-Taxes")
                    {
                        row["Credit"] = "0.00";
                        tax= Convert.ToDecimal(dtt.Rows[0]["LUX Tax"]);
                        row["Debit"] = dtt.Rows[0]["LUX Tax"].ToString();
                        row["Balance"] = "0.00";
                    }
                    else if (dv.Rows[k]["Vou"].ToString() == "SER-Taxes")
                    {
                        row["Credit"] = "0.00";
                        row["Debit"] = "0.00";
                        row["Balance"] = "0.00";
                        tarrif = 0;tax = 0;adva = 0;
                    }
                    row["TDebit"] = tarrif + tax;
                    row["TCredit"] = adva;
                    row["TBalance"] = (tarrif + tax) - adva;
                    dd.Rows.Add(row);
                }
            }
            invo++;
            return dd;
        }
        public string vou;
        public DataTable voucher()
        {
            DataTable ddv = new DataTable();
            ddv.Columns.Add("Vou", typeof(string));
            for(int i=1;i<=4;i++)
            {
                DataRow row = ddv.NewRow();
                if (i == 1)
                {
                    row["Vou"] = "Advance";
                }
                else if(i == 2)
                {
                    row["Vou"] = "Tarrif Amount";
                }
                else if (i == 3)
                {
                    row["Vou"] = "LUX-Taxes";
                }
                else if (i == 4)
                {
                    row["Vou"] = "SER-Taxes";
                }
                ddv.Rows.Add(row);
            }
            return ddv;
        }
    }
}
