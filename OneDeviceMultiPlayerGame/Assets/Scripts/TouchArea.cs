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
    /** タッチクラス0〜4 */
    Touch touch0;
    Touch touch1;
    Touch touch2;
    Touch touch3;
    Touch touch4;


    void Start() {
        this._gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update() {

        if (Input.touchCount > 0) {

            //for (int i = 0; i < Input.touchCount; i++) {
            //    Touch touch = Input.GetTouch(i);
            //    if (touch.phase == TouchPhase.Ended) {
            //        Debug.Log("fingerId:" + touch.fingerId + "が離れました");
            //    }
            //}
            Touch[] touches = Input.touches;
            Parallel.ForEach(touches, touch => { touchEnded(touch); });

        }
        //if (!this._gameManager.isGameStart) {
        //    //----------------ゲーム開始前のプレイヤースタンバイ時間------------------------
        //    ///やりたいこと
        //    ///指を認識して指の周りに周りに丸い絵を起きたい（プレイヤーの視認性向上）
        //    ///指が離れたら丸い絵を消したい

        //    // 指が増減した場合
        //    if (_touchCount != Input.touchCount) {
        //        _currentFingerList = Input.touches;
        //    }
        //    //----------------ゲーム開始前のプレイヤースタンバイ時間------------------------
        //} else {
        //    //----------------ゲーム開始後のプレイ時間-----------------------------------
        //    ///やりたいこと
        //    ///指が離れたら各指の離れた時間を保持

        //    // 指が離れた場合
        //    if (_touchCount > Input.touchCount) {
        //        Debug.Log("ゲーム開始後に指が離れた");
        //        getExceptFigers(false);
        //    }
        //    if (_touchCount < Input.touchCount) {
        //        Debug.Log("ゲーム開始後に指が増えた");
        //        //this.getExceptFigers(false);
        //        /// ↓これはテスト用
        //        _currentFingerList = Input.touches;

        //    }

        //    //----------------ゲーム開始後のプレイ時間-----------------------------------
        //}


        // 2個以上指が増えた場合
        if (_touchCount - Input.touchCount < -1) {

        }

        _touchCount = Input.touchCount;
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

    private void getExceptFigers(bool isIncrease) {

        Touch touch0 = _currentFingerList[0];

        switch (touch0.phase) {
            // Record initial touch position.
            case TouchPhase.Began:
                Debug.Log("タッチした");
                break;

            // Determine direction by comparing the current touch position with the initial one.
            case TouchPhase.Moved:
                Debug.Log("動いた");
                break;

            // Report that a direction has been chosen when the finger is lifted.
            case TouchPhase.Ended:
                Debug.Log("離れた");
                break;
        }


    }

    private int[] createFingerIdList(Touch[] touches) {
        return touches.Select(touch => touch.fingerId).ToArray();
    }

    private void touchEnded(Touch touch) {
        if (touch.phase == TouchPhase.Ended) {
            Debug.Log("fingerId:" + touch.fingerId + "が離れました");
        }
    }
}
