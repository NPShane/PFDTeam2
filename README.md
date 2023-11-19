# PFD Team2
## Google Calendar Feature

### Supervisor
1. Create Event/Task by clicking the create event button and filling in the form. Page should redirect you back to the calendar with the new Event/Task inserted
2. Display employee calendar by choosing the user for monitoring (Implemented halfway)
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

### Set up project for VADER and Python
1. CVheck that the PythonNET package is installed (NuGet packages for the project, then install a local copy of 32-bit Python 3.7 (THIS EXACT VERSION) and change this one line of code (FeedbackController.cs in Controllers directory), line 78: Runtime.PythonDLL = ...; change the string to the python dll in the install for your system (it could be similar to the string that is currently there). Hopefully this works.

## Note
- The calendar function is prone to crashing due to an error connecting to the Google API. This might happen the first time it is run and subsequent runs are usually OK after.
