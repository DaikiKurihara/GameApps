using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour {

    /** GameManager */
    private CanvasManager _canvasManager;
    [SerializeField] private GameObject resultCanvas;

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

    // 現在タッチしているプレイヤーの数
    private int touchingPlayerCount = 0;
    // ゲーム開始時のプレイヤー数
    private int playerCount = 0;

    // 結果表示
    private bool isOpenResult = false;

    /** ゲーム開始からの経過時間 */
    private float passedTime { get; set; } = 0.0F;
    /** 指を離す指定時間 */
    private float standardTime { get; set; } = 0.0F;
    public float StandardTime {
        get { return this.standardTime; }
    }
    /** 指を離す最大時間 */
    private float maxTime;
    public float MaxTime {
        get {
            if (maxTime == 0) {
                return 15;
            }
            return maxTime;
        }
    }
    /** ビビらせバイブ有無 */
    [NonSerialized] public bool isOnVibration;
    /** ビビらせ音有無 */
    [NonSerialized] public bool isOnFeintSound;
    /** ゲームが終了した時間 */
    private float endTime { get; set; } = 0.0F;
    /** 指を離した時間を格納する辞書 */
    private Dictionary<int, float> leftTimeMap = new Dictionary<int, float>();
    /** フライングしたfingerIDリスト */
    private List<int> falseStartedList = new List<int>();
    /** fingerIDをキーにプレイヤーNoを格納する辞書 */
    private Dictionary<int, int> playerNumberMap = new Dictionary<int, int>();
    /** ビビらす時間 */
    private List<float> surpriseTimes = new List<float>();
    public List<float> SurpriseTimes {
        get { return surpriseTimes; }
    }
    /** <プレイヤーNo, <ランク, 秒差>> */
    private List<(int fingerId, int playerNum, int rank, float diff)> playersResult = new List<(int fingerId, int playerNum, int rank, float diff)>();
    public List<(int fingerId, int playerNum, int rank, float diff)> PlayersResult {
        get { return playersResult; }
    }

    void Start() {
        _canvasManager = GameObject.FindWithTag("CanvasManager").GetComponent<CanvasManager>();
    }

    void Update() {
        if (this.isGameStart) {
            this.passedTime += Time.deltaTime;
        }

        // 指が離れてから3秒後に結果を出す
        if (this.isOpenResult && this.passedTime - this.endTime > 3.0) {
            openResult();
            this.isOpenResult = false;
        }
    }

    /// <summary>
    /// ゲームスタート（（準備時間が終わって全プレイヤーの指がついている状態）に走る処理
    /// </summary>
    private void gameStart() {
        this.playerCount = Input.touchCount;
        this._canvasManager.dicidePlayerNumbers();
    }

    /// <summary>
    /// fingerIdを受け取り、fingerIdをキーにした経過時間のセットを作る
    /// </summary>
    /// <param name="fingerId"></param>
    public void fingerLeft(int fingerId) {
        this.leftTimeMap.Add(fingerId, this.passedTime - this.standardTime);
        if (!this.isStopped) {
            // 指を離す時間になっていない場合、フライングしたプレイヤーとして格納しておく
            falseStartPlayer(fingerId);
        }
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
        this.isOpenResult = true;
    }

    public void increaseTouchingPlayerCount() {
        this.touchingPlayerCount++;
    }

    public void decreaseTouchingPlayerCount() {
        if (touchingPlayerCount > 0) {
            this.touchingPlayerCount--;
        }
    }

    public void setMaxTime(string value) {
        int i;
        if (int.TryParse(value, out i)) {
            i = i == 0 ? int.Parse(MaxTimeMapConstant.DEFAULT_MAX_TIME) : i;
            Debug.Log($"変換成功{i}");
            maxTime = i;
        } else {
            maxTime = int.Parse(MaxTimeMapConstant.DEFAULT_MAX_TIME);
            Debug.Log($"変換失敗{maxTime}");
        }
    }

    /// <summary>
    /// フライングしたプレイヤーを格納
    /// </summary>
    /// <param name="fingerId"></param>
    private void falseStartPlayer(int fingerId) {
        this.falseStartedList.Add(fingerId);
        if (this.falseStartedList.Count == this.playerCount) {
            this.gameReset();
        }
    }

    public void checkPlayerCount() {
        if (Input.touchCount != this.touchingPlayerCount) {
            Debug.Log($"プレイヤー人数に異常が発生しました。{this.touchingPlayerCount}");
            this.gameReset();
        }
    }

    public void dicidePlayerNumber(int fingerId, int playerNum) {
        this.playerNumberMap.Add(fingerId, playerNum);
    }

    private void openResult() {
        // フライングしたプレイヤーを除いたディクショナリをクローンする
        var cloneDict = new Dictionary<int, float>(this.leftTimeMap).
            Where(x => x.Value > 0).
            ToDictionary(d => d.Key, d => d.Value);
        // 話した時間の絶対値を取る
        List<int> keys = new List<int>(cloneDict.Keys);
        foreach (int k in keys) {
            cloneDict[k] = Math.Abs(cloneDict[k]);
        }
        // LINQを使ってValueでソート
        var sortedList = cloneDict.OrderBy(x => x.Value).ToList();
        // 結果の表示（Debug）
        int i = 1;
        int rank = 0;
        float previousPlayerTime = -1;
        foreach (KeyValuePair<int, float> kvp in sortedList) {
            if (previousPlayerTime != kvp.Value) {
                rank = i;
            }
            Debug.Log(string.Format("{0}位: 差:{1:0.00}, プレイヤー {2}", rank, kvp.Value, playerNumberMap[kvp.Key]));
            playersResult.Add((kvp.Key, playerNumberMap[kvp.Key], rank, kvp.Value));
            i++;
            previousPlayerTime = kvp.Value;
        }
        foreach (int fingerId in falseStartedList) {
            Debug.Log(string.Format("失格者:Player{0}", playerNumberMap[fingerId]));
            playersResult.Add((fingerId, playerNumberMap[fingerId], -1, -1));
        }
        _canvasManager.openResult(playersResult);
        resultCanvas.SetActive(true);
        resultCanvas.GetComponent<ResultCanvasManager>().openPlayerResults(playersResult);
    }


    private void gameReset() {
        this.isCountDownStart = false;
        this.isGameEnd = false;
        this.isGameStart = false;
        this.isStopped = false;
        this.playerCount = 0;
        this.leftTimeMap.Clear();
        this.playerNumberMap.Clear();
        this.falseStartedList.Clear();
        this.surpriseTimes.Clear();
        this._canvasManager.resetCanvas();
        this._canvasManager.turnLeftLightDefault();
        this.playersResult.Clear();
        this.touchingPlayerCount = 0;
    }

    /// <summary>
    /// ゲーム終了後に再開する
    /// </summary>
    public void gameRetry() {
        gameReset();
        this.isOpenResult = false; // リセットとの相違点
        this.resultCanvas.GetComponent<ResultCanvasManager>().gemeRetry();// リセットとの相違点
    }
}
