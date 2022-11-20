using Bookstore.Domain.Aggregates.UserAggregate;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Bookstore.Domain.Mongo.Maps
{
    public static class MongoClassMaps
    {
        public static void RegisterMaps()
        {
            BsonClassMap.RegisterClassMap<User>(cm =>
            {
                cm.AutoMap();
                cm.MapMember(c => c.Birthday).SetSerializer(new DateTimeSerializer(dateOnly: true));
            });
        }
    }
}
