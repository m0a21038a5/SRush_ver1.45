using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TPCamera : MonoBehaviour
{
    // Main�J����
    private MainCamera MC;
    // �v���C���[�̃Q�[���I�u�W�F�N�g
    private GameObject Player;

    // ����VirtualCamera��CVC�R���|�[�l���g
    private CinemachineVirtualCamera CVC;
    // ����VirtualCamera��CVC�R���|�[�l���g��Body�̕���
    private CinemachineTransposer CT;
    // ����VirtualCamera��CVC�R���|�[�l���g��Aim�̕���
    private CinemachineComposer CC;

    public Vector3 toEnemyVec;
    public float toEnemyAngle;

    private float velocity_v = 0;
    private float velocity_h = 0;

    // �ʏ펞�̃v���C���[�ƃJ�����̋���
    [SerializeField] float radius;
    // �J�����̍��E�����̉�]�p
    [SerializeField] float rotate_h;
    // �J�����̏㉺�����̉�]�p
    [SerializeField] float rotate_v;
    // �J�����̏㉺�����̉�]�̉���
    [SerializeField] float rotate_v_min;
    // �J�����̏㉺�����̉�]�̏��
    [SerializeField] float rotate_v_max;

    private float[] HsensiTable = { 1.00f, 1.10f, 1.30f, 1.60f, 1.80f, 2.00f, 2.30f, 2.60f, 3.00f, 3.50f, 4.00f };
    private float[] VsensiTable = { 0.50f, 0.60f, 0.70f, 0.80f, 0.90f, 1.00f, 1.15f, 1.30f, 1.50f, 1.75f, 2.00f };
    // �J�����̉�]�̊��x
    static public int Stick_sensi = 5;

    public float resetToAngle;
    public float reset_v = 0;

    public float h_offset;
    static public bool isOperateY = false;

    void Start()
    {
        MC = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainCamera>();
        // �v���C���[�擾
        Player = GameObject.FindGameObjectWithTag("Player");

        // ����VirtualCamera��CVC�R���|�[�l���g��Body��Aim�����擾
        CVC = GetComponent<CinemachineVirtualCamera>();
        CT = CVC.GetCinemachineComponent<CinemachineTransposer>();
        CC = CVC.GetCinemachineComponent<CinemachineComposer>();
        rotate_h = -91f;
        rotate_v = 2f;
    }

    public void targetEnemy()
    {
        toEnemyVec = (CVC.LookAt.position - Player.transform.position).normalized;
        toEnemyAngle = Mathf.Atan2(-toEnemyVec.x, toEnemyVec.z) * Mathf.Rad2Deg;

        rotate_h = Mathf.SmoothDampAngle(rotate_h, toEnemyAngle, ref velocity_h, 0.5f);
        rotate_v = Mathf.SmoothDampAngle(rotate_v, h_offset, ref velocity_v, 0.5f);

        CT.m_FollowOffset = calcOffset(radius, rotate_h, rotate_v);
    }

    public void targetPlayer(bool w_operation)
    {
        // W���쎞
        if (w_operation == true)
        {
            // �����ł��L�[���͂��������ꍇ��������D��
            if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || (Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f && isOperateY))
            {
                if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f)
                {
                    rotate_h += -Input.GetAxis("Horizontal") * HsensiTable[Stick_sensi];// ���X�e�B�b�N
                }
                if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f && isOperateY)
                {
                    rotate_v += -Input.GetAxis("Vertical") * VsensiTable[Stick_sensi];
                }
            }
            // �L�[���͂��S���Ȃ�������X�e�B�b�N����
            else
            {
                rotate_h += -Input.GetAxis("Mouse X") * HsensiTable[Stick_sensi];
                rotate_v += -Input.GetAxis("Mouse Y") * VsensiTable[Stick_sensi];
            }
        }
        // �ʏ펞
        else
        {
            rotate_h += -Input.GetAxis("Mouse X") * HsensiTable[Stick_sensi];
            rotate_v += -Input.GetAxis("Mouse Y") * VsensiTable[Stick_sensi];
        }

        // rotate_v��-180~180�Ő��K�����Ă���Clamp
        rotate_v = Mathf.Clamp(Mathf.Repeat(rotate_v+180.0f,360.0f)-180.0f, rotate_v_min, rotate_v_max);
        CT.m_FollowOffset = calcOffset(radius, rotate_h, rotate_v);
    }


    public void resetCamera(float reset_angle)
    {
        rotate_h = Mathf.SmoothDampAngle(rotate_h, reset_angle, ref reset_v, 0.1f);
        CT.m_FollowOffset = calcOffset(radius, rotate_h, rotate_v);
        if (Mathf.Abs(reset_v) < 5.0f)
        {
            MC.isCameraResetNow = false;
        }
    }


    public Vector3 calcOffset(float radius, float rotate_h, float rotate_v)
    {
        Vector3 offset;

        offset.x = radius * Mathf.Sin(rotate_h / 180.0f * Mathf.PI) * Mathf.Cos(rotate_v / 180.0f * Mathf.PI);
        offset.y = radius * Mathf.Sin(rotate_v / 180.0f * Mathf.PI);
        offset.z = radius * -Mathf.Cos(rotate_h / 180.0f * Mathf.PI) * Mathf.Cos(rotate_v / 180.0f * Mathf.PI);

        return offset;
    }

    public void setAngle(float h, float v)
    {
        rotate_h = h;
        rotate_v = v;
    }
}
