using UnityEngine;
using System.Collections;
using Assets.Source;
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

    [MenuItem("LD35/GenerateMelodies")]
    public static void CreateMelodies()
    {
        var conv = new MidiToMelodiesConvertor();
        conv.Start();

    }


    [MenuItem("LD35/GenerateMelodies-Test")]
    public static void CreateMelodiesTest()
    {
        var conv = new MidiToMelodiesConvertor();
        conv.generate = false;
        conv.Start();

    }
}
