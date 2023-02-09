using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;



public class Punch_success : MonoBehaviour
{


    GameObject LeftHand;
    GameObject RightHand;
    List<int> queue = new List<int>(); // int 값을 저장하는 큐

    Mode_for_myLoca mode_for_myloca;
    //MainCamera main_camera;
    GameObject mainCamera;
    XRBaseController controller;
    XRController xr_l;
    XRController xr_r;
    AudioSource audiosource;
    ONSPAudioSource OnspAudioSource_r;
    ONSPAudioSource OnspAudioSource_l;
    Oscillator_right oscillator_r;
    Oscillator_left oscillator_l;

    Speech_right speech_right;
    Speech_left speech_left;


    int queue_first;

    // Start is called before the first frame update
    void Start()
    {
        
        
        LeftHand = GameObject.Find("LeftHand Controller");
        xr_l = LeftHand.GetComponent<XRController>();
        RightHand = GameObject.Find("RightHand Controller");
        xr_r = RightHand.GetComponent<XRController>();

        mode_for_myloca = GameObject.Find("GameManager").GetComponent<Mode_for_myLoca>();
        mainCamera = GameObject.Find("Main Camera");

        audiosource = this.GetComponent<AudioSource>();

        if (mode_for_myloca.soundType_Sonification)
        {
            oscillator_r = RightHand.GetComponent<Oscillator_right>();
            oscillator_l = LeftHand.GetComponent <Oscillator_left>();
        }
        else if (mode_for_myloca.soundType_Speech)
        {
            speech_right = RightHand.GetComponent<Speech_right>();
            speech_left = LeftHand.GetComponent<Speech_left>();
        }


    }

   

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(mainCamera.transform.position.x+0.4f, mainCamera.transform.position.y, mainCamera.transform.position.z+0.4f);
        this.transform.localEulerAngles = new Vector3(0f, 45f, 0f);

        if (oscillator_r != null) // 충돌할 때도 소리를 들리게 하려면 이 if 문 없애면 됨
        {
            if (audiosource.isPlaying)
            {
                oscillator_r.enabled = false;
                oscillator_l.enabled = false;
            }
            else
            {
                oscillator_r.enabled = true;
                oscillator_l.enabled = true;
            }
        }

        if (speech_left != null)
        {
            if (audiosource.isPlaying)
            {
                speech_left.enabled = false;
                speech_right.enabled = false;
            }
            else
            {
                speech_left.enabled = true;
                speech_right.enabled = true;
            }
        }

        

        
    }

    

    void OnCollisionEnter(Collision collision) // collision이 들어오면
    {
        
        if (mode_for_myloca.trigger_collision) // collision 모드가 켜져 있을 때
        {
            if (collision.gameObject.name == "LeftHand Controller")
            {
                xr_l.SendHapticImpulse(0.7f, 1f);
                queue.Add(1);
            }

            else if (collision.gameObject.name == "RightHand Controller")
            {
                xr_r.SendHapticImpulse(0.7f, 1f);
                queue.Add(2);
            }
        }

        
        

        if (mode_for_myloca.concurrency_sequential) // sequential 모드가 켜져 있을 때
        {
            


            if ((audiosource.isPlaying == false)&&(mode_for_myloca.cardinality_oneElement))
            {
                
                audiosource.Play();
                queue.Clear();
                queue.Clear();
                
            }

            else if ((audiosource.isPlaying == false) && (mode_for_myloca.cardinality_twoElement)&&(queue.Count>1))
            {
                audiosource.Play();
                queue.Clear();
                queue.Clear();

                Invoke("second_audio_play", 0.3f); // 동시에 접촉한 뒤 잠시 그 상태 유지하기
            }
            else if ((audiosource.isPlaying == false) && (mode_for_myloca.cardinality_twoElement) && (queue.Count == 1))
            {
                audiosource.Play();
                queue.Clear();
                queue.Clear();

            }
            
            
            



        }

        


    }

    void second_audio_play()
    {
        audiosource.Play();
        queue.Clear();
    }


}
