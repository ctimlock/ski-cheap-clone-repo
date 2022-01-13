using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkiScript : MonoBehaviour
{
    [SerializeField]
    private float MaxSpeed = 10f;
    private float MinSpeed = 0.05f;
    [SerializeField]
    private float Acceleration = 1f;
    private float SpeedRamp = 0.66f;
    private float CurrentSpeed;
    private float TargetSpeed;
    private float Momentum = 0.5f;
    private Vector2 CurrentLocation;
    private Vector2 TargetLocation;
    [SerializeField]
    private PlayerInputRectification input;

    public void Start()
    {
        TargetLocation = Vector2.zero;
        CurrentLocation = this.transform.position;
        CurrentSpeed = 0;
        TargetSpeed = 0;
    }

    public void FixedUpdate()
    {
        MovePlayer();
    }

    public void MovePlayer()
    {
        CurrentLocation = this.transform.position;
        var previousTarget = TargetLocation;

        if (input.Stopped == true)
        {
            TargetSpeed = 0;
        } else {
            TargetSpeed = (Mathf.Abs(input.MouseAngle) * -1 + 90) / 90;

            if (TargetSpeed < MinSpeed)
            {
                TargetSpeed = 0;
            }

            TargetSpeed = Mathf.Pow(TargetSpeed, SpeedRamp) * MaxSpeed;
        }
        
        CurrentSpeed = Mathf.Lerp(CurrentSpeed, TargetSpeed, Time.deltaTime * Acceleration);

        if (CurrentSpeed < MinSpeed)
        {
            CurrentSpeed = 0;
        }

        TargetLocation = (input.TargetVector * CurrentSpeed) + CurrentLocation;

        var SmoothedLocation = Vector2.Lerp(previousTarget, TargetLocation, Time.deltaTime * Momentum);

        this.transform.position = Vector2.Lerp(CurrentLocation, SmoothedLocation, Time.deltaTime);
    }
}
