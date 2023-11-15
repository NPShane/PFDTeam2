using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PFDTeam2.Models;

namespace PFDTeam2.Controllers
{
    public class GoogleCalendarController : Controller
    {
        private static readonly string[] Scopes = { CalendarService.Scope.Calendar };
        private static readonly string ApplicationName = "WEB client 2";
        private static readonly string ClientSecretPath = "client_secret_381048076743-gu8s7o9uie7d0ate6fkaoh8l1c814fqa.apps.googleusercontent.com (1).json";
        private static readonly string CredentialsFolderPath = "App_Data/Calendar.API.Store";
        private readonly ILogger<GoogleCalendarController> _logger;
        private readonly IConfiguration _configuration;

        public GoogleCalendarController(IConfiguration configuration, ILogger<GoogleCalendarController> logger)
        {
            _configuration = configuration;
            _logger = logger;
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

        [HttpPost]
        public IActionResult CreateEvent(EventModel model, string googleAccount)
        {
            try
            {
                UserCredential credential;
                string[] Scopes = { "https://www.googleapis.com/auth/calendar" };
                using (var stream = new FileStream(ClientSecretPath, FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.FromStream(stream).Secrets,
                        Scopes,
                        googleAccount, // Use the selected Google account here
                        CancellationToken.None,
                        new FileDataStore(CredentialsFolderPath)).Result;
                }

                var service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

                // Create a new event
                var newEvent = new Event
                {
                    Summary = model.Summary,
                    Description = model.Description,
                    Start = new EventDateTime { DateTime = model.StartDateTime },
                    End = new EventDateTime { DateTime = model.EndDateTime },
                };

                // Add the event to the primary calendar
                var request = service.Events.Insert(newEvent, "primary");
                var createdEvent = request.Execute();

                // Log the created event ID using ILogger
                _logger.LogInformation("Event created: {EventId}", createdEvent.Id);

                // Redirect back to the Index action after creating the event
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log any exceptions using ILogger
                _logger.LogError(ex, "Error creating event");

                // Handle the exception or rethrow it
                throw;
            }
        }

        [HttpGet]
        public IActionResult CreateEvent()
        {
            try
            {
                // Fetch the list of test users from the OAuth consent screen
                var testUsers = GetTestUsersFromOAuthConsentScreen(); // Replace with your actual logic

                // Pass the list of test users to the view
                ViewBag.TestUsers = testUsers;

                return View("~/Views/Google/Create.cshtml");
            }
            catch (Exception ex)
            {
                // Log any exceptions using ILogger
                _logger.LogError(ex, "Error retrieving test users");

                // Handle the exception or rethrow it
                throw;
            }
        }

        [HttpGet]
        public IActionResult RetrieveCalendars(string specificUser)
        {
            try
            {
                // Fetch the list of test users from the OAuth consent screen
                var testUsers = GetTestUsersFromOAuthConsentScreen();

                // Check if the specific user is a valid test user
                if (!testUsers.Contains(specificUser))
                {
                    // Handle the case where the specific user is not a valid test user
                    return BadRequest("Invalid user");
                }

                foreach (var testUser in testUsers)
                {
                    // Authenticate with the credentials of the specific user
                    var credential = GetCredentialForTestUser(testUser);

                    var service = new CalendarService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = ApplicationName,
                    });

                    // Fetch the list of calendars for the specific user
                    var calendars = service.CalendarList.List().Execute().Items;

                    // Process the list of calendars (you may want to return or store this information)
                    foreach (var calendar in calendars)
                    {
                        // Process each calendar as needed
                        Console.WriteLine($"Calendar for {testUser}: {calendar.Summary}");
                    }
                }

                // Redirect or return a response as needed
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log any exceptions using ILogger
                _logger.LogError(ex, "Error retrieving calendars");

                // Handle the exception or rethrow it
                throw;
            }
        }

        private List<string> GetTestUsersFromOAuthConsentScreen()
        {
            // Your logic to retrieve test users from the OAuth consent screen
            // ...................................................

            // For example:
            return new List<string> { "ongchenyu7@gmail.com", "aar0planee@gmail.com", "joshuahee1234@gmail.com", "shane.leong.nah16@gmail.com" };
        }

        private UserCredential GetCredentialForTestUser(string testUser)
        {
            // Your logic to obtain credentials for the specified test user
            // ...................................................

            // For example, modify the existing credential retrieval logic:
            string[] Scopes = { "https://www.googleapis.com/auth/calendar" };
            using (var stream = new FileStream(ClientSecretPath, FileMode.Open, FileAccess.Read))
            {
                return GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes,
                    testUser, // Use the specified test user here
                    CancellationToken.None,
                    new FileDataStore(CredentialsFolderPath)).Result;
            }
        }

        // Define a class to represent the response from the downloadAccount endpoint
        public class DownloadAccountResponse
        {
            public List<UserInfo> users { get; set; }
        }

        // Define a class to represent user information in the response
        public class UserInfo
        {
            public string email { get; set; }
            // Add other user properties as needed
        }
    }
}
