using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialClear : MonoBehaviour
{

    public float delayTime = 1.2f; 

    public BeamHPManager bhpm;
    void Start()
    {
        
    }

    void Update()
    {
        if (bhpm.HP <= 0)
        {
            StartCoroutine(BeforeLoading(delayTime)); ///�[�J�ǉ�
            //SceneManager.LoadScene("Map 1");
        }
    }


    //////////////////�[�J�ǉ�/////////////////////
    
    private IEnumerator BeforeLoading(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Map 1");
    }

    //�{�X���j��A�{�X�j��A�j���[�V�����������Ă���
    //scene�J�ڂ���悤�ɒǉ����܂������A
    //�t���[�Y����ꍇ�́u�[�J�ǉ��v�̍s��
    //�u//SceneManager.LoadScene("Map 1");�v�́u//�v��
    //�����Ă��������B
    //�^�C�~���O���ς������琔�l�ς��Ă����v�ł����A
    //2.0f�Ȃǒl��傫������ƃt���[�Y����悤�ł�

    /////////////////�[�J�����܂�/////////////////////
}
