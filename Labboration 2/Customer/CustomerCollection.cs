using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration_2.Customer
{
    public class CustomerCollection
    {
        private List<Customer> _customerList;

        public CustomerCollection()
        {
            _customerList = new List<Customer>();
            _customerList.Add(new Customer("Linus", "hej"));
        }

        public void FetchSavedCustomers()
        {

        }

        public void AddNewCustomer(Customer newCustomer)
        {
            if (!_customerList.Contains(newCustomer))
            {
                _customerList.Add(newCustomer);
            }
        }

        public bool CustomerExists(string name)
        {
            return _customerList.Any(customer => customer.Name.Equals(name));
        }

        public bool PasswordIsMatching(string name, string password)
        {
            return _customerList.Any(customer => customer.Name.Equals(name) && customer.Password.Equals(password));
        }

    }
}
