using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
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


        public void UpsertQuestionnaire(Questionnaire questionnaire)
        {
            MongoCollection<Questionnaire> collection = GetQuestionnaireCollection();
            try
            {
                collection.Save(questionnaire, WriteConcern.Acknowledged);
            }
            catch (MongoCommandException ex)
            {
                string msg = ex.Message;
            }
        }

        public Questionnaire FindQuestionnaire(string id)
        {
            MongoCollection<Questionnaire> collection = GetQuestionnaireCollection();
            var query = Query<Questionnaire>.EQ(e => e.Id, id);

            try
            {
                Questionnaire findQuestionnaire = collection.FindOne(query);
                return findQuestionnaire;
            }
            catch (MongoConnectionException ex)
            {
                string msg = ex.Message;
            }
            return null;
        }

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
        }
    }

    public class QuestionnaireNotFoundException : Exception
    {
    }
}