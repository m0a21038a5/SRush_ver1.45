using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_Rigidboody : MonoBehaviour
{
    //���[�v�ɂ��܂��Ă��鎞�Ɏ��R�������Ȃ��悤�ɂ��邽�߂̃X�N���v�g
    private GameObject catchRope;

    public Rigidbody rigidy;

    private void Start()
    {
        catchRope = GameObject.Find("Ropebase");
        this.rigidy = this.GetComponent<Rigidbody>();
    }
    void Update()
    {


    }

    private void OnCollisionEnter(Collision other)
    {
        if (catchRope.GetComponent<catchRope>().freezePos == true)
        {
            //���[�v�ɂ��܂��Ă��鎞�Ɏ��R�������Ȃ��悤�ɂ���
            this.rigidy.constraints = RigidbodyConstraints.FreezePosition;
            //��freezeRotation�̃`�F�b�N���O���炵���������1�xfreezeRotation�Ƀ`�F�b�N������
            this.rigidy.freezeRotation = true;
        }
    }
}
