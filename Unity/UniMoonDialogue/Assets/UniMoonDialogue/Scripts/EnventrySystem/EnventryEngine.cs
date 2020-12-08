using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace UniMoonDialogue.Enventry
{
    public class EnventryEngine : SingletonMonoBehaviour<EnventryEngine>
    {
        public UnityAction<ItemType, EnventryItemBase> OnMyEnventryUpdated;

        public EnventryEngine()
        {
            BuildDataBase();
        }

        private List<EnventryItemBase> items = new List<EnventryItemBase>();
        private List<EnventryItemBase> myItems = new List<EnventryItemBase>();

        void BuildDataBase()
        {
            items = new List<EnventryItemBase>
            {
                new Item( name:"Sord1",maxStore:1),
                new Item( name:"Sord2",maxStore:2),
                new Item( name:"Sord3",maxStore:3)
            };
        }

        public enum ItemType { Item, Mission }
        public enum ItemStoreResult { Success = 0, Notpermmit, NotFound, AlreadyMax }

        public List<EnventryItemBase> GetAllItemList(ItemType type)
        {
            return items.Where(item => item.type == type).ToList();
        }

        public List<EnventryItemBase> GetMyItemList(ItemType type)
        {
            return myItems.Where(item => item.type == type).ToList();
        }

        public bool GetItem(string itemName, ItemType type, out ItemStoreResult result)
        {
            var item = items.First(x => x.name == itemName && x.type == type);
            if (item != null)
            {
                var ownedItem = myItems.FirstOrDefault(_ => _.name == item.name && _.type == type);

                //持ってなかったら新規で取得
                if (ownedItem == null)
                {
                    item.currentStore = 1;
                    myItems.Add(item);
                    result = ItemStoreResult.Success;
                    OnMyEnventryUpdated?.Invoke(type, item);
                    return true;
                }
                //持ってたら追加
                else if (ownedItem.currentStore < ownedItem.maxStore)
                {
                    ownedItem.currentStore++;
                    result = ItemStoreResult.Success;
                    OnMyEnventryUpdated?.Invoke(type, ownedItem);

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

        public bool UseItem(string itemName, ItemType type, out ItemStoreResult result)
        {
            var ownedItem = myItems.FirstOrDefault(_ => _.name == itemName && _.type == type);
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

                OnMyEnventryUpdated?.Invoke(type, ownedItem);
                return true;
            }
        }
    }
}