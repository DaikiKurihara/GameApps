using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingSceneCanvasManager : MonoBehaviour {

    private TextMeshProUGUI maxTimeValue;
    private List<string> maxTimeValues;
    private int currentMaxTimeIndex;
    private Toggle feintSoundToggle;
    private Toggle vibrationToggle;
    private SettingScenePhysicalLayerManager physicalLayerManager;

    void Start() {
        maxTimeValue = GameObject.FindWithTag(CommonConstant.MAX_TIME_VALUE).GetComponent<TextMeshProUGUI>();
        maxTimeValues = MaxTimeMapConstant.MAX_TIMES;
        currentMaxTimeIndex = MaxTimeMapConstant.DEFAULT_MAX_TIME_INDEX;
        maxTimeValue.text = maxTimeValues[currentMaxTimeIndex];
        feintSoundToggle = GameObject.FindWithTag(CommonConstant.FEINT_SOUND_TOGGLE).GetComponent<Toggle>();
        vibrationToggle = GameObject.FindWithTag(CommonConstant.VIBRATION_TOGGLE).GetComponent<Toggle>();
        physicalLayerManager = GameObject.FindWithTag(CommonConstant.PHYSICAL_LAYER_MANAGER).GetComponent<SettingScenePhysicalLayerManager>();
    }

    public void onChangeMaxTime(int index) {
        int tempCurrentIndex = currentMaxTimeIndex + index;
        if (tempCurrentIndex < 0 || tempCurrentIndex > maxTimeValues.Count - 1) {
            return;
        }
        physicalLayerManager.setting();
        currentMaxTimeIndex = tempCurrentIndex;
        maxTimeValue.text = maxTimeValues[currentMaxTimeIndex];
    }

    public void onBackToHome() {
        physicalLayerManager.setting();
        SceneManager.sceneLoaded += settingSceneToTitleSceneLoaded;
        // シーン切り替え
        SceneManager.LoadScene(CommonConstant.TITLE_SCENE);
    }

    private void settingSceneToTitleSceneLoaded(Scene next, LoadSceneMode mode) {
        TitleSceneManager titleSceneManager = GameObject.FindWithTag(CommonConstant.TITLE_SCENE_MANAGER).GetComponent<TitleSceneManager>();
        titleSceneManager.maxTime = maxTimeValue.text;
        titleSceneManager.isOnVibration = vibrationToggle.isOn;
        titleSceneManager.isOnFeintSound = feintSoundToggle.isOn;
        SceneManager.sceneLoaded -= settingSceneToTitleSceneLoaded;
    }
}
