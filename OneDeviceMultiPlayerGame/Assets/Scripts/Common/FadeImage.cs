using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeImage : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }

    public void fadeScene() {
        GetComponent<Animator>().enabled = true;
    }
}
