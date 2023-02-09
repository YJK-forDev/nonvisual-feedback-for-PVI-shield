using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Oscillator_right : MonoBehaviour
{
    public double frequency = 440.0; //440.0;
    private double increment;
    private double phase;
    private double sampling_frequency = 90000.0;//48000.0;  �Ҹ��� Ű���, �Ҹ��� ������

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
    void FixedUpdate() // ��ġ, ���� �̷��� ����
    {
        prev_y = curr_y;
        curr_y = transform.position.y; // ������ y ��ǥ
        sub_y = curr_y - prev_y; // y ��ǥ ���� ���̰� sub_y

        prev_z = curr_z;
        curr_z = transform.position.z;
        sub_z = curr_z - prev_z;
        gain = volume;

        if (mode.Feedback_mode_Periodic) // 
        {
            if (movement_right.punchStart) // Ʈ���� ��ư�� ������ audiosource.volume = 1f; (�Ҹ��� ��)
            {
                // if���� �ش��� ���� �Ҹ��� ���� (�� ��)
                if (((i >= 25) && (i <= 50)) || ((i >= 75) && (i <= 100)) || ((i >= 125) && (i <= 150)) || ((i >= 175) && (i <= 200)) || ((i >= 225) && (i <= 250)) || ((i >= 275) && (i <= 300)) || ((i >= 325) && (i <= 350)) || ((i >= 375) && (i <= 400)) || ((i >= 425) && (i <= 450)) || ((i >= 475) && (i <= 500)) || ((i >= 525) && (i <= 550)) || ((i >= 575) && (i <= 600)) || ((i >= 625) && (i <= 650)) || ((i >= 675) && (i <= 700)) || ((i >= 725) && (i <= 750)) || ((i >= 775) && (i <= 800))) // frameInts�� ���� �����ӿ����� �Ҹ��� ��
                {
                    audiosource.volume = 0f;
                    gain = gain - sub_z * 0.1; // ���� Ŀ�� ���� ������ Ŀ��
                   


                }
                else // �Ҹ��� ��
                {
                    
                    if (mode.trigger_onRequest)
                    {
                        if (!movement_right.requestOn) // ��ư�� �������� �ʴٸ�
                        {
                            audiosource.volume = 0f;
                            gain = gain - sub_z * 0.1; // ���� Ŀ�� ���� ������ Ŀ��
                        }
                        else
                        {
                            audiosource.volume = 1f;
                            gain = gain - sub_z * 0.1; // ���� Ŀ�� ���� ������ Ŀ��
                        }
                    }

                    if (mode.trigger_onMovement)
                    {
                        if (movement_right.Movementing)// ��ġ �߿� �����̰� ������
                        {
                            audiosource.volume = 1f;
                            gain = gain - sub_z * 0.1; // ���� Ŀ�� ���� ������ Ŀ��
                        }
                        else
                        {
                            audiosource.volume = 0f;
                            gain = gain - sub_z * 0.1; // ���� Ŀ�� ���� ������ Ŀ��
                        }

                    }

                    if (mode.trigger_proximity)
                    {

                        if (movement_right.closeToMe)// ��ġ �߿� �����̰� ������
                        {
                            audiosource.volume = 1f;
                            gain = gain - sub_z * 0.1; // ���� Ŀ�� ���� ������ Ŀ��
                        }
                        else
                        {
                            audiosource.volume = 0f;
                            gain = gain - sub_z * 0.1; // ���� Ŀ�� ���� ������ Ŀ��
                        }

                    }


                }
                i++;
            }

            else // ��ġ ��ư�� �������� �ʴٸ�
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
                gain = gain - sub_z * 0.1; // ���� Ŀ�� ���� ������ Ŀ��

                if (mode.trigger_onRequest)
                {
                    if (!movement_right.punchStart) // ��ġ ��ư �ȴ����ٸ�
                    {
                        audiosource.volume = 1f;
                        gain = gain - sub_z * 0.1; // ���� Ŀ�� ���� ������ Ŀ��
                    }
                    else // ��ġ ��ư �����ٸ�
                    {
                        if (!movement_right.requestOn) // ��ư�� �������� �ʴٸ�
                        {
                            audiosource.volume = 0f;
                            gain = gain - sub_z * 0.1; // ���� Ŀ�� ���� ������ Ŀ��
                        }
                        else
                        {
                            audiosource.volume = 1f;
                            gain = gain - sub_z * 0.1; // ���� Ŀ�� ���� ������ Ŀ��
                        }
                    }



                }

                if (mode.trigger_onMovement)
                {
                    if (!movement_right.punchStart) // ��ġ ��ư �ȴ����ٸ�
                    {
                        audiosource.volume = 1f;
                        gain = gain - sub_z * 0.1; // ���� Ŀ�� ���� ������ Ŀ��

                    }
                    else // ��ġ ��ư �����ٸ�
                    {
                        Debug.Log("movement �ν�: " + movement_right.Movementing);
                        if (movement_right.Movementing)// ��ġ �߿� �����̰� ������
                        {
                            Debug.Log("�������� �Ҹ����� ��");
                            audiosource.volume = 1f;
                            gain = gain - sub_z * 0.1; // ���� Ŀ�� ���� ������ Ŀ��
                        }
                        else
                        {
                            audiosource.volume = 0f;
                            gain = gain - sub_z * 0.1; // ���� Ŀ�� ���� ������ Ŀ��
                        }
                    }
                }

                if (mode.trigger_proximity)
                {
                    if (!movement_right.punchStart) // ��ġ ��ư �ȴ����ٸ�
                    {
                        audiosource.volume = 1f;
                        gain = gain - sub_z * 0.1; // ���� Ŀ�� ���� ������ Ŀ��
                    }
                    else // ��ġ ��ư �����ٸ�
                    {
                        if (movement_right.closeToMe)// ��ġ �߿� �����̰� ������
                        {
                            audiosource.volume = 1f;
                            gain = gain - sub_z * 0.1; // ���� Ŀ�� ���� ������ Ŀ��
                        }
                        else
                        {
                            audiosource.volume = 0f;
                            gain = gain - sub_z * 0.1; // ���� Ŀ�� ���� ������ Ŀ��
                        }
                    }
                }
            }
            else
            {
                audiosource.volume = 0f;

            }

            

        }

        else if (mode.Feedback_mode_Discrete) // discrete ����϶�
        {

            if (movement_right.punchStart) // Ʈ���� ��ư�� ������ audiosource.volume = 1f; (�Ҹ��� ��)
            {
                audiosource.volume = 0f;
                gain = gain - sub_z * 0.1; // ���� Ŀ�� ���� ������ Ŀ��

                if (mode.trigger_onRequest)
                {
                    
                    if (!movement_right.requestOn) // ��ư�� �������� �ʴٸ�
                    {
                        j = 0;
                        audiosource.volume = 0f;
                        gain = gain - sub_z * 0.1; // ���� Ŀ�� ���� ������ Ŀ��
                        
                    }
                    else
                    {
                        j++;
                        if (j <= 15)
                        {
                            audiosource.volume = 1f;
                            gain = gain - sub_z * 0.1; // ���� Ŀ�� ���� ������ Ŀ��
                            Debug.Log("j�� " + j);
                        }
                        else
                        {
                            audiosource.volume = 0f;
                        }

                    }
                    
                }
                // if���� �ش��� ���� �Ҹ��� ���� (�� ��)
                else
                {
                    
                    if (i >= 15)
                    {
                        audiosource.volume = 0f;
                        gain = gain - sub_z * 0.1; // ���� Ŀ�� ���� ������ Ŀ��
                    }
                    else
                    {
                        if (mode.trigger_onMovement)
                        {
                            if (movement_right.Movementing)// ��ġ �߿� �����̰� ������
                            {
                                audiosource.volume = 1f;
                                gain = gain - sub_z * 0.1; // ���� Ŀ�� ���� ������ Ŀ��
                                i++;
                            }
                            else
                            {
                                audiosource.volume = 0f;
                                gain = gain - sub_z * 0.1; // ���� Ŀ�� ���� ������ Ŀ��
                            }

                        }

                        if (mode.trigger_proximity)
                        {

                            if (movement_right.closeToMe)// ��ġ �߿� �����̰� ������
                            {
                                audiosource.volume = 1f;
                                gain = gain - sub_z * 0.1; // ���� Ŀ�� ���� ������ Ŀ��
                                i++;
                            }
                            else
                            {
                                audiosource.volume = 0f;
                                gain = gain - sub_z * 0.1; // ���� Ŀ�� ���� ������ Ŀ��
                            }

                        }
                    }
                    
                } 
                    
                
               
                

                
            }

            else // ��ġ ��ư�� �������� �ʴٸ�
            {
                audiosource.volume = 0f;
                i = 0;
                j = 0;
            }

        }

        frequency = frequency + sub_y * 100; // ���� Ŀ�� ���� �Ҹ��� ������

        
    }

    void OnAudioFilterRead(float[] data, int channels)
    { // ���α׷��� ������ �׷��ִ� �Լ�
        // onaudiofilterread�� �ִ� object���� audiosource�� none�� ��쿡 �����
        increment = frequency * 2.0 * Mathf.PI / sampling_frequency; //���α׷��� ������ ���� 
        //Mathf.PI�� ���̰� (3.14 ...)
        for (int i = 0; i < data.Length; i += channels)
        {
            phase += increment; // phase�� �ֱ�
                                //data[i] = (float) (gain * Mathf.Sin((float)phase)); // data[i]�� 


            if (gain * Mathf.Sin((float)phase) >= 0 * gain) // 
            {
                data[i] = (float)gain * 0.6f; //data [i] ���� �����̾�!! ���� Ŀ���� �Ҹ��� Ŀ��
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
