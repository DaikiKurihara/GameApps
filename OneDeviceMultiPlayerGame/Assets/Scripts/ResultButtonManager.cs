using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ResultButtonManager : MonoBehaviour {
    public void onRetryButton() {
        retry();
    }

    public void onGoToSettingButton() {
        SceneManager.LoadScene(CommonConstant.SETTING_SCENE);
        retry();
    }

    private void retry() {
        GameObject.FindWithTag(CommonConstant.GAME_MANAGER).GetComponent<GameManager>().gameRetry();
        GameObject.FindWithTag(CommonConstant.GOOGLE_MOBILE_ADS).GetComponent<GoogleMoblieAds>().ShowInterstitialAd();
    }
}
