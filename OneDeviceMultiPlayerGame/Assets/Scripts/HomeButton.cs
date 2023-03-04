using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeButton : MonoBehaviour {

    private HomeButtonManager buttonManager;

    void Start() {
        buttonManager = GameObject.FindWithTag(CommonConstant.HOME_BUTTON_MANAGER).GetComponent<HomeButtonManager>();
    }

    /// <summary>
    /// 長押し完了
    /// アニメーション側から呼ばれる
    /// </summary>
    public void endLongPress() {
        buttonManager.endLongPress();
    }

    /// <summary>
    /// ボタンが押された瞬間
    /// </summary>
    public void onHomeButtonDown() {
        buttonManager.onHomeButtonDown();
    }

    public void onHomeButtonUp() {
        buttonManager.onHomeButtonUp();
    }

    /// <summary>
    /// ボタンが同一ボタン範囲上で離された場合
    /// </summary>
    public void onPointerClick() {
        buttonManager.onPointerClick();
    }
}
