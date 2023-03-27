using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ResultButtonManager : MonoBehaviour {
    public void onRetryButton() {
        GameObject.FindWithTag(CommonConstant.GAME_MANAGER).GetComponent<GameManager>().gameRetry();
    }

    public void onGoToSettingButton() {
        SceneManager.LoadScene(CommonConstant.SETTING_SCENE);
        GameObject.FindWithTag(CommonConstant.GAME_MANAGER).GetComponent<GameManager>().gameRetry();
    }
}
