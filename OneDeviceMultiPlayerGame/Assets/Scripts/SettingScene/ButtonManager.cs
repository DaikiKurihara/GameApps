using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour {

    [SerializeField] protected ButtonManager button;
    private SettingSceneCanvasManager canvasManager;

    void Start() {
        canvasManager = GameObject.FindWithTag(CommonConstant.SETTING_SCENE_CANVAS_MANAGER).GetComponent<SettingSceneCanvasManager>();
    }

    public void onClick() {
        if (button == null) {
            return;
        }
        // 自身のオブジェクト名を渡す
        button.onClick(gameObject.name);
    }

    protected virtual void onClick(string objectName) { }

    protected void onChangeMaxTime(int index) {
        canvasManager.onChangeMaxTime(index);
    }

    protected void onClickHomeButton() {
        canvasManager.onBackToHome();
    }
}