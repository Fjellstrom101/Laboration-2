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

        public override bool CanConvert(Type typeToConvert) =>
            typeof(Customer).IsAssignableFrom(typeToConvert);

        public override Customer Read(
            ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            reader.Read();
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }

            var propertyName = reader.GetString();
            if (propertyName != "CustomerLevel")
            {
                throw new JsonException();
            }

            reader.Read();
            if (reader.TokenType != JsonTokenType.Number)
            {
                throw new JsonException();
            }

            var customerType = reader.GetInt32();
            var name = "";
            var password = "";
            var currency = Currencies.SEK;
            var cart = new List<CartItem>();


            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
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
                            var itemName = string.Empty;
                            var itemPrice = 0m;
                            var itemUnit = string.Empty;
                            var itemAmount = 0;

                            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                            {
                                if (reader.TokenType == JsonTokenType.PropertyName)
                                {
                                    propertyName = reader.GetString();
                                    reader.Read();

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
                                    cart.Add(new CartItem(){ Product = ProductCollection.GetProductByReference(itemName, itemUnit, itemPrice) , Amount = itemAmount});
                                }
                            }
                            break;
                    }
                    
                }
            }

            throw new JsonException();

        }

        public override void Write(
            Utf8JsonWriter writer, Customer customer, JsonSerializerOptions options)
        {
            if (customer == null) return;

            writer.WriteStartObject();
            
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
            

            writer.WriteString("Name", customer.Name);
            writer.WriteString("Password", Encrypt(customer.Password));
            writer.WriteNumber("Currency", (int)customer.Currency);

            
            writer.WriteStartArray("Cart");

            foreach (var cartItem in customer.Cart)
            {
                writer.WriteStartObject();

                writer.WriteString("Name", cartItem.Product.Name);
                writer.WriteString("Unit", cartItem.Product.Unit);
                writer.WriteNumber("Price", cartItem.Product.Price);
                writer.WriteNumber("Amount", cartItem.Amount);

                writer.WriteEndObject();
            }
            
            writer.WriteEndArray();
            
            writer.WriteEndObject();
        }
        private static string Encrypt(string input)
        {
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

            return Convert.ToBase64String(data);
        }
        private static string Decrypt(string input)
        {
            byte byteConstantEven = 0x53;
            byte byteConstantUneven = 0x32;


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
