using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsoleApp1.Models
{
    public interface IAppUserRepository
    {
        void AddUser(AppUser appUser);
        AppUser GetById(int userId);
        IReadOnlyCollection<AppUser> GetByFilter(int ageMin, int ageMax, bool gender);

        int Count();
    }

    interface IPhotoRepository
    {
        void AttachPhoto(byte[] content, Photo metadata);
        byte [] GetPhoto(Photo photo);
        IReadOnlyCollection<byte[]> GetPhotosByUser(AppUser appUser);
        

    }

    public class AppUserRepository : IAppUserRepository
    {
        private List<AppUser> _appUsers;
        public AppUserRepository()
        {
            _appUsers = new List<AppUser>();
        }
        public void AddUser(AppUser appUser)
        {
            var user = _appUsers
                .FirstOrDefault(p => p.UserId == appUser.UserId);

            if (user != null)
            {
                return;
            }
            
            _appUsers.Add(appUser);
        }

        public int Count()
        {
            return _appUsers != null ? _appUsers.Count : 0;
        }

        public IReadOnlyCollection<AppUser> GetByFilter(int ageMin, int ageMax, bool gender)
        {
            var queryResult = new List<AppUser>();
            foreach (var user in _appUsers)
            {
                if(user.Age >= ageMin && user.Age <= ageMax && user.Gender == gender)
                {
                    queryResult.Add(user);
                }
            }

            return queryResult;
        }

        public AppUser GetById(int userId)
        {
            return _appUsers
                .FirstOrDefault(p => p.UserId == userId);
        }

        
    }


    public class PhotoRepository : IPhotoRepository
    {
        private const string PATH_TO_SERVER_STORAGE = @"D:\Фото_Участников";
        public void AttachPhoto(byte[] content, Photo metadata)
        {
            var pathToPhoto = Path.Combine(PATH_TO_SERVER_STORAGE, metadata.GetPhysicalName);
            File.WriteAllBytes(pathToPhoto, content);
        }
         
        public byte[] GetPhoto(Photo metadata)
        {
            var pathToPhoto = Path.Combine(PATH_TO_SERVER_STORAGE, metadata.GetPhysicalName);
            if (File.Exists(pathToPhoto))
            {
                return File.ReadAllBytes(pathToPhoto);
            }
            return null;
        }

        public IReadOnlyCollection<byte[]> GetPhotosByUser(AppUser appUser)
        {
            var response = new List<byte[]>();
            var allFiles = Directory.GetFiles(PATH_TO_SERVER_STORAGE);
            foreach (var file in allFiles)
            {
                var userId = file.Split('_')[0];
                if (appUser.UserId.ToString() == userId)
                {
                    response.Add(File.ReadAllBytes(file));
                }
            }
            return response;
        }
    }
}
