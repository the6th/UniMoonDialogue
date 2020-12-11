using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace UniMoonDialogue
{
    public class InventryEngine : SingletonMonoBehaviour<InventryEngine>
    {
        //UnityAction
        public UnityAction<UserItem> OnMyItemUpdated;
        public UnityAction<UserMisson> OnMyMissionUpdated;

        public List<Item> AllItems
        {
            set { m_AllItems = value; }
            get { return m_AllItems; }
        }

        public List<Mission> AllMissions
        {
            set { m_AllMissions = value; }
            get { return m_AllMissions; }
        }

        public List<UserItem> MyItems { private set; get; } = new List<UserItem>();
        public List<UserMisson> MyMissions { private set; get; } = new List<UserMisson>();

        public enum InventyStatus { Success = 0, NotEnough, NotFound, AlreadyMax }


        [SerializeField]
        private List<Item> m_AllItems = new List<Item>();

        [SerializeField]
        private List<Mission> m_AllMissions = new List<Mission>();


        public int GetAmmountMyItem(Item item)
        {
            if (!MyItems.Select(_ => _).Any(_ => _.status == item)) return 0;

            var _item = MyItems.First(_ => _.status == item);
            return _item.currentStore;
        }

        public bool IsCompliteItems(List<Item> items)
        {
            foreach (var item in items)
            {
                if (!MyItems.Select(_ => _).Any(_ => _.status == item))
                    return false;
            }

            return true;
        }

        public int ClearAllMyItems(string tag = "")
        {
            int cnt = 0;

            for (int i = MyItems.Count - 1; i >= 0; i--)
            {
                if (tag == "" || MyItems[i].status.Tag == tag)
                {
                    MyItems.RemoveAt(i);
                    cnt++;
                }
            }

            return cnt;
        }

        public bool IsMyMission(Mission item)
        {
            return (MyMissions.Select(_ => _).Any(_ => _.status == item));
        }

        public bool PushMyMission(Mission _item, out InventyStatus result)
        {
            if (!AllMissions.Contains(_item))
            {
                result = InventyStatus.NotFound;
                return false;
            }

            var myMission = MyMissions.FirstOrDefault(_ => _.status == _item);

            //持ってなかったら新規で取得
            if (myMission == null)
            {
                var uMission = new UserMisson { status = _item };
                MyMissions.Add(uMission);
                Debug.Log("Mission 追加:" + _item._name);

                result = InventyStatus.Success;
                OnMyMissionUpdated?.Invoke(uMission);
                return true;
            }
            else
            {
                Debug.Log("Mission は既に追加されています。:" + _item._name);

                result = InventyStatus.AlreadyMax;
                return true;
            }
        }

        public bool PopMyMission(Mission _item, out InventyStatus result)
        {
            var myMission = MyMissions.FirstOrDefault(_ => _.status == _item);
            //持ってない場合
            if (myMission == null)
            {
                result = InventyStatus.NotFound;
                return false;
            }
            else
            {
                Debug.Log("Mission 削除:" + myMission.status._name);
                MyMissions.Remove(myMission);
                result = InventyStatus.Success;
                OnMyMissionUpdated?.Invoke(myMission);
                return true;
            }
        }

        public bool PushMyItem(Item item, out InventyStatus result, int ammount = 1)
        {
            if (!AllItems.Contains(item))
            {
                result = InventyStatus.NotFound;
                return false;
            }

            var myItem = MyItems.FirstOrDefault(_ => _.status == item);

            //持ってなかったら新規で取得
            if (myItem == null)
            {
                var userItem = new UserItem() { status = item, currentStore = Mathf.Min(ammount, item.maxStore) };
                MyItems.Add(userItem);

                result = InventyStatus.Success;
                OnMyItemUpdated?.Invoke(userItem);
                return true;
            }
            //持ってたら追加
            else if (myItem.currentStore + ammount < myItem.status.maxStore)
            {
                myItem.currentStore = Mathf.Min(myItem.currentStore + ammount, item.maxStore);
                result = InventyStatus.Success;
                OnMyItemUpdated?.Invoke(myItem);
                return true;
            }
            //もう持てなかったらエラー
            else
            {
                result = InventyStatus.AlreadyMax;
                return false;
            }
        }

        public bool PopMyItem(Item _item, out InventyStatus result, int ammount = 1)
        {
            var ownedItem = MyItems.FirstOrDefault(_ => _.status == _item);
            //持ってない場合
            if (ownedItem == null)
            {
                result = InventyStatus.NotFound;
                return false;
            }
            //足りてない
            else if (ownedItem.currentStore - ammount < 0)
            {
                result = InventyStatus.NotEnough;
                return false;
            }
            else
            {
                ownedItem.currentStore -= ammount;
                if (ownedItem.currentStore < 1)
                    MyItems.Remove(ownedItem);
                result = InventyStatus.Success;

                OnMyItemUpdated?.Invoke(ownedItem);
                return true;
            }
        }
    }
}