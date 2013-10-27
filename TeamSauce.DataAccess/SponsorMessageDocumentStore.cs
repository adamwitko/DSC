using System;
using System.Linq;
using System.Collections.Generic;
using MongoDB.Driver;
using TeamSauce.DataAccess.Model;

namespace TeamSauce.DataAccess
{
    public class SponsorMessageDocumentStore : IDisposable
    {
        private MongoServer _mongoServer = null;
        private bool _disposed = false;
        private string _connectionString;

        private string _dbName = "TeamSauceMongoLab";
        private string _collectionName = "SponsorMessage";

        public SponsorMessageDocumentStore()
        {
        }

        public SponsorMessageDocumentStore(string connectionstring)
        {
            _connectionString = connectionstring;
        }

        public IEnumerable<SponsorMessageDto> GetMessages()
        {
            var collection = GetSponsorMessageCollection();
            return collection.FindAll().OrderByDescending(message => message.Time).ToList();
        }

        public void PersistMessage(SponsorMessageDto message)
        {
            var collection = GetSponsorMessageCollection();
            collection.Insert(message);
        }

        private MongoCollection<SponsorMessageDto> GetSponsorMessageCollection()
        {
            var mongoClient = new MongoClient(_connectionString);
            _mongoServer = mongoClient.GetServer();
            var database = _mongoServer.GetDatabase(_dbName);
            var notesCollection = database.GetCollection<SponsorMessageDto>(_collectionName);
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
}
