using UnityEngine;
using UnityEditor;
using System.Collections;

// I copied this script in UNITY MANUAL {Unity version(5.3.6)}
// I just put some script ^^
// I want you to use and revise this script.

    // Editor script!!!
    // This script should live in "Assets/Editor" folder.

public class CreateAssetBundles : MonoBehaviour {

    // Creating AssetBundles.
    // See it UNITY MANUAL.
    public class CreateAssetBundle
    {
        [MenuItem("Assets/Build AssetBundles")]
        static void BuildAllAssetBundles()
        {
            // first string is where your assetbundles built
            // second build options
            // Third Target platform. I did it in android platform. So BuildTarget.Android
            BuildPipeline.BuildAssetBundles("Assets/AssetBundles", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
        }
    }

    // It bring AssetBundleNames.
    // Check AssetBundles built well.
    public class GetAssetBundleNames
    {
        [MenuItem("Assets/Get AssetBundle names")]
        static void GetNames()
        {
            var names = AssetDatabase.GetAllAssetBundleNames();
            foreach (var name in names)
                Debug.Log("AssetBundle: " + name);
        }

    }

    // I don't know about script that under. hhhhh
    public class MyPostprocessor : AssetPostprocessor
    {

        void OnPostprocessAssetbundleNameChanged(string path,
                string previous, string next)
        {
            Debug.Log("AB: " + path + " old: " + previous + " new: " + next);
        }
    }
}
