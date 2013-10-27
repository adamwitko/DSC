using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using TeamSauce.DataAccess.Model;

namespace TeamSauce.DataAccess
{
    public class TeamMessageDocumentStore : IDisposable
    {
        private MongoServer _mongoServer = null;
        private bool _disposed = false;
        private string _connectionString;

        private string _dbName = "TeamSauceMongoLab";
        private string _collectionName = "TeamMessage";

        public TeamMessageDocumentStore()
        {
        }

        public TeamMessageDocumentStore(string connectionstring)
        {
            _connectionString = connectionstring;
        }

        public IEnumerable<TeamMessageDto> GetMessages()
        {
            return GetSponsorMessageCollection().FindAll().ToArray();
        }

        public void PersistMessage(TeamMessageDto dto)
        {
            var collection = GetSponsorMessageCollection();
            collection.Insert(dto);
        }

        private MongoCollection<TeamMessageDto> GetSponsorMessageCollection()
        {
            var mongoClient = new MongoClient(_connectionString);
            _mongoServer = mongoClient.GetServer();
            var database = _mongoServer.GetDatabase(_dbName);
            var messages = database.GetCollection<TeamMessageDto>(_collectionName);
            return messages;
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
