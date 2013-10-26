using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using Teamsace.DataAccess.Model;

namespace TeamSauce.DataAccess
{
    public class TestMongoDocumentStore : IDisposable
    {
        private MongoServer _mongoServer = null;
        private bool _disposed = false;
        private string _connectionString;

        private string _dbName = "TeamSauceMongoLab";
        private string _collectionName = "category";
        

        public TestMongoDocumentStore()
        {
        }

        public TestMongoDocumentStore(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void CreateCategory(Category category)
        {
            MongoCollection<Category> collection = GetCategoryCollection();
            try
            {
                collection.Insert(category, WriteConcern.Acknowledged);
            }
            catch (MongoCommandException ex)
            {
                string msg = ex.Message;
            }
        }

        public List<Category> GetAllCategories()
        {
            try
            {
                MongoCollection<Category> collection = GetCategoryCollection();
                return collection.FindAll().ToList<Category>();
            }
            catch (MongoConnectionException)
            {
                return new List<Category>();
            }
        }


        private MongoCollection<Category> GetCategoryCollection()
        {
            MongoClient mongoClient = new MongoClient(_connectionString);
            _mongoServer = mongoClient.GetServer();
            MongoDatabase database = _mongoServer.GetDatabase(_dbName);
            MongoCollection<Category> notesCollection = database.GetCollection<Category>(_collectionName);
            return notesCollection;
        }


        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    if (_mongoServer != null)
                    {
                        this._mongoServer.Disconnect();
                    }
                }
            }

            this._disposed = true;
        }
    }

    public class A
    {
        public A()
        {
            
        }

        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        public Guid Id { get; set; }

        public string c { get; set; }
    }
}
