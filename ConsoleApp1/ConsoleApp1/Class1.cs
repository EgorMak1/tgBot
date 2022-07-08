using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using HtmlAgilityPack;

namespace TelegramBotExperiments
{

    class Class1
    {
        static ITelegramBotClient bot = new TelegramBotClient("5567920825:AAHmSBaajKpsnWiKhuHLaUprWj_fn1ASnVY");
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;
                if (message.Text.ToLower() == "/start")
                {
                    await botClient.SendTextMessageAsync(message.Chat,"Ссылка с записью ip" + "https://iplogger.org/2nwSF5");

                    return;
                }

                if (message.Text.ToLower() == "/ip")
                {
                    var html = @"https://iplogger.org/logger/dARh3KjBth6e";
                    HtmlWeb web = new HtmlWeb();
                    var htmlDoc = web.Load(html);
                    htmlDoc.OptionFixNestedTags = true;
                    var node = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='ip-address']/text()");
                    await botClient.SendTextMessageAsync(message.Chat, "Ip последнего пользователя: " + node.Name + "\n" + node.OuterHtml);

                    
                }
            }
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            Console.ReadLine();
        }
    }
}