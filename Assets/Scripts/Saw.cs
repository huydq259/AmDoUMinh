using UnityEngine;

public class SawMover : MonoBehaviour {

    public Transform[] points; // Assign in Inspector
    public float moveTime = 1.5f; // Time to move between points

    private int currentPointIndex = 0;

    void Start() {
        MoveToNextPoint();
    }

    void MoveToNextPoint() {
        currentPointIndex++;
        if (currentPointIndex >= points.Length) {
            currentPointIndex = 0;
        }

        LeanTween.move(this.gameObject,points[currentPointIndex].position, moveTime).setEaseOutBack().setOnComplete(MoveToNextPoint);
    }
}

