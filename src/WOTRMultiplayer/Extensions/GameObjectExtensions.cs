using System;
using UnityEngine;

namespace WOTRMultiplayer.Extensions
{
    public static class GameObjectExtensions
    {
        public static void CleanupAllChildren(this GameObject obj, Func<GameObject, bool> onTrueDelete)
        {
            for (int i = obj.transform.childCount - 1; i >= 0; i--)
            {
                var child = obj.transform.GetChild(i);
                if (onTrueDelete(child.gameObject))
                {
                    UnityEngine.Object.DestroyImmediate(child.gameObject);
                }
            }
        }
    }
}
