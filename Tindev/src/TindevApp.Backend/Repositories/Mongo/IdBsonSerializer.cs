using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TindevApp.Backend.Repositories.Mongo
{
    public class IdBsonSerializer : IBsonSerializer
    {
        public static IdBsonSerializer Instance { get; } = new IdBsonSerializer();

        public Type ValueType => typeof(string);

        public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var type = context.Reader.GetCurrentBsonType();
            switch (type)
            {
                case BsonType.ObjectId:
                    return context.Reader.ReadObjectId().ToString();
                case BsonType.String:
                    return context.Reader.ReadString();
                default:
                    var message = string.Format("Cannot convert a {0} to an Int32.", type);
                    throw new NotSupportedException(message);
            }
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            context.Writer.WriteObjectId(ObjectId.Parse(value.ToString()));
        }
    }
}
