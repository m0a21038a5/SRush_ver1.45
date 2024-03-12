using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultScore : MonoBehaviour
{
    public Text StageClearText;
    public GameObject AllRankingText;
    public GameObject AllResultButton;
    void Start()
    {
        //�uSCORE�v�Ƃ����L�[�ŕۑ�����Ă���Float�l��ǂݍ���
        float ClearScore = PlayerPrefs.GetFloat("SCORE");
        StageClearText.text = "���Ȃ��̃X�R�A�́E�E�E" + ClearScore + "!!";
    }

    void Update()
    {
        if (Input.anyKey)
        {//�{�^�����N���b�N������A�����L���O��\��������
            StageClearText.enabled = false;
            AllRankingText.SetActive(true);
        }
    }
}
