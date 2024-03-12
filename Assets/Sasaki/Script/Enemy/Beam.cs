using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    private Rigidbody rb;
    public MeshRenderer BeamMesh;
    public TrailRenderer BeamLineMesh;
    public float StopPlayer;
    //�r�[���ɓ���������v���C���[�̓������~�߂�
    void Start()
    {
        GameObject PlayerObject = GameObject.FindGameObjectWithTag("Player");
        rb = PlayerObject.GetComponent<Rigidbody>();


    }

    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Player")
        {//�v���C���[�̓������~�߂�
            StartCoroutine(StopPlayerCoroutine());
            //�v���C���[��������Ə�ɋ�����@���܂�h�~

        }else if (other.gameObject.tag == "Enemy")
        {
            
        }
        else if (other.gameObject.tag == "Floor")
        {
           // Destroy(this.gameObject);
        }

    }

    IEnumerator StopPlayerCoroutine()
    {
        BeamMesh.enabled = false;
        BeamLineMesh.enabled = false;
        rb.isKinematic = true;
        yield return new WaitForSecondsRealtime(StopPlayer);
        //Debug.Log("�ĉ�");
        rb.isKinematic = false;
        Destroy(this.gameObject);
    }
}
