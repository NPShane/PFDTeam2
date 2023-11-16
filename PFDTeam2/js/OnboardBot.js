const { App } = require('@slack/bolt');

const app = new App({
    signingSecret: 'b621c3bf14cecca722bbf13fb0b0960b',
    token: 'xoxb-6162142933683-6176820717153-USohXVECxXlnyV2tXaCyoirL',
});

// Introduction message
app.event('app_home_opened', ({ event, say }) => {
    say({
        text: `Hello! I'm your custom Slack bot. I'm here to help you with information about our company. Type \`help\` if you have questions.`,
        channel: event.user,
        blocks: [
            {
                type: 'section',
                text: {
                    type: 'mrkdwn',
                    text: 'Click the button below:',
                },
                accessory: {
                    type: 'button',
                    text: {
                        type: 'plain_text',
                        text: 'Click Me',
                    },
                    action_id: 'button_click', // Action ID for the button click
                },
            },
        ]
    });
});

// Handle button clicks
app.action('button_click', async ({ ack, say }) => {
    await ack();
    say('Button clicked!');
});

// Respond to user messages
app.message(/.*/, ({ message, say }) => {  //handle any message
    console.log('Received message:', message.text);
    say(`Hello <@${message.user}>! How can I assist you today?`);
});

// Provide FAQs button
app.message('FAQs', async ({ message, say, client }) => {
    await client.views.open({
        trigger_id: message.trigger_id,
        view: {
            type: 'modal',
            callback_id: 'faq_modal',
            title: {
                type: 'plain_text',
                text: 'Frequently Asked Questions',
            },
            blocks: [
                {
                    type: 'section',
                    text: {
                        type: 'mrkdwn',
                        text: 'Here are some frequently asked questions:',
                    },
                },
                {
                    type: 'section',
                    text: {
                        type: 'mrkdwn',
                        text: '- Q1: What is our company all about?',
                    },
                },
                {
                    type: 'section',
                    text: {
                        type: 'mrkdwn',
                        text: '- Q2: How can I contact support?',
                    },
                },
                // Add more FAQs as needed
            ],
            close: {
                type: 'plain_text',
                text: 'Close',
            },
        },
    });
});

// Start the app
(async () => {
    await app.start(process.env.PORT || 3000);
    console.log('Bot is running!');
})();
