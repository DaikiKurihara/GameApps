using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ResultButtonManager : MonoBehaviour {
    public void onRetryButton() {
        GameObject.FindWithTag("GameController").GetComponent<GameManager>().gameRetry();
    }

    public void onGoToSettingButton() {
        SceneManager.LoadScene(CommonConstant.SETTING_SCENE);
        GameObject.FindWithTag("GameController").GetComponent<GameManager>().gameRetry();
    }
}
