#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using System;
using System.Collections.Generic;
using System.Linq;
using SurveyGorilla.Models;
using Newtonsoft.Json.Linq;
using SurveyGorilla.Misc;

namespace SurveyGorilla.Logic
{
    public class ClientLogic
    {
        private SurveyContext _context;

        public ClientLogic(SurveyContext context)
        {
            _context = context;
        }

        public IEnumerable<ClientEntity> GetAllClient(int adminId)
        {
            return _context.Clients.Where(client => client.Survey.AdminId == adminId);
        }

        public ClientEntity GetClient(int adminId, int surveyId, int clientId)
        {
            return _context.Clients.Single(client =>
                client.Survey.AdminId == adminId &&
                client.SurveyId == surveyId &&
                client.Id == clientId
            );
        }

        public ClientEntity CreateClient(int adminId, int surveyId, ClientData clientData)
        {
            var client = new ClientEntity();
            if (new[] { clientData.Email, clientData.Info, clientData.Info.ToObject()["name"] }.Any(entry => entry == null))
            {
                throw new Exception("Important properties were not filled!");
            }
            client.EmailAddress = clientData.Email;
            client.Info = clientData.Info;
            client.SurveyId = surveyId;
            return client;
        }

        internal ClientEntity DeleteClient(int adminId, int surveyId, int clientId)
        {
            var client = GetClient(adminId, surveyId, clientId);
            _context.Remove(client);
            _context.SaveChanges();
            return client;
        }

        public ClientEntity UpdateClient(int adminId, int surveyId, int clientId, ClientData clientData)
        {
            var client = GetClient(adminId, surveyId, clientId);

            if (clientData.Email != null)
            {
                client.EmailAddress = clientData.Email;
            }
            if (clientData.Info != null)
            {
                var oldInfo = JObject.Parse(client.Info);
                var newInfo = JObject.Parse(clientData.Info);
                newInfo.Merge(oldInfo, new JsonMergeSettings()
                {
                    MergeArrayHandling = MergeArrayHandling.Union,
                    MergeNullValueHandling = MergeNullValueHandling.Ignore
                });
                client.Info = newInfo.ToString();
            }

            _context.SaveChanges();
            return client;
        }
    }
}
