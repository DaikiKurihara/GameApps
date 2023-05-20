using System;
using UnityEngine;

public static class ExternalUtil {
    public static string getBannerAdUnitId() {
        if (Debug.isDebugBuild) {
            Debug.Log("デバッグモードです。");
            return getTestBannerAdUnitId();
        } else {
            Debug.Log("リリースモードです。");
            TextAsset jsonData = Resources.Load<TextAsset>(CommonConstant.AD_UNIT_ID);

            if (jsonData == null) {
                return getTestBannerAdUnitId();
            }
            AdUnitId adUnitId = JsonUtility.FromJson<AdUnitId>(jsonData.ToString());

#if UNITY_ANDROID
            return adUnitId.AndroidBanner;
#elif UNITY_IPHONE
            Debug.Log("iOS用のバナーIDを返却します。" + adUnitId.iOSBanner);
            return adUnitId.iOSBanner;
#else
            return "unused"
#endif
        }

    }

    public static string getInterstitialAdUnitId() {

        if (Debug.isDebugBuild) {
            return getTestInterstitialAdUnitId();
        } else {
            TextAsset jsonData = Resources.Load<TextAsset>(CommonConstant.AD_UNIT_ID);

            if (jsonData == null) {
                return getTestInterstitialAdUnitId();
            }
            AdUnitId adUnitId = JsonUtility.FromJson<AdUnitId>(jsonData.ToString());
#if UNITY_ANDROID
            return adUnitId.AndroidInterStitial;

#elif UNITY_IPHONE
            Debug.Log("iOS用のインタースティシャルIDを返却します。" + adUnitId.iOSBanner);
            return adUnitId.iOSInterStitial;
#else
            return "unused"
#endif
        }

    }

    private static string getTestInterstitialAdUnitId() {
#if UNITY_ANDROID
            return "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
        return "ca-app-pub-3940256099942544/4411468910";
#else
            return "unused";
#endif
    }

    private static string getTestBannerAdUnitId() {
#if UNITY_ANDROID
            return "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
        return "ca-app-pub-3940256099942544/2934735716";
#else
            return "unused";
#endif

    }
}

[System.Serializable]
class AdUnitId {
    public string AndroidBanner;
    public string AndroidInterStitial;
    public string iOSBanner;
    public string iOSInterStitial;
}
