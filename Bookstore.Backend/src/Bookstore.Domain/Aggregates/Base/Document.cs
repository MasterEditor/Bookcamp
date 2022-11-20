using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bookstore.Domain.Aggregates.Base
{
    public abstract class Document
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public ObjectId Id { get; protected set; }

        protected Document()
        {
        }

        protected Document(ObjectId id)
        {
            Id = id;
        }
    }
}
