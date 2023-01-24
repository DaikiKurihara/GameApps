using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : SingletonMonoBehaviour<CanvasManager> {

    [SerializeField] private Canvas canvas;

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

        // touchPositionは画面ピクセルの位置なので、ワールド座標に変換
        Vector3 worldTouchPosition = Camera.main.ScreenToWorldPoint(touchPosition);
        worldTouchPosition.z = 0;
        GameObject touchAreaCircleInstans = TouchAreaCircle.Init(fingerId);
        // 変換したワールド座標をキャンバスのローカル座標に変換してボタンの位置に代入
        touchAreaCircleInstans.transform.position = worldTouchPosition;
        touchAreaCircleInstans.transform.SetParent(canvas.transform, false);
    }

    public void dicidePlayerNumbers() {
        for (int i = 0; i < Input.touchCount; i++) {
            Touch touch = Input.GetTouch(i);
            TouchAreaCircle touchAreaCircle = GameObject.FindWithTag(CommonConstant.FINGER_ID + touch.fingerId).gameObject.GetComponent<TouchAreaCircle>();
            touchAreaCircle.dicidePlayerNumber(i + 1);
        }
    }

    public void destroyTouchAreaCircle(int fingerId) {
        string tag = CommonConstant.FINGER_ID + fingerId.ToString();
        Destroy(GameObject.FindWithTag(tag).gameObject);
    }
}
