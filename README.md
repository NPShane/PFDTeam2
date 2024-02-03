# PFD Team2
## Google Calendar Feature

### Supervisor
1. Create Event/Task by clicking the create event button and filling in the form. Page should redirect you back to the calendar with the new Event/Task inserted
2. Display employee calendar by choosing the user for monitoring (Implemented halfway) *Code is not included in submission to prevent breaks
 ![image](https://github.com/NPShane/PFDTeam2/assets/117629507/eab682ee-b26e-4585-9cd1-883828c3def1)

### Employee
1. Create Event/Task by clicking the create event button and filling in the form. Page should redirect you back to the calendar with the new Event/Task inserted
2. Update Event/Task Status by clicking on itself. A popup will display, prompting to mark it as the desired status (i.e Mark as Done, Mark as in Progress, etc.) In the pop-up, there is an option for the event to be deleted as well.

## Tutorial for slack bot setup

### Node.js Installation
1. Install node.js
2. Open the terminal and type "npm init -y" then "npm install @slack/bolt"

### Visual Studio Setup
1. In Visual Studio 2022, open the terminal and type "node.js"

### ngrok Installation
1. Install ngrok
2. Extract the contents from the zip file
3. Open ngrok.exe
4. Type "ngrok http 3000"
5. Copy the forwarding URL (e.g. https://abcd1234.ngrok.io)

### Slack Configuration
1. Go to "https://api.slack.com/apps/A065GAHDJG0"
2. Go to "Event Subscriptions"
3. Click the toggle button to Enable Events
4. Paste the forwarding URL in the "Request URL" section and add "/slack/events"
5. The final URL should look like "https://abcd1234.ngrok.io/slack/events"
6. Go to "Interactivity & Shortcuts"
7. Click the toggle button to enable Interactivity
8. Paste the same request URL (https://abcd1234.ngrok.io/slack/events)

### Running the bot
1. Go to "https://app.slack.com/client/T064S46TFL3/C064Q5LNSNR"
2. Go to the "#self-service" channel
3. Follow the instructions in the pinned message
4. The bot should now be running

## Feedback Feature

### Set up project for VADER and Python in C#
1. Check that the PythonNET package is installed (NuGet packages for the project, then install a local copy of 32-bit Python 3.7 (THIS EXACT VERSION) and change this one line of code (FeedbackController.cs in Controllers directory), line 78: Runtime.PythonDLL = ...; change the string to the python dll in the install for your system (it could be similar to the string that is currently there). Hopefully this works.

## Facial Recognition with FACEIO

### Instructions to set up integrated platform
1. Go to "https://faceio.net/"
2. Sign up for a console
3. Create an application
4. Click Next until you reach the end to eventually create an application
5. Retrieve the Application Public ID and replace the code show in the snippet below:
![image](https://github.com/NPShane/PFDTeam2/assets/117629507/89f3c281-8b72-4c41-b6d1-6a96ed20042d)
6. Run the project on IIS Express (Web Browser: Google Chrome). Note that that the browser you are running on should be linked to the account that you used to sign up for a FACEIO Console.
   



## Note
- The calendar function is prone to crashing due to an error connecting to the Google API. This might happen the first time it is run and subsequent runs are usually OK after.
- Trello link: https://trello.com/b/zkM2G9lG
  
