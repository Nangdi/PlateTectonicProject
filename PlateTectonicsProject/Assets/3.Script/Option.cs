using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option : MonoBehaviour
{
    [SerializeField]
    private LeapMouseCursor player1_cursor;

    [SerializeField]
    private LeapMouseCursor player2_cursor;

    public float mouseSensitivity = 0.5f;
    public float distanceThreshold = 0.003f;
    public float handHeight = 0.3f;
    public float handStability = 0.5f;
    public void SetMotionSensitivity()
    {

    }
}
