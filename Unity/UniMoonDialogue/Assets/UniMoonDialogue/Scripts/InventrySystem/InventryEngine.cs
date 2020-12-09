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
        private List<Inv_Item> allItems;
        [SerializeField]
        private List<Inv_Mission> AllMissions;

        private List<Inv_Item> myItems = new List<Inv_Item>();
        private List<Inv_Mission> myMissions = new List<Inv_Mission>();

        public enum ItemType { Item, Mission }
        public enum ItemStoreResult { Success = 0, NotPermmit, NotFound, AlreadyMax }

        public List<Inv_Item> GetAllItemList()
        {
            return allItems;
        }
        public List<Inv_Mission> GetAllMissions()
        {
            return AllMissions;
        }
        public List<Inv_Item> GetMyItemList()
        {
            return myItems;
        }
        public List<Inv_Mission> GetMyMission()
        {
            return myMissions;
        }

        public bool AddItem(string itemName, out ItemStoreResult result, int ammount = 1)
        {
            var item = allItems.First(x => x.name == itemName);
            if (item != null)
            {
                var ownedItem = myItems.FirstOrDefault(_ => _.name == item.name);

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

        public bool TakeItem(string itemName, out ItemStoreResult result,int ammount = 1)
        {
            var ownedItem = myItems.FirstOrDefault(_ => _.name == itemName);
            //持ってない場合
            if (ownedItem == null)
            {
                result = ItemStoreResult.NotFound;
                return false;
            }
            //足りてない
            else if(ownedItem.currentStore - ammount < 0)
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