using ConsoleApp1.Handlers;
using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using ConsoleApp1.fp;

namespace ConsoleApp1
{
    class Program
    {
        private static IAppUserRepository _applicationUsers = new AppUserRepository();
        private static IPhotoRepository _userPhotos = new PhotoRepository();

        private const string BOT_ACCESS_TOKEN = "907333569:AAHiHA0PzXO3_2--ePQUCz9zFMMWyN5HGY8";
        private static ITelegramBotClient _botClient;

        static async void Handle(object sender, MessageEventArgs args)
        {
            FindPair a = new FindPair(_applicationUsers);

            if(args.Message.Text == "/find")
            {
                FindPair.ShowButton(_botClient,args);
                
            }
            if(args.Message.Text == "Next")
            {
                a.NextAction(_botClient,args);
            }
            if (args.Message.Type == MessageType.Photo)
            {
                var test = new PhotoHandler();
                test.HandlePhoto(_botClient, args, _applicationUsers.GetById(args.Message.From.Id));
            }
            var text = args.Message.Text;
            if (args.Message.Type == MessageType.Text && !string.IsNullOrEmpty(text))
            {
                var messageHandler = new MessageHandler(_botClient, _applicationUsers);
                var parseMessage = messageHandler.TryParseMessage(text);
                if (parseMessage.isParsed)
                {
                    var handler = parseMessage.handler;
                    handler.Handle(_botClient, args);
                }
                
                else
                {
                    await _botClient.SendTextMessageAsync(args.Message.Chat, parseMessage.responseMessage);
                }
            }

            else if(_applicationUsers.GetById(args.Message.From.Id) == null)
            {
                await _botClient.SendTextMessageAsync(args.Message.Chat,"Заполните анкету!");
            }

        }

        static void Main(string[] args)
        {
            _botClient = new TelegramBotClient(BOT_ACCESS_TOKEN);
            var aboutMe = _botClient.GetMeAsync().Result;

            Console.WriteLine($"{aboutMe.Id}:{aboutMe.FirstName}-{aboutMe.LastName}");

            _botClient.OnMessage += Handle;
            _botClient.StartReceiving();

            Console.ReadLine();
        }
    }
}
