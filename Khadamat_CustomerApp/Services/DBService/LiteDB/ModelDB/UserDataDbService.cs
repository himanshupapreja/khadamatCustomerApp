using Khadamat_CustomerApp.Models;
using LiteDB;
using LiteDB.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Khadamat_CustomerApp.Services.DBService.LiteDB.ModelDB
{
    public class UserDataDbService
    {
        private LiteDBService liteDBService;

        public UserDataDbService()
        {
            liteDBService = LiteDBService.Instance;
        }

        public UserModel CreateUserDataInDB(UserModel item)
        {
            return liteDBService.CreateItem(item);
        }

        public UserModel DeleteItemFromDB(BsonValue id, UserModel item)
        {
            return liteDBService.DeleteItem(id, item);
        }

        public bool IsUserDbPresentInDB()
        {
            UserModel model = liteDBService.ReadAllItems<UserModel>().FirstOrDefault(t => t.ID != 0);
            if (model == null)
            {
                return false;
            }
            return true;
        }

        public IEnumerable<UserModel> ReadAllItems()
        {
            return liteDBService.ReadAllItems<UserModel>();
        }

        public UserModel UpdateUserDataInDb(BsonValue bsonid, UserModel item)
        {
            return liteDBService.UpdateItem(bsonid, item);
        }
    }
}
