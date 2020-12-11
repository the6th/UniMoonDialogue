using UnityEngine;

namespace UniMoonDialogue
{
    [CreateAssetMenu(menuName = "Inventry/Create Mission", fileName = "New Mission")]
    public class Mission : ScriptableObject
    {
        public string _name;
        public string _discription;
        public Sprite _icon;
    }

    public class UserMisson
    {
        public Mission status;
    }

}