using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace UniMoonDialogue.Enventry
{
    public class EnventryEngine : SingletonMonoBehaviour<EnventryEngine>
    {
        public UnityAction<ItemType, EnventryItemBase> OnMyEnventryUpdated;

        [SerializeField]
        EnventryItemList EntentryItem;

        private List<Item> myItems = new List<Item>();
        private List<Mission> myMissions = new List<Mission>();

        public enum ItemType { Item, Mission }
        public enum ItemStoreResult { Success = 0, Notpermmit, NotFound, AlreadyMax }

        public List<Item> GetAllItemList()
        {
            return EntentryItem.Items;
        }
        public List<Mission> GetAllMissions()
        {
            return EntentryItem.Missions;
        }
        public List<Item> GetMyItemList()
        {
            return myItems;
        }
        public List<Mission> GetMyMission()
        {
            return myMissions;
        }

        public bool GetItem(string itemName,  out ItemStoreResult result)
        {
            var item = EntentryItem.Items.First(x => x.name == itemName);
            if (item != null)
            {
                var ownedItem = myItems.FirstOrDefault(_ => _.name == item.name );

                //持ってなかったら新規で取得
                if (ownedItem == null)
                {
                    item.currentStore = 1;
                    myItems.Add(item);
                    result = ItemStoreResult.Success;
                    OnMyEnventryUpdated?.Invoke(ItemType.Item, item);
                    return true;
                }
                //持ってたら追加
                else if (ownedItem.currentStore < ownedItem.maxStore)
                {
                    ownedItem.currentStore++;
                    result = ItemStoreResult.Success;
                    OnMyEnventryUpdated?.Invoke(ItemType.Item, ownedItem);

                    return true;
                }
                //もう持てなかったらエラー
                else
                {
                    result = ItemStoreResult.AlreadyMax;
                    return false;
                }
            }

            result = ItemStoreResult.NotFound;
            return false;
        }

        public bool UseItem(string itemName,  out ItemStoreResult result)
        {
            var ownedItem = myItems.FirstOrDefault(_ => _.name == itemName);
            //持ってない場合
            if (ownedItem == null)
            {
                result = ItemStoreResult.NotFound;
                return false;
            }
            else
            {
                ownedItem.currentStore--;
                if (ownedItem.currentStore < 1)
                    myItems.Remove(ownedItem);
                result = ItemStoreResult.Success;

                OnMyEnventryUpdated?.Invoke(ItemType.Item, ownedItem);
                return true;
            }
        }
    }
}