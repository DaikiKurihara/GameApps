using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ResultButtonManager : MonoBehaviour {

    [SerializeField] private GameObject resultScreen;
    private GameManager gameManager;

    void Start() {
        gameManager = GameObject.FindWithTag(CommonConstant.GAME_MANAGER).GetComponent<GameManager>();
    }

    public void onRetryButton() {
        retry();
    }

    public void onGoToSettingButton() {
        SceneManager.sceneLoaded += gameSceneToSettingSceneLoaded;
        SceneManager.LoadScene(CommonConstant.SETTING_SCENE);
        retry();
    }

    private void retry() {
        GameObject.FindWithTag(CommonConstant.GAME_MANAGER).GetComponent<GameManager>().gameRetry();
        showInterstitialAd();
    }

    private void gameSceneToSettingSceneLoaded(Scene next, LoadSceneMode mode) {
        SettingSceneCanvasManager settingSceneManager = GameObject.FindWithTag(CommonConstant.SETTING_SCENE_CANVAS_MANAGER).GetComponent<SettingSceneCanvasManager>();
        settingSceneManager.setFloatMaxTime(gameManager.MaxTime);
        settingSceneManager.isOnVibration = gameManager.isOnVibration;
        settingSceneManager.isOnFeint = gameManager.isOnFeintSound;
        SceneManager.sceneLoaded -= gameSceneToSettingSceneLoaded;
    }

    private void showInterstitialAd() {
        GameObject ads = GameObject.FindWithTag(CommonConstant.GOOGLE_MOBILE_ADS);
        if (ads != null) {
            ads.GetComponent<GoogleMoblieAds>().ShowInterstitialAd();
        }
    }

    public void hiddenResult() {
        resultScreen.SetActive(false);
    }

    public void displayResult() {
        resultScreen.SetActive(true);
    }
}
