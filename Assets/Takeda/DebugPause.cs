using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPause : MonoBehaviour
{
    // Update���\�b�h�͖��t���[���Ăяo����܂�
    void Update()
    {
        // �ꎞ��~�L�[�i�Ⴆ��P�j�������ꂽ���m�F���܂�
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale == 1)
            {
                // �Q�[�����ꎞ��~���Ă��Ȃ���΁A�ꎞ��~���܂�
                Time.timeScale = 0;
            }
            else
            {
                // �Q�[�����ꎞ��~���Ă���΁A�ĊJ���܂�
                Time.timeScale = 1;
            }
        }
    }
}
