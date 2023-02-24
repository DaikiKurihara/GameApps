using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingSceneCanvasManager : SingletonMonoBehaviour<SettingSceneCanvasManager> {

    private TextMeshProUGUI maxTimeValue;
    private List<string> maxTimeValues;
    private int currentMaxTimeIndex;
    private Toggle feintSoundToggle;
    private Toggle vibrationToggle;

    public void Awake() {
        if (this != Instance) {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        maxTimeValue = GameObject.FindWithTag(CommonConstant.MAX_TIME_VALUE).GetComponent<TextMeshProUGUI>();
        maxTimeValues = MaxTimeMapConstant.MAX_TIMES;
        currentMaxTimeIndex = MaxTimeMapConstant.DEFAULT_MAX_TIME_INDEX;
        maxTimeValue.text = maxTimeValues[currentMaxTimeIndex];
        feintSoundToggle = GameObject.FindWithTag(CommonConstant.FEINT_SOUND_TOGGLE).GetComponent<Toggle>();
        vibrationToggle = GameObject.FindWithTag(CommonConstant.VIBRATION_TOGGLE).GetComponent<Toggle>();
    }

    public void onChangeMaxTime(int index) {
        int tempCurrentIndex = currentMaxTimeIndex + index;
        if (tempCurrentIndex < 0 || tempCurrentIndex > maxTimeValues.Count - 1) {
            return;
        }
        currentMaxTimeIndex = tempCurrentIndex;
        maxTimeValue.text = maxTimeValues[currentMaxTimeIndex];
    }

    public void onBackToHome() {
        SceneManager.sceneLoaded += homeSceneLoaded;
        // シーン切り替え
        SceneManager.LoadScene(CommonConstant.HOME_SCENE);
    }

    private void homeSceneLoaded(Scene next, LoadSceneMode mode) {
        GameManager gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        gameManager.setMaxTime(maxTimeValue.text);
        gameManager.isOnVibration = vibrationToggle.isOn;
        gameManager.isOnFeintSound = feintSoundToggle.isOn;
        SceneManager.sceneLoaded -= homeSceneLoaded;
    }
}
