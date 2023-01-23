
using TMPro;
using UnityEngine;
public class TouchAreaCircle : MonoBehaviour {
    private int fingerId { get; set; }
    private int playerNumber { get; set; }

    private Vector2 startPos;
    private Vector2 worldPos;
    private Vector2 center = new Vector2(0, 0);

    /// <summary>
    /// fingerIDを設定したプレハブを作成する
    /// </summary>
    /// <param name="fingerId"></param>
    /// <returns> プレハブのインスタンス </returns>
    public static GameObject Init(int fingerId) {
        GameObject touchAreaCirclePrefab = Resources.Load<GameObject>("TouchAreaCircle");
        GameObject touchAreaCirclePrefabInsatance = Instantiate(touchAreaCirclePrefab);
        // タグの付与
        // fingerIDは最大5個想定なので、あらかじめインスペクタから決め打ちで追加している
        touchAreaCirclePrefabInsatance.tag = CommonConstant.FINGER_ID + fingerId.ToString();
        TouchAreaCircle TouchAreaCircle = touchAreaCirclePrefabInsatance.GetComponent<TouchAreaCircle>();
        TouchAreaCircle.fingerId = fingerId;
        return touchAreaCirclePrefabInsatance;
    }

    void Update() {
        // 指に合わせて移動する
        foreach (Touch touch in Input.touches) {
            if (touch.fingerId == this.fingerId) {
                Debug.Log("動いてる指のfingerIdは：" + fingerId);
                this.startPos = transform.position;
                this.worldPos = Camera.main.ScreenToWorldPoint(touch.position);
                // センターからの角度を求めて回転させる
                float degree = Mathf.Atan2(worldPos.y, worldPos.x) * Mathf.Rad2Deg;
                // 横向き固定画面のため、取得した角度+90度回転させた角度でZ軸を設定する
                this.GetComponent<RectTransform>().rotation = Quaternion.Euler(new Vector3(0, 0, 90 + degree));
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
        playerNumTxt.text = "Player" + playerNumber.ToString();
    }
}