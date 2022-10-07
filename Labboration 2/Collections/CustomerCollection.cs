using System.Text.Json;

namespace Laboration_2
{
    public static class CustomerCollection
    {
        //En statisk klass som innehåller en lista med Customers. Här finns alla metoder för att hämta och spara kunder från fil.
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
                _customerList.Add(new BronzeCustomer("Fnatte", "321"));
                _customerList.Add(new GoldCustomer("Tjatte", "213"));
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

            return (outCustomer = _customerList.Find(a => a.Name.ToLower().Equals(name.ToLower()) && a.VerifyPassword(password))) != null;
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
