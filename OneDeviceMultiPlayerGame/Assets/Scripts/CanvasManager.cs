using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : SingletonMonoBehaviour<CanvasManager> {

    [SerializeField] private Canvas canvas;

    [SerializeField] private GameObject leftSignLight;

    private GameManager _gameManager;

    private Timer timer;

    /** 現在タッチしているプレイヤーのIDリスト */
    private List<int> playerIds = new List<int>();
    public List<int> PlayerIds {
        get {
            return this.playerIds;
        }
    }



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
        playerIds.Add(fingerId);
        // ゲームマネージャーにプレイヤーが増えたことを通知してタップ数のチェックに利用する
        this._gameManager.increaseTouchingPlayerCount();
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
        GameObject touchAreaCircle = getTouchAreaCircleByFingerId(fingerId);
        if (touchAreaCircle == null) {
            return;
        }
        Destroy(touchAreaCircle);
        playerIds.Remove(fingerId);
        // ゲームマネージャーにプレイヤーが減ったことを通知してタップ数のチェックに利用する
        this._gameManager.decreaseTouchingPlayerCount();
    }

    /// <summary>
    /// ゲーム開始後にタップが終了した時の処理
    /// </summary>
    /// <param name="fingerId"></param>
    public void touchFinished(int fingerId) {
        TouchAreaCircle touchAreaCircle = getTouchAreaCircleByFingerId(fingerId).GetComponent<TouchAreaCircle>();
        touchAreaCircle.left();
        // ゲームマネージャーにプレイヤーが減ったことを通知してタップ数のチェックに利用する
        this._gameManager.decreaseTouchingPlayerCount();
    }

    /// <summary>
    /// 中央の円オブジェクトの色を変える
    /// </summary>
    public void turnLeftLightBlue() {
        this.leftSignLight.GetComponent<SpriteRenderer>().color = ColorConstant.LEFT_LIGHT_BLUE;
    }

    /// <summary>
    /// fingerIDからタッチエリアの円オブジェクトを取得する
    /// </summary>
    /// <param name="fingerId"></param>
    /// <returns></returns>
    private GameObject getTouchAreaCircleByFingerId(int fingerId) {
        return GameObject.FindWithTag($"{CommonConstant.FINGER_ID}{fingerId}");
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
