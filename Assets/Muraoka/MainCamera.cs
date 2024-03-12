using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MainCamera : MonoBehaviour
{

    public string CameraType = "";

    [SerializeField] public GameObject TPCamera;
    private TPCamera TPC;

    [SerializeField] public GameObject TPCameraTarget;
    private TPCameraTarget TPCT;

    private GameObject Player;

    public bool isCameraResetNow;

    public bool isCameraFree = true;
    public Vector2 cameraMoveV = Vector2.zero;
    public GameObject pastEnemy;

    public float returnVecRange;

    private Combo combo;

    private target target;

    multipleTarget mt;

    public float reset_angle;

    void Start()
    {
        TPC = TPCamera.GetComponent<TPCamera>();
        TPCT = TPCameraTarget.GetComponent<TPCameraTarget>();

        Player = GameObject.FindGameObjectWithTag("Player");
        target = Player.GetComponent<target>();

        combo = Player.GetComponent<Combo>();

        mt = GameObject.FindGameObjectWithTag("Manager").GetComponent<multipleTarget>();
    }


    void LateUpdate()
    {
        // �^�[�Q�b�g���ς������
        if (target.TargetStatue != null && pastEnemy != target.TargetStatue)
        {
            pastEnemy = target.TargetStatue;
            isCameraFree = true;
        }
        else if (target.TargetBeam != null && pastEnemy != target.TargetBeam)
        {
            pastEnemy = target.TargetBeam;
            isCameraFree = true;
        }
        else if (target.TargetBoss != null && pastEnemy != target.TargetBoss)
        {
            pastEnemy = target.TargetBoss;
            isCameraFree = true;
        }
        else if ((target.TargetStatue == null && target.TargetBeam == null && target.TargetBoss == null) && pastEnemy != null)
        {
            pastEnemy = null;
            isCameraFree = true;
        }







        if (Input.GetMouseButtonDown(2) || Input.GetKeyDown("joystick button 11"))
        {
            isCameraResetNow = true;
            reset_angle = Mathf.Atan2(-Player.transform.forward.x, Player.transform.forward.z) * Mathf.Rad2Deg;
        }

        cameraMoveV.x += Input.GetAxis("Mouse X");
        cameraMoveV.y += Input.GetAxis("Mouse Y");

        // �^�[�Q�b�g���ڃJ��������v���C���[���ڃJ�����֑J��
        if (isCameraFree == false && (cameraMoveV.magnitude > returnVecRange) )
        {
            //pastObj = TPCT.targetObj;
            isCameraFree = true;
        }

        // �v���C���[���ڃJ��������^�[�Q�b�g���ڃJ�����֑J�ځi���j
        if (isCameraFree == true && target.TargetStatue != null && (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown("joystick button 6")))
        {
            cameraMoveV = Vector2.zero;
            isCameraFree = false;
        }

        // �v���C���[���ڃJ��������^�[�Q�b�g���ڃJ�����֑J�ځi�r�[���j
        if (isCameraFree == true && target.TargetBeam != null && (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown("joystick button 6")))
        {
            cameraMoveV = Vector2.zero;
            isCameraFree = false;
        }

        // �v���C���[���ڃJ��������^�[�Q�b�g���ڃJ�����֑J�ځi�{�X�j
        if (isCameraFree == true && target.TargetBoss != null && (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown("joystick button 6")))
        {
            cameraMoveV = Vector2.zero;
            isCameraFree = false;
        }


        if (!mt.ChangeCamera)
        {

            // �J�������Z�b�g��
            if (isCameraResetNow == true)
            {
                TPC.resetCamera(reset_angle);
            }
            // �J�������Z�b�g�����Ă��Ȃ��Ƃ�
            else
            {
                // �������^�[�Q�b�g���Ă��āA�^�[�Q�b�g���ڃJ�����ɂ��Ă��鎞
                if (TPCT.isTarget == true && isCameraFree == false)
                {
                    TPC.targetEnemy();
                }
                else
                {
                    // W���쎞�i�R���{�����󒆁j
                    if (combo.isCombo == true && combo.isFloor == false)
                    {
                        TPC.targetPlayer(true);
                    }
                    // �ʏ펞
                    else
                    {
                        TPC.targetPlayer(false);
                    }

                }
            }
        }


    }

}
