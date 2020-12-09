using UnityEngine.UI;
using UnityEngine;

#if ENABLE_Bolt
using Ludiq;
using Bolt;
#endif

namespace UniMoonDialogue.Inventry
{
    public abstract class InventryItemBase:ScriptableObject
    {
        public InventryEngine.ItemType type { protected set; get; }
        public string guid;
        public string name;
        public string description;
        public int maxStore = 1;
        public int currentStore = 0;
        public Image Thumbnail;

        public virtual void Entry(string name, string description = "", int maxStore = 1)
        {
            var x = typeof(Vector3);
            this.description = description;
            this.maxStore = maxStore;
        }
    }

#if ENABLE_Bolt
    [Inspectable]
    [CreateAssetMenu]
    [TypeIcon(typeof(Inv_Item))]
#endif
    public class Inv_Item : InventryItemBase
    {
        public Inv_Item(string name, string description = "", int maxStore = 1)
        {
            type = InventryEngine.ItemType.Item;
            base.Entry(name, description, maxStore);
        }
    }

#if ENABLE_Bolt
    [CreateAssetMenu]
    [Inspectable]
    [TypeIcon(typeof(Item_Add))]
#endif
    public class Inv_Mission : InventryItemBase
    {
        public Inv_Mission(string name)
        {
            type = InventryEngine.ItemType.Mission;
            base.Entry(name);
        }
    }
}
