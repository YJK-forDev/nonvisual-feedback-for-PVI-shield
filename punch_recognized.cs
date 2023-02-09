using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class punch_recognized : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioClip l_UpperCut;
    public AudioClip l_Jab;
    public AudioClip l_Hook;

    public AudioClip r_UpperCut;
    public AudioClip r_Jab;
    public AudioClip r_Hook;


    public List<string> punch_list_left;
    public List<string> punch_list_right;

    Mode_for_myLoca modes;
    AudioSource left_punch_audio;
    AudioSource right_punch_audio;

    int right_flag = 0;







    void Start()
    {
        modes = GameObject.Find("GameManager").GetComponent<Mode_for_myLoca>();
        left_punch_audio = GameObject.Find("left_punch_audio").GetComponent<AudioSource>();
        right_punch_audio = GameObject.Find("right_punch_audio").GetComponent<AudioSource>();



    }

    // Update is called once per frame
    void Update()
    {
        
        if (!modes.concurrency_sequential) // consequential ����� (�ߵ�)
        {
            if (punch_list_left.Count!=0 && punch_list_right.Count != 0) // ���� �� ��ġ�� ������ ��
            {
                Debug.Log("hand_2"); 
                switch (punch_list_right[0])
                {
                    case "uppercut":
                        {
                            right_punch_audio.clip = r_UpperCut;
                            break;
                        }
                    case "jab":
                        {
                            right_punch_audio.clip = r_Jab;
                            break;
                        }
                    case "hook":
                        {
                            right_punch_audio.clip = r_Hook;
                            break;
                        }
                }
                punch_list_right.Clear(); // right ����Ʈ�� ���
                right_punch_audio.Play();
                

                switch (punch_list_left[0])
                {
                    case "uppercut":
                        {
                            left_punch_audio.clip = l_UpperCut;
                            break;
                        }
                    case "jab":
                        {
                            left_punch_audio.clip = l_Jab;
                            break;
                        }
                    case "hook":
                        {
                            left_punch_audio.clip = l_Hook;
                            break;
                        }
                }

                

                punch_list_left.Clear(); // left ����Ʈ�� ���, ���⼭�� ����Ʈ clear�� �� �Ͼ
                left_punch_audio.Play();


            }

            else if (punch_list_right.Count != 0 || punch_list_left.Count != 0) // �ΰ� �� �ϳ��� ��ġ ������ ��
            {
                Debug.Log("hand_1");
                if (punch_list_right.Count != 0)
                {
                    

                    switch (punch_list_right[0])
                    {
                        case "uppercut":
                            {
                                right_punch_audio.clip = r_UpperCut;
                                break;
                            }
                        case "jab":
                            {
                                right_punch_audio.clip = r_Jab;
                                break;
                            }
                        case "hook":
                            {
                                right_punch_audio.clip = r_Hook;
                                break;
                            }
                    }
                    punch_list_right.Clear(); // right ����Ʈ�� ���
                    right_punch_audio.Play();

                    


                }

                else if (punch_list_left.Count != 0)
                {
                    

                    switch (punch_list_left[0])
                    {
                        case "uppercut":
                            {
                                left_punch_audio.clip = l_UpperCut;
                                break;
                            }
                        case "jab":
                            {
                                left_punch_audio.clip = l_Jab;
                                break;
                            }
                        case "hook":
                            {
                                left_punch_audio.clip = l_Hook;
                                break;
                            }
                    }
                    punch_list_left.Clear(); // left ����Ʈ�� ���
                    left_punch_audio.Play();

                    Debug.Log("���ʸ�");

                }

                
            }





        }

        else // sequential �����
        {
            
            if (punch_list_left.Count != 0 && punch_list_right.Count != 0) // ���� �� ��ġ�� ������ �� (sequential �Ǳ��ϴµ�, ��ư���� �� ���� ������ ���� ���÷� �ؾ���)
            {
                Debug.Log("���ʴ� ����");
                if (right_flag == 0)
                {
                    switch (punch_list_right[0])
                    {
                        case "uppercut":
                            {
                                right_punch_audio.clip = r_UpperCut;
                                break;
                            }
                        case "jab":
                            {
                                right_punch_audio.clip = r_Jab;
                                break;
                            }
                        case "hook":
                            {
                                right_punch_audio.clip = r_Hook;
                                break;
                            }
                    }
                    
                    right_punch_audio.Play();
                    right_flag = 1; //play �ߴٴ� ��
                }
                

                if (right_flag ==1 && !right_punch_audio.isPlaying)
                {
                    switch (punch_list_left[0])
                    {
                        case "uppercut":
                            {
                                left_punch_audio.clip = l_UpperCut;
                                break;
                            }
                        case "jab":
                            {
                                left_punch_audio.clip = l_Jab;
                                break;
                            }
                        case "hook":
                            {
                                left_punch_audio.clip = l_Hook;
                                break;
                            }
                    }


                    punch_list_right.Clear(); // right ����Ʈ ���
                    punch_list_left.Clear(); // left ����Ʈ�� ���, ���⼭�� ����Ʈ clear�� �� �Ͼ
                    left_punch_audio.Play();
                    right_flag = 0;
                    Debug.Log("�ʰ� ���ϴ� ����");
                }
                


            }

            else if (punch_list_right.Count != 0 || punch_list_left.Count != 0) // �ΰ� �� �ϳ��� ��ġ ������ ��
            {
                Debug.Log("�ϳ��� ����");
                if (punch_list_right.Count != 0)
                {


                    switch (punch_list_right[0])
                    {
                        case "uppercut":
                            {
                                right_punch_audio.clip = r_UpperCut;
                                break;
                            }
                        case "jab":
                            {
                                right_punch_audio.clip = r_Jab;
                                break;
                            }
                        case "hook":
                            {
                                right_punch_audio.clip = r_Hook;
                                break;
                            }
                    }
                    punch_list_right.Clear(); // right ����Ʈ�� ���
                    right_punch_audio.Play();




                }

                else if (punch_list_left.Count != 0)
                {


                    switch (punch_list_left[0])
                    {
                        case "uppercut":
                            {
                                left_punch_audio.clip = l_UpperCut;
                                break;
                            }
                        case "jab":
                            {
                                left_punch_audio.clip = l_Jab;
                                break;
                            }
                        case "hook":
                            {
                                left_punch_audio.clip = l_Hook;
                                break;
                            }
                    }
                    punch_list_left.Clear(); // left ����Ʈ�� ���
                    left_punch_audio.Play();

                   

                }


            }
        }
        
    }
}
