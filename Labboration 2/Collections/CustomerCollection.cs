using System.Text.Json;

namespace Laboration_2
{
    public static class CustomerCollection
    {
        //En statisk klass som innehåller en lista med Customers. Här finns alla metoder för att hämta och spara kunder från fil, metoder för att lägga till och hämta kunder.
        //För att en kund ska kunna hämtas från listan måste rätt lösenord anges. Klassen är statisk för att det bara ska finnas en samling med kunder när programmet körs, för att förhindra att klassen kan instansieras flera gånger och användas på fel sätt.
        private static List<Customer>? _customerList;
        private const string FileName = "Customers.json";


        private static void FetchSavedCustomersFromFile()
        {
            //En metod som hämtar alla sparade kunder från fil. Den initsierar listan _customerList. Sen kontrollerar den ifall filen "Customers.json" existerar.
            //Ifall den gör det hämtas alla sparade kunder från filen. Annars skapas kunderna Knatte, Fnatte och Tjatte, läggs till i listan _customerList och sparas ner till filen "Customers.json"

            _customerList = new List<Customer>();

            if (File.Exists(FileName))
            {
                //Sparade kunder finns. Vi hämtar dom

                var options = new JsonSerializerOptions
                {
                    Converters = { new CustomerConverter() },
                    WriteIndented = true
                };

                var fileInput = File.ReadAllText(FileName);
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
            //En metod som gör om listan _customerList till en JSON array och sparar ner till textfilen "Customers.json"
            CheckIfListInitialized();

            var options = new JsonSerializerOptions
            {
                Converters = { new CustomerConverter() },
                WriteIndented = true
            };

            var jsonString = JsonSerializer.Serialize(_customerList, options);

            File.WriteAllText(FileName, jsonString);
        }

        public static void AddNewCustomer(Customer newCustomer)
        {
            //En metod som lägger till en ny kund till listan _customerList ifall kunden inte redan existerar i listan.
            CheckIfListInitialized();

            if (!_customerList.Contains(newCustomer))
            {
                _customerList.Add(newCustomer);
            }
        }

        public static bool CustomerExists(string name)
        {
            //En metod som kontrollerar ifall en kund existerar i listan _customerList
            CheckIfListInitialized();

            //Om det finns en kund med matchande användarnamn så returneras True
            return _customerList.Any(customer => customer.Name.ToLower().Equals(name.ToLower()));
        }

        public static bool TryGetCustomer(string name, string password, out Customer outCustomer)
        {
            //En metod som försöker hämta en kund från listan. Ifall kunden existerar i listan så sparas kunden i outCustomer och true retuneras. Annars retuneras false och outCustomer kommer att vara null.
            CheckIfListInitialized();

            return (outCustomer = _customerList.Find(a => a.Name.ToLower().Equals(name.ToLower()) && a.VerifyPassword(password))) != null;
        }

        private static void CheckIfListInitialized()
        {
            //En metod som kontrollerar ifall listan är initsierad. Om den inte är det så anropar den metoden FetchSavedCustomersFromFile();
            if (_customerList == null)
            {
                FetchSavedCustomersFromFile();
            }
        }


    }
}
