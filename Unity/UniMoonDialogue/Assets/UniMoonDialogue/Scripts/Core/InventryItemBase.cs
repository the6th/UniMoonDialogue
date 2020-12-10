using UnityEngine;
using System;

#if ENABLE_BOLT
using Ludiq;
#endif

namespace UniMoonDialogue
{
    public class UserItem
    {
        public int currentStore = 0;
        public Item status;
        public InventryEngine.ItemType type = InventryEngine.ItemType.Item;
    }

    public class UserMisson
    {
        public Mission status;
        public InventryEngine.ItemType type = InventryEngine.ItemType.Mission;
    }


}
