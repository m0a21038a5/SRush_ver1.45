using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class start : MonoBehaviour
{
    // �v���C���[���ǂ��Ń��[�v�ɐG�ꂽ���𒲂ׂ�X�N���v�g
    public bool startarea = false;

    private GameObject Ropemove;
    // Start is called before the first frame update
    void Start()
    {
        Ropemove = GameObject.Find("Ropebase");
    }


    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //�v���C���[���G�ꂽ�ʒu��m��
            startarea = true;
            //���[�v�𓮂������߂̕ϐ�
            Ropemove.GetComponent<Ropemove>().moveStart = true;
        }
    }
}
