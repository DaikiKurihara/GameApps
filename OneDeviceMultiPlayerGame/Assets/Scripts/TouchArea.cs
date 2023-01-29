using System.Threading.Tasks;
using UnityEngine;

public class TouchArea : MonoBehaviour {
    /** 現在タッチしている指の本数 */
    private int _touchCount = 0;
    /** GameManager */
    private GameManager _gameManager;
    /** GameManager */
    private CanvasManager _canvasManager;

    void Start() {
        this._gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        _canvasManager = GameObject.FindWithTag("CanvasManager").GetComponent<CanvasManager>();
    }

    // Update is called once per frame
    void Update() {
        if (!this._gameManager.IsGameStart) {
            //----------------ゲーム開始前のプレイヤースタンバイ時間------------------------
            // 触れている指が1本以下だったらカウントダウンしない
            this._gameManager.IsCountDownStart = Input.touchCount < 2;

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
        if (_touchCount != Input.touchCount) {
            this._gameManager.checkPlayerCount();
        }

        this._touchCount = Input.touchCount;
    }


    private void touchBegan() {
        foreach (Touch touch in Input.touches) {
            if (touch.phase == TouchPhase.Began) {
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
        if (!this._gameManager.IsGameStart) {
            return;
        }
        foreach (Touch touch in Input.touches) {
            if (touch.phase == TouchPhase.Ended) {
                this._gameManager.addLeftTimeMap(touch.fingerId);
                this._canvasManager.touchFinished(touch.fingerId);
            }
        }
    }
}
