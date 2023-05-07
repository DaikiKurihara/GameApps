using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class ResultCanvasManager : MonoBehaviour {
    /// <summary>
    /// 結果表示行を各プレイヤーごとに生成して表示する
    /// </summary>
    /// <param name="playersResult"></param>
    public void openPlayerResults(List<(int fingerId, int playerNum, int rank, float diff)> playersResult) {
        foreach ((int fingerId, int playerNum, int rank, float diff) result in playersResult) {
            GameObject viewContent = GameObject.FindWithTag(CommonConstant.RESULT_VIEW_CONTENT);
            GameObject playerResult = Resources.Load<GameObject>(CommonConstant.PLAYER_RESULT);
            GameObject playerResultInstance = Instantiate(playerResult);
            TextMeshProUGUI textObj = playerResultInstance.GetComponent<TextMeshProUGUI>();
            textObj.text = $"<mspace=0.45em>{generateRankAbbreviation(result.rank)}:Player{result.playerNum}</mspace> {format(result.diff)}";
            playerResultInstance.transform.SetParent(viewContent.transform, false);
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
        if (rank < 0) {
            return "DQ ";
        } else if (rank == 1) {
            return "1st";
        } else if (rank == 2) {
            return "2nd";
        } else if (rank == 3) {
            return "3rd";
        } else {
            return $"{rank}th";
        }
    }

    private string format(float diff) {
        if (diff < 0) {
            return " --  ";
        } else {
            return $"+{diff.ToString("N2")}";
        }
    }
}
