using System;
using System.IO;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniMoonDialogue
{
    [Serializable]
    public class LuaTextAsset
    {

        public const string Extension = ".lua";

        [SerializeField] private string path = "";

        [SerializeField] private string textString = "";

        [SerializeField] private string byteString = "";

        public string Path => path;
        public string text => textString;

        public byte[] bytes => Encoding.ASCII.GetBytes(byteString);

        public static implicit operator TextAsset(LuaTextAsset textAsset)
        {
            return new TextAsset(textAsset.textString);
        }

        public static implicit operator LuaTextAsset(TextAsset textAsset)
        {
            return new LuaTextAsset { textString = textAsset.text };
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(LuaTextAsset))]
    public class LuaInspectorEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var path = property.FindPropertyRelative("path").stringValue;
            var loaded = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
            var field = EditorGUI.ObjectField(position, label, loaded, typeof(Object), false);
            var loadPath = AssetDatabase.GetAssetPath(field);
            var fileExtension = Path.GetExtension(loadPath);
            if (field == null || fileExtension != LuaTextAsset.Extension)
            {
                property.Set("path", "");
                property.Set("textString", "");
                property.Set("byteString", "");
            }
            else
            {
                var pathProperty = property.FindPropertyRelative("path");
                property.Set("path", loadPath.Substring(loadPath.IndexOf("Assets", StringComparison.Ordinal)));
                property.Set("textString", File.ReadAllText(pathProperty.stringValue));
                property.Set("byteString", Encoding.ASCII.GetString(File.ReadAllBytes(pathProperty.stringValue)));
            }
        }
    }

    public static class SerializedPropertyExtension
    {
        public static void Set(this SerializedProperty property, string name, string value)
        {
            var pathProperty = property.FindPropertyRelative(name);
            pathProperty.stringValue = value;
        }
    }
#endif
}