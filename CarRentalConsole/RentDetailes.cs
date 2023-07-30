using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalConsole
{
    internal class RentDetailes
    {
        public int RentId { get; set; }
        public int CarId { get; set; }
        public string CustmerName { get; set; }
        public string Address { get; set; }

        public string MobileNo { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal RentalFee { get; set; }
        public bool IsPaid { get; set; }
    }
}
