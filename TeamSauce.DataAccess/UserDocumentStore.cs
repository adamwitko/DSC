using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using TeamSauce.DataAccess.Model;

namespace TeamSauce.DataAccess
{
    public class UserDocumentStore : IDisposable
    {
        private MongoServer _mongoServer = null;
        private bool _disposed = false;
        private string _connectionString;

        private string _dbName = "TeamSauceMongoLab";
        private string _collectionName = "user";

        public UserDocumentStore()
        {
            
        }

        public UserDocumentStore(string connectionstring)
        {
            _connectionString = connectionstring;
        }

        public UserDto IsUserValid(string username, string password)
        {
            try
            {
                var query = Query<UserDto>.EQ(user => user.Username, username);
                
                var collection = GetUserCollection();
                var result = collection.FindAs<UserDto>(query);
                return result.Single();
            }
            catch (MongoConnectionException)
            {
                return null;
            }
        }

        private MongoCollection<UserDto> GetUserCollection()
        {
            var mongoClient = new MongoClient(_connectionString);
            _mongoServer = mongoClient.GetServer();
            var database = _mongoServer.GetDatabase(_dbName);
            var notesCollection = database.GetCollection<UserDto>(_collectionName);
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
