using Microsoft.AspNetCore.Mvc;
using RestSharp;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace twilioTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhatsAppController : ControllerBase
    {

        [HttpPost("sendMessageWithTwilio")]
        public  IActionResult SendMessageTwilio()
        {
            var accountSid = "ACb8293ad8f8ea35cbf11eb4c7e61d595a";
            var authToken = "cd0aa46abbf126e68ec2462dc7bf3224";
            TwilioClient.Init(accountSid, authToken);

            var messageOptions = new CreateMessageOptions(
              new PhoneNumber("whatsapp:+201095410698"));

            messageOptions.From = new PhoneNumber("whatsapp:+14155238886");
            messageOptions.ContentSid = "HX229f5a04fd0510ce1b071852155d3e75";

            messageOptions.ContentVariables = "{\"1\":\"Hello Ameer From Twilio <3\"}";

            var message = MessageResource.Create(messageOptions);

            return Ok(message);
        }

        [HttpPost("sendMessageByPortal")]
        public async Task<IActionResult> SendMessagePortal()
        {
            var options = new RestClientOptions("https://pe1zjm.api.infobip.com")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("/whatsapp/1/message/template", Method.Post);
            request.AddHeader("Authorization", "App 9f830a3a4e936ab7786a0fc56570db82-975f6341-a207-4859-bbf0-0f3622f6d06d");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            var body = @"{""messages"":[{""from"":""447860099299"",""to"":""201095410698"",""messageId"":""43492ff3-e409-438a-a317-a35d3610ca10"",""content"":{""templateName"":""test_whatsapp_template_en"",""templateData"":{""body"":{""placeholders"":[""Ameer""]}},""language"":""en""}}]}";
            request.AddStringBody(body, DataFormat.Json);
            RestResponse response = await client.ExecuteAsync(request);
            return Ok(response.Content);
        }

        [HttpPost("sendMessageByUltraMSG")]
        public async Task<IActionResult> SendMessageUltra()
        {
            var url = "https://api.ultramsg.com/instance104692/messages/chat";
            var client = new RestClient(url);

            var request = new RestRequest(url, Method.Post);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("token", "23ckvdelabv2rgyc");
            request.AddParameter("to", "+201275552747");
            request.AddParameter("body", "Thank you for apply with our UMAMi");


            RestResponse response = await client.ExecuteAsync(request);
            var output = response.Content;
            return Ok(output);
        }

        [HttpGet("checkWhatsappNumber")]
        public async Task<IActionResult> CheckNumber()
        {
            var url = "https://api.ultramsg.com/instance104692/contacts/check";
            var client = new RestClient(url);
            var request = new RestRequest(url, Method.Get);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("token", "23ckvdelabv2rgyc");
            request.AddParameter("chatId", "1287776154@c.us");
            request.AddParameter("nocache", "");



            RestResponse response = await client.ExecuteAsync(request);
            var output = response.Content;
            return Ok(output);
        }

        [HttpGet("sendMessageWithTelegram")]
        public async Task sendMessageTelegram()
        {
            using var cts = new CancellationTokenSource();
            var bot = new TelegramBotClient("7758131661:AAE8LhL0wBaaw_iuZy5C61u7DqkPqhCZn1M", cancellationToken: cts.Token);
            var me = await bot.GetMe();
            bot.OnMessage += OnMessage;

            // method that handle messages received by the bot:
            async Task OnMessage(Telegram.Bot.Types.Message msg, UpdateType type)
            {
                if (msg.Text is null) return;   // we only handle Text messages here
                Console.WriteLine($"Received {type} '{msg.Text}' in {msg.Chat}");

                // let's echo back received text in the chat
               var x = await bot.SendMessage(msg.Chat, $"{msg.From} said: {msg.Text}");
            }
        }
    }
}
