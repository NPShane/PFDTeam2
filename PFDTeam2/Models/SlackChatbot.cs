/*

using System;
using System.Threading.Tasks;
using SlackNet;
using SlackNet.Interaction;
using SlackNet.Interaction.Experimental.SlashCommands;
using SlackNet.Interaction.Experimental.SlashCommands.Request;

namespace PFDTeam2.Models
{
	public class SlackChatbot
	{
		private readonly ISlackApiClient slackApiClient;

		public SlackChatbot(string appToken)
		{
			slackApiClient = new SlackApiClient(appToken);
		}

		public async Task StartAsync()
		{
			// Implement your setup and event handling logic here

			// Example: Responding to a slash command
			slackApiClient.Interactivity (SlashCommandCallback);
		}

		private async Task<SlashCommandResponse> HandleSlashCommand(SlashCommandRequest command)
		{
			// Implement your logic for handling slash commands
			// You can access command.Text, command.UserId, and other properties
			string responseText = ProcessSlashCommand(command.Text);

			return new SlashCommandResponse
			{
				ResponseType = ResponseType.InChannel, // Or another response type
				Text = responseText
			};
		}

		private string ProcessSlashCommand(string commandText)
		{
			// chatbot logic
			// process the commandText, generate a response
			return "Welcome to Workato! How may I help you?";
		}

		public async Task StopAsync()
		{
			// Clean up and stop any running tasks if necessary
			if (slackApiClient is IDisposable disposableClient)
			{
				disposableClient.Dispose();
			}
		}
	}
}
*/