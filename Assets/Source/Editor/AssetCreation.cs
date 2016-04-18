using UnityEngine;
using System.Collections;
using Assets.UnityLibrary.Scripts.Editor;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AssetCreation
{
    [MenuItem("Assets/Create/MelodyData")]
    public static void CreateMelodyData()
    {
        var data = ScriptableObjectUtility.CreateAsset<MelodyData>();
    }
}
