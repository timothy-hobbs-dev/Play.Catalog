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
        public static void AddMongoDb<T>(this WebApplicationBuilder builder, string collectionName)
        where T : IEntity
        {
            var serviceSettings = builder.Configuration
            .GetSection(nameof(ServiceSettings))
            .Get<ServiceSettings>();
            var mongoDBSettings = builder.Configuration
            .GetSection(nameof(MongoDBSettings))
            .Get<MongoDBSettings>();

            var mongoDbClient = new MongoClient(mongoDBSettings.ConnnectionString);
            builder.Services.AddSingleton(mongoDbClient.GetDatabase(serviceSettings.ServiceName));
            builder.Services.AddSingleton<IRepository<T>>(sp =>
            {
                var db = sp.GetService<IMongoDatabase>();
                return new MongoRepository<T>(db!, collectionName);
            });


            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

        }
    }
}