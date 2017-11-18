#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using Newtonsoft.Json;
using SurveyGorilla.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SurveyGorilla.Misc;

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

        public SurveyEntity UpdateSurvey(int adminId, int surveyId, SurveyData surveyData)
        {
            var survey = GetSurvey(adminId, surveyId);
            if (surveyData.Name != null)
            {
                survey.Name = surveyData.Name;
            }
            if (surveyData.Info != null)
            {
                var dbInfo = JObject.Parse(survey.Info);
                var newInfo = JObject.Parse(surveyData.Info);
                newInfo.Merge(dbInfo, new JsonMergeSettings
                {
                    MergeArrayHandling = MergeArrayHandling.Concat
                });
                survey.Info = newInfo.ToString();
            }
            _context.SaveChanges();
            return survey;
        }

        public SurveyEntity CreateSurvey(int adminId, SurveyData surveyData)
        {
            var survey = new SurveyEntity();
            if (new[] { surveyData.Name, surveyData.Info }.Any(entry => entry == null))
            {
                throw new Exception("Important properties were not filled!");
            }
            survey.AdminId = adminId;
            survey.Name = surveyData.Name;
            var surveyInfo = new {
                created = DateTime.UtcNow
            };
            survey.Info = surveyInfo.ToJson();
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
