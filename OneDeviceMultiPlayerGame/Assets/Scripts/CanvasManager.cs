using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : SingletonMonoBehaviour<CanvasManager> {

    [SerializeField] private Canvas canvas;
    private GameManager _gameManager;
    private Timer timer;

    public void Awake() {
        if (this != Instance) {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        _gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        timer = GameObject.FindWithTag("Timer").GetComponent<Timer>();
    }

    public void generateTouchAreaCircle(Vector2 touchPosition, int fingerId) {

        // touchPositionは画面ピクセルの位置なので、ワールド座標に変換
        Vector3 worldTouchPosition = Camera.main.ScreenToWorldPoint(touchPosition);
        worldTouchPosition.z = 0;
        GameObject touchAreaCircleInstans = TouchAreaCircle.Init(fingerId);
        // 変換したワールド座標をキャンバスのローカル座標に変換してボタンの位置に代入
        touchAreaCircleInstans.transform.position = worldTouchPosition;
        touchAreaCircleInstans.transform.SetParent(canvas.transform, false);
        // ゲームマネージャーにプレイヤーが増えたことを通知してタップ数のチェックに利用する
        this._gameManager.increasePlayerCount();
    }

    /// <summary>
    /// 画面に表示するプレイヤーNoを決定する
    /// </summary>
    public void dicidePlayerNumbers() {
        for (int i = 0; i < Input.touchCount; i++) {
            Touch touch = Input.GetTouch(i);
            TouchAreaCircle touchAreaCircle = getTouchAreaCircleByFingerId(touch.fingerId).GetComponent<TouchAreaCircle>();
            touchAreaCircle.dicidePlayerNumber(i + 1);
        }

    }

    /// <summary>
    /// タッチしているエリアの円オブジェクトを削除する
    /// </summary>
    /// <param name="fingerId"></param>
    public void destroyTouchAreaCircle(int fingerId) {
        Destroy(getTouchAreaCircleByFingerId(fingerId));
        // ゲームマネージャーにプレイヤーが減ったことを通知してタップ数のチェックに利用する
        this._gameManager.decreasePlayerCount();
    }

    /// <summary>
    /// ゲーム開始後にタップが終了した時の処理
    /// </summary>
    /// <param name="fingerId"></param>
    public void touchFinished(int fingerId) {
        TouchAreaCircle touchAreaCircle = getTouchAreaCircleByFingerId(fingerId).GetComponent<TouchAreaCircle>();
        touchAreaCircle.left();
        // ゲームマネージャーにプレイヤーが増えたことを通知してタップ数のチェックに利用する
        this._gameManager.decreasePlayerCount();
    }

    /// <summary>
    /// fingerIDからタッチエリアの円オブジェクトを取得する
    /// </summary>
    /// <param name="fingerId"></param>
    /// <returns></returns>
    private GameObject getTouchAreaCircleByFingerId(int fingerId) {
        return GameObject.FindWithTag($"{CommonConstant.FINGER_ID}{fingerId}").gameObject;
    }

    public void resetCanvas() {
        this.timer.reset();
        destroyAllTouchAreaCircle();
    }

    /// <summary>
    /// 画面に存在する円オブジェクトを全て削除する
    /// </summary>
    private void destroyAllTouchAreaCircle() {
        for (int i = 0; i < this.canvas.transform.childCount; i++) {
            GameObject go = this.canvas.transform.GetChild(i).gameObject;
            if (go.tag.Contains(CommonConstant.FINGER_ID)) {
                Destroy(go);
            }
        }
    }
}
