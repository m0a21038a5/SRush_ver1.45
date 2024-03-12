using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    //Rigidbody���g�p���Ď�l���𓮂����X�N���v�g�ł�
    //����Floor�^�O�����Ă�������
    //moveSpeed ��l���̑���
    public float moveSpeed;
    private Rigidbody rb;
    private float movementLRInputValue;
    private float movementFBInputValue;
   private float JumpTimeCountUp;
    // jumpupSpeed �W�����v���ďオ�������̃X�s�[�h
    //jumpdownSpeed �W�����v�����̃X�s�[�h
    public float jumpupSpeed;
    public float jumpdownSpeed;
    private bool isJumping = false;
    public Vector3 colPosition;

    private Vector3 PlayerPos;
    private float xInput;
    private float zInput; 
    void Start()
    { 
      //  PlayerPos = GetComponent<Transform>().position;//�ŏ��̎��_�ł̃v���C���[�̃|�W�V�������擾
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
            //���E�㉺�L�[�ňړ�
            movementLRInputValue = Input.GetAxis("Horizontal");
            movementFBInputValue = Input.GetAxis("Vertical");
        Vector3 movementLR = transform.right * movementLRInputValue * moveSpeed * Time.deltaTime;
        Vector3 movementFB = transform.forward * movementFBInputValue * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + movementLR + movementFB);
       

        //�X�y�[�X�ŃW�����v
        if (Input.GetKey(KeyCode.Space) && isJumping == false)
            {
                rb.velocity = Vector3.up * jumpupSpeed;
                JumpTimeCountUp += Time.deltaTime;
                isJumping = true;
                if (JumpTimeCountUp > 3.0f)
                {
                    rb.velocity = Vector3.up * -jumpdownSpeed;
                    JumpTimeCountUp = 0;
                }
            }
        /*
        //�v���C���[�̈ړ������Ɍ����ύX
        Vector3 diff = transform.position - PlayerPos;//�v���C���[���ǂ̕����ɐi��ł��邩���킩��悤�ɁA�����ʒu�ƌ��ݒn�̍��W�������擾
        //�v���C���[�̑O��cube��u���Ċp�x�����ǐՂ�����悤�ɂ���H�H�H
        if (diff.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(diff); //�x�N�g���̏���Quaternion.LookRotation�Ɉ����n����]�ʂ��擾���v���C���[����]������
        }
        PlayerPos = transform.position;
        */
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isJumping = false;
        }
    }
   
    
}
