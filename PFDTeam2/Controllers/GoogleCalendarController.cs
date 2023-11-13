using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static System.Net.WebRequestMethods;

namespace PFDTeam2.Controllers
{
    public class GoogleCalendarController : Controller
    {

        private static readonly string[] Scopes = { CalendarService.Scope.Calendar };
        private static readonly string ApplicationName = "WEB client 2";
        private static readonly string ClientSecretPath = "C:\\Users\\User\\OneDrive\\Documents\\client_secret_381048076743-gu8s7o9uie7d0ate6fkaoh8l1c814fqa.apps.googleusercontent.com (1).json";
        private static readonly string CredentialsFolderPath = "App_Data/Calendar.API.Store";

        private readonly IConfiguration _configuration;

        public GoogleCalendarController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index(string someParameter)
        {
            string[] Scopes = { "https://www.googleapis.com/auth/calendar" };
            UserCredential credential;

            using (var stream = new FileStream(ClientSecretPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(CredentialsFolderPath)).Result;
            }

            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Now you have an authenticated service instance (service) to interact with Google Calendar API.
            // Proceed to display the calendar and add events.

            return View("~/Views/Google/Index.cshtml");
        }

        [HttpGet]
        public IActionResult GetEvents()
        {
            UserCredential credential;

            using (var stream = new FileStream(ClientSecretPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    new ClientSecrets
                    {
                        ClientId = _configuration["Authentication:Google:381048076743-gu8s7o9uie7d0ate6fkaoh8l1c814fqa.apps.googleusercontent.com"],
                        ClientSecret = _configuration["Authentication:Google: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"]
                    },
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(CredentialsFolderPath)).Result;
            }

            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define parameters for the request
            var listRequest = service.Events.List("primary");
            listRequest.TimeMin = DateTime.Now;
            listRequest.ShowDeleted = false;
            listRequest.SingleEvents = true;
            listRequest.MaxResults = 10; // Adjust as needed

            // Fetch the events from the API
            var events = listRequest.Execute().Items;

            // Convert events to a format suitable for FullCalendar
            var formattedEvents = events.Select(e => new
            {
                title = e.Summary,
                start = e.Start.DateTime,
                end = e.End.DateTime
            });

            return Json(formattedEvents);
        }
        public IActionResult AddEvent()
        {
            UserCredential credential;

            using (var stream = new FileStream(ClientSecretPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(CredentialsFolderPath)).Result;
            }

            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            var newEvent = new Event
            {
                Summary = "Event Name",
                Description = "Event Description",
                Start = new EventDateTime { DateTime = DateTime.Now.AddDays(1) },
                End = new EventDateTime { DateTime = DateTime.Now.AddDays(2) },
            };

            var request = service.Events.Insert(newEvent, "primary");
            var createdEvent = request.Execute();

            // Handle the response or redirect to another page.
            // You might want to store the createdEvent.Id for future updates/deletion.

            return RedirectToAction("Index");

        }
    }
}
