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
}
