using System.Threading.Tasks;
using UnityEngine;

public class TouchArea : MonoBehaviour {
    /** 現在タッチしている指の本数 */
    private int _touchCount = 0;
    /** GameManager */
    private GameManager gameManager;
    /** CanvasManager */
    private CanvasManager canvasManager;
    private PhysicalLayerManager physicalLayerManager;

    void Start() {
        gameManager = GameObject.FindWithTag(CommonConstant.GAME_MANAGER).GetComponent<GameManager>();
        canvasManager = GameObject.FindWithTag(CommonConstant.CANVAS_MANAGER).GetComponent<CanvasManager>();
        physicalLayerManager = GameObject.FindWithTag(CommonConstant.PHYSICAL_LAYER_MANAGER).GetComponent<PhysicalLayerManager>();
    }

    // Update is called once per frame
    void Update() {
        if (!gameManager.IsGameStart) {
            //----------------ゲーム開始前のプレイヤースタンバイ時間------------------------
            // 触れている指が2本以上でカウントダウン開始
            gameManager.IsCountDownStart = Input.touchCount >= 2;

            touchBegan();

            touchEnded();
            //----------------ゲーム開始前のプレイヤースタンバイ時間------------------------
        } else {
            //----------------ゲーム開始後のプレイ時間-----------------------------------
            touchFinished();
            if (Input.touchCount == 0 && !gameManager.IsGameEnd) {
                gameManager.IsGameEnd = true;
            }
            //----------------ゲーム開始後のプレイ時間-----------------------------------
        }
        // iPhoneではタップの限界を超えた場合にtouchCountが0になるのでプレイヤー人数をチェックする
        if (_touchCount != 0 && Input.touchCount == 0) {
            gameManager.checkPlayerCount();
        }

        _touchCount = Input.touchCount;
    }

    private void touchBegan() {
        foreach (Touch touch in Input.touches) {
            if (touch.phase == TouchPhase.Began) {
                physicalLayerManager.onTouch();
                canvasManager.generateTouchAreaCircle(touch.position, touch.fingerId);
            }
        }
    }

    /// <summary>
    /// 指を離した瞬間を検知する
    /// </summary>
    /// <param name="touch"></param>
    private void touchEnded() {
        if (gameManager.IsGameStart) {
            return;
        }
        foreach (Touch touch in Input.touches) {
            if (touch.phase == TouchPhase.Ended) {
                canvasManager.destroyTouchAreaCircle(touch.fingerId);
            }
        }
    }

    /// <summary>
    /// ゲーム開始後に指が離れた場合
    /// </summary>
    private void touchFinished() {
        if (!gameManager.IsGameStart || gameManager.IsGameEnd) {
            return;
        }
        foreach (Touch touch in Input.touches) {
            if (touch.phase == TouchPhase.Ended) {
                if (!canvasManager.PlayerIds.Contains(touch.fingerId) || canvasManager.FinishedPlayerIds.Contains(touch.fingerId)) {
                    // 画面上にいるプレイヤーではないもしくはすでにタッチ終了したプレイヤーの場合処理終了
                    return;
                }
                physicalLayerManager.onLeftTouch();
                canvasManager.touchFinished(touch.fingerId);
            }
        }
    }
}
