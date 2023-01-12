using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : SingletonMonoBehaviour<CanvasManager> {

    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject touchAreaCircle;
    [SerializeField] private GameObject hoge;

    public void Awake() {
        if (this != Instance) {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start() {

    }

    public void generateTouchAreaCircle(Vector2 touchPosition) {

        var obj = Instantiate(touchAreaCircle);
        // touchPositionは画面ピクセルの位置なので、ワールド座標に変換
        Vector3 worldTouchPosition = Camera.main.ScreenToWorldPoint(touchPosition);
        worldTouchPosition.z = 0;
        // 変換したワールド座標をキャンバスのローカル座標に変換してボタンの位置に代入
        obj.transform.position = canvas.transform.InverseTransformPoint(worldTouchPosition);
        obj.transform.SetParent(canvas.transform, false);
    }
}
