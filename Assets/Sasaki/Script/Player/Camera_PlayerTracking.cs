using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_PlayerTracking : MonoBehaviour
{
    //CameraTarget �J�������ǐՂ���Ώ�
    public GameObject CameraTarget;
    private Vector3 offset;

    void Start()
    {
        offset = transform.position - CameraTarget.transform.position;
    }

    void Update()
    {
        //�J�������ǐՂ���
        transform.position = CameraTarget.transform.position + offset;
    }
}
