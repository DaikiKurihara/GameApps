using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class ResultCanvasManager : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }

    /// <summary>
    /// 結果表示行を各プレイヤーごとに生成して表示する
    /// </summary>
    /// <param name="playersResult"></param>
    public void openPlayerResults(List<(int fingerId, int playerNum, int rank, float diff)> playersResult) {
        foreach ((int fingerId, int playerNum, int rank, float diff) result in playersResult) {
            GameObject viewContent = GameObject.FindWithTag(CommonConstant.RESULT_VIEW_CONTENT);
            GameObject playerResult = Resources.Load<GameObject>(CommonConstant.PLAYER_RESULT);
            GameObject playerResultInstance = Instantiate(playerResult);
            playerResultInstance.transform.SetParent(viewContent.transform, false);
            TextMeshProUGUI textObj = playerResultInstance.GetComponent<TextMeshProUGUI>();
            textObj.text = $"{generateRankAbbreviation(result.rank)} : {result.playerNum} +{result.diff}";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void gemeRetry() {
        GameObject viewContent = GameObject.FindWithTag(CommonConstant.RESULT_VIEW_CONTENT);
        foreach (Transform n in viewContent.transform) {
            Destroy(n.gameObject);
        }
        gameObject.SetActive(false);
    }

    private string generateRankAbbreviation(int rank) {
        if (rank == 1) {
            return "1st";
        } else if (rank == 2) {
            return "2nd";
        } else if (rank == 3) {
            return "3rd";
        } else {
            return $"{rank}th";
        }
    }
}
