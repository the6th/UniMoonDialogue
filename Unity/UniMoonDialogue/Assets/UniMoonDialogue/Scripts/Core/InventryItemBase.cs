using UnityEngine;

#if ENABLE_Bolt
using Ludiq;
#endif

namespace UniMoonDialogue.Inventry
{
    [CreateAssetMenu(menuName = "Inventry/Create Item", fileName = "New Item")]
#if ENABLE_Bolt
    [Inspectable]
#endif
    public class Item : InventryItemBase
    {
        public int maxStore = 1;
        public int currentStore = 0;

        public Item(string name, string description = "", int maxStore = 1)
        {
            type = InventryEngine.ItemType.Item;
            base.Entry(name, description, maxStore);
        }
    }

    [CreateAssetMenu(menuName = "Inventry/Create Mission", fileName = "New Mission")]

#if ENABLE_Bolt
    [Inspectable]
#endif
    public class Mission : InventryItemBase
    {
        public Mission(string name)
        {
            type = InventryEngine.ItemType.Mission;
            base.Entry(name);
        }
    }

    public abstract class InventryItemBase : ScriptableObject
    {
        public Sprite icon;
        public InventryEngine.ItemType type { protected set; get; }
        public string guid;
        public string nameForDisplay;
        public string description;

        public virtual void Entry(string name, string description = "", int maxStore = 1)
        {
            this.description = description;
        }
    }

}
