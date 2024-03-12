using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInputDebug : MonoBehaviour
{
    void Update()
    {
        // �S�ẴL�[���`�F�b�N
        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
        {
            // �L�[��������Ă��邩�ǂ������`�F�b�N
            if (Input.GetKey(keyCode))
            {
                // �L�[�̖��O���f�o�b�O���O�ɕ\��
                Debug.Log("�L�[��������Ă��܂�: " + keyCode.ToString());
            }
        }
    }
}
