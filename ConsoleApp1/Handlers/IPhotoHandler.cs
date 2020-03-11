using ConsoleApp1.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace ConsoleApp1.Handlers
{
    class PhotoHandler
    {

        private string PATH_TO_SERVER_STORAGE = @"D:\Фото_Участников";
        private static PhotoRepository _userPhotoRepository = new PhotoRepository();
        public bool isPhoto(MessageEventArgs photo)
        {
            return photo.Message.Type == MessageType.Photo ? true : false;
        }
        public async void SendPhoto(ITelegramBotClient botClient, MessageEventArgs photo, AppUser user)
        {
            Photo newPhoto = new Photo(Guid.NewGuid(), user.UserId);
            var path = Path.Combine(PATH_TO_SERVER_STORAGE, newPhoto.GetPhysicalName);
            using (MemoryStream mem = new MemoryStream())
            {
                var r1 = await botClient.GetInfoAndDownloadFileAsync(photo.Message.Photo[1].FileId, mem);
                _userPhotoRepository.AttachPhoto(mem.ToArray(), newPhoto);
            }
            
        }
        public async void HandlePhoto(ITelegramBotClient botClient, MessageEventArgs args, AppUser user)
        {
            if (isPhoto(args) && user != null)
            {
                var userPhoto = args.Message.Photo[1];
                SendPhoto(botClient, args, user);
                if (user.Gender)
                {
                    await botClient.SendTextMessageAsync(
                       args.Message.Chat,
                       $"Уважаемый {user.Name}, вашe фото загрузилось!");
                }
                else
                {
                    await botClient.SendTextMessageAsync(
                   args.Message.Chat,
                   $"Уважаемая {user.Name}, вашe фото загрузилось!Теперь можете искать собеседника!");
                   
                }

            }
            else if (user == null)
            {
                await botClient.SendTextMessageAsync(
               args.Message.Chat,
               $"Уважаемый пользователь, сперва заполните анкету!");
            }

        }
    }
}
