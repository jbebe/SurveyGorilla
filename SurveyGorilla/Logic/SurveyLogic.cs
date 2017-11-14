using Newtonsoft.Json;
using SurveyGorilla.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace SurveyGorilla.Logic
{
    public class SurveyLogic
    {
        private readonly SurveyContext _context;

        public SurveyLogic(SurveyContext context)
        {
            _context = context;
        }

        public IEnumerable<SurveyEntity> GetAllSurvey(int adminId)
        {
            return _context.Surveys.Where(survey => survey.AdminId == adminId);
        }

        public SurveyEntity GetSurvey(int adminId, int surveyId)
        {
            return _context.Surveys.Single(survey => survey.AdminId == adminId && survey.Id == surveyId);
        }

        public SurveyEntity UpdateSurvey(int adminId, int surveyId, SurveyData data)
        {
            var survey = GetSurvey(adminId, surveyId);
            var dbInfo = JObject.Parse(survey.Info);
            var newInfo = JObject.Parse(data.Info);
            newInfo.Merge(dbInfo, new JsonMergeSettings
            {
                MergeArrayHandling = MergeArrayHandling.Merge
            });
            survey.Info = newInfo.ToString();
            return survey;
        }

        public SurveyEntity CreateSurvey(int adminId, SurveyData data)
        {
            var survey = new SurveyEntity();
            survey.AdminId = adminId;
            var surveyInfo = new {
                created = DateTime.UtcNow,
                name = JObject.Parse(data.Info).Value<string>("name")
            };
            survey.Info = JsonConvert.SerializeObject(surveyInfo);
            _context.Surveys.Add(survey);
            _context.SaveChanges();
            return survey;
        }
        
        public SurveyEntity DeleteSurvey(int adminId, int surveyId)
        {
            var surveyEntity = GetSurvey(adminId, surveyId);
            _context.Surveys.Remove(surveyEntity);
            _context.SaveChanges();
            return surveyEntity;
        }

    }
}
