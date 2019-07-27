using CrystalDecisions.CrystalReports.Engine;
using HmsReport.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using CrystalDecisions.Shared;
using System.Web;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;
using System.Configuration;
using System.IO;

namespace HmsReport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int incount,staydays,IG;
        public DateTime arrdate,deptdate;
        public decimal adv,trf,tax,td,tc,gtot,gtax,dis,adv1;
        public MainWindow()
        {
            InitializeComponent();
            ch.ino = 889;
            ch.inv = 889;
            ch.invoice = 889;
        }
        Checkout ch = new Checkout();
        ReportDocument re = new ReportDocument();
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            
            DataTable dkr = ch.InvoiceCount();
            incount = Convert.ToInt32(dkr.Rows[0]["Invoice"]);
            for (int i = 1; i <= incount; i++)
            {
                DataTable dt = CheckoutData();
                re.Load("../../Reports/CheckoutSubReport2.rpt");
                DataTable d = HotelData();
                re.Load("../../Reports/CheckoutSubReport.rpt");
                DataTable dd = Report();
                re.Load("../../Reports/CheckoutReport.rpt");
                re.Subreports[1].SetDataSource(dt);
                re.Subreports[0].SetDataSource(d);
                re.SetDataSource(dd);
               // re.PrintToPrinter(0, false, 0, 0);
                re.Refresh();
                IG = ch.invoice;
                if (File.Exists(@"D:\Apr" + (IG - 1) + ".pdf"))
                    File.Delete(@"D:\Apr" + (IG - 1) + ".pdf");
                re.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, @"D:\Apr" + (IG - 1) + ".pdf");
            }
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
            d.Columns.Add("DayTotal", typeof(string));
            d.Columns.Add("TDebit", typeof(decimal));
            d.Columns.Add("TCredit", typeof(decimal));
            d.Columns.Add("TBalance", typeof(decimal));
            DataTable dd = ch.TariffAmounts();
            for (int j = 1; j <= dd.Rows.Count; j++)
            {
                staydays = Convert.ToInt32(dd.Rows[0]["Stayed Days"]);
                arrdate = Convert.ToDateTime(dd.Rows[0]["Arrival Date"]);
                deptdate = Convert.ToDateTime(dd.Rows[0]["Depature Date"]);
                for (DateTime i = arrdate; i < deptdate; i=i.AddDays(1))
                {
                    for (int k = 1; k <= 3; k++)
                    {
                        row = d.NewRow();
                        if (k == 1)
                        {
                            row["Date"] = i;
                            row["Bill/voucher"] = "Advance";
                            if(i == arrdate)
                            { adv = Convert.ToDecimal(dd.Rows[0]["Advance"]); row["Credit"] = adv; }
                            else { adv = Convert.ToDecimal(0.00); row["Credit"] = adv; }
                            
                        }
                        else if (k == 2)
                        {
                            row["Date"] = i;
                            row["Bill/voucher"] = "Tarrif Amount";
                            trf = Convert.ToDecimal(dd.Rows[0]["Tariff"]); ///staydays;
                            row["Debit"] = trf;
                        }
                        else if (k == 3)
                        {
                            row["Date"] = i;
                            row["Bill/voucher"] = "LUX-Taxes";
                            if (dd.Rows[0]["LUX Tax"].ToString() == null || dd.Rows[0]["LUX Tax"].ToString() == "")
                            {
                                tax = 0;
                            }
                            tax = Convert.ToDecimal(dd.Rows[0]["LUX Tax"])/ staydays;
                            row["Debit"] = tax;
                        }
                        //else if (k == 4)
                        //{
                        //    row["Date"] = i;
                        //    row["Bill/voucher"] = "SER-Taxes";
                        //    row["Debit"] = "0.00";
                        //}
                        d.Rows.Add(row);
                    }
                    td = trf + tax;
                    row["DayTotal"] = "Day Total";
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
            d.Columns.Add("Name", typeof(string));
            d.Columns.Add("Address", typeof(string));
            d.Columns.Add("City", typeof(string));
            d.Columns.Add("State", typeof(string));
         //   d.Columns.Add("ResNo", typeof(string));
            d.Columns.Add("Room", typeof(string));
            d.Columns.Add("Pax", typeof(string));
            d.Columns.Add("Invoice", typeof(string));
            d.Columns.Add("Type", typeof(string));
            d.Columns.Add("Tarrif", typeof(decimal));
            d.Columns.Add("ArrivalDate", typeof(string));
            d.Columns.Add("DepartureDate", typeof(string));
            d.Columns.Add("ArrivalTime", typeof(string));
            d.Columns.Add("DepartureTime", typeof(string));

            DataTable dt = ch.InvoiceData();
            DataRow row = d.NewRow();
            row["Name"] = dt.Rows[0]["Name"].ToString();
            row["Address"] = dt.Rows[0]["Address Line 1"].ToString();
            row["City"] = dt.Rows[0]["city"].ToString();
            row["State"] = dt.Rows[0]["State"].ToString();
        //    row["ResNo"] = dt.Rows[0]["REG NO"].ToString();
            row["Room"] = dt.Rows[0]["Room No"].ToString();
            row["Pax"] = 2; // dt.Rows[0]["Pax"].ToString();
            row["Invoice"] = dt.Rows[0]["Invoice No"].ToString();
            row["Type"] = dt.Rows[0]["Room Type"].ToString();
            row["Tarrif"] = dt.Rows[0]["Tariff"].ToString();
            row["ArrivalDate"] = dt.Rows[0]["Arrival Date"].ToString();
            row["DepartureDate"] = dt.Rows[0]["Depature Date"].ToString();
            row["ArrivalTime"] = dt.Rows[0]["Arrival Time"];
            row["DepartureTime"] = dt.Rows[0]["Depature Time"];
            d.Rows.Add(row);
            ch.invoice++;
            return d;
        }
        public DataTable Report()
        {
            DataTable d = new DataTable();
            d.Columns.Add("GrandTotal", typeof(decimal));
            d.Columns.Add("Tax", typeof(decimal));
            d.Columns.Add("Discount", typeof(decimal));
            d.Columns.Add("Total", typeof(decimal));
            DataTable da = ch.TotalAmounts();
            for(int j = 0; j< da.Rows.Count;j++)
            {
                DataRow row = d.NewRow();
                adv1 = Convert.ToDecimal(da.Rows[0]["Advance"].ToString());
                gtot = Convert.ToDecimal(da.Rows[0]["Total"].ToString());
                gtax = Convert.ToDecimal(da.Rows[0]["Tax"]); // *staydays;
                if (da.Rows[0]["Discount"].ToString() == null || da.Rows[0]["Discount"].ToString() == "")
                {dis = Convert.ToDecimal(0.00);}
                else
                { dis = Convert.ToDecimal(da.Rows[0]["Discount"].ToString());}
                row["GrandTotal"] = (gtot - adv1) + gtax;
                row["Total"] = Convert.ToDecimal(da.Rows[0]["Tariff"].ToString());
                row["Tax"] = gtax;
                row["Discount"] = dis;
                d.Rows.Add(row);
            }
            ch.inv++;
            return d;
        }
        //public string MapPath(string path);
        //string filepath;
        //public void p()
        //{
        //    filepath = Server.MapPath("~/" + "a.pdf");
        //    re.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, filepath);
        //    System.IO.FileInfo fileinfo = new System.IO.FileInfo(filepath);
        //    Response.AddHeader("Content-Disposition", "inline;filename=a.pdf");
        //    Response.ContentType = "application/pdf";
        //    Response.Writefile(fileinfo.FullName);
        //}
    }
}