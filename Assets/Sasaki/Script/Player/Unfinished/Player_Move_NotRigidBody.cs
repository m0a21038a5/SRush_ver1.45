using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move_NotRigidBody : MonoBehaviour
{
    //Rigidbody���g�p���Ȃ��Ŏ�l���𓮂����X�N���v�g�ł��B�W�����v�������ł��Ă܂���
    private Vector3 moveX;
    private Vector3 moveZ;
    private Vector3 moveY;
    // MoveSpeed �v���C���[�̑���
    public float MoveSpeed;
    Vector3 target;
    Rigidbody rd;
    private Vector3 PlayerPos;
    private float JumpTimeCountUp;
    // jumpupSpeed �W�����v���ďオ�������̃X�s�[�h
    //jumpdownSpeed �W�����v�����̃X�s�[�h
    public float jumpupSpeed;
    public float jumpdownSpeed;
    private bool isJumping = false;
    void Start()
    {
        target = transform.position;
        rd = GetComponent<Rigidbody>();
        rd.useGravity = false;
        rd.isKinematic = true;
        moveX = new Vector3(1.0f, 0.0f, 0.0f);
        moveZ  = new Vector3(0.0f, 0.0f, 1.0f);
        moveY = new Vector3(0.0f, 1.0f, 0.0f);
    }

    void Update()
    {
        TargetPosition();
        transform.position = Vector3.Lerp(transform.position, target, MoveSpeed * Time.deltaTime);
        //�X�y�[�X�L�[�������ĕb�Ԃ̓W�����v����
        JumpUp();
        JumpDown();
        /*
        //�v���C���[�̈ړ������Ɍ����ύX
        Vector3 diff = transform.position - PlayerPos;//�v���C���[���ǂ̕����ɐi��ł��邩���킩��悤�ɁA�����ʒu�ƌ��ݒn�̍��W�������擾
        PlayerPos = transform.position;
        if (diff.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(diff); //�x�N�g���̏���Quaternion.LookRotation�Ɉ����n����]�ʂ��擾���v���C���[����]������
        }*/
    }
    void TargetPosition()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            target = transform.position + moveX;
            return;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            target = transform.position - moveX;
            return;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            target = transform.position + moveZ;
            return;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            target = transform.position - moveZ;
            return;
        }
        if (Input.GetKey(KeyCode.Space) && isJumping == false)
        {
            target = transform.position + moveY;
            JumpTimeCountUp += Time.deltaTime;
            isJumping = true;
            if (JumpTimeCountUp > 3.0f)
            {
                target = transform.position - moveY;
                JumpTimeCountUp = 0;
            
                return;
            }
            return;
        }
    }
    void JumpUp()
    {
        transform.position = Vector3.Lerp(transform.position, target, jumpupSpeed * Time.deltaTime);
    }
    void JumpDown()
    {
        transform.position = Vector3.Lerp(transform.position, target, jumpdownSpeed * Time.deltaTime);
    }

}
