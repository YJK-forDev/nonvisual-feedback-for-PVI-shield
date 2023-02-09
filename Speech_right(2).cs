using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Timers;

public class Speech_right : MonoBehaviour
{
    public Transform movementSource_right;

    Vector3 current_position = new Vector3(0, 0, 0);
    Vector3 past_position = new Vector3(0, 0, 0);
    Vector3 sub_position = new Vector3(0, 0, 0);

    float x_changed;
    float y_changed;
    float z_changed;

    float max;
    string which_speech;

    AudioSource speech_audiosource;
    //ONSPAudioSource onsp_audiosource;
    Mode_for_myLoca mode;
    MovementRecognizer_right result;
    public AudioClip Left;
    public AudioClip Up;
    public AudioClip Front;
    int played = 0;

    int up_flag = 0;


    // Start is called before the first frame update
    void Start()
    {
        speech_audiosource = GetComponent<AudioSource>();
        //onsp_audiosource = GetComponent<ONSPAudioSource>();
        //onsp_audiosource.enabled = false;
        //speech_audiosource.spatialBlend = 0;
        speech_audiosource.Pause();
        speech_audiosource.pitch = 1.2f;
        //speech_audiosource.dopplerLevel = 0f;
        //speech_audiosource.bypassEffects = false;
        mode = GameObject.Find("GameManager").GetComponent<Mode_for_myLoca>();
        result = GameObject.Find("Movement Recognizer_right").GetComponent<MovementRecognizer_right>();

        if (mode.Feedback_mode_Periodic) //periodic 모드이면 0.2초후부터 0.7초마다 함수를 호출함
        {
            InvokeRepeating("periodic_", 0.2f, 1f);
        }
        




    }

    void periodic_()
    {
        Debug.Log("0.7초마다 호출됩니다.");
        up_flag = 0; // up은 짧아서 한번호출될때 여러번 작동할 수 있음. 그래서 한번만 말할 수 있도록 up_flag를 따로 썼음. 
        // 0.7초마다 up_flag는 0으로 초기화됨

        if (mode.trigger_onRequest) // request 모드일 때 
        {

            if ((result.requestOn) && (result.punchStart)) // request 버튼과 펀치 버튼이 눌려있다면
            {
                

                if (speech_audiosource.isPlaying == false && up_flag!=1) // timestamp가 100의 홀수 배수이면,  
                {
                    

                    switch (which_speech)
                    {
                        case "left":
                            
                            speech_audiosource.clip = Left;
                            speech_audiosource.Play();

                            break;


                        case "up":
                            
                            speech_audiosource.clip = Up;
                            speech_audiosource.Play();
                            up_flag = 1;

                            break;



                        case "front":
                            
                            speech_audiosource.clip = Front;
                            speech_audiosource.Play();

                            break;



                        case "no Movement":
                            
                            speech_audiosource.Pause();

                            break;

                    }


                }
          


            }


        }

        else if (mode.trigger_onMovement)
        {
            if ((result.Movementing) && (result.punchStart))
            {
                if (speech_audiosource.isPlaying == false && up_flag != 1)
                {

                    switch (which_speech)
                    {
                        case "left":

                            speech_audiosource.clip = Left;
                            speech_audiosource.Play();

                            break;


                        case "up":

                            speech_audiosource.clip = Up;
                            speech_audiosource.Play();
                            up_flag = 1;

                            break;



                        case "front":

                            speech_audiosource.clip = Front;
                            speech_audiosource.Play();

                            break;



                        case "no Movement":

                            speech_audiosource.Pause();

                            break;

                    }


                }
            }


        }


        else if (mode.trigger_proximity)
        {
            if ((result.closeToMe) && (result.punchStart))
            {
                if (speech_audiosource.isPlaying == false && up_flag != 1)
                {

                    switch (which_speech)
                    {
                        case "left":

                            speech_audiosource.clip = Left;
                            speech_audiosource.Play();

                            break;


                        case "up":

                            speech_audiosource.clip = Up;
                            speech_audiosource.Play();
                            up_flag = 1;

                            break;



                        case "front":

                            speech_audiosource.clip = Front;
                            speech_audiosource.Play();

                            break;



                        case "no Movement":

                            speech_audiosource.Pause();

                            break;

                    }


                }
            }

        }
    }

