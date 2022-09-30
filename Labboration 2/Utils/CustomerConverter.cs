using System;
using System.Collections.Generic;
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

            string? propertyName = reader.GetString();
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
            string? name = "";
            string? password = "";
            Currecies currency = Currecies.SEK;


            if (!reader.Read() || reader.GetString() != "CustomerValue")
            {
                throw new JsonException();
            }
            if (!reader.Read() || reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            Customer customer;

            switch (customerType)
            {
                case 0:
                    customer = (Customer)JsonSerializer.Deserialize(ref reader, typeof(Customer));
                    break;
                case 1:
                    customer = (BronzeCustomer)JsonSerializer.Deserialize(ref reader, typeof(BronzeCustomer));
                    break;
                case 2:
                    customer = (SilverCustomer)JsonSerializer.Deserialize(ref reader, typeof(SilverCustomer));
                    break;
                case 3:
                    customer = (GoldCustomer)JsonSerializer.Deserialize(ref reader, typeof(GoldCustomer));
                    break;
                default: throw new JsonException();

            }
            /*Customer customer = customerType switch
            {
                0 => new Customer(name, password),
                1 => new BronzeCustomer(name, password),
                2 => new SilverCustomer(name, password),
                3 => new GoldCustomer(name, password),
                _ => throw new JsonException()
            };*/

            if (!reader.Read() || reader.TokenType != JsonTokenType.EndObject)
            {
                throw new JsonException();
            }

            return customer;

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
                    JsonSerializer.Serialize(writer, customer);
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

            writer.WritePropertyName("CustomerValue");
            JsonSerializer.Serialize(writer, customer);

            /*writer.WriteString("Name", customer.Name);
            writer.WriteString("Password", customer.Password); //Säkerhet 101
            writer.WriteNumber("Currency", (int)customer.Currency);*/

            /*
            writer.WriteStartArray("Cart");

            foreach (var cartItem in customer.Cart)
            {
                writer.WriteStartObject();

                writer.WriteString("Name", cartItem.Name);
                writer.WriteString("Unit", cartItem.Unit);
                writer.WriteNumber("Price", cartItem.Price);

                writer.WriteEndObject();
            }
            
            writer.WriteEndArray();
            */
            writer.WriteEndObject();
        }
    }
}
