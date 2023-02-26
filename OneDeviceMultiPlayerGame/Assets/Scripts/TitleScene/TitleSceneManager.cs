using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TitleSceneManager : MonoBehaviour {
    public bool isOnVibration = true;
    public bool isOnFeintSound = true;
    public string maxTime;

    // Start is called before the first frame update
    void Start() {

    }

    public void onClickStart() {
        SceneManager.sceneLoaded += gameSceneLoaded;
        // シーン切り替え
        SceneManager.LoadScene(CommonConstant.GAME_SCENE);
    }

    public void onClickSetting() {
        SceneManager.LoadScene(CommonConstant.SETTING_SCENE);
    }

    private void gameSceneLoaded(Scene next, LoadSceneMode mode) {
        GameManager gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        gameManager.isOnFeintSound = isOnFeintSound;
        gameManager.isOnVibration = isOnVibration;
        gameManager.setMaxTime(maxTime);
        SceneManager.sceneLoaded -= gameSceneLoaded;
    }
}
