using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace Laboration_2
{
    public class CustomerConverter : JsonConverter<Customer>
    {
        //En klass som används för att konvertera ett objekt av typen Customer till JSON med hjälp av arv från JsonConverter<Customer>
        //Klassen talar alltså om för JsonSerializer hur den ska "serializa" ett objekt av typen till Customer genom att instansieras i
        //en JsonSerializerOptions, som sedan skickas med in i som ett argument i JsonSerializer.Serialize metoden

        public override bool CanConvert(Type typeToConvert) =>
            typeof(Customer).IsAssignableFrom(typeToConvert);

        public override Customer Read(
            ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            //En metod som talar om hur JsonSerializer ska återskapa ett Customer objekt från JSON. Override:ar base metoden Read() i JsonConverter

            //Är första tokentypen ett JSON StartObject? Om det inte är det, är JSON objektet/strängen inte giltligt och vi ger en exception.
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            //Är är tokentypen ett JSON PropertyName? Om det inte är det, är JSON objektet/strängen inte giltligt och vi ger en exception.
            //Här kommer CustomerLevel vara lagrat, alltså ifall kunden är en Baskund, bronskund osv.

            reader.Read();
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }

            //Vi hämtar namnet på property:n och kontrollerar att den verkligen innehåller värdet "CustomerLevel". Annars exception
            var propertyName = reader.GetString();
            if (propertyName != "CustomerLevel")
            {
                throw new JsonException();
            }

            //Vi läser in värdet på CustomerLevel.
            reader.Read();
            if (reader.TokenType != JsonTokenType.Number)
            {
                throw new JsonException();
            }

            //Eftersom vi måste ha både namn och lösenord när vi skapar en instans av objektet kund måste vi först hämta ut den information innan vi kan skapa objektet.
            //Därför deklarerar vi lite variabler.
            //Vi sparar också värdet på CustomerLevel i variabeln customerType. 0 = Baskund, 1 = Bronskund osv.
            var customerType = reader.GetInt32();
            var name = "";
            var password = "";
            var currency = Currencies.SEK;
            var cart = new List<CartItem>();


            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    //Om TokenType är lika med EndObject har vi kommit till slutet av kunden. Vi har därför all nödvändig information för att skapa och retunera en kund.
                    var customer = customerType switch
                    {
                        0 => new Customer(name, password) {Cart = cart},
                        1 => new BronzeCustomer(name, password) { Cart = cart },
                        2 => new SilverCustomer(name, password) { Cart = cart },
                        3 => new GoldCustomer(name, password) { Cart = cart },
                        _ => throw new JsonException()
                    };
                    customer.Currency = currency;
                    return customer;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    //Om TokenType är lika med PropertyName innehåller det information om kunden. Med hjälp av en switch sats lagrar vi det i rätt variabel
                    propertyName = reader.GetString();
                    reader.Read();

                    switch (propertyName)
                    {
                        case "Name":
                            name = reader.GetString();
                            break;
                        case "Password":
                            password = Decrypt(reader.GetString());
                            break;
                        case "Currency":
                            currency = (Currencies)reader.GetInt32();
                            break;
                        case "Cart":
                            //Vi deklarerar variabler där informationen om produkterna kommer att lagras.
                            var itemName = string.Empty;
                            var itemPrice = 0m;
                            var itemUnit = string.Empty;
                            var itemAmount = 0;

                            //Eftersom kundvagnen ligger i en egen JSON array får vi loopa igenom den också för att få ut alla varor. Vi loopar tills vi kommer till TokenType av typen EndArray
                            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                            {
                                if (reader.TokenType == JsonTokenType.PropertyName)
                                {
                                    
                                    propertyName = reader.GetString();
                                    reader.Read();
                                    //Vi hämtar PropertyName och med hjälp av en switch sats lagrar vi informationen i rätt variabel.
                                    switch (propertyName)
                                    {
                                        case "Name":
                                            itemName = reader.GetString();
                                            break;
                                        case "Unit":
                                            itemUnit = reader.GetString();
                                            break;
                                        case "Price":
                                            itemPrice = reader.GetDecimal();
                                            break;
                                        case "Amount":
                                            itemAmount = reader.GetInt32();
                                            break;
                                        default: throw new JsonException();

                                    }
                                }
                                else if (reader.TokenType == JsonTokenType.EndObject)
                                {
                                    //Om TokenType är lika med EndObject har vi kommit till slutet av produkten. Vi hämtar en referens till just den produkten ut ProductCollection.
                                    //Då kommer produkterna som läggs till här och i shoppen att peka på samma objekt, om det nu är samma objekt. Du förstår vad jag menar.
                                    //Om produkten inte längre finns i produktdatabasen kommer den inte att läggas till i kundvagnen.

                                    if (ProductCollection.GetProductByReference(itemName, itemUnit, itemPrice) != null)
                                    {
                                        cart.Add(new CartItem() { Product = ProductCollection.GetProductByReference(itemName, itemUnit, itemPrice), Amount = itemAmount });
                                    }
                                }
                            }
                            break;
                    }
                    
                }
            }
            //Om vi kommer hit och ingen kund har retunerats är JSON ogiltlig, och vi ger en exception
            throw new JsonException();

        }

        public override void Write(
            Utf8JsonWriter writer, Customer customer, JsonSerializerOptions options)
        {
            //En metod som talar om hur JsonSerializer ska serialize:a ett Customer objekt till JSON. Override:ar base metoden Write() i JsonConverter

            //Vi skriver ut ett StartToken. Vi talar alltså om att här är början på ett JSON objekt.
            writer.WriteStartObject();
            
            //Beroende på vilken typ av kund som skickats in så skriver vi ut en property med namnet "CustomerLevel". Den kommer få värdet 0 om det är en baskund, 1 om det bronskund osv.
            switch (customer)
            {
                case BronzeCustomer:
                    writer.WriteNumber("CustomerLevel", 1);
                    break;
                case SilverCustomer:
                    writer.WriteNumber("CustomerLevel", 2);
                    break;
                case GoldCustomer:
                    writer.WriteNumber("CustomerLevel", 3);
                    break;
                case Customer:
                    writer.WriteNumber("CustomerLevel", 0);
                    break;
                default: throw new JsonException();
            }
            
            //Vi skriver sedan ut alla Customers properties som JSON properties. Lösenordet krypteras med hjälp av metoden Encrypt()

            writer.WriteString("Name", customer.Name);
            writer.WriteString("Password", Encrypt(customer.Password));
            writer.WriteNumber("Currency", (int)customer.Currency);

            //Vi skriver även ut kundvagnen som en JSON array.
            writer.WriteStartArray("Cart");

            foreach (var cartItem in customer.Cart)
            {
                //För varje produkt i kundvagnen skriver vi ett StartObject token, sedan skriver vi ut alla properties, och sen ett EndObject token
                writer.WriteStartObject();
               
                writer.WriteString("Name", cartItem.Product.Name);
                writer.WriteString("Unit", cartItem.Product.Unit);
                writer.WriteNumber("Price", cartItem.Product.Price);
                writer.WriteNumber("Amount", cartItem.Amount);

                writer.WriteEndObject();
            }
            //Vi avslutar JSON array:n för kundvagnen
            writer.WriteEndArray();
            //Vi avslutar JSON objektet för customer.
            writer.WriteEndObject();
        }
        private static string Encrypt(string input)
        {
            //En enkel "krypterare" som krypterar med hjälp av XOR på byte nivå. Alla bokstäver på en jämn position i strängen kommer att krypteras med 0x53 och alla på ojämn position med 0x32
            byte byteConstantEven = 0x53;
            byte byteConstantUneven = 0x32;


            byte[] data = Encoding.UTF8.GetBytes(input);
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (i % 2) switch
                {
                    0 => (byte)(data[i] ^ byteConstantEven),
                    1 => (byte)(data[i] ^ byteConstantUneven)
                };
            }
            //Sen retunerar den strängen konverterad till Base64 för att säkert kunna spara till textfil
            return Convert.ToBase64String(data);
        }
        private static string Decrypt(string input)
        {
            //En enkel "krypterare" som dekrypterar(?) med hjälp av XOR på byte nivå. Alla bokstäver på en jämn position i strängen kommer att dekrypteras med 0x53 och alla på ojämn position med 0x32
            byte byteConstantEven = 0x53;
            byte byteConstantUneven = 0x32;

            //Vi konverterar strängen från Base64
            byte[] data = Convert.FromBase64String(input); ;
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (i % 2) switch
                {
                    0 => (byte)(data[i] ^ byteConstantEven),
                    1 => (byte)(data[i] ^ byteConstantUneven)
                };
            }

            return Encoding.UTF8.GetString(data);
        }
    }
}
