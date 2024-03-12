using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    //�������Canvas
    public GameObject Tutorial;
    //���j���[Canvas
    public GameObject Menu;
    //Retry�m�F���
    public GameObject Retry;
    //ReturnToTitle�m�F���
    public GameObject RTT;
    //���ʒ��߉��
    public GameObject Sd;
    //�J�������߉��
    public GameObject Cd;

    Soundtest st;

    // Start is called before the first frame update

    void Start()
    {
        st = GameObject.Find("SEPlayer").GetComponent<Soundtest>();
    }
    //Retry�{�^�����������Ƃ�
    public void ToRePlay()
    {
        //Retry�m�F��ʕ\��
        Retry.SetActive(true);
        //Menu��ʔ�\��
        Menu.SetActive(false);
        st.positive1Player();
    }

    //ReturnToTitle�{�^������������
    public void ToTitle()
    {
        //ReturnToTitle�m�F��ʕ\��
        RTT.SetActive(true);
        //Menu��ʔ�\��
        Menu.SetActive(false);
        st.positive1Player();
    }

    //Tutorial�{�^�����������Ƃ�
    public void ToTutorial()
    {
        //���������ʂ�\��
        Tutorial.SetActive(true);
        //���j���[��ʂ��\��
        Menu.SetActive(false);
        st.positive1Player();
    }

    //���������ʂŖ߂�{�^�����������Ƃ�
    public void ReturnToMenu()
    {
        //���������ʂ��\��
        Tutorial.SetActive(false);
        //���j���[��ʂ�\��
        Menu.SetActive(true);
        st.negative1Player();
    }

    //���j���[��ʂŁ~����������Ƃ�
    public void closeMenu()
    {
        //���j���[��ʂ��\��
        Menu.SetActive(false);
        Time.timeScale = 1;
        st.negative1Player();
    }

    //Retry���܂�
    public void Retry_Y()
    {
        //���݂̃V�[�����ēǂݍ���
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
        st.SE_ButtonPlayer();
    }

    //Retry���܂���
    public void Retry_N()
    {
        //Retry�m�F��ʔ�\��
        Retry.SetActive(false);
        //Menu��ʕ\��
        Menu.SetActive(true);
        st.negative1Player();
    }

    //�^�C�g���ɖ߂�
    public void RTT_Y()
    {
        //Title�V�[���֑J��
        SceneManager.LoadScene("Title 1");
        Time.timeScale = 1;
        st.SE_ButtonPlayer();
        
    }

    //�^�C�g���ɖ߂�Ȃ�
    public void RTT_N()
    {
        //ReturnToTitle�m�F��ʔ�\��
        RTT.SetActive(false);
        //Menu��ʕ\��
        Menu.SetActive(true);
        st.negative1Player();
    }

    public void ToSd()
    {
        //���j���[��ʔ�\��
        Menu.SetActive(false);
        //���ʒ��߉�ʕ\��
        Sd.SetActive(true);
        st.positive1Player();
    }

    public void closeSd()
    {
        //���j���[��ʕ\��
        Menu.SetActive(true);
        //���ʒ��߉�ʔ�\��
        Sd.SetActive(false);
        st.negative1Player();
    }

    public void ToCd()
    {
        //���j���[��ʔ�\��
        Menu.SetActive(false);
        //���ʒ��߉�ʕ\��
        Cd.SetActive(true);
        st.positive1Player();
    }

    public void closeCd()
    {
        //���j���[��ʕ\��
        Menu.SetActive(true);
        //���ʒ��߉�ʔ�\��
        Cd.SetActive(false);
        st.negative1Player();
    }

    public void playChoicesSE()
    {
        st.SE_TargetLockedPlayer();
    }
}
