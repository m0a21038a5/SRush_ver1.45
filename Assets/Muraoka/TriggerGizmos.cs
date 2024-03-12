using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGizmos : MonoBehaviour
{
    [Header("��������A���������Gizmo��\�����邾���̃X�N���v�g�ł�")]
    [Header("��������c�ԁA��������c�ATag���t���Ă��Ȃ��c��")]
    [Header("�傫����Transform�ŘM���Ă��������i�s�s�����������瑺���܂Łj")]
    [Space(12)]
    public Vector3 Size = new Vector3();

    private void OnDrawGizmos()
    {
        if (transform.tag == "FallTrigger")
        {
            Gizmos.color = new Color(1f, 0, 0, 1f);
        }
        else if (transform.tag == "WaterTrigger")
        {
            Gizmos.color = new Color(0, 0, 1f, 1f);
        }
        else
        {
            Gizmos.color = new Color(1f, 1f, 0, 1f);
        }

        Size = transform.localScale;
        Gizmos.DrawWireCube( transform.position, transform.localScale );
        Gizmos.color -= new Color(0f, 0f, 0, 0.8f);
        Gizmos.DrawCube( transform.position, transform.localScale);
    }
}
