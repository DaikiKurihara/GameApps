using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : SingletonMonoBehaviour<CanvasManager> {

    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject touchAreaCircle;

    public void Awake() {
        if (this != Instance) {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start() {

    }

    public void generateTouchAreaCircle(Vector2 touchPosition, int fingerId) {

        //var obj = Instantiate(touchAreaCircle);
        // touchPositionは画面ピクセルの位置なので、ワールド座標に変換
        Vector3 worldTouchPosition = Camera.main.ScreenToWorldPoint(touchPosition);
        worldTouchPosition.z = 0;
        GameObject touchAreaCircleInstans = TouchAreaCircleMove.Init(fingerId);
        // 変換したワールド座標をキャンバスのローカル座標に変換してボタンの位置に代入
        touchAreaCircleInstans.transform.position = worldTouchPosition;
        touchAreaCircleInstans.transform.SetParent(canvas.transform, false);
    }
}
