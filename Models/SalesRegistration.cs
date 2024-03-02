using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiInterface.Models
{
    public class SalesRegistration
    {
        public int Id { get; set; }
        public DateTime? SaleDate { get; set; }
        public string DishCode { get; set; }
        public string DishName { get; set; }
        public string QuantitySold { get; set; }
        public string TotalCost { get; set; }
        public PriceList? PriceList { get; set; }
    }
}
