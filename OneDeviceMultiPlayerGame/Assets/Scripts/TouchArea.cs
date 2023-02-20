using System.Threading.Tasks;
using UnityEngine;

public class TouchArea : MonoBehaviour {
    /** 現在タッチしている指の本数 */
    private int _touchCount = 0;
    /** GameManager */
    private GameManager _gameManager;
    /** CanvasManager */
    private CanvasManager _canvasManager;
    private PhysicalLayerManager _physicalLayerManager;

    void Start() {
        _gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        _canvasManager = GameObject.FindWithTag("CanvasManager").GetComponent<CanvasManager>();
        _physicalLayerManager = GameObject.FindWithTag("PhysicalLayerManager").GetComponent<PhysicalLayerManager>();
    }

    // Update is called once per frame
    void Update() {
        if (!this._gameManager.IsGameStart) {
            //----------------ゲーム開始前のプレイヤースタンバイ時間------------------------
            // 触れている指が2本以上でカウントダウン開始
            this._gameManager.IsCountDownStart = Input.touchCount >= 2;

            this.touchBegan();

            this.touchEnded();
            //----------------ゲーム開始前のプレイヤースタンバイ時間------------------------
        } else {
            //----------------ゲーム開始後のプレイ時間-----------------------------------
            this.touchFinished();
            if (Input.touchCount == 0 && !this._gameManager.IsGameEnd) {
                this._gameManager.IsGameEnd = true;
            }
            //----------------ゲーム開始後のプレイ時間-----------------------------------
        }
        // iPhoneではタップの限界を超えた場合にtouchCountが0になるのでプレイヤー人数をチェックする
        if (_touchCount != 0 && Input.touchCount == 0) {
            this._gameManager.checkPlayerCount();
        }

        this._touchCount = Input.touchCount;
    }

    private void touchBegan() {
        foreach (Touch touch in Input.touches) {
            if (touch.phase == TouchPhase.Began) {
                _physicalLayerManager.onTouch();
                this._canvasManager.generateTouchAreaCircle(touch.position, touch.fingerId);
            }
        }
    }

    /// <summary>
    /// 指を離した瞬間を検知する
    /// </summary>
    /// <param name="touch"></param>
    private void touchEnded() {
        if (this._gameManager.IsGameStart) {
            return;
        }
        foreach (Touch touch in Input.touches) {
            if (touch.phase == TouchPhase.Ended) {
                this._canvasManager.destroyTouchAreaCircle(touch.fingerId);
            }
        }
    }

    /// <summary>
    /// ゲーム開始後に指が離れた場合
    /// </summary>
    private void touchFinished() {
        if (!this._gameManager.IsGameStart || this._gameManager.IsGameEnd) {
            return;
        }
        foreach (Touch touch in Input.touches) {
            if (touch.phase == TouchPhase.Ended) {
                if (!this._canvasManager.PlayerIds.Contains(touch.fingerId)) {
                    // 画面上にいるプレイヤーではない場合処理終了
                    return;
                }
                _physicalLayerManager.onLeftTouch();
                this._canvasManager.touchFinished(touch.fingerId);
            }
        }
    }
}
