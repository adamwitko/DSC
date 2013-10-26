using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using TeamSauce.DataAccess.Model;
using Teamsace.DataAccess.Model;

namespace TeamSauce.DataAccess
{
    public class QuestionnaireDocumentStore : IDisposable
    {
        private MongoServer _mongoServer = null;
        private bool _disposed = false;
        private string _connectionString;

        private string _dbName = "TeamSauceMongoLab";
        private string _collectionName = "questionnaire";


        public QuestionnaireDocumentStore()
        {
            
        }

        public QuestionnaireDocumentStore(string connectionstring)
        {
            _connectionString = connectionstring;
        }

        public List<Questionnaire> GetAllQuestionnaires()
        {
            try
            {
                MongoCollection<Questionnaire> collection = GetQuestionnaireCollection();
                return collection.FindAll().ToList<Questionnaire>();
            }
            catch (MongoConnectionException)
            {
                return new List<Questionnaire>();
            }
        }


        public void CreateQuestionnaire(Questionnaire questionnaire)
        {
            MongoCollection<Questionnaire> collection = GetQuestionnaireCollection();
            try
            {
                collection.Insert(questionnaire, WriteConcern.Acknowledged);
            }
            catch (MongoCommandException ex)
            {
                string msg = ex.Message;
            }
        }

       /* public Questionnaire FindQuestionnaire(Guid id)
        {
            try
            {
                MongoClient mongoClient = new MongoClient(_connectionString);
                _mongoServer = mongoClient.GetServer();
                MongoDatabase database = _mongoServer.GetDatabase(_dbName);
                MongoCollection<Questionnaire> notesCollection = database.G<Questionnaire>(_collectionName);

            }
            catch (Exception ex)
            {
                throw new QuestionnaireNotFoundException();
            }
            
        }*/

        private MongoCollection<Questionnaire> GetQuestionnaireCollection()
        {
            MongoClient mongoClient = new MongoClient(_connectionString);
            _mongoServer = mongoClient.GetServer();
            MongoDatabase database = _mongoServer.GetDatabase(_dbName);
            MongoCollection<Questionnaire> notesCollection = database.GetCollection<Questionnaire>(_collectionName);
            return notesCollection;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public class QuestionnaireNotFoundException : Exception
    {
    }
}