using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace ConsoleApp1.Handlers
{
    interface ICommandHandler
    {
        void Handle(ITelegramBotClient botClient, MessageEventArgs args);
    }
    class UpdateProfileMessageHandler : ICommandHandler
    {

        private readonly IAppUserRepository _userRepository;
        public UpdateProfileMessageHandler(
            IAppUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async void Handle(ITelegramBotClient botClient, MessageEventArgs args)
        {
            var text = args.Message.Text.ToLower().Replace(" ", "").Split(',');
            var name = text[1];
            var gender = text[2] == "мужской";
            var age = Int32.Parse(text[3]);
            var number = text[4];
            var addUserTemp = new AppUser(args.Message.From.Id, name, gender, age, number);
            _userRepository.AddUser(addUserTemp);

            if (gender)
            {
                await botClient.SendTextMessageAsync(
                    args.Message.Chat,
                    $"Уважаемый {name}, ваша анкета принята в обработку! Ждите приятных сообщении!\nТакже вы можете загрузить свое фото!\nЛюбое фото,которое вы отправите сохраняется!");

            }
            else
            {
                await botClient.SendTextMessageAsync(
                    args.Message.Chat,
                    $"Уважаемая {name}, ваша анкета принята в обработку! Ждите приятных сообщении!\nТакже вы можете загрузить свое фото!\nЛюбое фото,которое вы отправите сохраняется!");

            }
        }

    }
}
