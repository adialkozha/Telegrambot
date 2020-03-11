using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ConsoleApp1;
using ConsoleApp1.Models;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace ConsoleApp1.fp
{
    public class FindPair
    {
        private IAppUserRepository _users = new AppUserRepository();
        private static int index = 0;
        public FindPair(IAppUserRepository a)
        {
            _users = a;
        }
        public static async void ShowButton(ITelegramBotClient _bot,MessageEventArgs args)
        {

            var buttonNext = new KeyboardButton("Next");
            var buttonBack = new KeyboardButton("Back");
            var markup = new ReplyKeyboardMarkup(new[]
            {
                    buttonBack,
                    buttonNext
            });
            markup.OneTimeKeyboard = false;
            await _bot.SendTextMessageAsync(args.Message.Chat.Id, " ", replyMarkup: markup);
        }
        public async void NextAction(ITelegramBotClient _bot, MessageEventArgs args)
        {
            Random rnd = new Random();
            var app = _users.GetById(rnd.Next(0, _users.Count()));
            PhotoRepository pr = new PhotoRepository();
            var photos = pr.GetPhotosByUser(app);
            var fs = new MemoryStream();
            fs.Read(pr.GetPhoto(app.photos[0]));
            await _bot.SendPhotoAsync(args.Message.Chat.Id,fs);

            //AppUser temp = _users.GetById(index); 
            //File file = new File(temp.photos[0].GetPhysicalName);
            //await _bot.SendPhotoAsync(args.Message.Chat.Id,);
            
            index++;
        }
    }
}
