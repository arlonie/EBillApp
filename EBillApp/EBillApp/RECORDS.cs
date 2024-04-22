using System;
using SQLite;
using System.Collections.Generic;
using System.Text;

namespace EBillApp
{
    public class RECORDS
    {
        [PrimaryKey, AutoIncrement]
        public int METER_NUM { get; set; }
        public double PRESENT_READ { get; set; }
        public double PREVIOUS_READ { get; set; }
        public double CONSUMPTION_READ { get; set; }
        public double ELECTRICITY_CHARGE { get; set; }
        public string TYPE_OF_REGIS { get; set; }
        public double DEMAND_CHARGE { get; set; }
        public double SERVICE_CHARGE { get; set; }
        public double PRINCIPAL_AMOUNT { get; set; }
        public double AMOUNT_PAYABLE { get; set; }
    }
}
