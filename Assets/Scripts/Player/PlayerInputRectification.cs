using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputRectification : MonoBehaviour
{
    private Vector2 MousePosition;
    [SerializeField]
    private float MaxAngle = 120f;
    [SerializeField]
    private float DeadZone = 45f;


    [SerializeField, Range (0, 1)]
    private float TurnSpeed = 0.9f;
    [HideInInspector]
    public float MouseAngle;
    public float TargetAngle;
    [HideInInspector]
    public bool Stopped;

    [HideInInspector]
    public Vector2 TargetVector;

    /**
    * This should not have any gameplay logic - that should be contained inside the SkiScript.
    * This script should just provide a target Vector & a target angle.
    */
    public void GetTargetVectorFromMouse(InputAction.CallbackContext context)
    {
        var previousAngle = TargetAngle;
        var absoluteMousePosition = (Vector2) Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
        var subjectPosition = (Vector2) this.transform.position;

        MousePosition = absoluteMousePosition - subjectPosition;

        MouseAngle = Vector2.SignedAngle(Vector2.down, MousePosition);

        if (Mathf.Abs(MouseAngle) > 180 - DeadZone)
        {
            Stopped = true;
        } else {
            Stopped = false;

            MouseAngle = Mathf.Clamp(MouseAngle, MaxAngle * -1, MaxAngle);
            
            TargetAngle = Mathf.Lerp(previousAngle, MouseAngle, Time.deltaTime * TurnSpeed);

            var targetAngleRad =  TargetAngle * Mathf.Deg2Rad;

            TargetVector = new Vector2(Mathf.Sin(targetAngleRad), Mathf.Cos(targetAngleRad) * -1);
        }
    }


}
