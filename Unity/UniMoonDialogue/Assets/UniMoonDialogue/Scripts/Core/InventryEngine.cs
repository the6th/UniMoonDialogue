using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace UniMoonDialogue
{
    public class InventryEngine : SingletonMonoBehaviour<InventryEngine>
    {
        public class UserItem 
        {
            public int currentStore = 0;
        }

        public UnityAction<ItemType, InventryItemBase> OnMyEnventryUpdated;

        [SerializeField]
        private List<Item> allItems = null;
        [SerializeField]
        private List<Mission> AllMissions = null;

        [SerializeField]
        private Dictionary<Item, UserItem> myItems = new Dictionary<Item, UserItem>();

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
        public Dictionary<Item, UserItem> GetMyItemList()
        {
            return myItems;
        }
        public List<Mission> GetMyMission()
        {
            return myMissions;
        }

        public int CheckItem(Item item)
        {
            if (!myItems.Keys.Contains(item)) return -10;

            var _item = myItems.Keys.First(x => x == item);
            return myItems[_item].currentStore;
        }

        public bool CheckHaveAll(List<Item> items)
        {
            foreach (var item in items)
            {
                if (!myItems.Keys.Contains(item))
                    return false;

            }
            return true;
        }

        public int ClearAllItems(string tag = "")
        {
            int cnt = 0;

            foreach(var item in myItems.Reverse())
            {
                if (tag == "" || item.Key.tag == tag)
                {
                    myItems.Remove(item.Key);
                    cnt++;
                }
            }
            return cnt;
        }

        public bool CheckMission(Mission item)
        {
            return myMissions.Contains(item);
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
                Debug.Log("Mission 削除:" + ownedItem.nameForDisplay);
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
                var ownedItem = myItems.Keys.FirstOrDefault(_ => _ == item);

                //持ってなかったら新規で取得
                if (ownedItem == null)
                {
                    myItems.Add(
                        item,
                        new UserItem() { currentStore = Mathf.Min(ammount, item.maxStore) }
                    );

                    result = ItemStoreResult.Success;
                    OnMyEnventryUpdated?.Invoke(ItemType.Item, item);
                    return true;
                }
                //持ってたら追加
                else if (myItems[ownedItem].currentStore + ammount < ownedItem.maxStore)
                {
                    myItems[ownedItem].currentStore = Mathf.Min(myItems[ownedItem].currentStore + ammount, item.maxStore);
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
            var ownedItem = myItems.Keys.FirstOrDefault(_ => _ == _item);
            //持ってない場合
            if (ownedItem == null)
            {
                result = ItemStoreResult.NotFound;
                return false;
            }
            //足りてない
            else if (myItems[ownedItem].currentStore - ammount < 0)
            {
                result = ItemStoreResult.NotPermmit;
                return false;
            }
            else
            {
                myItems[ownedItem].currentStore -= ammount;
                if (myItems[ownedItem].currentStore < 1)
                    myItems.Remove(ownedItem);
                result = ItemStoreResult.Success;

                OnMyEnventryUpdated?.Invoke(ItemType.Item, ownedItem);
                return true;
            }
        }
    }
}