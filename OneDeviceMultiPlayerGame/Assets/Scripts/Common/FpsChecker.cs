using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <Summary>
/// シーンのフレームレートを計測して画面に表示するスクリプトです。
/// </Summary>
public class FpsChecker : MonoBehaviour {
    // フレームレートを表示するテキストです。
    public TextMeshProUGUI fpsText;

    // Update()が呼ばれた回数をカウントします。
    int frameCount;

    // 前回フレームレートを表示してからの経過時間です。
    float elapsedTime;

    void Start() {

    }

    void Update() {
        // 呼ばれた回数を加算します。
        frameCount++;

        // 前のフレームからの経過時間を加算します。
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= 1.0f) {
            // 経過時間が1秒を超えていたら、フレームレートを計算します。
            float fps = 1.0f * frameCount / elapsedTime;

            // 計算したフレームレートを画面に表示します。(小数点以下2ケタまで)
            string fpsRate = $"FPS: {fps.ToString("F2")}";
            fpsText.SetText(fpsRate);

            // フレームのカウントと経過時間を初期化します。
            frameCount = 0;
            elapsedTime = 0f;
        }
    }
}