    // Update is called once per frame
    void Update()
    {


        current_position = movementSource_right.position; // 현재 위치
        sub_position = current_position - past_position; // 현재와 이전 position 간의 차이

        x_changed = sub_position.x;
        y_changed = sub_position.y;
        z_changed = sub_position.z;

        // Find max
        max = Math.Abs(x_changed);
        if (max < Math.Abs(y_changed))
        {
            max = Math.Abs(y_changed);
        }

        if (max < Math.Abs(z_changed))
        {
            max = Math.Abs(z_changed);
        }




        // max값에 따라서 speech 설정
        if ((Math.Abs(x_changed) >= 0.002f) && (max == Math.Abs(x_changed)))
        {
            if (x_changed < 0)
            {
                which_speech = "left";
            }
           
        }

        else if ((Math.Abs(y_changed) >= 0.002f) && (max == Math.Abs(y_changed)))
        {
            if (y_changed > 0)
            {
                which_speech = "up";
            }
         
        }

        else if ((Math.Abs(z_changed) >= 0.002f) && (max == Math.Abs(z_changed)))
        {
            if (z_changed > 0)
            {
                which_speech = "front";
            }
          
        }

        else
        {
            which_speech = "no Movement";
        }


        past_position = current_position;








        if (mode.Feedback_mode_Continuous)
        {
            //펀치 시에,  continuous


            if (mode.trigger_onRequest) // request 모드일 때 
            {

                if ((result.requestOn)&& (result.punchStart)) // request 버튼과 펀치 버튼이 눌려있다면
                {
                    
                    if (speech_audiosource.isPlaying == false)
                    {

                        switch (which_speech)
                        {
                            case "left":

                                speech_audiosource.clip = Left;
                                speech_audiosource.Play();

                                break;


                            case "up":

                                speech_audiosource.clip = Up;
                                speech_audiosource.Play();

                                break;



                            case "front":

                                speech_audiosource.clip = Front;
                                speech_audiosource.Play();

                                break;



                            case "no Movement":

                                speech_audiosource.Pause();

                                break;

                        }


                    }
                    
                }

            }

            else if (mode.trigger_onMovement)
            {
                if ((result.Movementing)&&(result.punchStart))
                {
                    if (speech_audiosource.isPlaying == false)
                    {

                        switch (which_speech)
                        {
                            case "left":

                                speech_audiosource.clip = Left;
                                speech_audiosource.Play();

                                break;


                            case "up":

                                speech_audiosource.clip = Up;
                                speech_audiosource.Play();

                                break;



                            case "front":

                                speech_audiosource.clip = Front;
                                speech_audiosource.Play();

                                break;



                            case "no Movement":

                                speech_audiosource.Pause();

                                break;

                        }


                    }
                }
               
                
            }


            else if (mode.trigger_proximity)
            {
                if ((result.closeToMe)&&(result.punchStart))
                {
                    if (speech_audiosource.isPlaying == false)
                    {

                        switch (which_speech)
                        {
                            case "left":

                                speech_audiosource.clip = Left;
                                speech_audiosource.Play();

                                break;


                            case "up":

                                speech_audiosource.clip = Up;
                                speech_audiosource.Play();

                                break;



                            case "front":

                                speech_audiosource.clip = Front;
                                speech_audiosource.Play();

                                break;



                            case "no Movement":

                                speech_audiosource.Pause();

                                break;

                        }


                    }
                }    
                
            }

        }

        else if (mode.Feedback_mode_Discrete)
        {
            //한번만 말해줌

            if (mode.trigger_onRequest) // request 모드일 때 
            {

                if ((result.requestOn) && (result.punchStart)) // request 버튼과 펀치 버튼이 눌려있다면
                {

                    if ((speech_audiosource.isPlaying == false) && (played == 0))
                    {

                        switch (which_speech)
                        {
                            case "left":

                                speech_audiosource.clip = Left;
                                speech_audiosource.Play();
                                played++;

                                break;


                            case "up":

                                speech_audiosource.clip = Up;
                                speech_audiosource.Play();
                                played++;

                                break;



                            case "front":

                                speech_audiosource.clip = Front;
                                speech_audiosource.Play();
                                played++;

                                break;



                            case "no Movement":

                                speech_audiosource.Pause();

                                break;

                        }


                    }

                   

                }

                else 
                {
                    played = 0;
                }

            }





            else if (mode.trigger_onMovement)
            {
                if ((result.Movementing) && (result.punchStart))
                {
                    if ((speech_audiosource.isPlaying == false) && (played ==0))
                    {

                        switch (which_speech)
                        {
                            case "left":

                                speech_audiosource.clip = Left;
                                speech_audiosource.Play();
                                played++;

                                break;


                            case "up":

                                speech_audiosource.clip = Up;
                                speech_audiosource.Play();
                                played++;

                                break;



                            case "front":

                                speech_audiosource.clip = Front;
                                speech_audiosource.Play();
                                played++;

                                break;



                            case "no Movement":

                                speech_audiosource.Pause();

                                break;

                        }


                    }
                }

                else if  (!result.punchStart)
                {
                    played = 0;
                }


            }


            else if (mode.trigger_proximity)
            {
                if ((result.closeToMe) && (result.punchStart))
                {
                    if ((speech_audiosource.isPlaying == false) && (played == 0))
                    {

                        switch (which_speech)
                        {
                            case "left":

                                speech_audiosource.clip = Left;
                                speech_audiosource.Play();
                                played++;

                                break;


                            case "up":

                                speech_audiosource.clip = Up;
                                speech_audiosource.Play();
                                played++;

                                break;



                            case "front":

                                speech_audiosource.clip = Front;
                                speech_audiosource.Play();
                                played++;

                                break;



                            case "no Movement":

                                speech_audiosource.Pause();

                                break;

                        }


                    }
                }

                else if (!result.punchStart)
                {
                    played = 0;
                }

            }



        }




        // x좌표 차이, y좌표 차이, z좌표 차이를 구함
        // 그 중 가장 큰 것에 따라서 up, down / left, right / front, back

    }






}
