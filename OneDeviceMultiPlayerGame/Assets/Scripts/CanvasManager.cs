using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {

    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject leftSignLight;
    [SerializeField] private GameObject backGround;
    [SerializeField] private GameObject errorPopup;
    private PhysicalLayerManager _physicalLayerManager;

    private GameManager _gameManager;

    private Timer timer;

    /** 現在タッチしているプレイヤーのIDリスト */
    private List<int> playerIds = new List<int>();
    public List<int> PlayerIds {
        get {
            return this.playerIds;
        }
    }

    private List<int> finishedPlayerIds = new List<int>();
    public List<int> FinishedPlayerIds {
        get {
            return this.finishedPlayerIds;
        }
    }

    private List<Color32> feintColors;

    void Start() {
        _gameManager = GameObject.FindWithTag(CommonConstant.GAME_MANAGER).GetComponent<GameManager>();
        _physicalLayerManager = GameObject.FindWithTag(CommonConstant.PHYSICAL_LAYER_MANAGER).GetComponent<PhysicalLayerManager>();
        timer = GameObject.FindWithTag(CommonConstant.TIMER).GetComponent<Timer>();
        presentLeftColor();
        feintColors = _gameManager.Colors;
        feintColors.RemoveAt(_gameManager.LeftCircleColorIndex);
    }

    public void generateTouchAreaCircle(Vector2 touchPosition, int fingerId) {
        // touchPositionは画面ピクセルの位置なので、ワールド座標に変換
        Vector3 worldTouchPosition = Camera.main.ScreenToWorldPoint(touchPosition);
        worldTouchPosition.z = 0;
        GameObject touchAreaCircleInstans = TouchAreaCircle.Init(fingerId);
        touchAreaCircleInstans.transform.SetParent(canvas.transform, false);
        // 変換したワールド座標をキャンバスのローカル座標に変換してボタンの位置に代入
        touchAreaCircleInstans.transform.position = worldTouchPosition;
        touchAreaCircleInstans.transform.SetSiblingIndex(backGround.transform.GetSiblingIndex() + 1);
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
            this._gameManager.dicidePlayerNumber(touch.fingerId, i + 1);
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
        this._gameManager.fingerLeft(fingerId);
        finishedPlayerIds.Add(fingerId);
    }

    /// <summary>
    /// 中央の円オブジェクトの色を変える
    /// </summary>
    public void turnLeftLight() {
        _physicalLayerManager.onFire();
        this.leftSignLight.GetComponent<Image>().color = _gameManager.Colors[_gameManager.LeftCircleColorIndex];
    }

    /// <summary>
    /// 中央の円オブジェクトの色をデフォルトにする
    /// </summary>
    public void turnLeftLightDefault() {
        this.leftSignLight.GetComponent<Image>().color = ColorConstant.LEFT_LIGHT_DEFAULT;
    }

    /// <summary>
    /// タッチサークルの上部に結果を表示する
    /// </summary>
    /// <param name="players"></param>
    public void openResult(List<(int fingerId, int playerNum, int rank, float diff)> players) {
        _physicalLayerManager.result();
        foreach ((int fingerId, int playerNum, int rank, float diff) playerResult in players) {
            getTouchAreaCircleByFingerId(playerResult.fingerId).GetComponent<TouchAreaCircle>()
                .openResult(playerResult.rank, playerResult.diff);
        }
    }

    /// <summary>
    /// fingerIDからタッチエリアの円オブジェクトを取得する
    /// </summary>
    /// <param name="fingerId"></param>
    /// <returns></returns>
    private GameObject getTouchAreaCircleByFingerId(int fingerId) {
        return GameObject.FindWithTag($"{CommonConstant.FINGER_ID}{fingerId}");
    }

    public void displayError() {
        errorPopup.SetActive(true);
    }

    /// <summary>
    /// キャンバスを初期状態に戻す
    /// </summary>
    public void resetCanvas() {
        this.timer.reset();
        this.playerIds.Clear();
        this.finishedPlayerIds.Clear();
        this.presentLeftColor();
        feintColors = _gameManager.Colors;
        feintColors.RemoveAt(_gameManager.LeftCircleColorIndex);
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

    /// <summary>
    /// 指を離す色を表示する
    /// </summary>
    public void presentLeftColor() {
        Debug.Log($"実際の画面表示のインデックス{_gameManager.LeftCircleColorIndex}");
        this.leftSignLight.GetComponent<Image>().color = _gameManager.Colors[_gameManager.LeftCircleColorIndex];
    }

    public void supriseColorChange() {
        if (feintColors.Count <= 0) {
            return;
        }
        int feintIndex = Random.Range(0, feintColors.Count);
        this.leftSignLight.GetComponent<Image>().color = feintColors[feintIndex];
        // 一度ビビらしに使用した色は使わない
        feintColors.RemoveAt(feintIndex);
    }
}
