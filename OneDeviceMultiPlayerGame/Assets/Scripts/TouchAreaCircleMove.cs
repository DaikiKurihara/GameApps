
using UnityEngine;
public class TouchAreaCircleMove : MonoBehaviour {
    public int fingerId { get; set; }

    Vector2 startPos;
    Vector2 worldPos;

    public static GameObject Init(int fingerId) {
        GameObject touchAreaCirclePrefab = Resources.Load<GameObject>("TouchAreaCircle");
        GameObject touchAreaCirclePrefabInsatance = Instantiate(touchAreaCirclePrefab);
        touchAreaCirclePrefabInsatance.tag = "FingerID" + fingerId.ToString();
        TouchAreaCircleMove touchAreaCircleMove = touchAreaCirclePrefabInsatance.GetComponent<TouchAreaCircleMove>();
        touchAreaCircleMove.fingerId = fingerId;
        return touchAreaCirclePrefabInsatance;
    }

    void Update() {
        foreach (Touch touch in Input.touches) {
            if (touch.fingerId == this.fingerId) {
                Debug.Log("動いてる指のfingerIdは：" + fingerId);
                this.startPos = transform.position;
                this.worldPos = Camera.main.ScreenToWorldPoint(touch.position);
                transform.position = Vector3.Lerp(this.startPos, this.worldPos, 1);
            }
        }
    }
}