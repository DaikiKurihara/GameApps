using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchMoveTest : MonoBehaviour {
    Vector2 startPos;
    Vector2 screenPos;
    Vector2 worldPos;

    void Update() {
        if (Input.GetMouseButton(0)) {
            startPos = transform.position;
            screenPos = Input.mousePosition;
            worldPos = Camera.main.ScreenToWorldPoint(screenPos);

            transform.position = Vector3.Lerp(startPos, worldPos, 1);
        }
    }
}