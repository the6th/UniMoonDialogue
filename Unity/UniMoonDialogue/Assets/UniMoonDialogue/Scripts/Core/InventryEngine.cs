using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace UniMoonDialogue.Inventry
{
    public class InventryEngine : SingletonMonoBehaviour<InventryEngine>
    {
        public UnityAction<ItemType, InventryItemBase> OnMyEnventryUpdated;

        [SerializeField]
        private List<Item> allItems = null;
        [SerializeField]
        private List<Mission> AllMissions = null;

        [SerializeField]
        private List<Item> myItems = new List<Item>();
        [SerializeField]
        private List<Mission> myMissions = new List<Mission>();

        public enum ItemType { Item, Mission }
        public enum ItemStoreResult { Success = 0, NotPermmit, NotFound, AlreadyMax }

        public List<Item> GetAllItemList()
        {
            return allItems;
        }
        public List<Mission> GetAllMissions()
        {
            return AllMissions;
        }
        public List<Item> GetMyItemList()
        {
            return myItems;
        }
        public List<Mission> GetMyMission()
        {
            return myMissions;
        }

        public int CheckItem(Item item)
        {
            var _item = allItems.First(x => x == item);
            return _item.currentStore;
        }

        public bool CheckMission(Mission item)
        {
            var _item = AllMissions.First(x => x == item);
            return (_item != null);
        }

        public bool AddMission(Mission _item, out ItemStoreResult result)
        {
            var item = AllMissions.First(x => x == _item);
            if (item != null)
            {
                var ownedItem = myMissions.FirstOrDefault(_ => _ == item);

                //持ってなかったら新規で取得
                if (ownedItem == null)
                {
                    myMissions.Add(item);
                    result = ItemStoreResult.Success;
                    OnMyEnventryUpdated?.Invoke(ItemType.Mission, item);
                    return true;
                }
                else
                {
                    result = ItemStoreResult.AlreadyMax;
                    return true;
                }
            }

            result = ItemStoreResult.NotFound;
            return false;
        }

        public bool TakeMission(Mission _item, out ItemStoreResult result)
        {
            var ownedItem = myMissions.FirstOrDefault(_ => _ == _item);
            //持ってない場合
            if (ownedItem == null)
            {
                result = ItemStoreResult.NotFound;
                return false;
            }
            else
            {
                myMissions.Remove(ownedItem);
                result = ItemStoreResult.Success;
                OnMyEnventryUpdated?.Invoke(ItemType.Mission, ownedItem);
                return true;
            }
        }

        public bool AddItem(Item _item, out ItemStoreResult result, int ammount = 1)
        {
            var item = allItems.First(x => x == _item);
            if (item != null)
            {
                var ownedItem = myItems.FirstOrDefault(_ => _ == item);

                //持ってなかったら新規で取得
                if (ownedItem == null)
                {
                    item.currentStore = Mathf.Min(ammount, item.maxStore);
                    myItems.Add(item);
                    result = ItemStoreResult.Success;
                    OnMyEnventryUpdated?.Invoke(ItemType.Item, item);
                    return true;
                }
                //持ってたら追加
                else if (ownedItem.currentStore + ammount < ownedItem.maxStore)
                {
                    ownedItem.currentStore = Mathf.Min(ownedItem.currentStore + ammount, item.maxStore);
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

        public bool TakeItem(Item _item, out ItemStoreResult result, int ammount = 1)
        {
            var ownedItem = myItems.FirstOrDefault(_ => _ == _item);
            //持ってない場合
            if (ownedItem == null)
            {
                result = ItemStoreResult.NotFound;
                return false;
            }
            //足りてない
            else if (ownedItem.currentStore - ammount < 0)
            {
                result = ItemStoreResult.NotPermmit;
                return false;
            }
            else
            {
                ownedItem.currentStore -= ammount;
                if (ownedItem.currentStore < 1)
                    myItems.Remove(ownedItem);
                result = ItemStoreResult.Success;

                OnMyEnventryUpdated?.Invoke(ItemType.Item, ownedItem);
                return true;
            }
        }
    }
}