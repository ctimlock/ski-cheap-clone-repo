using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour
{
    //https://www.fesliyanstudios.com/sound-effects-search.php?q= ||Sound library


   public Sound [] sounds;
 

    public static AudioManager instance;
    // Start is called before the first frame update
    void Awake()
    {

        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

            WendigoController.nearHuntGrowl += PlaySound;
       

        //Add Trigger Sounds
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }
    private void Start()
    {
        PlaySound("startup01");
            //GameObject wendigo = GameObject.FindGameObjectWithTag("Respawn");
           // WendigoController huntGrowl = wendigo.GetComponent<WendigoController>();
            
            }

    //Find s Sound in Array and Play
    public void PlaySound (string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null) Debug.Log("Sound not found, named: " + s.name);

        s.source.Play();
    }
}
