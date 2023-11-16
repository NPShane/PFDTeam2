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
                id = e.Id,  // Include the event ID
                description = e.Description,
                title = e.Summary,
                start = e.Start.DateTime,
                end = e.End.DateTime,
                progress = e.ExtendedProperties
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

                // Create a new event with progress status
                var newEvent = new Event
                {
                    Summary = model.Summary,
                    Description = model.Description,
                    Start = new EventDateTime { DateTime = model.StartDateTime },
                    End = new EventDateTime { DateTime = model.EndDateTime },
                    //Progress = model.Progress,
                    ExtendedProperties = new Event.ExtendedPropertiesData
                    {
                        Shared = new Dictionary<string, string>
                {
                    { "Progress", model.Progress } // Set the Progress property from the model
                }
                    }
                };

                // Add the event to the primary calendar
                var request = service.Events.Insert(newEvent, googleAccount);
                var createdEvent = request.Execute();

                // Log the created event ID using ILogger
                _logger.LogInformation("Event created: {EventId}", createdEvent.Id);
                model.Id = createdEvent.Id;

                // Redirect back to the Index action after creating the event
                //return RedirectToAction("Index");
                return View("~/Views/Staff/StaffCalendar.cshtml");
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

                // Authenticate with the credentials of the specific user
                var credential = GetCredentialForTestUser(specificUser);

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
                    Console.WriteLine($"Calendar for {specificUser}: {calendar.Summary}");
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

        // Controllers/GoogleCalendarController.cs
        [HttpPost]
        public IActionResult DeleteEvent(string eventId)
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
                        "user",
                        CancellationToken.None,
                        new FileDataStore(CredentialsFolderPath)).Result;
                }

                var service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

                // Delete the event using the eventId
                service.Events.Delete("primary", eventId).Execute();

                return Ok();
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult UpdateEventProgress(string eventId, string progress)
        {
            try
            {
                if (!IsValidProgress(progress))
                {
                    // Log an error if an invalid progress value is provided
                    _logger.LogError("Invalid progress value: {Progress}", progress);
                    return BadRequest("Invalid progress value");
                }

                // Use the Google Calendar API to update the event progress
                UserCredential credential;
                string[] Scopes = { "https://www.googleapis.com/auth/calendar" };

                using (var stream = new FileStream(ClientSecretPath, FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.FromStream(stream).Secrets,
                        Scopes,
                        "user", // Use a default user for this purpose
                        CancellationToken.None,
                        new FileDataStore(CredentialsFolderPath)).Result;
                }

                var service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

                // Fetch the event from Google Calendar
                var existingEvent = service.Events.Get("primary", eventId).Execute();

                if (existingEvent == null)
                {
                    // Log an error if the event is not found
                    _logger.LogError("Event not found with ID: {EventId}", eventId);
                    return NotFound("Event not found");
                }

                // Check if ExtendedProperties is null
                if (existingEvent.ExtendedProperties == null)
                {
                    existingEvent.ExtendedProperties = new Event.ExtendedPropertiesData();
                }

                // Check if Shared is null
                if (existingEvent.ExtendedProperties.Shared == null)
                {
                    existingEvent.ExtendedProperties.Shared = new Dictionary<string, string>();
                }

                // Update the progress property of the event
                existingEvent.ExtendedProperties.Shared["Progress"] = progress;

                // Update the event on Google Calendar
                var request = service.Events.Update(existingEvent, "primary", eventId);
                var updatedEvent = request.Execute();

                // Log the event progress update using ILogger
                _logger.LogInformation("Event progress updated: EventId={EventId}, Progress={Progress}", eventId, progress);

                return Ok(); // Return a success response
            }
            catch (Google.GoogleApiException gex)
            {
                // Log Google API-specific exceptions
                _logger.LogError(gex, "Google API Exception updating event progress");
                return StatusCode(500, $"Google API Exception updating event progress: {gex.Error.Message}");
            }
            catch (Exception ex)
            {
                // Log any other exceptions using ILogger
                _logger.LogError(ex, "Error updating event progress");
                return StatusCode(500, $"Error updating event progress: {ex.Message}"); // Return a 500 Internal Server Error response with more details
            }
        }

        private bool IsValidProgress(string progress)
        {
            // Check if the progress value is one of the allowed values
            return progress == "completed" || progress == "in-progress" || progress == "not-started";
        }


    }
}