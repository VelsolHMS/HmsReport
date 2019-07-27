using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HmsReport
{
    class CheckoutClass
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
    }
}
