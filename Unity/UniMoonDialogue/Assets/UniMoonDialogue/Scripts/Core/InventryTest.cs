using UnityEngine;

namespace UniMoonDialogue
{
    public class InventryTest : MonoBehaviour
    {
        InventryEngine Inventry;
        // Start is called before the first frame update
        void Start()
        {
            if (Test1())
            {
                Test2();
                Test3();
            }
        }

        bool Test1()
        {
            Inventry = InventryEngine.Instance;

            if(Inventry.AllItems.Count < 1)
            {
                Debug.LogError("Itemが登録されていません");
                return false;
            }

            foreach (var item in Inventry.AllItems)
            {
                Debug.Log($"{item.name}");
            }
            return true;
        }

        void Test2()
        {
            InventryEngine.InventyStatus result;
            Inventry.PushMyItem(Inventry.AllItems[0], out result);
            Debug.Log(result);
            Inventry.PushMyItem(Inventry.AllItems[0], out result);
            Debug.Log(result);
            Inventry.PushMyItem(Inventry.AllItems[0], out result);
            Debug.Log(result);

            Inventry.PushMyItem(Inventry.AllItems[1], out result);
            Debug.Log(result);
            Inventry.PushMyItem(Inventry.AllItems[1], out result);
            Debug.Log(result);
            Inventry.PushMyItem(Inventry.AllItems[1], out result);
            Debug.Log(result);

            ShowMyIntentryItem();

        }

        void Test3()
        {
            InventryEngine.InventyStatus result;
            Inventry.PopMyItem(Inventry.AllItems[0], out result);
            Debug.Log(result);
            Inventry.PopMyItem(Inventry.AllItems[0], out result);
            Debug.Log(result);
            Inventry.PopMyItem(Inventry.AllItems[1], out result);
            Debug.Log(result);

            ShowMyIntentryItem();
        }

        void ShowMyIntentryItem()
        {
            foreach (var item in Inventry.MyItems)
            {
                Debug.Log($"{item.status.name} ({item.currentStore}/{item.status.maxStore})");
            }
        }

    }
}