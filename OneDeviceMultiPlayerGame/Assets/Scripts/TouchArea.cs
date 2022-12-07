using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class TouchArea : MonoBehaviour {
    /** 現在タッチしている指の本数 */
    private int _touchCount = 0;
    /** 現在タッチしている指のIDのリスト */
    private List<int> _currentFingerIdList = new List<int>();
    /** 現在タッチしている指のIDのリスト */
    private Touch[] _currentFingerList;
    /** GameManager */
    private GameManager _gameManager;
    /** 指を離した時間を格納する辞書 */
    private Dictionary<int, float> leftTimeMap = new Dictionary<int, float>();


    void Start() {
        this._gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update() {

        if (!this._gameManager.isGameStart) {
            //----------------ゲーム開始前のプレイヤースタンバイ時間------------------------
            ///やりたいこと
            ///指を認識して指の周りに周りに丸い絵を起きたい（プレイヤーの視認性向上）
            ///指が離れたら丸い絵を消したい

            // 指が増減した場合
            if (this._touchCount != Input.touchCount) {
                this._currentFingerList = Input.touches;
            }
            //----------------ゲーム開始前のプレイヤースタンバイ時間------------------------
        } else {
            //----------------ゲーム開始後のプレイ時間-----------------------------------
            ///やりたいこと
            ///指が離れたら各指の離れた時間を保持

            Parallel.ForEach(Input.touches, touch => { touchEnded(touch); });
            if (_touchCount > Input.touchCount) {
                Debug.Log("ゲーム開始後に指が離れた");
            }

            if (_touchCount < Input.touchCount) {
                Debug.Log("ゲーム開始後に指が増えた");
            }

            if (Input.touchCount == 0 && !this._gameManager.isGameEnd) {
                this._gameManager.gameEnd();
            }

            //----------------ゲーム開始後のプレイ時間-----------------------------------
        }


        // 2個以上指が増えた場合
        if (this._touchCount - Input.touchCount < -1) {

        }

        this._touchCount = Input.touchCount;
    }

    /// <summary>
    /// 話した指のFingerIdを取得する
    /// </summary>
    /// <param name="touchCount"></param>
    /// <returns> 話した指のFingerId </returns>
    private int _getLeftFingerId(int touchCount) {
        List<int> tempFingerList = new List<int>();
        for (int i = 0; i < touchCount; i++) {
            tempFingerList.Add(Input.GetTouch(i).fingerId);
        }
        int leftFingerId = this._currentFingerIdList.Except(tempFingerList).First<int>();
        Debug.Log("差分" + leftFingerId);
        return leftFingerId;
    }


    /// <summary>
    /// 指定した要素をcurrentFingerIdListから削除する
    /// </summary>
    /// <param name="fingerId"></param>
    private void _removeAtFromList(int fingerId) {
        this._currentFingerIdList.Remove(fingerId);
        Debug.Log("差分削除後のリスト" + string.Join(",", this._currentFingerIdList));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fingerId"></param>
    private void addLeftTimeMap(int fingerId) {
        this.leftTimeMap.Add(fingerId, this._gameManager.getPassedTime());
        Debug.Log(string.Join(",", this.leftTimeMap.ToArray()));
    }

    /// <summary>
    /// 指を離した瞬間を検知する
    /// </summary>
    /// <param name="touch"></param>
    private void touchEnded(Touch touch) {
        if (touch.phase == TouchPhase.Ended) {
            Debug.Log("fingerId:" + touch.fingerId + "が離れました");
            this.addLeftTimeMap(touch.fingerId);
        }
    }
}
