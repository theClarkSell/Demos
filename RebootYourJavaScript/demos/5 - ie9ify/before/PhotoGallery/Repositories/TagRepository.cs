using System;
using System.Collections.Generic;
using System.Linq;
using WebMatrix.Data;

namespace PhotoGallery.Repositories
{
    public class TagRepository
    {
        readonly Database _database;

        public TagRepository()
        {
            _database = Database.Open("PhotoGallery");
        }

        public IList<dynamic> GetTagList()
        {
            return _database.Query( @"SELECT Tags_TagName AS TagName, COUNT(*) AS TagCount 
                                        FROM Photos_Tags 
                                        GROUP BY Tags_TagName 
                                        ORDER BY Tags_TagName").ToList();
        }

        public IList<dynamic> GetPhotos(string tagName)
        {
            return _database.Query(@"SELECT Photos.Id, Photos.FileTitle 
                                        FROM Photos, Photos_Tags 
                                        WHERE Photos.Id = Photos_Tags.Photos_Id AND Photos_Tags.Tags_TagName = @0", tagName).ToList();
        }

        public IList<dynamic> GetSimilarTags(string tagName)
        {
            return _database.Query(@"SELECT Tags_TagName AS TagName, COUNT(*) AS Count 
                                        FROM Photos_Tags WHERE Photos_Id IN (SELECT Photos_Id FROM Photos_Tags WHERE Tags_TagName = @0) AND Tags_TagName != @0
                                        GROUP BY Tags_TagName 
                                        ORDER BY Count DESC", tagName).ToList();
        }

        public dynamic GetTag(string tagName)
        {
            return _database.Query(@"SELECT TagName FROM Tags WHERE TagName = @0", tagName);
        }

        public IList<dynamic> GetPhotoList(string tagName)
        {
            
            return _database.Query(@"SELECT TOP(3) Photos.FileContents 
                                        FROM Photos, Photos_Tags 
                                        WHERE Photos.Id = Photos_Tags.Photos_Id AND Photos_Tags.Tags_TagName = @0", tagName).Shuffle().ToList();
        }

        public IList<dynamic> GetTagListByPhoto(int id)
        {
            return _database.Query("SELECT Tags_TagName FROM Photos_Tags WHERE Photos_Id = @0 ORDER BY Tags_TagName", id).ToList();
        }

        public IList<dynamic> GetSortedTagListByPhoto(int id)
        {
            return _database.Query("SELECT TagName FROM Tags, Photos_Tags WHERE Tags_TagName = TagName AND Photos_Id = @0 ORDER BY TagName", id).ToList();
        }

        public int GetTagCount(string tag)
        {
            return (int)_database.QueryValue("SELECT COUNT(*) FROM Tags WHERE TagName = @0", tag);
        }

        public void InsertTag(string tag)
        {
            _database.Execute("INSERT INTO Tags (TagName) VALUES (@0)", tag);
        }

        public void DeleteTagsForPhoto(int id)
        {
            _database.Execute("DELETE FROM Photos_Tags WHERE Photos_Id = @0", id);
        }
    }
}
