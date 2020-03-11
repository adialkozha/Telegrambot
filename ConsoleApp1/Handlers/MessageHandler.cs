using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace ConsoleApp1.Handlers
{
    class MessageHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IAppUserRepository _appUserRepository;
        public MessageHandler(ITelegramBotClient botClient, 
            IAppUserRepository appUserRepository)
        {
            _botClient = botClient;
            _appUserRepository = appUserRepository;
        }

        // Анкета,Арман,парень,29,+6666666
        public (bool isParsed, string responseMessage, ICommandHandler handler) TryParseMessage(string text)
        {
            if (text == "/start") { return (false, "Заполните анкету!\nСначало пишите \"анкета\" ,свое имя,пол мужской или женский,сколько вам полных лет,номер телефона.", null); }

            var textToProcess = text.Replace(" ", string.Empty);
            var splitted = textToProcess.Split(',');

            if(splitted.Length != 5)
            {
                return (false, "Я вас не понимаю!", null);
            }

            if(string.Equals(splitted[0].ToLower(), "анкета", StringComparison.InvariantCultureIgnoreCase))
            {
                var handler = new UpdateProfileMessageHandler(_appUserRepository);
                return (true, "Принято на обработку!", handler);
            }

            return (false, "Я вас не понимаю!", null);
        }   
    }
}
