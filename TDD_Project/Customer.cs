using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDD_Project
{
    public class Customer : IEquatable<Customer>
    {
        public string socialSecurityNumber { get; set; }

        public string Customer_name { get; set; }

        public bool Equals(Customer other)
        {
            if (other == null)
            {
                return false;
            }
            return Customer_name == other.Customer_name&& socialSecurityNumber == other.socialSecurityNumber;

        }
    }
}