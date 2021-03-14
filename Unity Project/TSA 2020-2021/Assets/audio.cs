using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audio : MonoBehaviour
{
    // Start is called before the first frame update
    public List<AudioClip> music = new List<AudioClip>();
    private AudioSource musicSource;
    private bool silent = false;
    private float tick = 0;
    private float lastSong = 3;
    void Start ()
    {
        musicSource = this.GetComponent<AudioSource>();
    }
    void Update()
    {
        if(musicSource.isPlaying == false && silent == false)
        {
            int song = Random.Range(0,6);
            if (song > 4){
                silent = true;
            }else{
                if(lastSong != song)
                {
                    musicSource.clip = music[song];
                    lastSong = song;
                    musicSource.Play();
                }else{
                    while(lastSong == song){
                        song = Random.Range(0,5);
                    }
                    musicSource.clip = music[song];
                    musicSource.Play();
                }
            }
        }
        if(silent == true){
            if (tick == 180)
            {
                silent = false;
                tick = 0;
            }else{
                tick += Time.deltaTime;
            }
        }
    }
}
