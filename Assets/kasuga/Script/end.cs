using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class end : MonoBehaviour
{
    // �v���C���[���ǂ��Ń��[�v�ɐG�ꂽ���𒲂ׂ�X�N���v�g
    public bool endarea=false;
    
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
            endarea = true;
            //���[�v�𓮂������߂̕ϐ�
            Ropemove.GetComponent<Ropemove>().moveEnd = true;
        }
    }
}
