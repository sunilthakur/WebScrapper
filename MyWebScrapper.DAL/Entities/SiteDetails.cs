using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebScrapper.DAL.Entities
{
    public class SiteDetails
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string SiteName { get; set; }
        public string SiteTitle { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public List<string> SiteLinks { get; set; }
        public List<string> SiteSocialLinks { get; set; }
        public string SiteImage { get; set; }
    }
}
