using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class background : MonoBehaviour
{
    public AudioClip punches;
    public AudioClip Success;

    public bool need_punches_l = true;
    public bool need_success_l;
    public bool need_punches_r = true;
    public bool need_success_r;
    AudioSource audiosource;

    // Start is called before the first frame update
    void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (need_success_l || need_success_r) 
        {
            audiosource.clip = Success;
            audiosource.Play();
            need_success_l = false;
            need_success_r = false;
        }
        else if (need_punches_l && need_punches_r)
        {
            if (!audiosource.isPlaying)
            {
                audiosource.clip = punches;
                audiosource.Play();
                Debug.Log("ÆÝÄ¡ Àç»ýÁß");
            }
            
            
        }
    }
}
