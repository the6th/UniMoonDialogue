using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

namespace UniMoonDialogue.Enventry
{
    public abstract class EnventryItemBase
    {
        public EnventryEngine.ItemType type { protected set; get; }
        public string guid;
        public string name;
        public string description;
        public Image Thumbnail;
        public int maxStore = 1;
        public int currentStore = 0;

        public virtual void Entry(string name, string description = "", int maxStore = 1)
        {
            this.name = name;
            this.description = description;
            this.maxStore = maxStore;
        }
    }

    public class Item : EnventryItemBase
    {
        public Item(string name, string description = "", int maxStore = 1)
        {
            type = EnventryEngine.ItemType.Item;
            base.Entry(name, description, maxStore);
        }
    }

    public class Mission : EnventryItemBase
    {
        public Mission(string name)
        {
            type = EnventryEngine.ItemType.Mission;
            base.Entry(name);

        }
    }

    
}