using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScriptImpl : ButtonManager {
    protected override void onClick(string objectName) {
        switch (objectName) {
            case CommonConstant.PLUS_BUTTON:
                onClickPlus();
                break;
            case CommonConstant.MINUS_BUTTON:
                onClickMinus();
                break;
            case CommonConstant.HOME_BUTTON:
                onClickHome();
                break;
            default:
                return;
        }
    }

    private void onClickMinus() {
        base.onChangeMaxTime(-1);
    }

    private void onClickPlus() {
        base.onChangeMaxTime(1);
    }

    private void onClickHome() {
        base.onClickHomeButton();
    }
}
