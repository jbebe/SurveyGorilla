using System;
using System.Collections.Generic;
using System.Linq;
using SurveyGorilla.Models;
using Newtonsoft.Json.Linq;
using SurveyGorilla.Misc;
using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace SurveyGorilla.Logic
{
    public class ClientLogic
    {
        private SurveyContext _context;

        public ClientLogic(SurveyContext context)
        {
            _context = context;
        }

        public IEnumerable<ClientEntity> GetAllClient(int adminId, int surveyId)
        {
            return _context.Clients
                .Include(s => s.Survey)
                .Where(client => client.Survey.AdminId == adminId && client.SurveyId == surveyId);
        }

        public ClientEntity GetClient(int adminId, int surveyId, int clientId)
        {
            return _context.Clients.Include(c => c.Survey).Single(client =>
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
            if (!_context.Surveys.Any(s => s.Id == surveyId && s.AdminId == adminId))
            {
                throw new Exception("This survey does not belong to current admin!");
            }
            client.Email = clientData.Email;
            client.Info = clientData.Info;
            client.SurveyId = surveyId;
            client.Token = Crypto.Sha256($"{client.Email}{adminId}{surveyId}");
            _context.Add(client);
            _context.SaveChanges();
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
                client.Email = clientData.Email;
            }
            if (clientData.Info != null)
            {
                client.Info = clientData.Info;
            }

            _context.SaveChanges();
            return client;
        }

        public ClientEntity UpdateClientByToken(string token, ClientData clientData)
        {
            if (!IsTokenValid(token))
            {
                return null;
            }
            var client = _context.Clients.Single(c => c.Token == token);
            client.Info = clientData.Info;

            _context.SaveChanges();
            return client;
        }

        private bool IsTokenValid(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new Exception("Token is missing from path!");
            }
            var client = _context.Clients.Include(c => c.Survey).Single(c => c.Token == token);
            dynamic surveyInfoObj = client.Survey.Info.ToObject();
            dynamic availability = surveyInfoObj.availability;
            var surveyStart = JsonConvert.DeserializeObject<DateTime>(availability.start);
            var surveyEnd = JsonConvert.DeserializeObject<DateTime>(availability.end);
            var currentGMT = DateTime.UtcNow;
            if (currentGMT >= surveyStart && currentGMT <= surveyEnd)
            {
                return true;
            } else
            {
                throw new ArgumentOutOfRangeException($"({currentGMT} >= {surveyStart} && {currentGMT} <= {surveyEnd}) is not ");
            }
        }

        public object GetInfoByToken(string token)
        {
            var client = _context.Clients
                .Include(c => c.Survey)
                .ThenInclude(s => s.Admin)
                .Single(c => c.Token == token);
            return new
            {
                adminName = client.Survey.Admin.Info.ToObject().Value<string>("name"),
                surveyName = client.Survey.Name,
                surveyStart = client.Survey.Info.ToObject()["availability"].Value<string>("start"),
                surveyEnd = client.Survey.Info.ToObject()["availability"].Value<string>("end"),
                questions = client.Survey.Info.ToObject()["questions"].ToString(Formatting.None)
            };
        }
    }
}
