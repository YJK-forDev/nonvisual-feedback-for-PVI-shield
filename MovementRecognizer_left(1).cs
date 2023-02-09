using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using PDollarGestureRecognizer;
using System.IO;
using UnityEngine.UI;

public class MovementRecognizer_left : MonoBehaviour
{
    public XRNode inputSource; // input source가 left hand / right hand 인지 지정
    public InputHelpers.Button inputButton; // 무슨 버튼을 눌러야 하는지 지정
    public InputHelpers.Button requestButton; // 무슨 버튼을 눌러야 하는지 지정

    public float inputThreshold = 0.1f;
    public Transform movementSource;
    Vector3 past_movementSource = new Vector3(0, 0, 0);
    public float newPositionThresholdDistance = 0.05f; // movement update 도중에 이전 위치와 현재 위치 간의 거리가 최소 0.05f만큼 차이나야 새로운 position list에 추가함 
    public GameObject debugCubePrefab;
    public bool creationMode = true;
    public string newGetureName;

    public float recognitionThreshold = 0.9f;
    private Text recogBehav;
    


    private bool isMoving = false;
    private List<Vector3> positionsList = new List<Vector3>();
    private List<Gesture> trainingSet = new List<Gesture>(); // training set은 리스트

    public bool punchStart = false;
    public bool requestOn = false;

    public bool Movementing = false;
    public bool closeToMe = false;
    GameObject mainCamera;
    GameObject Lcontroller;

    GameObject canvas;

    punch_recognized whichPunch_sound;
    background bg_audio_library;
    AudioSource bg_audio;



    // Start is called before the first frame update
    void Start()
    {
        recogBehav = GameObject.Find("Text").GetComponent<Text>(); // canvas 안에 있는 text
        //bg_audio = GameObject.Find("Background").GetComponent<AudioSource>();
        bg_audio_library = GameObject.Find("Background").GetComponent<background>();

        string[] gestureFiles = Directory.GetFiles(Application.persistentDataPath, "*_left.xml");
        foreach (var item in gestureFiles)
        {
            trainingSet.Add(GestureIO.ReadGestureFromFile(item)); // xml 파일에서 제스쳐를 읽어옴
        }

        mainCamera = GameObject.Find("Main Camera");
        Lcontroller = GameObject.Find("LeftHand Controller");

        canvas = GameObject.Find("Canvas");
       
        whichPunch_sound = canvas.GetComponent<punch_recognized>();

    }

    // Update is called once per frame
    void Update()
    {
        InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(inputSource), inputButton, out bool isPressed, inputThreshold); // 인스펙터 창에서 설정한 input source의 input button이 threshold 만큼 눌러졌다면
        InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(inputSource), requestButton, out bool requestPressed, inputThreshold); // 인스펙터 창에서 설정한 request button이 threshold 만큼 눌러졌다면

        punchStart = isPressed;
        requestOn = requestPressed;

        if (Vector3.Distance(movementSource.position, past_movementSource) > 0.001f)
        {
            Movementing = true;
            past_movementSource = movementSource.position;
        }
        else
        {
            Movementing = false;
            past_movementSource = movementSource.position;
        }

        if (Vector3.Distance(mainCamera.transform.position, Lcontroller.transform.position) < 0.5f)
        {
            closeToMe = true;
        }
        else
        {
            closeToMe = false;
        }


        //Start the movement
        if (!isMoving && isPressed)
        {
            StartMovement();
            bg_audio_library.need_punches_l = false;
        }
        //Ending the movement
        else if (isMoving && !isPressed) // 버튼에서 손을 떼면
        {
            EndMovement();
            bg_audio_library.need_success_l = true;
            
            
        }
        //Updating the movement
        else if (isMoving && isPressed)
        {
            UpdateMovement();
            
        }
        else
        {
            bg_audio_library.need_punches_l = true;
        }
    }

    void StartMovement() // 가장 처음 시작할 때
    {
        Debug.Log("Start Movement");
        isMoving = true;
        positionsList.Clear();
        positionsList.Add(movementSource.position); // 위치 좌표가 리스트에 추가됨
        if (debugCubePrefab)
        {
            Destroy(Instantiate(debugCubePrefab, movementSource.position, Quaternion.identity), 3); // 3초 있다 궤적을 보여주는 cube는 없앰
        }

    }

    void EndMovement()
    {
        Debug.Log("End Movement");
        isMoving = false;

        //Create the gesture from the position list
        Point[] pointArray = new Point[positionsList.Count];
        

        for (int i = 0; i < positionsList.Count; i++)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(positionsList[i]);
            Debug.Log("position list : " + positionsList[i]);
            pointArray[i] = new Point(screenPoint.x, screenPoint.y, 0); // point array에 point list 각 좌표들의 x,y 좌표를 저장함

        }

        Gesture newGesture = new Gesture(pointArray);

        //Add a new gesture to training set
        if (creationMode)
        {
            newGesture.Name = newGetureName;
            trainingSet.Add(newGesture); //creation mode라면 point array에 저장되어 있는 좌표들을 training Set에 저장함

            string fileName = Application.persistentDataPath + "/" + newGetureName + "_left" + ".xml";
            
            
            GestureIO.WriteGesture(pointArray, newGetureName, fileName); // xml 파일에 해당 제스쳐를 추가하여 저장함
        }
        //recognize
        else
        {
            
            Result result = PointCloudRecognizer.Classify(newGesture, trainingSet.ToArray()); //입력받은 newGesture를 보고 training set 중 무엇으로 분류되었는지를 results에 저장함 

            Debug.Log("left___" + result.GestureClass + result.Score);
            recogBehav.text = "Recognized: " + result.GestureClass + ", Score: " + result.Score; // 예측한 제스쳐와 그에 대한 score를 받아와서 text에 띄워줌

            // 여기서 audiosource 말하기
            switch (result.GestureClass)
            {
                case "Left uppercut":
                    {
                        whichPunch_sound.punch_list_left.Add("uppercut"); 
                        break;
                    }
                case "Left Jab":
                    {
                        whichPunch_sound.punch_list_left.Add("jab");
                        break;
                    }
                case "Left Hook":
                    {
                        whichPunch_sound.punch_list_left.Add("hook");
                        break;
                    }
            }

            



        }
    }

    void UpdateMovement() // 그리고 있는 중
    {
        Debug.Log("Update Movement");
        Vector3 lastPosition = positionsList[positionsList.Count - 1];
        if (Vector3.Distance(movementSource.position, lastPosition) > newPositionThresholdDistance) // 이전 위치와 현재 위치가 newpositionthreshold 이상 차이나면 새롭게 현재 위치를 list에 추가함
        {
            positionsList.Add(movementSource.position);
            if (debugCubePrefab) // debug cube prefab이 존재한다면
            {
                Destroy(Instantiate(debugCubePrefab, movementSource.position, Quaternion.identity), 3);
            }
        }

    }


}
