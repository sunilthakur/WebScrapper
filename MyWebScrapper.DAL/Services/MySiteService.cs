using MongoDB.Driver;
using MyWebScrapper.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebScrapper.DAL.Services
{
    public class MySiteService
    {
        private readonly IMongoCollection<SiteDetails> siteDetails;

        public MySiteService(IMongoDbContext settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            siteDetails = database.GetCollection<SiteDetails>(settings.BooksCollectionName);
        }

        public List<SiteDetails> Get() =>
            siteDetails.Find(site => true).ToList();

        public SiteDetails Get(string id) =>
            siteDetails.Find<SiteDetails>(site => site.Id == id).FirstOrDefault();

        public SiteDetails Create(SiteDetails book)
        {
            siteDetails.InsertOne(book);
            return book;
        }
    }
}
