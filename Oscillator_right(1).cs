using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Oscillator_right : MonoBehaviour
{
    public double frequency = 440.0; //440.0;
    private double increment;
    private double phase;
    private double sampling_frequency = 90000.0;//48000.0;  소리를 키우면, 소리가 낮춰짐

    public double gain;
    public double prev_gain;
    public double volume = 0.4;

    public float[] frequencies;
    public int thisFreq;
    private double prev_y;
    private double prev_z;
    private double curr_y;
    private double curr_z;

    private double sub_y;
    public double sub_z;
    private double temp = 0;
    private double time;

    Mode_for_myLoca mode;
    AudioSource audiosource;
    MovementRecognizer_right movement_right;
    //List<int> frameInts = new List<int>();
    int i = 0;
    int j = 0;


    // Start is called before the first frame update
    void Start()
    {
        /*
        frequencies = new float[8];
        frequencies[0] = 440;
        frequencies[1] = 494;
        frequencies[2] = 554;
        frequencies[3] = 587;
        frequencies[4] = 659;
        frequencies[5] = 740;
        frequencies[6] = 831;
        frequencies[7] = 880;
        */

        mode = GameObject.Find("GameManager").GetComponent<Mode_for_myLoca>();
        movement_right = GameObject.Find("Movement Recognizer_right").GetComponent<MovementRecognizer_right>();
        audiosource = GetComponent<AudioSource>();


        prev_y = transform.position.y; //previous y
        curr_y = transform.position.y; //current y
        prev_z = transform.position.z; //previous z
        curr_z = transform.position.z; //current z
        gain = volume;
        prev_gain = volume;
    }

    // Update is called once per frame
    void FixedUpdate() // 피치, 길이 이런거 조정
    {
        prev_y = curr_y;
        curr_y = transform.position.y; // 현재의 y 좌표
        sub_y = curr_y - prev_y; // y 좌표 간의 차이가 sub_y

        prev_z = curr_z;
        curr_z = transform.position.z;
        sub_z = curr_z - prev_z;
        gain = volume;

        if (mode.Feedback_mode_Periodic) // 
        {
            if (movement_right.punchStart) // 트리거 버튼이 눌리면 audiosource.volume = 1f; (소리가 남)
            {
                // if문에 해당할 때는 소리가 끊김 (안 남)
                if (((i >= 25) && (i <= 50)) || ((i >= 75) && (i <= 100)) || ((i >= 125) && (i <= 150)) || ((i >= 175) && (i <= 200)) || ((i >= 225) && (i <= 250)) || ((i >= 275) && (i <= 300)) || ((i >= 325) && (i <= 350)) || ((i >= 375) && (i <= 400)) || ((i >= 425) && (i <= 450)) || ((i >= 475) && (i <= 500)) || ((i >= 525) && (i <= 550)) || ((i >= 575) && (i <= 600)) || ((i >= 625) && (i <= 650)) || ((i >= 675) && (i <= 700)) || ((i >= 725) && (i <= 750)) || ((i >= 775) && (i <= 800))) // frameInts에 없는 프레임에서는 소리가 남
                {
                    audiosource.volume = 0f;
                    gain = gain - sub_z * 0.1; // 값이 커질 수록 볼륨이 커짐
                   


                }
                else // 소리가 남
                {
                    
                    if (mode.trigger_onRequest)
                    {
                        if (!movement_right.requestOn) // 버튼이 눌려있지 않다면
                        {
                            audiosource.volume = 0f;
                            gain = gain - sub_z * 0.1; // 값이 커질 수록 볼륨이 커짐
                        }
                        else
                        {
                            audiosource.volume = 1f;
                            gain = gain - sub_z * 0.1; // 값이 커질 수록 볼륨이 커짐
                        }
                    }

                    if (mode.trigger_onMovement)
                    {
                        if (movement_right.Movementing)// 펀치 중에 움직이고 있으면
                        {
                            audiosource.volume = 1f;
                            gain = gain - sub_z * 0.1; // 값이 커질 수록 볼륨이 커짐
                        }
                        else
                        {
                            audiosource.volume = 0f;
                            gain = gain - sub_z * 0.1; // 값이 커질 수록 볼륨이 커짐
                        }

                    }

                    if (mode.trigger_proximity)
                    {

                        if (movement_right.closeToMe)// 펀치 중에 움직이고 있으면
                        {
                            audiosource.volume = 1f;
                            gain = gain - sub_z * 0.1; // 값이 커질 수록 볼륨이 커짐
                        }
                        else
                        {
                            audiosource.volume = 0f;
                            gain = gain - sub_z * 0.1; // 값이 커질 수록 볼륨이 커짐
                        }

                    }


                }
                i++;
            }

            else // 펀치 버튼이 눌려있지 않다면
            {
                audiosource.volume = 0f;
                i = 0;
            }


        }

        else if (mode.Feedback_mode_Continuous)
        {
            if (movement_right.punchStart)
            {
                audiosource.volume = 0f;
                gain = gain - sub_z * 0.1; // 값이 커질 수록 볼륨이 커짐

                if (mode.trigger_onRequest)
                {
                    if (!movement_right.punchStart) // 펀치 버튼 안눌렀다면
                    {
                        audiosource.volume = 1f;
                        gain = gain - sub_z * 0.1; // 값이 커질 수록 볼륨이 커짐
                    }
                    else // 펀치 버튼 눌렀다면
                    {
                        if (!movement_right.requestOn) // 버튼이 눌려있지 않다면
                        {
                            audiosource.volume = 0f;
                            gain = gain - sub_z * 0.1; // 값이 커질 수록 볼륨이 커짐
                        }
                        else
                        {
                            audiosource.volume = 1f;
                            gain = gain - sub_z * 0.1; // 값이 커질 수록 볼륨이 커짐
                        }
                    }



                }

                if (mode.trigger_onMovement)
                {
                    if (!movement_right.punchStart) // 펀치 버튼 안눌렀다면
                    {
                        audiosource.volume = 1f;
                        gain = gain - sub_z * 0.1; // 값이 커질 수록 볼륨이 커짐

                    }
                    else // 펀치 버튼 눌렀다면
                    {
                        Debug.Log("movement 인식: " + movement_right.Movementing);
                        if (movement_right.Movementing)// 펀치 중에 움직이고 있으면
                        {
                            Debug.Log("움직여서 소리나는 중");
                            audiosource.volume = 1f;
                            gain = gain - sub_z * 0.1; // 값이 커질 수록 볼륨이 커짐
                        }
                        else
                        {
                            audiosource.volume = 0f;
                            gain = gain - sub_z * 0.1; // 값이 커질 수록 볼륨이 커짐
                        }
                    }
                }

                if (mode.trigger_proximity)
                {
                    if (!movement_right.punchStart) // 펀치 버튼 안눌렀다면
                    {
                        audiosource.volume = 1f;
                        gain = gain - sub_z * 0.1; // 값이 커질 수록 볼륨이 커짐
                    }
                    else // 펀치 버튼 눌렀다면
                    {
                        if (movement_right.closeToMe)// 펀치 중에 움직이고 있으면
                        {
                            audiosource.volume = 1f;
                            gain = gain - sub_z * 0.1; // 값이 커질 수록 볼륨이 커짐
                        }
                        else
                        {
                            audiosource.volume = 0f;
                            gain = gain - sub_z * 0.1; // 값이 커질 수록 볼륨이 커짐
                        }
                    }
                }
            }
            else
            {
                audiosource.volume = 0f;

            }

            

        }

        else if (mode.Feedback_mode_Discrete) // discrete 모드일때
        {

            if (movement_right.punchStart) // 트리거 버튼이 눌리면 audiosource.volume = 1f; (소리가 남)
            {
                audiosource.volume = 0f;
                gain = gain - sub_z * 0.1; // 값이 커질 수록 볼륨이 커짐

                if (mode.trigger_onRequest)
                {
                    
                    if (!movement_right.requestOn) // 버튼이 눌려있지 않다면
                    {
                        j = 0;
                        audiosource.volume = 0f;
                        gain = gain - sub_z * 0.1; // 값이 커질 수록 볼륨이 커짐
                        
                    }
                    else
                    {
                        j++;
                        if (j <= 15)
                        {
                            audiosource.volume = 1f;
                            gain = gain - sub_z * 0.1; // 값이 커질 수록 볼륨이 커짐
                            Debug.Log("j는 " + j);
                        }
                        else
                        {
                            audiosource.volume = 0f;
                        }

                    }
                    
                }
                // if문에 해당할 때는 소리가 끊김 (안 남)
                else
                {
                    
                    if (i >= 15)
                    {
                        audiosource.volume = 0f;
                        gain = gain - sub_z * 0.1; // 값이 커질 수록 볼륨이 커짐
                    }
                    else
                    {
                        if (mode.trigger_onMovement)
                        {
                            if (movement_right.Movementing)// 펀치 중에 움직이고 있으면
                            {
                                audiosource.volume = 1f;
                                gain = gain - sub_z * 0.1; // 값이 커질 수록 볼륨이 커짐
                                i++;
                            }
                            else
                            {
                                audiosource.volume = 0f;
                                gain = gain - sub_z * 0.1; // 값이 커질 수록 볼륨이 커짐
                            }

                        }

                        if (mode.trigger_proximity)
                        {

                            if (movement_right.closeToMe)// 펀치 중에 움직이고 있으면
                            {
                                audiosource.volume = 1f;
                                gain = gain - sub_z * 0.1; // 값이 커질 수록 볼륨이 커짐
                                i++;
                            }
                            else
                            {
                                audiosource.volume = 0f;
                                gain = gain - sub_z * 0.1; // 값이 커질 수록 볼륨이 커짐
                            }

                        }
                    }
                    
                } 
                    
                
               
                

                
            }

            else // 펀치 버튼이 눌려있지 않다면
            {
                audiosource.volume = 0f;
                i = 0;
                j = 0;
            }

        }

        frequency = frequency + sub_y * 100; // 값이 커질 수록 소리가 높아짐

        
    }

    void OnAudioFilterRead(float[] data, int channels)
    { // 사인그래프 파형을 그려주는 함수
        // onaudiofilterread가 있는 object에서 audiosource가 none인 경우에 실행됨
        increment = frequency * 2.0 * Mathf.PI / sampling_frequency; //사인그래프 형식을 조정 
        //Mathf.PI는 파이값 (3.14 ...)
        for (int i = 0; i < data.Length; i += channels)
        {
            phase += increment; // phase가 주기
                                //data[i] = (float) (gain * Mathf.Sin((float)phase)); // data[i]가 


            if (gain * Mathf.Sin((float)phase) >= 0 * gain) // 
            {
                data[i] = (float)gain * 0.6f; //data [i] 값은 볼륨이야!! 값이 커지면 소리가 커져
            }
            else
            {
                data[i] = (-(float)gain) * 0.6f;
            }


            if (channels == 2)
            {
                data[i + 1] = data[i];
            }

            if (phase > (Mathf.PI * 2))
            {
                phase = 0.0;
            }
        }
    }
}
