using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SurveyGorilla.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyGorilla.Logic
{
    public class SurveyLogic
    {
        private readonly SurveyContext _context;

        public SurveyLogic(SurveyContext context)
        {
            _context = context;
        }

        public void CreateSurvey(ISession session, SurveyData data)
        {
            var survey = new SurveyEntity();
            survey.AdminId = session.GetInt32(Session.adminId).Value;
            var surveyInfo = new {
                created = DateTime.UtcNow,
                name = data.Name
            };
            survey.Info = JsonConvert.SerializeObject(surveyInfo);
            _context.Surveys.Add(survey);
            _context.SaveChanges();
        }

        public IEnumerable<SurveyEntity> GetAllSurvey(ISession session)
        {
            var adminId = session.GetInt32(Session.adminId);
            return _context.Surveys.Where(survey => survey.AdminId == adminId);
        }

        public SurveyEntity GetSurvey(ISession session, int id)
        {
            var adminId = session.GetInt32(Session.adminId);
            return _context.Surveys.Single(s => s.AdminId == adminId && s.Id == id);
        }
    }
}
