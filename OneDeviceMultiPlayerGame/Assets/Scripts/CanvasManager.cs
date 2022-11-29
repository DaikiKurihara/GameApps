using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : SingletonMonoBehaviour<CanvasManager> {

    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject button;

    public void Awake() {
        if (this != Instance) {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start() {
    }

    /// <summary>
    /// スタートタイマーによるゲーム開始時の挙動
    /// </summary>
    public void gameStart() {
        Debug.Log("ゲーム開始");
        GameObject touchAreaObj = GameObject.FindWithTag("TouchArea");
        TouchArea touchArea = touchAreaObj.GetComponent<TouchArea>();
        touchArea.isGameStarted = true;
    }

    public void generateTouchButton(Vector2 touchPosition) {

        var obj = Instantiate(button);
        // touchPositionは画面ピクセルの位置なので、ワールド座標に変換
        Vector3 worldTouchPosition = Camera.main.ScreenToWorldPoint(touchPosition);
        worldTouchPosition.z = 0;
        // 変換したワールド座標をキャンバスのローカル座標に変換してボタンの位置に代入
        obj.transform.position = canvas.transform.InverseTransformPoint(worldTouchPosition);
        obj.transform.SetParent(canvas.transform, false);
    }
}
