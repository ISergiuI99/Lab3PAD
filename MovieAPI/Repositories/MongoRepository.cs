using Common.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MovieAPI.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieAPI.Repositories
{
    public class MongoRepository<T> : IMongoRepository<T> where T : MongoDocument
    {
        private readonly IMongoDatabase _db;
        private readonly IMongoCollection<T> _Collection;
        public MongoRepository(IMongoDbSettings dbSettings)
        {
            _db = new MongoClient(dbSettings.ConnectionString).GetDatabase(dbSettings.DatabaseName);

            string tableName = typeof(T).Name.ToLower();
            _Collection = _db.GetCollection<T>(tableName);

        }

        public void DeleteRecord(Guid id)
        {
            _Collection.DeleteOne(doc => doc.Id == id);
        }

        public List<T> GetAllRecords()
        {
            var records = _Collection.Find(new BsonDocument()).ToList();
            return records;

        }

        public T GetRecordById(Guid id)
        {
            var record = _Collection.Find(doc => doc.Id == id).FirstOrDefault();
            
            return record;
        }

        public T InsertRecord(T record)
        {
            _Collection.InsertOne(record);
            return record;
        }

        public void DeleteRecord(T record)
        {
            _Collection.ReplaceOne(doc => doc.Id == record.Id, record, new ReplaceOptions() { IsUpsert = true });
        }
    }
}
