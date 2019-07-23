﻿using DAL;
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
        public int InvoiceNo;
        public string Name;
        public string AddressLine1;
        public string Area;
        public string State;
        public string Country;
        public int REGNO;
        public int RoomNo;
        public int Pax;
        public DateTime ArrivalDate;
        public DateTime ArrivalTime;
        public int StayedDays;
        public DateTime DepatureDays;
        public DateTime DepatureTime;
        public string RoomType;
        public decimal Tariff;
        public decimal Total;
        public decimal Discount;
        public decimal DiscountAmount;
        public decimal TotalAfterDiscount;
        public decimal Advance;
        public decimal LUXTax;
        public int ino,inv,invoice;

        public DataTable InvoiceCount()
        {
            var LIST = new List<SqlParameter>();
            string S = "SELECT Count([Invoice No]) AS Invoice FROM Sheet1$";
            DataTable d = DbFunctions.ExecuteCommand<DataTable>(S, LIST);
            return d;
        }
        public DataTable TariffAmounts()
        {
            var LIST = new List<SqlParameter>();
            string S = "SELECT [Arrival Date],Advance,Tariff,[Stayed Days],[LUX Tax] FROM Sheet1$ where [Invoice No] = '" + ino+"'";
            DataTable d = DbFunctions.ExecuteCommand<DataTable>(S, LIST);
            return d;
        }
        public DataTable TotalAmounts()
        {
            var LIST = new List<SqlParameter>();
            string S = "SELECT Sum(Advance) AS Advance,Sum([Total After Discount]) AS Total,Sum([LUX Tax]) AS Tax,Sum([Discount Amount]) AS Discount  FROM Sheet1$ where [Invoice No] = '" + inv + "'";
            DataTable d = DbFunctions.ExecuteCommand<DataTable>(S, LIST);
            return d;
        }
        public DataTable InvoiceData()
        {
            var LIST = new List<SqlParameter>();
            string S = "SELECT Name,[Address Line 1],Area,State,[REG NO],[Room No],"+
                "Pax,CONVERT(VARCHAR(10),[Arrival Date],103) AS [Arrival Date], CONVERT(VARCHAR(10),[Depature Date],103) AS [Depature Date],[Invoice No],[Room Type],Tariff,[Arrival Time],[Depature Time] FROM Sheet1$ where [Invoice No] = '" + invoice + "'";
            DataTable d = DbFunctions.ExecuteCommand<DataTable>(S, LIST);
            return d;
        }
    }
}