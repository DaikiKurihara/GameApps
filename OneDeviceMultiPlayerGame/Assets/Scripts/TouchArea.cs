using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TouchArea : MonoBehaviour {
    // Start is called before the first frame update
    private int touchCount = 0;
    private List<int> currentFingerList = new List<int>();

    void Start() {

    }

    // Update is called once per frame
    void Update() {
        // タッチしている指が増減した場合
        if (touchCount != Input.touchCount) {

            // 指が増えた場合
            if (touchCount < Input.touchCount) {
                _createCurrentFingerIdList(Input.touchCount);
            }

            // 指が離れた場合
            if (touchCount > Input.touchCount) {
                int leftFfngerID = _getLeftFingerId(Input.touchCount);
                _removeAtFromList(leftFfngerID);

            }
            touchCount = Input.touchCount;
        }

    }

    /// <summary>
    /// 今タップしている指のリストを作る
    /// </summary>
    private void _createCurrentFingerIdList(int touchCount) {
        // TODO: touchCountが1ずつ増加するとは限らないのでループで追加する必要がある
        // touchCountは1始まりだが、GetTouchのインデックスは0始まりなので-1する
        this.currentFingerList.Add(Input.GetTouch(touchCount - 1).fingerId);
        Debug.Log("現在のFignerList" + string.Join(",", currentFingerList));
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
        int leftFingerId = this.currentFingerList.Except(tempFingerList).First<int>();
        Debug.Log("差分" + leftFingerId);
        return leftFingerId;
    }

    /// <summary>
    /// 指定した要素をcurrentFingerListから削除する
    /// </summary>
    /// <param name="fingerId"></param>
    private void _removeAtFromList(int fingerId) {
        this.currentFingerList.Remove(fingerId);
        Debug.Log("差分削除後のリスト" + string.Join(",", this.currentFingerList));
    }
}
