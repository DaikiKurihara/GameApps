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
        this.animate();
        // 指に合わせて移動する
        foreach (Touch touch in Input.touches) {
            if (touch.fingerId == this.fingerId && !isLeft) {
                this.startPos = transform.position;
                this.worldPos = Camera.main.ScreenToWorldPoint(touch.position);
                // センターからの角度を求めて回転させる
                float degree = Mathf.Atan2(worldPos.y, worldPos.x) * Mathf.Rad2Deg;
                // 横向き固定画面のため、取得した角度+90度回転させた角度でZ軸を設定する
                this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90 + degree));
                transform.position = Vector3.Lerp(this.startPos, this.worldPos, 1);
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
        this.isLeft = true;
        this.GetComponent<Image>().color = ColorConstant.CIRCLE_FINISHED;
    }

    public void openResult(int rank, float diff) {
        // 非アクティブなオブジェクトはfindWithTagでは取得できないためtransformのメソッドを使う
        GameObject rankObj = transform.GetChild(1).gameObject;
        rankObj.SetActive(true);
        TextMeshProUGUI rankTxt = rankObj.GetComponent<TextMeshProUGUI>();
        // 小数点第２位で誤差表記
        rankTxt.text = $"{rank} : +{diff.ToString("N2")}";
    }

    /// <summary>
    /// 円オブジェクト生成時にアニメーションを付与する
    /// </summary>
    private void animate() {
        if (!isAnimationEnd) {
            // 約0.1秒かけて60大きくし、0.1秒で元の大きさまで戻す
            if (this.animationCount < 5) {
                Vector3 size = this.transform.localScale;
                size = new Vector3(this.transform.localScale.x + 12, this.transform.localScale.y + 12);
                this.transform.localScale = size;
            } else if (this.animationCount > 4 && this.animationCount < 10) {
                Vector3 size = this.transform.localScale;
                size = new Vector3(this.transform.localScale.x - 12, this.transform.localScale.y - 12);
                this.transform.localScale = size;
            } else {
                this.isAnimationEnd = true;
            }
            this.animationCount++;
        }

    }
}