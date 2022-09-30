using System.Text.Json;

namespace Laboration_2
{
    public abstract class CustomerCollection
    {
        private static List<Customer> _customerList;
        private static readonly string _fileName = "Customers.json";


        private static void FetchSavedCustomersFromFile()
        {

            _customerList = new List<Customer>();

            if (File.Exists(_fileName))
            {
                //Sparade kunder finns. Vi hämtar dom

                var options = new JsonSerializerOptions
                {
                    Converters = { new CustomerConverter() },
                    WriteIndented = true
                };

                var fileInput = File.ReadAllText(_fileName);
                _customerList = JsonSerializer.Deserialize<List<Customer>>(fileInput, options)!; 
            }
            else
            {
                //Första uppstarten? Skapar förinlagda kunder och spara ner till fil.
                _customerList.Add(new Customer("Knatte", "123"));
                _customerList.Add(new Customer("Fnatte", "321"));
                _customerList.Add(new Customer("Tjatte", "213"));
                SaveCustomersToFile();
            }
        }
        public static void SaveCustomersToFile()
        {
            //Gör om listan till en json array och sparar ner till en textfil
            CheckIfListInitialized();

            var options = new JsonSerializerOptions
            {
                Converters = { new CustomerConverter() },
                WriteIndented = true
            };

            var jsonString = JsonSerializer.Serialize(_customerList, options);

            File.WriteAllText(_fileName, jsonString);
        }

        public static void AddNewCustomer(Customer newCustomer)
        {
            CheckIfListInitialized();

            if (!_customerList.Contains(newCustomer))
            {
                _customerList.Add(newCustomer);
            }
        }

        public static bool CustomerExists(string name)
        {
            CheckIfListInitialized();

            //Om det finns en kund med matchande användarnamn så returneras True
            return _customerList.Any(customer => customer.Name.ToLower().Equals(name.ToLower()));
        }

        public static bool TryGetCustomer(string name, string password, out Customer outCustomer)
        {
            CheckIfListInitialized();

            outCustomer = _customerList.Find(customer => customer.Name.ToLower().Equals(name.ToLower()));

            if (outCustomer != null)
            {
                return outCustomer.VerifyPassword(password);
            }

            outCustomer = null;
            return false;
        }

        private static void CheckIfListInitialized()
        {
            if (_customerList == null)
            {
                FetchSavedCustomersFromFile();
            }
        }


    }
}
