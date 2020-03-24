using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using RentalFlo.Repositories;
using RentalFlo.Repositories.MongoDB;

namespace RentalFlo.Repositories.MongoDB
{
    public class MongoHelper
    {
        public static IMongoCollection<T> GetCollection<T>(string connectionString, string databaseName, string collectionName)
        {
            var client = new MongoClient(connectionString);

            var database = client.GetDatabase(databaseName);

            var collection = database.GetCollection<T>(collectionName);

            return collection;
        }
    }
}

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddMongoDBCollection<T>(this IServiceCollection services, string connectionString, string databaseName, string collectionName)
        {
            return services.AddScoped<IMongoCollection<T>>(provider => MongoHelper.GetCollection<T>(connectionString, databaseName, collectionName));
        }
    }
}
