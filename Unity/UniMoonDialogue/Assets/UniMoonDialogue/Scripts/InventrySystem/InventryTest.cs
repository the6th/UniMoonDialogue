using UnityEngine;

namespace UniMoonDialogue.Inventry
{
    public class InventryTest : MonoBehaviour
    {
        InventryEngine enventry;
        // Start is called before the first frame update
        void Start()
        {
            Test1();
            Test2();
            Test3();
        }

        void Test1()
        {
            enventry = InventryEngine.Instance;

            foreach (var item in enventry.GetAllItemList())
            {
                Debug.Log($"{item.name}/{item.type}");
            }
        }

        void Test2()
        {
            InventryEngine.ItemStoreResult result;
            enventry.AddItem(enventry.GetAllItemList()[0].name, out result);
            Debug.Log(result);
            enventry.AddItem(enventry.GetAllItemList()[0].name, out result);
            Debug.Log(result);
            enventry.AddItem(enventry.GetAllItemList()[0].name, out result);
            Debug.Log(result);

            enventry.AddItem(enventry.GetAllItemList()[1].name, out result);
            Debug.Log(result);
            enventry.AddItem(enventry.GetAllItemList()[1].name, out result);
            Debug.Log(result);
            enventry.AddItem(enventry.GetAllItemList()[1].name, out result);
            Debug.Log(result);

            ShowMyIntentryItem();

        }

        void Test3()
        {
            InventryEngine.ItemStoreResult result;
            enventry.TakeItem(enventry.GetAllItemList()[0].name, out result);
            Debug.Log(result);
            enventry.TakeItem(enventry.GetAllItemList()[0].name, out result);
            Debug.Log(result);
            enventry.TakeItem(enventry.GetAllItemList()[1].name, out result);
            Debug.Log(result);

            ShowMyIntentryItem();
        }

        void ShowMyIntentryItem()
        {
            foreach (var item in enventry.GetMyItemList())
            {
                Debug.Log($"[{item.type}]{item.name} ({item.currentStore}/{item.maxStore})");
            }
        }

    }
}