using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TitleSceneManager : MonoBehaviour {
    public bool isOnVibration = true;
    public bool isOnFeintSound = true;
    public string maxTime;
    [SerializeField] private GameObject infoPopup;

    // Start is called before the first frame update
    void Start() {

    }

    public void onClickStart() {
        SceneManager.sceneLoaded += titleSceneToGameSceneLoaded;
        SceneManager.LoadScene(CommonConstant.GAME_SCENE);
    }

    public void onClickSetting() {
        SceneManager.sceneLoaded += titleSceneToSettingSceneLoaded;
        SceneManager.LoadScene(CommonConstant.SETTING_SCENE);
    }

    private void titleSceneToSettingSceneLoaded(Scene next, LoadSceneMode mode) {
        SettingSceneCanvasManager canvasManager = GameObject.FindWithTag(CommonConstant.SETTING_SCENE_CANVAS_MANAGER).GetComponent<SettingSceneCanvasManager>();
        canvasManager.setMaxTime(maxTime);
        canvasManager.isOnVibration = isOnVibration;
        canvasManager.isOnFeint = isOnFeintSound;
        SceneManager.sceneLoaded -= titleSceneToSettingSceneLoaded;
    }

    private void titleSceneToGameSceneLoaded(Scene next, LoadSceneMode mode) {
        GameManager gameManager = GameObject.FindWithTag(CommonConstant.GAME_MANAGER).GetComponent<GameManager>();
        gameManager.isOnFeintSound = isOnFeintSound;
        gameManager.isOnVibration = isOnVibration;
        gameManager.setMaxTime(maxTime);
        SceneManager.sceneLoaded -= titleSceneToGameSceneLoaded;
    }

    public void onClickInfo() {
        infoPopup.SetActive(true);
    }
}
