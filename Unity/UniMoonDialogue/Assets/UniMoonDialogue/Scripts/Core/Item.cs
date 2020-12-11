using UnityEngine;

namespace UniMoonDialogue
{
    [CreateAssetMenu(menuName = "Inventry/Create Item", fileName = "New Item")]

    public class Item : ScriptableObject
    {
        public string _name;
        public string _discription;
        public Sprite _icon;
        public string Tag;
        public int maxStore;
    }

    public class UserItem
    {
        public int currentStore = 0;
        public Item status;
    }

}