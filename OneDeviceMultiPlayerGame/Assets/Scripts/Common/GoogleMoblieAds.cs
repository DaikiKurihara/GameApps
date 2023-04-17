using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class GoogleMoblieAds : SingletonMonoBehaviour<GoogleMoblieAds> {
    public void Awake() {
        if (this != Instance) {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

#if UNITY_ANDROID
  private string interStitialAdUnitId = "ca-app-pub-3940256099942544/1033173712";
  private string adUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
    private string interStitialAdUnitId = "ca-app-pub-3940256099942544/4411468910";
    private string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
  private string adUnitId = "unused";
  private string interStitialAdUnitId = "unused";
#endif

    BannerView bannerView;

    private InterstitialAd interstitialAd;

    // Start is called before the first frame update
    void Start() {
        // When true all events raised by GoogleMobileAds will be raised
        // on the Unity main thread. The default value is false.
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });
        LoadAd();
        LoadInterstitialAd();
    }

    /// <summary>
    /// Creates the banner view and loads a banner ad.
    /// </summary>
    public void LoadAd() {
        // create an instance of a banner view first.
        if (bannerView == null) {
            CreateBannerView();
        }
        // create our request used to load the ad.
        var adRequest = new AdRequest.Builder()
            .AddKeyword("unity-admob-sample")
            .Build();

        // send the request to load the ad.
        Debug.Log("Loading banner ad.");
        bannerView.LoadAd(adRequest);
    }

    /// <summary>
    /// Creates a Adaptive banner at top of the screen.
    /// </summary>
    public void CreateBannerView() {
        Debug.Log("Creating banner view");

        // If we already have a banner, destroy the old one.
        if (bannerView != null) {
            DestroyAd();
        }

        // Create a 320x50 banner at top of the screen
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
        AdSize adaptiveSize = AdSize.GetPortraitAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);

        bannerView = new BannerView(adUnitId, adaptiveSize, AdPosition.Bottom);

    }

    public void DestroyAd() {
        if (bannerView != null) {
            bannerView.Destroy();
            bannerView = null;
        }
    }

    /// <summary>
    /// Loads the interstitial ad.
    /// </summary>
    private void LoadInterstitialAd() {

        // Clean up the old ad before loading a new one.
        if (interstitialAd != null) {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest.Builder()
                .AddKeyword("unity-admob-sample")
                .Build();

        // send the request to load the ad.
        InterstitialAd.Load(interStitialAdUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) => {
                // if error is not null, the load request failed.
                if (error != null || ad == null) {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                interstitialAd = ad;
                ad.OnAdFullScreenContentClosed += () => {
                    Debug.Log("閉じられたので新たに読み込みます。");
                    LoadInterstitialAd();
                };
            });
    }

    /// <summary>
    /// Shows the interstitial ad.
    /// </summary>
    public void ShowInterstitialAd() {
        int random = Random.Range(0, 2);
        if (random == 0) {
            // 1/2の確率でインタースティシャル広告を出す
            return;
        }
        if (interstitialAd != null && interstitialAd.CanShowAd()) {
            Debug.Log("Showing interstitial ad.");
            interstitialAd.Show();
        } else {
            Debug.LogError("Interstitial ad is not ready yet.");
        }
    }

}