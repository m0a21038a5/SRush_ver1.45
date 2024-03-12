using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    // �v���C���[�̑���(��)
    public bool isPlayWalkLSound = false; // ���̃X�N���v�g��ON
    // �v���C���[�̑���(�E)
    public bool isPlayWalkRSound = false; // ���̃X�N���v�g��ON
    // �v���C���[���ːi����Ƃ��̉�
    public bool isPlayRushSound = false; // �v���C���[��target�X�N���v�g��ON
    // �v���C���[�̍U����
    public bool isPlayAttackSound = false; // �v���C���[��target�X�N���v�g��ON
    // �v���C���[�̍U���q�b�g��
    public bool isPlayAttackHitSound = false; // �v���C���[��target�X�N���v�g��ON
    // �v���C���[���G�̍U���ɓ����������̉�
    public bool isPlayDamageSound = false; // �G��AttackCollision��EnemyDamage��ON
    // �v���C���[�����S�������̃W���O��
    public bool isPlayDeadSound = false;// �G��AttackCollision��EnemyDamage��ON
    // �v���C���[�̃W�����v��
    public bool isPlayJumpSound = false; // �v���C���[��move1_ver2�X�N���v�g��ON
    // �v���C���[�����ɗ��������̉�
    public bool isPlayFallSound = false; // ���̃X�N���v�g��ON
    // �v���C���[�����ɗ��������̉�
    public bool isPlayFallWaterSound = false; // ���̃X�N���v�g��ON

    // �v���C���[�̃`�F�C����
    public int playerChainCount = 0;

    // �v���C���[��targrt�X�N���v�g
    private target target;
    // �v���C���[��move1�X�N���v�g
    private move1_ver2 move1;
    // �v���C���[��Combo�X�N���v�g
    private Combo combo;

    // �^�����������p
    private bool isCollisionFloor;
    private float moveValue;
    private Vector3 beforeFramePos;
    private string footKind = "L";

    void Start()
    {
        // �v���C���[��targrt�X�N���v�g�擾
        target = GetComponent<target>();
        // �v���C���[��move1�X�N���v�g�擾
        move1 = GetComponent<move1_ver2>();
        // �v���C���[��Combo�X�N���v�g�擾
        combo = GetComponent<Combo>();
    }

    void Update()
    {
        //�����ɋ^��������������
        if (isCollisionFloor == true)
        {
            moveValue += Vector3.Magnitude(transform.position - beforeFramePos);
        }
        beforeFramePos = transform.position;

        if (moveValue >= 5.0f)
        {
            if (footKind == "L")
            {
                isPlayWalkLSound = true;
                footKind = "R";
            }
            else
            {
                isPlayWalkRSound = true;
                footKind = "L";
            }
            moveValue = 0.0f;
        }

        //�R���{�擾
        playerChainCount = combo.ComboCount;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Floor")
        {
            isCollisionFloor = true;
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Floor")
        {
            isCollisionFloor = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "FallTrigger")
        {
            isPlayFallSound = true;
        }
        if (other.transform.tag == "WaterTrigger")
        {
            isPlayFallWaterSound = true;
        }
    }
}
