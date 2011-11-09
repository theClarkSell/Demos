using System;
using System.Collections.Generic;
using System.Linq;
using WebMatrix.Data;

namespace PhotoGallery.Repositories
{
    public class PhotoGalleryRepository
    {
        readonly Database _database;
        public PhotoGalleryRepository()
        {
            _database = Database.Open("PhotoGallery");
        }

        public IList<dynamic> GetGalleryList()
        {
            return _database.Query(@"SELECT Galleries.Id, Galleries.Name, COUNT(Photos.Id) AS PhotoCount
                               FROM Galleries LEFT OUTER JOIN Photos ON Galleries.Id = Photos.GalleryId
                               GROUP BY Galleries.Id, Galleries.Name").ToList();
        }

        public int GetGalleryCount(string galleryName)
        {
            return _database.QueryValue("SELECT COUNT(*) FROM Galleries WHERE LOWER(Name) = LOWER(@0)", galleryName);
        }

        public int CreateGallery(string galleryName)
        {
            _database.Execute("INSERT INTO Galleries (Name) VALUES (@0)", galleryName);

            return (int)_database.GetLastInsertId();
        }

        public dynamic GetGallery(int galleryId)
        {
            return _database.QuerySingle("SELECT * FROM Galleries WHERE Id = @0", galleryId);
        }

        public IList<dynamic> GetTopPhotosForGallery(int galleryId)
        {
            return _database.Query("SELECT TOP(3) FileContents, UploadDate FROM Photos WHERE GalleryId = @0 ORDER BY UploadDate DESC", galleryId).ToList();
        }

        public IList<dynamic> GetPhotosForGallery(int galleryId)
        {
            return _database.Query("SELECT Id, FileTitle FROM Photos WHERE GalleryId = @0", galleryId).ToList();
        }

        public int InsertImage(int galleryId, int userId, string fileTitle,
            string fileExtension, string imageFormat, int length, byte[] image)
        {
            _database.Execute(@"INSERT INTO Photos
                    (GalleryId, UserId, Description, FileTitle, FileExtension, 
                    ContentType, FileSize, UploadDate, FileContents) VALUES 
                    (@0, @1, @2, @3, @4, @5, @6, @7, @8)",
                    galleryId, userId, "", fileTitle, fileExtension,
                    imageFormat, length, DateTime.Now, image);

            return (int)_database.GetLastInsertId();
        }

        public void RemovePhoto(int photoId)
        {
            _database.Execute(@"DELETE 
                                FROM Photos_Tags 
                                WHERE Photos_Id = @0", photoId);

            _database.Execute(@"DELETE 
                                FROM Comments 
                                WHERE PhotoId = @0", photoId);
            _database.Execute(@"DELETE 
                                FROM Photos 
                                WHERE Id = @0", photoId);
        }

        public dynamic GetPhoto(int photoId)
        {
            return _database.QuerySingle("SELECT * FROM Photos WHERE Id = @0", photoId);
        }

        public void InsertComment(int id, string newComment, int userId)
        {
            _database.Execute(@"INSERT INTO Comments (PhotoId, CommentDate, UserId, CommentText) 
                    VALUES (@0, @1, @2, @3)", id, DateTime.Now, userId, newComment);
        }

        public IList<dynamic> GetCommentsByPhoto(int id)
        {
            return _database.Query(@"SELECT * FROM Comments INNER JOIN UserProfiles
                              ON Comments.UserId = UserProfiles.UserId WHERE PhotoId = @0 
                              ORDER BY Comments.CommentDate", id).ToList();
        }

        public IList<dynamic> GetCommentsByUser()
        {
            return _database.Query(@"SELECT * FROM Comments INNER JOIN UserProfiles
                                ON Comments.UserId = UserProfiles.UserId
                                ORDER BY Comments.CommentDate").ToList();
        }

        public void InsertPhotoTag(int id, string tag)
        {
            _database.Execute("INSERT INTO Photos_Tags (Photos_Id, Tags_TagName) VALUES (@0, @1)", id, tag);
        }
    }
}