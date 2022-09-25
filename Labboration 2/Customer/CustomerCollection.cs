using System.Text.Json;

namespace Laboration_2
{
    public class CustomerCollection
    {
        private List<Customer> _customerList;
        private string _fileName = "Customers.json";

        public CustomerCollection()
        {
            _customerList = new List<Customer>();

            if (File.Exists(_fileName))
            {
                //Sparade kunder finns. Vi hämtar dom
                FetchSavedCustomersFromFile();
            }
            else
            {
                //Första uppstarten? Skapar förinlagda kunder
                _customerList.Add(new Customer("Knatte", "123"));
                _customerList.Add(new Customer("Fnatte", "321"));
                _customerList.Add(new Customer("Tjatte", "213"));
            }
        }

        public void FetchSavedCustomersFromFile()
        {

            string fileInput = File.ReadAllText(_fileName);
            _customerList = JsonSerializer.Deserialize<List<Customer>>(fileInput)!;
        }
        public void SaveCustomersToFile()
        {
            //Gör om listan till en json array och sparar ner till en textfil
            File.WriteAllText(_fileName, JsonSerializer.Serialize(_customerList));
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
            //Om det finns en kund med matchande användarnamn så returneras True
            return _customerList.Any(customer => customer.Name.Equals(name));
        }

        public bool TryGetCustomer(string name, string password, out Customer outCustomer)
        {
            outCustomer = _customerList.Find(customer => customer.Name.Equals(name));

            if (outCustomer != null)
            {
                return outCustomer.VerifyPassword(password);
            }

            return false;
        }
        public Customer GetCustomer(string name, string password)
        {
            return _customerList.Find(customer => customer.Name.Equals(name) && customer.Password.Equals(password));
        }

    }
}
