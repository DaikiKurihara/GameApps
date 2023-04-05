using System.Collections;
using UnityEngine;

public class TitleButtonScript : MonoBehaviour {

    private TitleSceneManager titleSceneManager;

    // Start is called before the first frame update
    void Start() {
        titleSceneManager = GameObject.FindWithTag(CommonConstant.TITLE_SCENE_MANAGER).GetComponent<TitleSceneManager>();
    }

    public void onClickStart() {
        titleSceneManager.onClickStart();
    }

    public void onClickSetting() {
        titleSceneManager.onClickSetting();
    }

    public void onClickInfo() {
        titleSceneManager.onClickInfo();
    }

    public void onClickContactUs() {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSexSAlFCGs4BxuCySFm2YoUkDtIYdjDCQUr6tlQn_QGCIwiwA/viewform");
    }
}
