using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using SurveyGorilla.Misc;
using SurveyGorilla.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SurveyGorilla.Logic
{
    public class MailLogic
    {
        public static string SendGridApiKey;
        private static readonly EmailAddress _noreplyAddr = new EmailAddress("noreply@surveygorilla.com", "noreply");

        private readonly SurveyContext _context;
        private readonly Func<HttpRequest> _request;
        private HttpRequest _Request
        {
            get
            {
                return _request();
            }
        }

        public MailLogic(SurveyContext context, Func<HttpRequest> getRequest)
        {
            _context = context;
            _request = getRequest;
        }
        
        public SendGrid.Response SendToAllSurveyMembers(int adminId, int surveyId)
        {
            var survey = _context.Surveys.Single(s => s.Id == surveyId && s.AdminId == adminId);
            var clients = _context.Clients.Where(c => c.SurveyId == surveyId).ToList();
            var surveyInfo = survey.Info.ToObject();
            var surveyStart = surveyInfo.SelectToken("availability").Value<DateTime>("start");
            var surveyEnd = surveyInfo.SelectToken("availability").Value<DateTime>("end");
            var from = _noreplyAddr;
            var tos = clients
                .Select(c => {
                    var name = c.Info.ToObject().Value<string>("name");
                    return new EmailAddress(c.Email, name);
                })
                .ToList();
            var subjects = clients.Select(c => 
                $"Incoming survey from {c.Survey.Admin.Info.ToObject().Value<string>("name")} @surveygorilla.com").ToList();
            var link = $"{_Request.Scheme }://{_Request.Host}{_Request.PathBase}/survey";
            var htmlContent =
                $"Survey name: -surveyName-<br>" +
                $"Available from -surveyStart- to -surveyEnd-<br>" +
                $"Access the survey here:<br>" +
                $"<a href=\"-tokenLink-\">-tokenLink-</a><br>" +
                $"Your name: -clientName-";
            var substitutions = clients.Select(c => new Dictionary<string, string>
            {
                { "surveyName", c.Survey.Name },
                { "surveyStart", surveyStart.ToString() },
                { "surveyEnd", surveyEnd.ToString() },
                { "tokenLink", $"{link}/{c.Token}" },
                { "clientName", c.Info.ToObject().Value<string>("name") },
            }).ToList();
            var msg = MailHelper.CreateMultipleEmailsToMultipleRecipients(from, tos, subjects, null, htmlContent, substitutions);
            var gridClient = new SendGridClient(SendGridApiKey);
            var response = gridClient.SendEmailAsync(msg).Result;
            return response;
        }

        public SendGrid.Response SendToSurveyMember(int adminId, int surveyId, int clientId)
        {
            var client = _context.Clients.Single(c => c.Id == clientId && c.SurveyId == surveyId && c.Survey.AdminId == adminId);
            dynamic surveyInfo = client.Survey.Info.ToObject();
            dynamic availability = surveyInfo.availability;
            var surveyStart = JsonConvert.DeserializeObject<DateTime>(availability.start);
            var surveyEnd = JsonConvert.DeserializeObject<DateTime>(availability.end);
            dynamic info = client.Info.ToObject();
            var from = _noreplyAddr;
            var to = new EmailAddress(client.Email, info.name);
            var gridClient = new SendGridClient(SendGridApiKey);
            var subject = $"Incoming survey from {client.Survey.Admin.Info.ToObject().Value<string>("name")} @surveygorilla.com";
            var link = $"{_Request.Scheme }://{_Request.Host}{_Request.PathBase}/survey/{client.Token}";
            var htmlContent =
                $"Survey name: {client.Survey.Name}<br>" +
                $"Available from {surveyStart} to {surveyEnd}<br>" +
                $"Access the survey here:<br>" +
                $"<a href=\"{link}\">{link}</a><br>" +
                $"Your name: {info.name}";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);
            var response = gridClient.SendEmailAsync(msg).Result;
            return response;
        }

    }
}
