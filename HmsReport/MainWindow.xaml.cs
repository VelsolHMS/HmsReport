﻿using CrystalDecisions.CrystalReports.Engine;
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
        public decimal adv,trf,tax,td,tc,gtot,gtax,dis;
        public MainWindow()
        {
            InitializeComponent();
            ch.ino = 1;
            ch.inv = 1;
            ch.invoice = 1;
        }
        Checkout ch = new Checkout();
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            ReportDocument re = new ReportDocument();
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
                re.PrintToPrinter(0, false, 0, 0);
                re.Refresh();
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
                            trf = Convert.ToDecimal(dd.Rows[0]["Tariff"]); ///staydays;
                            row["Debit"] = trf;
                        }
                        else if (k == 3)
                        {
                            row["Date"] = arrdate.AddDays(i);
                            row["Bill/voucher"] = "LUX-Taxes";
                            tax = Convert.ToDecimal(dd.Rows[0]["LUX Tax"]); // / staydays;
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
            d.Columns.Add("ResNo", typeof(string));
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
            row["City"] = dt.Rows[0]["Area"].ToString();
            row["State"] = dt.Rows[0]["State"].ToString();
            row["ResNo"] = dt.Rows[0]["REG NO"].ToString();
            row["Room"] = dt.Rows[0]["Room No"].ToString();
            row["Pax"] = dt.Rows[0]["Pax"].ToString();
            row["Invoice"] = dt.Rows[0]["Invoice No"].ToString();
            row["Type"] = dt.Rows[0]["Room Type"].ToString();
            row["Tarrif"] = dt.Rows[0]["Tariff"].ToString();
            row["ArrivalDate"] = dt.Rows[0]["Arrival Date"].ToString();
            row["DepartureDate"] = dt.Rows[0]["Depature Date"].ToString();
            row["ArrivalTime"] = dt.Rows[0]["Arrival Time"].ToString();
            row["DepartureTime"] = dt.Rows[0]["Depature Time"].ToString();
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
            DataTable da = ch.TotalAmounts();
            for(int j = 0; j< da.Rows.Count;j++)
            {
                DataRow row = d.NewRow();
                gtot = Convert.ToDecimal(da.Rows[0]["Tariff"].ToString());
                gtax = Convert.ToDecimal(da.Rows[0]["Tax"].ToString());
                if (da.Rows[0]["Discount"].ToString() == null || da.Rows[0]["Discount"].ToString() == "")
                { dis = Convert.ToDecimal(0.00); }
                else
                { dis = Convert.ToDecimal(da.Rows[0]["Discount"].ToString()); }
                row["GrandTotal"] = (gtot + gtax) - dis ;
                row["Tax"] = gtax;
                row["Discount"] = dis;
                d.Rows.Add(row);
            }
            ch.inv++;
            return d;
        }
    }
}
