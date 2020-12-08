using UnityEngine;

namespace UniMoonDialogue.Enventry
{
    public class EnventryTest : MonoBehaviour
    {
        EnventryEngine enventry;
        // Start is called before the first frame update
        void Start()
        {
            Test1();
            Test2();
            Test3();
        }

        void Test1()
        {
            enventry = EnventryEngine.Instance;

            foreach (var item in enventry.GetAllItemList(EnventryEngine.ItemType.Item))
            {
                Debug.Log($"{item.name}/{item.type}");
            }
        }

        void Test2()
        {
            EnventryEngine.ItemStoreResult result;
            enventry.GetItem(enventry.GetAllItemList(EnventryEngine.ItemType.Item)[0].name, EnventryEngine.ItemType.Item, out result);
            Debug.Log(result);
            enventry.GetItem(enventry.GetAllItemList(EnventryEngine.ItemType.Item)[0].name, EnventryEngine.ItemType.Item, out result);
            Debug.Log(result);
            enventry.GetItem(enventry.GetAllItemList(EnventryEngine.ItemType.Item)[0].name, EnventryEngine.ItemType.Item, out result);
            Debug.Log(result);

            enventry.GetItem(enventry.GetAllItemList(EnventryEngine.ItemType.Item)[1].name, EnventryEngine.ItemType.Item, out result);
            Debug.Log(result);
            enventry.GetItem(enventry.GetAllItemList(EnventryEngine.ItemType.Item)[1].name, EnventryEngine.ItemType.Item, out result);
            Debug.Log(result);
            enventry.GetItem(enventry.GetAllItemList(EnventryEngine.ItemType.Item)[1].name, EnventryEngine.ItemType.Item, out result);
            Debug.Log(result);

            ShowMyIntentryItem();

        }

        void Test3()
        {
            EnventryEngine.ItemStoreResult result;
            enventry.UseItem(enventry.GetAllItemList(EnventryEngine.ItemType.Item)[0].name, EnventryEngine.ItemType.Item, out result);
            Debug.Log(result);
            enventry.UseItem(enventry.GetAllItemList(EnventryEngine.ItemType.Item)[0].name, EnventryEngine.ItemType.Item, out result);
            Debug.Log(result);
            enventry.UseItem(enventry.GetAllItemList(EnventryEngine.ItemType.Item)[1].name, EnventryEngine.ItemType.Item, out result);
            Debug.Log(result);

            ShowMyIntentryItem();
        }

        void ShowMyIntentryItem(EnventryEngine.ItemType type = EnventryEngine.ItemType.Item)
        {
            foreach (var item in enventry.GetMyItemList(type))
            {
                Debug.Log($"[{item.type}]{item.name} ({item.currentStore}/{item.maxStore})");
            }
        }

    }
}