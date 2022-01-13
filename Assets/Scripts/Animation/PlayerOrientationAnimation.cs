using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOrientationAnimation : MonoBehaviour
{
    //Turning
   [SerializeField] PlayerInputRectification TargetAngleScript;
    //TO DO Generate this value to at run time on script to avoid step checks
   [SerializeField] Sprite [] playerTurningSpiteList;
    //TO DO Add in spirtes to array in Unity, 30x/y, 60x/y, regular.
    int spriteCount;

    //Jumping
    [SerializeField] Sprite playerJumpSpite;
   public float playerJumpDuration;

   //Crashing
    [SerializeField] Sprite playerCrashSpite;
    [SerializeField] float playerCrashDuration;

    SpriteRenderer playerSpriteRenderer;
    int movementStateIndex = 1;
    void Start()
    {
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        spriteCount = playerTurningSpiteList.Length;
    }

    void Update()
    {


         MovementState();
    }

    void MovementState()
    {
        switch (movementStateIndex)
        {
            case 1:
                playerSpriteRenderer.sprite = UpdateTurningSprite(AngleToIndex(TargetAngleScript.TargetAngle));
                break;
            case 2:
                 UpdateJump();
                break;
            case 3:
                UpdateCrash();
                break;
            case 4:
                UpdateJump();
                break;
            default:
                break;

                //TO DO Generate Switch Trigger or State Machine

        }
    }
    int AngleToIndex (float targetAngle)
    {
        int rangeOfAngle = 180 / (spriteCount);
        if (rangeOfAngle > 180 || rangeOfAngle < 0) rangeOfAngle = 180;
        float targetAngleAsPositive;
        targetAngleAsPositive  = targetAngle < 0  ? (91 - (targetAngle * -1)) : (targetAngle + 89);
        int index = Mathf.FloorToInt(targetAngleAsPositive / rangeOfAngle);
        if (index < 0) index = 0;
        if (index > spriteCount - 1) index = spriteCount - 1;
        return index ;
    }

    Sprite UpdateTurningSprite(int index) 
    {
        return playerTurningSpiteList[index];
    }

    void UpdateJump()
    {
      playerJumpDuration = 1f;
      playerSpriteRenderer.sprite = playerJumpSpite;
         Debug.Log("JUMP");

    }
    void UpdateCrash()
    {
        playerCrashDuration = 1f;
playerSpriteRenderer.sprite = playerCrashSpite;
        Debug.Log("CRASH");

    }
    void UpdateEaten()
    {
        //Eaten Animation?
    }
}