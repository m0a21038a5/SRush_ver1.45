using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class multipleTarget : MonoBehaviour
{
    single s;
    multiplePlayer mp;

    [SerializeField]
    public List<GameObject> multipleTargetObject;//突進の対象


    [SerializeField]
    float radian;

    
    [SerializeField]
    move1_ver2 m2;
    [SerializeField]
    CinemachineVirtualCamera TargetCamera;//特殊カメラ
    [SerializeField]
    GameObject MultitargetUI;//複数ターゲット用UI

    public bool target;
    public bool multiple;
    public bool ChangeCamera;
    public Animator Playerani;

    Rigidbody rb;
    Combo c;
    [SerializeField] GameObject P;
    Soundtest st;// M追加

    // Start is called before the first frame update
    void Start()
    {
        s = GameObject.FindGameObjectWithTag("Manager").GetComponent<single>();
        mp = GameObject.FindGameObjectWithTag("Player").GetComponent<multiplePlayer>();
        target = false;
        TargetCamera.Priority = 1;
        MultitargetUI.SetActive(false);
        rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        c = GameObject.FindGameObjectWithTag("Player").GetComponent<Combo>();
        st = GameObject.Find("SEPlayer").GetComponent<Soundtest>();// M追加
    }

    // Update is called once per frame
    void Update()
    {
        //複数ターゲット、必殺技発動時にカメラ切り替え
        if (ChangeCamera)
        {
            TargetCamera.Priority = 20;
            m2.enabled = false;
            Playerani.enabled = false;
            MultitargetUI.SetActive(true);
            rb.isKinematic = true;
            P.SetActive(false);
        }

        if(!ChangeCamera && !c.SpecialMode)
        {
            TargetCamera.Priority = 1;
            m2.enabled = true;
            Playerani.enabled = true;
            MultitargetUI.SetActive(false);
            rb.isKinematic = false;
            P.SetActive(true);
        }

        //複数ターゲットの対象を決める
        if ( target && s.targetList.Count > 1)
        {
            multiple = true;
            ChangeCamera = true;
            float Distance;
            Vector3 Center = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
            foreach (GameObject obj in s.targetList)
            {
                Vector3 TargetPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, obj.transform.position);
                Distance = Vector3.Distance(TargetPosition, Center);
                Debug.Log(Distance);
                if (!multipleTargetObject.Contains(obj) && Distance < radian && multipleTargetObject.Count < 7)
                {
                    multipleTargetObject.Add(obj);
                    Rigidbody rB = obj.GetComponent<Rigidbody>();
                    // RigidbodyのX軸の位置制約を設定（trueで制約をかける、falseで解除）
                    rB.constraints = rB.constraints | RigidbodyConstraints.FreezePositionX;

                    // RigidbodyのY軸の位置制約を設定
                    rB.constraints = rB.constraints | RigidbodyConstraints.FreezePositionY;

                    // RigidbodyのZ軸の位置制約を設定
                    rB.constraints = rB.constraints | RigidbodyConstraints.FreezePositionZ;
                    st.SE_TargetLockedPlayer();// M追加
                }
            }
        }

        //ターゲットが1体以下の場合は、複数ターゲットを中止
        if ((Input.GetKeyUp("joystick button 7") || Input.GetKeyUp("joystick button 0") || Input.GetMouseButtonUp(1)) && multipleTargetObject.Count < 2)
        {
            target = false;
            ChangeCamera = false;
            multipleTargetObject.Clear();
        }
    }
    
}
