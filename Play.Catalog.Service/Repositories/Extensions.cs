using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Settings;

namespace Play.Catalog.Service.Repositories
{
    public static class Extensions
    {
        public static void AddMongoDb(this WebApplicationBuilder builder)
        {
            var serviceSettings = builder.Configuration
            .GetSection(nameof(ServiceSettings))
            .Get<ServiceSettings>();
            var mongoDBSettings = builder.Configuration
            .GetSection(nameof(MongoDBSettings))
            .Get<MongoDBSettings>();

            var mongoDbClient = new MongoClient(mongoDBSettings.ConnnectionString);
            builder.Services.AddSingleton(mongoDbClient.GetDatabase(serviceSettings.ServiceName));
            builder.Services.AddSingleton<IRepository<Item>>(sp =>
            {
                var db = sp.GetService<IMongoDatabase>();
                return new MongoRepository<Item>(db!, "items");
            });


            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

        }
    }
}