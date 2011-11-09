using WebMatrix.Data;

namespace PhotoGallery.Repositories
{
    public class AccountRepository
    {
        readonly Database _database;
        public AccountRepository()
        {
            _database = Database.Open("PhotoGallery");
        }

        public dynamic GetAccountEmail(string email)
        {
            return _database.QuerySingle("SELECT Email FROM UserProfiles WHERE LOWER(Email) = LOWER(@0)", email);
        }

        public void CreateAccount(string email)
        {
            _database.Execute("INSERT INTO UserProfiles (Email, DisplayName, Bio) VALUES (@0, @1, @2)", email, email, "");
        }
    }
}