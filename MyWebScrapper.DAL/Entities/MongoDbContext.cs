using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebScrapper.DAL.Entities
{
    public class MongoDbContext : IMongoDbContext
    {
        public string BooksCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IMongoDbContext
    {
        string BooksCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
