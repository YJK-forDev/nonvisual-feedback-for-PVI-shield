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
        
        if (!modes.concurrency_sequential) // consequential 모드라면 (잘됨)
        {
            if (punch_list_left.Count!=0 && punch_list_right.Count != 0) // 두쪽 다 펀치가 들어왔을 때
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
                punch_list_right.Clear(); // right 리스트는 비움
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

                

                punch_list_left.Clear(); // left 리스트는 비움, 여기서만 리스트 clear가 잘 일어남
                left_punch_audio.Play();


            }

            else if (punch_list_right.Count != 0 || punch_list_left.Count != 0) // 두곳 중 하나만 펀치 들어왔을 때
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
                    punch_list_right.Clear(); // right 리스트는 비움
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
                    punch_list_left.Clear(); // left 리스트는 비움
                    left_punch_audio.Play();

                    Debug.Log("왼쪽만");

                }

                
            }





        }

        else // sequential 모드라면
        {
            
            if (punch_list_left.Count != 0 && punch_list_right.Count != 0) // 두쪽 다 펀치가 들어왔을 때 (sequential 되긴하는데, 버튼에서 손 떼는 순간을 완전 동시로 해야함)
            {
                Debug.Log("두쪽다 들어옴");
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
                    right_flag = 1; //play 했다는 뜻
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


                    punch_list_right.Clear(); // right 리스트 비움
                    punch_list_left.Clear(); // left 리스트도 비움, 여기서만 리스트 clear가 잘 일어남
                    left_punch_audio.Play();
                    right_flag = 0;
                    Debug.Log("늦게 말하는 왼쪽");
                }
                


            }

            else if (punch_list_right.Count != 0 || punch_list_left.Count != 0) // 두곳 중 하나만 펀치 들어왔을 때
            {
                Debug.Log("하나만 들어옴");
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
                    punch_list_right.Clear(); // right 리스트는 비움
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
                    punch_list_left.Clear(); // left 리스트는 비움
                    left_punch_audio.Play();

                   

                }


            }
        }
        
    }
}
