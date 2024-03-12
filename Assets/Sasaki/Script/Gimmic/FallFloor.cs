using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallFloor : MonoBehaviour
{//��ɏ��Ɨ����鏰�ł��A���Ԍo�߂ŕ������܂� Player��Player�^�O�����Ȃ��Ɠ��삵�܂���
    //FallSpeed �����鑬�� �@FallDistance ������I�_  FallStart �����ʒu  FallRevivalFloorTime ��������
    public float FallDistance;
    public float FallSpeed;
    public float FallStart;
    public float FallRevivalFloorTime;
    public bool isStopAbilityFallFloor;
    private Vector3 Floorpos;
    public bool FallStartNow;
    void Start()
    {
        Floorpos = transform.position;
    }

    void Update()
    {
        if (FallStartNow == true)
        {
            FallFloorDrop();
        }
        else if (FallStartNow == false)
        {

        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            FallStartNow = true;
        }
    }
    void FallFloorDrop()
    {
        // FallDistance�������ɗ�����
        Floorpos.y -= Time.deltaTime * FallSpeed;
        transform.position = Floorpos;
        // FallDistance�ɂȂ����猩���Ȃ�����
        if (Floorpos.y < FallDistance)
        {
            this.gameObject.SetActive(false);
            // 3�b��ɕ�������
            Invoke("Revival", FallRevivalFloorTime);
        }
    }
    void Revival()
    {
        //���̃��]�b�g���Ăяo���ꂽ�畜��
        this.gameObject.SetActive(true);
        Floorpos.y = FallStart;
        transform.position = Floorpos;
        FallStartNow = false;
    }
}
