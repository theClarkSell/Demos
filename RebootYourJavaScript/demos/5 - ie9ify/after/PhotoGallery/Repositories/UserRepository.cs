using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.Data;

namespace PhotoGallery.Repositories
{
    public class UserRepository
    {
        readonly Database _database;

        public UserRepository()
        {
            _database = Database.Open("PhotoGallery");
        }

        public dynamic GetUserById( int id )
        {
            return _database.QuerySingle(@"SELECT UserId, DisplayName, Bio FROM UserProfiles WHERE UserId = @0", id);
        }

        public dynamic GetPhotos(int id)
        {
            return _database.Query("SELECT Id, FileTitle FROM Photos WHERE UserId = @0 ORDER BY FileTitle", id).ToList();
        }

        public int UpdateUserProfile(string displayName, string bio, int userId)
        {
            return _database.Execute(@"UPDATE UserProfiles SET DisplayName = @0, Bio = @1 WHERE UserId = @2", 
                    displayName, 
                    bio, 
                    userId);
        }
    }
}