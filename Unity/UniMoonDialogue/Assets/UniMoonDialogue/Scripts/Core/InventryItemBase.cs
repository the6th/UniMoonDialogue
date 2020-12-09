using UnityEngine;
using System;

#if ENABLE_BOLT
using Ludiq;
#endif

namespace UniMoonDialogue
{
    [CreateAssetMenu(menuName = "Inventry/Create Item", fileName = "New Item")]
#if ENABLE_BOLT
    //[Inspectable]
#endif
    public class Item : InventryItemBase
    {
        /// <summary>
        /// 最大で保持できる数
        /// </summary>
        public int maxStore = 1;

        /// <summary>
        /// アイテムのタグ、まとめて処理する時に使う
        /// </summary>
        public string tag;

        public Item()
        {
            type = InventryEngine.ItemType.Item;

        }
    }

    [CreateAssetMenu(menuName = "Inventry/Create Mission", fileName = "New Mission")]

#if ENABLE_BOLT
    //[Inspectable]
#endif
    public class Mission : InventryItemBase
    {
        public Mission()
        {
            type = InventryEngine.ItemType.Mission;
        }
    }

    public abstract class InventryItemBase : ScriptableObject
    {
        /// <summary>
        /// アイコン画像
        /// </summary>
        public Sprite icon;
        public InventryEngine.ItemType type { protected set; get; }
        /// <summary>
        /// ユニークキー（未使用)
        /// </summary>
        public string key;
        /// <summary>
        /// 表示用名称
        /// </summary>
        public string nameForDisplay;
        /// <summary>
        /// 表示用説明文
        /// </summary>
        public string description;

        public virtual void Entry(string name, string description = "", int maxStore = 1)
        {
            this.description = description;
        }
    }

}
