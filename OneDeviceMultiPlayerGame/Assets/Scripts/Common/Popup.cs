using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour {
    /// <summary>
    /// 
    /// </summary>
    public void dismissChildPanel() {
        Animator animator = transform.GetChild(0).GetComponent<Animator>();
        animator.SetTrigger("Dismiss");
    }
}
