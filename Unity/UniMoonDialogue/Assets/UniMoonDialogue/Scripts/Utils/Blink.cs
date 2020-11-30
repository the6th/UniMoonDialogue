using UnityEngine;
using UnityEngine.UI;

namespace UniMoonDialogue
{
    public class Blink : MonoBehaviour
    {
        public float speed = 1.6f;
        Material mat;
        Text text;
        // Use this for initialization
        void Start()
        {
            if (GetComponent<Renderer>())
            {
                mat = GetComponent<Renderer>().material;
                mat.EnableKeyword("_Color");
            }
            else
            {
                text = GetComponent<Text>();
            }
        }

        // Update is called once per frame
        void Update()
        {
            //return;
            float a;
            a = Mathf.Abs(Mathf.Cos(Time.time * speed)) / 2f + 0.0f;

            if (mat)
            {
                mat.SetColor("_Color", new Color(
                    mat.color.r,
                    mat.color.g,
                    mat.color.b,
                    a
                    ));

            }
            else if (text)
            {
                text.color = new Color(
                text.color.r,
                text.color.g,
                text.color.b,
                a
                );
            }

        }
    }
}