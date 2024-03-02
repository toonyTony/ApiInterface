using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiInterface.Models
{
    public class PriceList
    {
        public int Id { get; set; }
        public string DishCode { get; set; }
        public string DishName { get; set; }
        public string Price { get; set; }
        List<SalesRegistration> Registrations { get; set; }
    }
}
