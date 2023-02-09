using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Mode_for_myLoca : MonoBehaviour
{
    public bool soundType_Sonification = false;
    public bool soundType_Speech = false;
    public bool Feedback_mode_Continuous = false;
    public bool Feedback_mode_Discrete = false;
    public bool Feedback_mode_Periodic = false;
    public bool trigger_onRequest = false;
    public bool trigger_onMovement = false;
    public bool trigger_proximity = false;
    public bool trigger_collision = false;
    public bool concurrency_sequential = false;
    public bool cardinality_oneElement = false;
    public bool cardinality_twoElement = false;




    GameObject left;
    GameObject right;
    
    Oscillator_left Loscillator;
    Oscillator_right Roscillator;

    Speech_left Lspeech;
    Speech_right Rspeech;


    // Start is called before the first frame update
    void Start()
    {
        right = GameObject.Find("RightHand Controller");
        left = GameObject.Find("LeftHand Controller");
        


        if (soundType_Speech)
        {
            //delete 2 scripts from each controller(left, right)
            
            //Roscillator = right.GetComponent<Oscillator_right>();
            //Roscillator.gain = 0f;
            //Roscillator.prev_gain = 0f;
            //Roscillator.enabled = false;

            //Loscillator = left.GetComponent<Oscillator_left>();
            //Loscillator.enabled = false;


        }

        else if (soundType_Sonification)
        {
            Lspeech = left.GetComponent<Speech_left>();
            Lspeech.enabled = false;

            Rspeech = right.GetComponent<Speech_right>();
            Rspeech.enabled = false;    
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
