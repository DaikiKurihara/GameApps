using System.Threading.Tasks;
using UnityEngine;

public class TouchArea : MonoBehaviour {
    /** 現在タッチしている指の本数 */
    private int _touchCount = 0;
    /** GameManager */
    private GameManager _gameManager;

    void Start() {
        this._gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update() {

        if (!this._gameManager.IsGameStart) {
            //----------------ゲーム開始前のプレイヤースタンバイ時間------------------------
            ///やりたいこと
            ///指を認識して指の周りに周りに丸い絵を起きたい（プレイヤーの視認性向上）
            ///指が離れたら丸い絵を消したい


            //----------------ゲーム開始前のプレイヤースタンバイ時間------------------------
        } else {
            //----------------ゲーム開始後のプレイ時間-----------------------------------
            ///やりたいこと
            ///指が離れたら各指の離れた時間を保持

            this.touchEnded();

            if (Input.touchCount == 0 && !this._gameManager.IsGameEnd) {
                this._gameManager.IsGameEnd = true;
            }

            //----------------ゲーム開始後のプレイ時間-----------------------------------
        }

        this._touchCount = Input.touchCount;
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
