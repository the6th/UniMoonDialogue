using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UniMoonDialogue.Enventry
{
    [CreateAssetMenu]
    //ScriptableObjectを継承したクラス
    public class EnventryItemList : ScriptableObject
    {
        public List<Item> Items = new List<Item>();
        public List<Mission> Missions = new List<Mission>();

    }
}