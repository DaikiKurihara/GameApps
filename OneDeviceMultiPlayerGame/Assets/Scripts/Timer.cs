using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public static float currentCountDownTime;
    public TextMeshProUGUI countDownText;
    [SerializeField] float countTime = 3.0F;

    void Start() {
        currentCountDownTime = countTime;    // カウントダウン開始秒数をセット
    }

    void Update() {
        // カウントダウンタイムを整形して表示
        countDownText.text = string.Format("{0:00}", currentCountDownTime);
        // 経過時刻を引いていく
        currentCountDownTime -= Time.deltaTime;
        if (currentCountDownTime <= 0.0F) {
            currentCountDownTime = 0.0F;
            // 0になった時の処理
        }

    }
}
