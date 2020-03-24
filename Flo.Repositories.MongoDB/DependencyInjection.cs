using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using Flo.Repositories;
using Flo.Repositories.MongoDB;

namespace Flo.Repositories.MongoDB
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

        public static IServiceCollection AddMongoDBRepo<T, TKeyType, TRepo, TValidator, TAuthoriser>(this IServiceCollection services, string connectionString, string databaseName, string collectionName)
            where T : Entity<TKeyType>
            where TRepo : class, IRepository<T, TKeyType>
            where TValidator : class, IValidator<T, TKeyType>
            where TAuthoriser : class, IAuthoriser<T, TKeyType>
        {
                 return services.AddMongoDBCollection<T>(connectionString, databaseName, collectionName)
                .AddScoped<IRepository<T, TKeyType>, TRepo>()
                .AddScoped<IValidator<T, TKeyType>, TValidator>()
                .AddScoped<IAuthoriser<T, TKeyType>, TAuthoriser>()
                .AddScoped<IRawGet<T, TKeyType>, RawGet<T, TKeyType>>()
                .AddScoped<ICrud<T, TKeyType>, Crud<T, TKeyType>>();
        }
    }
}
