using System;
using UnityEngine;
using System.Collections;

// I copied this script in UNITY MANUAL {Unity version(5.3.6)}
// I just put some script ^^
// but i don't know well about assetbundle too.
// I want you to use and revise this script.

    // I put this script in Editor.
    // However, i think... This script don't need be in Editor folder.
    // i don't know well... 
    // AssetBundles is difficult...

public class DownLoadingAssetBundles : MonoBehaviour
{
    // BundleURL = assetbundles that what you want to download.
    public string BundleURL="file://C:/Users/jay09/Desktop/assetbundle/Assets/AssetBundles/pink.unity3d";
    // AssetName is name of your AssetBundles,
    public string AssetName1 = "CratePink";
    public string AssetName2 = "CratePink";
    public string AssetName3 = "CratePink";
    public string AssetName4 = "CratePink";
    public string AssetName5 = "CratePink";
    public string AssetName6 = "CratePink";

    // version....
    public int version = 1;

    void Start()
    {
        StartCoroutine(DownloadAndCache());
    }

    IEnumerator DownloadAndCache()
    {
        // Wait for the Caching system to be ready
        while (!Caching.ready)
            yield return null;

        // Load the AssetBundle file from Cache if it exists with the same version or download and store it in the cache
        using (WWW www = WWW.LoadFromCacheOrDownload(BundleURL, version))
        {
            yield return www;
            if (www.error != null)
                throw new Exception("WWW download had an error:" + www.error);
            AssetBundle bundle = www.assetBundle;
            if (AssetName1 == "")
                Instantiate(bundle.mainAsset);
            else { 
                Instantiate(bundle.LoadAsset(AssetName1));
                Instantiate(bundle.LoadAsset(AssetName2));
                Instantiate(bundle.LoadAsset(AssetName3));
                Instantiate(bundle.LoadAsset(AssetName4));
                Instantiate(bundle.LoadAsset(AssetName5));
                Instantiate(bundle.LoadAsset(AssetName6));
            }
            // Unload the AssetBundles compressed contents to conserve memory
            bundle.Unload(false);

        } // memory is freed from the web stream (www.Dispose() gets called implicitly)
    }



}
