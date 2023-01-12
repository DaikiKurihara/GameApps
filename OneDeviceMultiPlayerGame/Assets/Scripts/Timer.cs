using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour {

    public static float currentCountDownTime;
    private float _currentCountUpTime = 0.0F;
    public TextMeshProUGUI countDownText;
    [SerializeField] float countTime = 5.0F;
    private GameManager _gameManager;
    private bool _isStarted;
    /** 指を離す時間（最大）の指定 */
    [SerializeField] int maxLeavingTime = 20;
    private bool _leaveFingerCounted = false;
    private float _leavingTime = 0.0F;


    void Start() {
        _gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        // カウントダウン開始秒数をセット
        currentCountDownTime = countTime;
    }

    void Update() {
        // カウントダウン
        if (!this._gameManager.IsGameStart) {
            // 経過時刻を引いていく
            currentCountDownTime -= Time.deltaTime;
            // 残り5.00秒で表示「05」残り4.00秒で表示「04」としたいため。+1秒しないと4.99秒で表示「04」となり感覚とズレる
            this.countDownText.text = string.Format("{0:00.00}", currentCountDownTime + 1.0);
            if (currentCountDownTime <= 0.000F) {
                currentCountDownTime = 0.00F;
                this.gameStart();
                this.countDownText.text = "";
                this._leavingTime = createLeaveFingerTime();
                Debug.Log("離す時間は：" + this._leavingTime);
            }
        }


        // 離す時間までのカウントアップ
        if (this._gameManager.IsGameStart && !_leaveFingerCounted) {
            _currentCountUpTime += Time.deltaTime;
            this.countDownText.text = string.Format("{0:00.00}", _currentCountUpTime - 1.0);

            if (this._currentCountUpTime >= this._leavingTime) {
                Debug.Log("離せ！");
                this.stop();
                this._leaveFingerCounted = true;
                this.countDownText.text = " Leave!!!!!";
            }
        }
    }

    /// <summary>
    /// ゲーム開始の通知
    /// </summary>
    private void gameStart() {
        this._gameManager.IsGameStart = true;
    }

    /// <summary>
    /// 指を離す時間の通知
    /// </summary>
    private void stop() {
        this._gameManager.IsStopped = true;
    }

    /// <summary>
    /// 指を離す指定時間を生成する
    /// </summary>
    private float createLeaveFingerTime() {

        // 0.5秒単位で生成する
        float leavingTime = (Random.Range(3 * 2, maxLeavingTime * 2)) / 2;
        // ゲームマネージャーに離す時間確定を通知
        this._gameManager.decideStandardTime(leavingTime);
        return leavingTime;
    }
}
