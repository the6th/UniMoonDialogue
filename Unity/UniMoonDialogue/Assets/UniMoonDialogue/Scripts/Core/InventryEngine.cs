using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace UniMoonDialogue
{
    public class InventryEngine : SingletonMonoBehaviour<InventryEngine>
    {
        public UnityAction<ItemType, UserItem> OnMyEnventryUpdated;
        public UnityAction<ItemType, UserMisson> OnMyMissionUpdated;

        [SerializeField]
        private List<Item> allItems = null;
        [SerializeField]
        private List<Mission> AllMissions = null;

        [SerializeField]
        private List<UserItem> myItems = new List<UserItem>();

        [SerializeField]
        private List<UserMisson> myMissions = new List<UserMisson>();

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
        public List<UserItem> GetMyItemList()
        {
            return myItems;
        }
        public List<UserMisson> GetMyMission()
        {
            return myMissions;
        }

        public int CheckItem(Item item)
        {
            if (!myItems.Select(_ => _).Any(_ => _.status == item)) return 0;

            var _item = myItems.First(_ => _.status == item);
            return _item.currentStore;
        }

        public bool CheckHaveAll(List<Item> items)
        {
            foreach (var item in items)
            {
                if (!myItems.Select(_ => _).Any(_ => _.status == item))
                    return false;
            }
            return true;
        }

        public int ClearAllItems(string tag = "")
        {
            int cnt = 0;

            for (int i = myItems.Count - 1; i >= 0; i--)
            {
                if (tag == "" || myItems[i].status.Tag == tag)
                {
                    myItems.RemoveAt(i);
                    cnt++;
                }
            }
            return cnt;
        }

        public bool CheckMission(Mission item)
        {
            return (myMissions.Select(_ => _).Any(_ => _.status == item));
        }

        public bool AddMission(Mission _item, out ItemStoreResult result)
        {
            var item = AllMissions.First(x => x == _item);
            if (item != null)
            {
                var ownedItem = myMissions.FirstOrDefault(_ => _.status == item);

                //持ってなかったら新規で取得
                if (ownedItem == null)
                {
                    var uMission = new UserMisson { status = item };
                    myMissions.Add(uMission);
                    Debug.Log("Mission 追加:" + item._name);

                    result = ItemStoreResult.Success;
                    OnMyMissionUpdated?.Invoke(ItemType.Mission, uMission);
                    return true;
                }
                else
                {
                    Debug.Log("Mission は既に追加されています。:" + item._name);

                    result = ItemStoreResult.AlreadyMax;
                    return true;
                }
            }

            result = ItemStoreResult.NotFound;
            return false;
        }

        public bool TakeMission(Mission _item, out ItemStoreResult result)
        {
            var ownedItem = myMissions.FirstOrDefault(_ => _.status == _item);
            //持ってない場合
            if (ownedItem == null)
            {
                result = ItemStoreResult.NotFound;
                return false;
            }
            else
            {
                Debug.Log("Mission 削除:" + ownedItem.status._name);
                myMissions.Remove(ownedItem);
                result = ItemStoreResult.Success;
                OnMyMissionUpdated?.Invoke(ItemType.Mission, ownedItem);
                return true;
            }
        }

        public bool AddItem(Item _item, out ItemStoreResult result, int ammount = 1)
        {
            var item = allItems.First(x => x == _item);
            if (item != null)
            {
                var ownedItem = myItems.FirstOrDefault(_ => _.status == item);

                //持ってなかったら新規で取得
                if (ownedItem == null)
                {
                    var userItem = new UserItem() { status = item, currentStore = Mathf.Min(ammount, item.maxStore) };
                    myItems.Add(userItem);

                    result = ItemStoreResult.Success;
                    OnMyEnventryUpdated?.Invoke(ItemType.Item, userItem);
                    return true;
                }
                //持ってたら追加
                else if (ownedItem.currentStore + ammount < ownedItem.status.maxStore)
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
            var ownedItem = myItems.FirstOrDefault(_ => _.status == _item);
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