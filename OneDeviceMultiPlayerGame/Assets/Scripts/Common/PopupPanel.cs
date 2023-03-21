using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupPanel : MonoBehaviour {
    public void dismissParent() {
        transform.localScale = new Vector3(1, 1);
        transform.parent.gameObject.SetActive(false);
    }
}
