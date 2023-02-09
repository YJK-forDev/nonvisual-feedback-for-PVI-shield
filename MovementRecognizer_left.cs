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
    public XRNode inputSource; // input source�� left hand / right hand ���� ����
    public InputHelpers.Button inputButton; // ���� ��ư�� ������ �ϴ��� ����
    public InputHelpers.Button requestButton; // ���� ��ư�� ������ �ϴ��� ����

    public float inputThreshold = 0.1f;
    public Transform movementSource;
    Vector3 past_movementSource = new Vector3(0, 0, 0);
    public float newPositionThresholdDistance = 0.05f; // movement update ���߿� ���� ��ġ�� ���� ��ġ ���� �Ÿ��� �ּ� 0.05f��ŭ ���̳��� ���ο� position list�� �߰��� 
    public GameObject debugCubePrefab;
    public bool creationMode = true;
    public string newGetureName;

    public float recognitionThreshold = 0.9f;
    private Text recogBehav;
    


    private bool isMoving = false;
    private List<Vector3> positionsList = new List<Vector3>();
    private List<Gesture> trainingSet = new List<Gesture>(); // training set�� ����Ʈ

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
        recogBehav = GameObject.Find("Text").GetComponent<Text>(); // canvas �ȿ� �ִ� text
        //bg_audio = GameObject.Find("Background").GetComponent<AudioSource>();
        bg_audio_library = GameObject.Find("Background").GetComponent<background>();

        string[] gestureFiles = Directory.GetFiles(Application.persistentDataPath, "*_left.xml");
        foreach (var item in gestureFiles)
        {
            trainingSet.Add(GestureIO.ReadGestureFromFile(item)); // xml ���Ͽ��� �����ĸ� �о��
        }

        mainCamera = GameObject.Find("Main Camera");
        Lcontroller = GameObject.Find("LeftHand Controller");

        canvas = GameObject.Find("Canvas");
       
        whichPunch_sound = canvas.GetComponent<punch_recognized>();

    }

    // Update is called once per frame
    void Update()
    {
        InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(inputSource), inputButton, out bool isPressed, inputThreshold); // �ν����� â���� ������ input source�� input button�� threshold ��ŭ �������ٸ�
        InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(inputSource), requestButton, out bool requestPressed, inputThreshold); // �ν����� â���� ������ request button�� threshold ��ŭ �������ٸ�

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
        else if (isMoving && !isPressed) // ��ư���� ���� ����
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

    void StartMovement() // ���� ó�� ������ ��
    {
        Debug.Log("Start Movement");
        isMoving = true;
        positionsList.Clear();
        positionsList.Add(movementSource.position); // ��ġ ��ǥ�� ����Ʈ�� �߰���
        if (debugCubePrefab)
        {
            Destroy(Instantiate(debugCubePrefab, movementSource.position, Quaternion.identity), 3); // 3�� �ִ� ������ �����ִ� cube�� ����
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
            pointArray[i] = new Point(screenPoint.x, screenPoint.y, 0); // point array�� point list �� ��ǥ���� x,y ��ǥ�� ������

        }

        Gesture newGesture = new Gesture(pointArray);

        //Add a new gesture to training set
        if (creationMode)
        {
            newGesture.Name = newGetureName;
            trainingSet.Add(newGesture); //creation mode��� point array�� ����Ǿ� �ִ� ��ǥ���� training Set�� ������

            string fileName = Application.persistentDataPath + "/" + newGetureName + "_left" + ".xml";
            
            
            GestureIO.WriteGesture(pointArray, newGetureName, fileName); // xml ���Ͽ� �ش� �����ĸ� �߰��Ͽ� ������
        }
        //recognize
        else
        {
            
            Result result = PointCloudRecognizer.Classify(newGesture, trainingSet.ToArray()); //�Է¹��� newGesture�� ���� training set �� �������� �з��Ǿ������� results�� ������ 

            Debug.Log("left___" + result.GestureClass + result.Score);
            recogBehav.text = "Recognized: " + result.GestureClass + ", Score: " + result.Score; // ������ �����Ŀ� �׿� ���� score�� �޾ƿͼ� text�� �����

            // ���⼭ audiosource ���ϱ�
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

    void UpdateMovement() // �׸��� �ִ� ��
    {
        Debug.Log("Update Movement");
        Vector3 lastPosition = positionsList[positionsList.Count - 1];
        if (Vector3.Distance(movementSource.position, lastPosition) > newPositionThresholdDistance) // ���� ��ġ�� ���� ��ġ�� newpositionthreshold �̻� ���̳��� ���Ӱ� ���� ��ġ�� list�� �߰���
        {
            positionsList.Add(movementSource.position);
            if (debugCubePrefab) // debug cube prefab�� �����Ѵٸ�
            {
                Destroy(Instantiate(debugCubePrefab, movementSource.position, Quaternion.identity), 3);
            }
        }

    }


}
