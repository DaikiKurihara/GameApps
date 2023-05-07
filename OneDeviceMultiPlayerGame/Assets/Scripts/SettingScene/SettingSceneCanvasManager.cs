using System;
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
    [NonSerialized] public string maxTimeText;
    [NonSerialized] public bool isOnFeint;
    [NonSerialized] public bool isOnVibration;
    private SettingScenePhysicalLayerManager physicalLayerManager;

    void Start() {
        maxTimeValue = GameObject.FindWithTag(CommonConstant.MAX_TIME_VALUE).GetComponent<TextMeshProUGUI>();
        maxTimeValues = MaxTimeMapConstant.MAX_TIMES;
        if (currentMaxTimeIndex < 0) {
            currentMaxTimeIndex = MaxTimeMapConstant.DEFAULT_MAX_TIME_INDEX;
        }
        Debug.Log("インデックスは" + currentMaxTimeIndex);
        maxTimeValue.text = maxTimeValues[currentMaxTimeIndex];
        feintSoundToggle = GameObject.FindWithTag(CommonConstant.FEINT_SOUND_TOGGLE).GetComponent<Toggle>();
        vibrationToggle = GameObject.FindWithTag(CommonConstant.VIBRATION_TOGGLE).GetComponent<Toggle>();
        physicalLayerManager = GameObject.FindWithTag(CommonConstant.PHYSICAL_LAYER_MANAGER).GetComponent<SettingScenePhysicalLayerManager>();
        vibrationToggle.isOn = isOnVibration;
        feintSoundToggle.isOn = isOnFeint;
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

    public void setFloatMaxTime(float maxTime) {
        string maxTimeText = ((int)maxTime).ToString();
        setMaxTime(maxTimeText);
    }

    /// <summary>
    /// 見つからなかった場合（初期表示とか）はcurrentMaxTimeIndexに-1が設定される
    /// </summary>
    /// <param name="maxTime"></param>
    public void setMaxTime(string maxTime) {
        currentMaxTimeIndex = MaxTimeMapConstant.MAX_TIMES.FindIndex(x => x.Equals(maxTime));
    }
}
