using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Rope_Action : MonoBehaviour
{
    //���V�t�g�Őe�q�֌W�̉����ƃL�[���͂œ�����悤�ɂ���
    private GameObject Player;
    private GameObject catchRope;
    public Rigidbody rigidy;
    private GameObject Ropemove;
    private GameObject Playermove;
    // Start is called before the first frame update
    void Start()
    {
        Playermove = GameObject.Find("Player");
        Player = GameObject.Find("Player");
        catchRope = GameObject.Find("Ropebase");
        Ropemove = GameObject.Find("Ropebase");
        this.rigidy = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            //�e�q�֌W�̉���
            Player.gameObject.transform.parent = null;
            //FreezePosition�̃I���ƃI�t��؂�ւ���ϐ�
            catchRope.GetComponent<catchRope>().freezePos = false;
            //���[�v�𓮂������߂̕ϐ�
            Ropemove.GetComponent<Ropemove>().moveStart = false;
            Ropemove.GetComponent<Ropemove>().moveEnd = false;
            //�d�͂������悤�ɂ���
            this.rigidy.constraints = RigidbodyConstraints.None;
            //��freezeRotation�̃`�F�b�N���O���炵���������1�xfreezeRotation�Ƀ`�F�b�N������
            this.rigidy.freezeRotation = true;
            //�L�[���͂ŃL�����𓮂�����悤�ɂ���
            Playermove.GetComponent<Player_Move>().enabled = true;
        }
    }


}
