using UnityEngine.SceneManagement;
using UnityEngine;

public class HomeButtonManager : MonoBehaviour {
    private bool isLongPressed = false;
    private GameObject homeButtonGhost;
    private Animator ghostAnimator;
    private bool isOnVibration;
    private bool isOnFeintSound;
    private float maxTime;

    void Start() {
        homeButtonGhost = GameObject.FindWithTag(CommonConstant.HOME_BUTTON_GHOST);
        ghostAnimator = homeButtonGhost.GetComponent<Animator>();
    }

    public void endLongPress() {
        isLongPressed = true;
    }

    public void onHomeButtonDown() {
        // 一度長押し完了したけどボタンエリア外で離された場合、trueになってしまうのでここで再度falseにしておく
        isLongPressed = false;
        ghostAnimator.SetBool(CommonConstant.HOME_BUTTON_MOVE, true);
    }

    public void onHomeButtonUp() {
        ghostAnimator.SetBool(CommonConstant.HOME_BUTTON_MOVE, false);
    }

    /// <summary>
    /// ホームボタン上で指を離した場合に呼ばれる
    /// ホームボタン上で指を離したかつ長押しが完了していたら画面遷移
    /// </summary>
    public void onPointerClick() {
        if (isLongPressed) {
            onBackToHome();
        }
        isLongPressed = false;
    }

    public void onBackToHome() {
        SceneManager.sceneLoaded += GameSceceToTitleSceneLoaded;
        GameManager gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        isOnFeintSound = gameManager.isOnFeintSound;
        isOnVibration = gameManager.isOnVibration;
        maxTime = gameManager.MaxTime;
        // シーン切り替え
        SceneManager.LoadScene(CommonConstant.TITLE_SCENE);
    }

    private void GameSceceToTitleSceneLoaded(Scene next, LoadSceneMode mode) {
        TitleSceneManager titleSceneManager = GameObject.FindWithTag(CommonConstant.TITLE_SCENE_MANAGER).GetComponent<TitleSceneManager>();
        titleSceneManager.maxTime = $"{maxTime}";
        titleSceneManager.isOnVibration = isOnVibration;
        titleSceneManager.isOnFeintSound = isOnFeintSound;
        SceneManager.sceneLoaded -= GameSceceToTitleSceneLoaded;
    }
}
