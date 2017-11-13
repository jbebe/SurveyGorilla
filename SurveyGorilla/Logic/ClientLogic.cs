using System;
using System.Collections.Generic;
using System.Linq;
using SurveyGorilla.Models;

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
    }
}
