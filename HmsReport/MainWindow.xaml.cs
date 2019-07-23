using CrystalDecisions.CrystalReports.Engine;
using HmsReport.Model;
using System;
using System.Data;
using System.Windows;

namespace HmsReport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int incount,staydays;
        public DateTime arrdate;
        public decimal adv,trf,tax,td,tc;
        public MainWindow()
        {
            InitializeComponent();
            ch.ino = 1;
        }
        Checkout ch = new Checkout();
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            ReportDocument re = new ReportDocument();
            DataTable dt = CheckoutData();
            re.Load("../../Reports/CheckoutSubReport2.rpt");
            DataTable d = HotelData();
            re.Load("../../Reports/CheckoutSubReport.rpt");
            DataTable dd = Report();
            re.Load("../../Reports/CheckoutReport.rpt");
            re.Subreports[1].SetDataSource(dt);
            re.Subreports[0].SetDataSource(d);
            re.SetDataSource(dd);
            re.PrintToPrinter(0, false, 0, 0);
            re.Refresh();
        }
        public DataRow row;
        public DataTable CheckoutData()
        {

            DataTable d = new DataTable();
            d.Columns.Add("Date", typeof(DateTime));
            d.Columns.Add("Bill/voucher", typeof(string));
            d.Columns.Add("Debit", typeof(decimal));
            d.Columns.Add("Credit", typeof(decimal));
            d.Columns.Add("Balance", typeof(decimal));
            d.Columns.Add("TDebit", typeof(decimal));
            d.Columns.Add("TCredit", typeof(decimal));
            d.Columns.Add("TBalance", typeof(decimal));
            DataTable dk = ch.InvoiceCount();
            incount = Convert.ToInt32(dk.Rows[0]["Invoice"]);
            for (int j = 1; j <= incount; j++)
            {
                DataTable dd = ch.TariffAmounts();
                staydays = Convert.ToInt32(dd.Rows[0]["Stayed Days"]);
                arrdate = Convert.ToDateTime(dd.Rows[0]["Arrival Date"]);
                for (int i = 0; i < staydays; i++)
                {
                    for (int k = 1; k <= 4; k++)
                    {
                        row = d.NewRow();
                        if (k == 1)
                        {
                            row["Date"] = arrdate.AddDays(i);
                            row["Bill/voucher"] = "Advance";
                            if(i == 0)
                            { adv = Convert.ToDecimal(dd.Rows[0]["Advance"]); row["Credit"] = adv; }
                            else { adv = Convert.ToDecimal(0.00); row["Credit"] = adv; }
                            
                        }
                        else if (k == 2)
                        {
                            row["Date"] = arrdate.AddDays(i);
                            row["Bill/voucher"] = "Tarrif Amount";
                            trf = Convert.ToDecimal(dd.Rows[0]["Tariff"])/staydays;
                            row["Debit"] = trf;
                        }
                        else if (k == 3)
                        {
                            row["Date"] = arrdate.AddDays(i);
                            row["Bill/voucher"] = "LUX-Taxes";
                            tax = Convert.ToDecimal(dd.Rows[0]["LUX Tax"]) / staydays;
                            row["Debit"] = tax;
                        }
                        else if (k == 4)
                        {
                            row["Date"] = arrdate.AddDays(i);
                            row["Bill/voucher"] = "SER-Taxes";
                            row["Debit"] = "0.00";
                        }
                        d.Rows.Add(row);
                    }
                    td = trf + tax;
                    row["TDebit"] = td;
                    tc = adv;
                    row["TCredit"] = tc;
                    row["TBalance"] = td - tc;
                    
                }
                ch.ino++;
            }
            return d;
        }
        public DataTable HotelData()
        {
            DataTable d = new DataTable();
            return d;
        }
        public DataTable Report()
        {
            DataTable d = new DataTable();
            return d;
        }
    }
}
