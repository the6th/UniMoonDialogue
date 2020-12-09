#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace UniMoonDialogue
{
    [CustomEditor(typeof(Item))]
    public class SetItemIcon : Editor
    {
        public override Texture2D RenderStaticPreview
        (
            string assetPath,
            Object[] subAssets,
            int width,
            int height
        )
        {
            var obj = target as Item;
            var icon = obj.icon;

            if (icon == null)
            {
                return base.RenderStaticPreview(assetPath, subAssets, width, height);
            }

            var preview = AssetPreview.GetAssetPreview(icon);
            var final = new Texture2D(width, height);

            EditorUtility.CopySerialized(preview, final);

            return final;
        }
    }

    [CustomEditor(typeof(Mission))]
    public class SetMissionIcon : Editor
    {
        public override Texture2D RenderStaticPreview
        (
            string assetPath,
            Object[] subAssets,
            int width,
            int height
        )
        {
            var obj = target as Mission;
            var icon = obj.icon;

            if (icon == null)
            {
                return base.RenderStaticPreview(assetPath, subAssets, width, height);
            }

            var preview = AssetPreview.GetAssetPreview(icon);
            var final = new Texture2D(width, height);

            EditorUtility.CopySerialized(preview, final);

            return final;
        }
    }

}
#endif
