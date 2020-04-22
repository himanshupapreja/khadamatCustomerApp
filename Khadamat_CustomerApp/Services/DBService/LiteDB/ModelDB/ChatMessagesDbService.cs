using Khadamat_CustomerApp.Models;
using LiteDB;
using LiteDB.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Khadamat_CustomerApp.Services.DBService.LiteDB.ModelDB
{
    public class GroupChatMessagesDbService
    {
        private LiteDBService liteDBService;

        public GroupChatMessagesDbService()
        {
            liteDBService = LiteDBService.Instance;
        }

        public GroupChatDBModel CreateGroupChatDataInDB(GroupChatDBModel item)
        {
            return liteDBService.CreateItem(item);
        }

        public GroupChatDBModel DeleteGroupChatItemFromDB(BsonValue id, GroupChatDBModel item)
        {
            return liteDBService.DeleteItem(id, item);
        }

        public bool IsGroupChatPresentInDB()
        {
            GroupChatDBModel model = liteDBService.ReadAllItems<GroupChatDBModel>().FirstOrDefault(t => t.ID != 0);
            if (model == null)
            {
                return false;
            }
            return true;
        }

        public IEnumerable<GroupChatDBModel> ReadAllItems()
        {
            return liteDBService.ReadAllItems<GroupChatDBModel>();
        }

        public GroupChatDBModel UpdateGroupChatDataInDb(BsonValue bsonid, GroupChatDBModel item)
        {
            return liteDBService.UpdateItem(bsonid, item);
        }
    }

    public class SingleChatMessagesDbService
    {
        private LiteDBService liteDBService;

        public SingleChatMessagesDbService()
        {
            liteDBService = LiteDBService.Instance;
        }

        public SingleChatDBModel CreateSingleChatDataInDB(SingleChatDBModel item)
        {
            return liteDBService.CreateItem(item);
        }

        public SingleChatDBModel DeleteSingleChatItemFromDB(BsonValue id, SingleChatDBModel item)
        {
            return liteDBService.DeleteItem(id, item);
        }

        public bool IsSingleChatPresentInDB()
        {
            SingleChatDBModel model = liteDBService.ReadAllItems<SingleChatDBModel>().FirstOrDefault(t => t.ID != 0);
            if (model == null)
            {
                return false;
            }
            return true;
        }

        public IEnumerable<SingleChatDBModel> ReadAllItems()
        {
            return liteDBService.ReadAllItems<SingleChatDBModel>();
        }

        public SingleChatDBModel UpdateSingleChatDataInDb(BsonValue bsonid, SingleChatDBModel item)
        {
            return liteDBService.UpdateItem(bsonid, item);
        }
    }
}