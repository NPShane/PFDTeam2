const { App } = require('@slack/bolt');

const app = new App({
    signingSecret: 'b621c3bf14cecca722bbf13fb0b0960b',
    token: 'xoxb-6162142933683-6176820717153-USohXVECxXlnyV2tXaCyoirL',
});

// Introduction message
/*app.event('app_home_opened', ({ event, say }) => {
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
});*/

/*app.event('app_home_opened', ({ event, say }) => {
    say({
        text: `Hello! I'm your custom Slack bot. I'm here to help you with information about our company. Type \`help\` if you have questions.`,
        channel: event.user,
    });
});*/


// Listen for message events
/*app.message(async ({ message, say }) => {
    // Check if the message is in the #self-service channel
    if (message.channel === 'C064Q5LNSNR') { // Channel ID for #self-service
        // Check the content of the message
        if (message.text.toLowerCase() === 'help') {
            await say({
                text: 'This is the #self-service channel! How can I assist you?',
                blocks: [
                    {
                        type: 'section',
                        text: {
                            type: 'mrkdwn',
                            text: 'Instructions:\n- Type *faqs* to view FAQs.',
                        },
                    },
                ],
            });
        } else if (message.text.toLowerCase() === 'faqs') {
            // Provide FAQs button
            await app.client.chat.postMessage({
                channel: message.channel,
                text: 'Here are some frequently asked questions:',
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
            });
        }
        // Add more conditions for other commands or features
    }
});*/

// FAQs data
const faqs = [
    {
        question: 'Who we are?',
        answer: 'Our company is dedicated to...',
    },
    {
        question: 'How can I contact support?',
        answer: 'You can reach our support team at support@example.com or by using the #support channel.',
    },
    // Add more FAQs as needed
];

// Helpdesk data
const helpdeskChannels = {
    technical: 'C065Z6VLUHY', // Replace with the actual channel ID for technical support
    hr: 'C0656RMKUJV',       // Replace with the actual channel ID for HR support
};

// Listen for message events
app.message(async ({ message, say }) => {
    // Check if the message is in the #self-service channel
    if (message.channel === 'C064Q5LNSNR') { // Channel ID for #self-service
        // Check the content of the message
        if (message.text.toLowerCase() === 'help') {
            await say({
                text: 'This is the #self-service channel! How can I assist you?',
                blocks: [
                    {
                        type: 'section',
                        text: {
                            type: 'mrkdwn',
                            text: 'Instructions:\n- Type *faqs* to view FAQs.\n- Type *helpdesk* for helpdesk support.',
                        },
                    },
                ],
            });
        } else if (message.text.toLowerCase() === 'faqs') {
            // Provide FAQs
            /*const faqList = faqs.map((faq, index) => `*Q${index + 1}:* ${faq.question}`).join('\n');
            await say(`Here are some frequently asked questions:\n${faqList}`);*/

            // Provide FAQs with clickable questions
            const buttons = faqs.map((faq, index) => ({
                type: 'button',
                text: {
                    type: 'plain_text',
                    text: `${index + 1}: ${faq.question}`,
                },
                action_id: `faq_button_${index}`,
            }));

            await say({
                text: 'Here are some frequently asked questions:',
                blocks: [
                    {
                        type: 'section',
                        text: {
                            type: 'mrkdwn',
                            text: 'Click on a question to view the answer.',
                        },
                    },
                    {
                        type: 'actions',
                        block_id: 'faq_buttons',
                        elements: buttons,
                    },
                ],
            });
        } else if (message.text.toLowerCase() === 'helpdesk') {
            // Provide Helpdesk options
            await say({
                text: 'Please select the type of help you need:',
                blocks: [
                    {
                        type: 'section',
                        block_id: 'helpdesk_options',
                        text: {
                            type: 'mrkdwn',
                            text: '*Technical Support* :gear:\n*HR Support* :bust_in_silhouette:',
                        },
                        accessory: {
                            type: 'static_select',
                            action_id: 'helpdesk_selection',
                            placeholder: {
                                type: 'plain_text',
                                text: 'Select an option',
                            },
                            options: [
                                {
                                    text: {
                                        type: 'plain_text',
                                        text: 'Technical Support',
                                    },
                                    value: 'technical',
                                },
                                {
                                    text: {
                                        type: 'plain_text',
                                        text: 'HR Support',
                                    },
                                    value: 'hr',
                                },
                            ],
                        },
                    },
                ],
            });
        }
        // Add more conditions for other commands or features
    }
});

// Listen for message events
/*app.message(async ({ message, say }) => {
    // Check if the message object and message.text are defined
    if (message && message.text) {
        // Check if the message is in the #self-service channel
        if (message.channel === 'C064Q5LNSNR') { // Channel ID for #self-service
            // Check the content of the message
            const lowercasedText = message.text.toLowerCase();
            if (lowercasedText === 'help') {
                await say({
                    text: 'This is the #self-service channel! How can I assist you?',
                    blocks: [
                        {
                            type: 'section',
                            text: {
                                type: 'mrkdwn',
                                text: 'Instructions:\n- Type *faqs* to view FAQs.\n- Type *helpdesk* for helpdesk support.',
                            },
                        },
                    ],
                });
            } else if (lowercasedText === 'faqs') {
                // Provide FAQs
                const faqList = faqs.map((faq, index) => `*Q${index + 1}:* ${faq.question}`).join('\n');
                await say(`Here are some frequently asked questions:\n${faqList}`);
            } else if (lowercasedText === 'helpdesk') {
                // Provide Helpdesk options
                await say({
                    text: 'Please select the type of help you need:',
                    blocks: [
                        {
                            type: 'section',
                            block_id: 'helpdesk_options',
                            text: {
                                type: 'mrkdwn',
                                text: '*Technical Support* :gear:\n*HR Support* :bust_in_silhouette:',
                            },
                            accessory: {
                                type: 'static_select',
                                action_id: 'helpdesk_selection',
                                placeholder: {
                                    type: 'plain_text',
                                    text: 'Select an option',
                                },
                                options: [
                                    {
                                        text: {
                                            type: 'plain_text',
                                            text: 'Technical Support',
                                        },
                                        value: 'technical',
                                    },
                                    {
                                        text: {
                                            type: 'plain_text',
                                            text: 'HR Support',
                                        },
                                        value: 'hr',
                                    },
                                ],
                            },
                        },
                    ],
                });
            }
            // Add more conditions for other commands or features
        }
    }
});*/

// Listen for FAQ button clicks
faqs.forEach((faq, index) => {
    app.action(`faq_button_${index}`, async ({ ack, body, say }) => {
        await ack();

        // Display the answer when the button is clicked
        await say(`*Q: ${faq.question}*\n${faq.answer}`);
    });
});

// Listen for helpdesk selection
app.action('helpdesk_selection', async ({ ack, body, say }) => {
    await ack();

    const selectedOption = body.actions[0].selected_option.value;

    if (selectedOption in helpdeskChannels) {
        const helpdeskChannel = helpdeskChannels[selectedOption];
        await say(`Please describe your issue in the <#${helpdeskChannel}> channel. Our team will assist you.`);
    }
});

// Start the app
(async () => {
    await app.start(process.env.PORT || 3000);
    console.log('Bolt app is running!');
})();
