using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickAnim : MonoBehaviour
{
    public float moveSpeed = 33f; // ��Ɉړ����鑬�x�i�s�N�Z��/�b�j
    public float moveDistance = 13f; // �ړ����鋗���i�s�N�Z���j
    private Vector3 initialPosition;
    private bool movingUp = true;

    private void Start()
    {
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        // ��Ɉړ����邩�A���ɖ߂邩������
        if (movingUp)
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

            // �ړ�������moveDistance�𒴂����牺�ɖ߂�
            if (transform.localPosition.y - initialPosition.y >= moveDistance)
            {
                movingUp = false;
            }
        }
        else
        {
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

            // �����ʒu�ɖ߂�����Ăя�Ɉړ�
            if (transform.localPosition.y <= initialPosition.y)
            {
                movingUp = true;
            }
        }
    }
}