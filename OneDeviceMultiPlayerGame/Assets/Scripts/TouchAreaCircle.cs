using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TouchAreaCircle : MonoBehaviour {
    private int fingerId { get; set; }
    private bool isLeft = false;
    private int playerNumber = -1;
    private int animationCount = 0;
    private bool isAnimationEnd = false;

    private Vector2 startPos;
    private Vector2 worldPos;

    /// <summary>
    /// fingerIDを設定したプレハブを作成する
    /// </summary>
    /// <param name="fingerId"></param>
    /// <returns> プレハブのインスタンス </returns>
    public static GameObject Init(int fingerId) {
        GameObject touchAreaCirclePrefab = Resources.Load<GameObject>(CommonConstant.TOUCH_AREA_CIRCLE);
        GameObject touchAreaCirclePrefabInsatance = Instantiate(touchAreaCirclePrefab);
        // 新規タグの追加
        string tag = CommonConstant.FINGER_ID + fingerId.ToString();
        touchAreaCirclePrefabInsatance.tag = tag;
        TouchAreaCircle TouchAreaCircle = touchAreaCirclePrefabInsatance.GetComponent<TouchAreaCircle>();
        TouchAreaCircle.fingerId = fingerId;
        return touchAreaCirclePrefabInsatance;
    }

    void Update() {
        animate();
        // 指に合わせて移動する
        foreach (Touch touch in Input.touches) {
            if (touch.fingerId == fingerId && !isLeft) {
                startPos = transform.position;
                worldPos = Camera.main.ScreenToWorldPoint(touch.position);
                // センターからの角度を求めて回転させる
                float degree = Mathf.Atan2(worldPos.y, worldPos.x) * Mathf.Rad2Deg;
                // 横向き固定画面のため、取得した角度+90度回転させた角度でZ軸を設定する
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90 + degree));
                transform.position = Vector3.Lerp(startPos, worldPos, 1);
            }
        }
    }

    public void dicidePlayerNumber(int playerNumber) {
        this.playerNumber = playerNumber;
        // 非アクティブなオブジェクトはfindWithTagでは取得できないためtransformのメソッドを使う
        GameObject textObj = transform.GetChild(0).gameObject;
        // デフォルトはfalseになっている
        textObj.SetActive(true);
        TextMeshProUGUI playerNumTxt = textObj.GetComponent<TextMeshProUGUI>();
        this.playerNumber = playerNumber;
        playerNumTxt.text = $"Player{playerNumber}";
    }

    public void left() {
        isLeft = true;
        GetComponent<Image>().color = ColorConstant.CIRCLE_FINISHED;
    }

    /// <summary>
    /// 円オブジェクト生成時にアニメーションを付与する
    /// </summary>
    private void animate() {
        if (!isAnimationEnd) {
            // 約0.1秒かけて60大きくし、0.1秒で元の大きさまで戻す
            if (animationCount < 5) {
                Vector2 sd = GetComponent<RectTransform>().sizeDelta;
                sd = new Vector2(sd.x + 12, sd.y + 12);
                GetComponent<RectTransform>().sizeDelta = sd;
            } else if (animationCount > 4 && animationCount < 10) {
                Vector2 sd = GetComponent<RectTransform>().sizeDelta;
                sd = new Vector2(sd.x - 12, sd.y - 12);
                GetComponent<RectTransform>().sizeDelta = sd;
            } else {
                isAnimationEnd = true;
            }
            animationCount++;
        }

    }
}