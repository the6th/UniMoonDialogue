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

            foreach (var item in enventry.GetAllItemList())
            {
                Debug.Log($"{item.name}/{item.type}");
            }
        }

        void Test2()
        {
            EnventryEngine.ItemStoreResult result;
            enventry.GetItem(enventry.GetAllItemList()[0].name, out result);
            Debug.Log(result);
            enventry.GetItem(enventry.GetAllItemList()[0].name, out result);
            Debug.Log(result);
            enventry.GetItem(enventry.GetAllItemList()[0].name, out result);
            Debug.Log(result);

            enventry.GetItem(enventry.GetAllItemList()[1].name, out result);
            Debug.Log(result);
            enventry.GetItem(enventry.GetAllItemList()[1].name, out result);
            Debug.Log(result);
            enventry.GetItem(enventry.GetAllItemList()[1].name, out result);
            Debug.Log(result);

            ShowMyIntentryItem();

        }

        void Test3()
        {
            EnventryEngine.ItemStoreResult result;
            enventry.UseItem(enventry.GetAllItemList()[0].name, out result);
            Debug.Log(result);
            enventry.UseItem(enventry.GetAllItemList()[0].name, out result);
            Debug.Log(result);
            enventry.UseItem(enventry.GetAllItemList()[1].name, out result);
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