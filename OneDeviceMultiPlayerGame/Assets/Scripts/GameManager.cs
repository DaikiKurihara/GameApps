using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class GameManager : SingletonMonoBehaviour<GameManager> {

    /** GameManager */
    private CanvasManager _canvasManager;

    // 準備画面突入有無のフラグ
    private bool isCountDownStart = false;
    public bool IsCountDownStart {
        set { this.isCountDownStart = value; }
        get { return this.isCountDownStart; }
    }

    // 準備時間が終わって全プレイヤーの指がついている状態
    private bool isGameStart = false;
    public bool IsGameStart {
        // falseは代入不可
        set {
            this.isGameStart = true;
            this.gameStart();
        }
        get { return this.isGameStart; }
    }
    // ゲーム終了（ゲーム開始→ストップタイム→指が全部離れた状態）フラグ
    private bool isGameEnd = false;
    public bool IsGameEnd {
        set {
            this.isGameEnd = true;
            decideEndTime();
        } // falseは代入不可
        get { return this.isGameEnd; }
    }
    // 指を離す時間になったか
    private bool isStopped = false;
    public bool IsStopped {
        set { this.isStopped = true; } // falseは代入不可
        get { return this.isStopped; }
    }

    // 結果表示
    private bool openResult = false;

    /** ゲーム開始からの経過時間 */
    private float passedTime { get; set; } = 0.0F;
    /** 指を離す指定時間 */
    private float standardTime { get; set; } = 0.0F;
    /** 指を離す指定時間 */
    private float endTime { get; set; } = 0.0F;
    /** 指を離した時間を格納する辞書 */
    private Dictionary<int, float> leftTimeMap = new Dictionary<int, float>();

    public void Awake() {
        if (this != Instance) {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        _canvasManager = GameObject.FindWithTag("CanvasManager").GetComponent<CanvasManager>();
    }

    void Update() {
        if (this.isGameStart) {
            this.passedTime += Time.deltaTime;
        }

        // 指が離れてから3秒後に結果を出す
        if (this.openResult && this.passedTime - this.endTime > 3.0) {
            Debug.Log("結果発表" + string.Join(",", this.leftTimeMap.ToArray()));
            this.openResult = false;
        }
    }

    /// <summary>
    /// ゲームスタート（（準備時間が終わって全プレイヤーの指がついている状態）に走る処理
    /// </summary>
    private void gameStart() {
        this._canvasManager.dicidePlayerNumbers();
    }

    /// <summary>
    /// fingerIdを受け取り、fingerIdをキーにした経過時間のセットを作る
    /// </summary>
    /// <param name="fingerId"></param>
    public void addLeftTimeMap(int fingerId) {
        Debug.Log(fingerId + "のマップを作ります。" + "差分は" + (this.passedTime - this.standardTime));
        this.leftTimeMap.Add(fingerId, this.passedTime - this.standardTime);
    }

    /// <summary>
    /// 指を離すべき基準時間を決定する
    /// </summary>
    public void decideStandardTime(float standardTime) {
        this.standardTime = standardTime;
    }

    /// <summary>
    /// 全員の指が離れた時間を決定する
    /// </summary>
    private void decideEndTime() {
        this.endTime = this.passedTime;
        this.openResult = true;
    }
}
