﻿using Microsoft.AspNetCore.Http;
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

    }
}
