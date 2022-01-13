using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class WendigoController : MonoBehaviour
{
    Transform playerTracking;
    [Header ("Hunt Trigger Distance")]
    public int huntTriggerMinDistance;
    [Header ("Hunt Near Distance")]
    public int huntNearPlayerMinDistance;
    float huntDistanceToPlayer;
    bool hasReachedPlayer;
    bool isNearPlayer;
    bool isNearHasTriggered;
    float time;
    float timeTotal;
WaitForSeconds waitOnHunt = new WaitForSeconds (0.1f);
   [Range(1f, 10f)] 
   public float huntSpeedModifier;
   IEnumerator resetRoutine;

   //Audio
       public static event Action<string> nearHuntGrowl; 
    void Start()
    {
        TrackPlayer();
    }

    private void ManageSprite()
    {
        
    }

    private void TrackPlayer()
    {
        try
        {
        
        playerTracking = GameObject.FindGameObjectWithTag("Player").transform;

        }
        catch (System.Exception)
        {
            Debug.LogError("No Player detected");
            throw;

        }
    }

    void Update()
    {
TimeCheck();
       if (!hasReachedPlayer) CheckForHuntDistance();
    }

    private void CheckForHuntDistance()
    {
        huntDistanceToPlayer = Vector2.Distance(playerTracking.position, this.transform.position);
        if ( huntDistanceToPlayer > huntTriggerMinDistance) StartHunt();
         isNearPlayer = huntDistanceToPlayer < huntNearPlayerMinDistance ? true : false;
         if ( huntDistanceToPlayer <= 0.5f) hasReachedPlayer = true;
    }

    private void StartHunt()
    {
        Debug.Log("Hunt Triggered");
        StartCoroutine(HuntPlayer());
    }

    IEnumerator HuntPlayer()
    {
        do 
        {
            transform.position = Vector3.Lerp(transform.position,playerTracking.position, time * HuntSpeed());
            if(isNearPlayer == true && !isNearHasTriggered) wendigoIsNearingPlayer() ;
            if(hasReachedPlayer == true) wendigoHasCaughtPlayer() ;
            yield return waitOnHunt;

}
        while (playerTracking); //Useless while do loop kinda

        yield return null;
    }
       private void wendigoIsNearingPlayer()
    {
        StartCoroutine(TriggerNearSoundFX());
   
    }
        IEnumerator TriggerNearSoundFX ()
        {
        isNearHasTriggered = true;
        Debug.Log("Trigger Sound");
        nearHuntGrowl ?.Invoke("growl");
        yield return waitOnHunt;
        }

    private void wendigoHasCaughtPlayer()
    {
       if (resetRoutine == null){
        resetRoutine = CaughtSceneReset();
        StartCoroutine(resetRoutine);
       }
    }

 
    IEnumerator CaughtSceneReset()
    {
        //FUN DEATH JUNK>>
        huntSpeedModifier = 0;
        this.transform.position = playerTracking.position + new Vector3 (0,-0.5f,0);
        this.transform.parent = playerTracking;

        Color colour = new Color();
        colour.r = 225f;
        this.GetComponent<SpriteRenderer>().color -= colour;

        //<<
        WaitForSeconds waitEnd = new WaitForSeconds (1f);
        yield return waitEnd;
        //play end fx 
        resetRoutine = null;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private float HuntSpeed()
    {
        var huntSpeedTimeMultiplier = timeTotal/100;
       var huntSpeedUpdated = huntSpeedModifier * huntSpeedTimeMultiplier;
       return huntSpeedUpdated;
    }

    void TimeCheck (){
        time = Time.smoothDeltaTime;
        timeTotal += Time.deltaTime;
        
    }
}
