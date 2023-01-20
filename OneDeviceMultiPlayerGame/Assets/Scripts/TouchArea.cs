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
        if (Input.touchCount == 0) {
            this._gameManager.IsCountDownStart = false;
        } else {
            this._gameManager.IsCountDownStart = true;
        }

        if (!this._gameManager.IsGameStart) {
            //----------------ゲーム開始前のプレイヤースタンバイ時間------------------------
            ///やりたいこと
            ///指を認識して指の周りに周りに丸い絵を起きたい（プレイヤーの視認性向上）
            ///指が離れたら丸い絵を消したい
            this.touchBegan();

            //----------------ゲーム開始前のプレイヤースタンバイ時間------------------------
        } else {
            //----------------ゲーム開始後のプレイ時間-----------------------------------
            ///やりたいこと

            this.touchEnded();

            if (Input.touchCount == 0 && !this._gameManager.IsGameEnd) {
                this._gameManager.IsGameEnd = true;
            }

            //----------------ゲーム開始後のプレイ時間-----------------------------------
        }

        this._touchCount = Input.touchCount;
    }


    private void touchBegan() {
        foreach (Touch touch in Input.touches) {
            if (touch.phase == TouchPhase.Began) {
                Debug.Log("fingerId:" + touch.fingerId + "がタッチ開始しました。");
                this._canvasManager.generateTouchAreaCircle(touch.position, touch.fingerId);
            }
        }
    }

    /// <summary>
    /// 指を離した瞬間を検知する
    /// </summary>
    /// <param name="touch"></param>
    private void touchEnded() {


        foreach (Touch touch in Input.touches) {
            if (touch.phase == TouchPhase.Ended) {
                Debug.Log("fingerId:" + touch.fingerId + "が離れました");
                this._gameManager.addLeftTimeMap(touch.fingerId);
            }
        }
    }

    /// <summary>
    /// 呼ばれた段階での指の配列を返す
    /// </summary>
    /// <returns></returns>
    public Touch[] getTouches() {
        return Input.touches;
    }
}
