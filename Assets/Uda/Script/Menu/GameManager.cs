using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //���j���[Canvas
    public GameObject Menu;
    //�������Canvas
    public GameObject Tutorial;
    //�\������Ă����ʂ̏��
    public int MenuCount = 0;
    //player�p
    [SerializeField]
    GameObject Player;
    //Retry�m�F���
    public GameObject Retry;
    //ReturnToTitle�m�F���
    public GameObject RTT;
    //���ʒ��߉��
    public GameObject Sd;
    //�J�������߉��
    public GameObject Cd;
    //BGM���ʒ���Slider
    public  GameObject BGMSlider;
    //SE���ʒ���Slider
    public GameObject SESlider;
    
    Soundtest SE;
    BGMPlayer BGM;
    //�ߐړG�p�̔z��
    GameObject[] Statues;
  
    public MainCamera mc;
    GameObject[] BeamObjects;

    public GameObject Boss;
    Clear c;

    public GameObject StartCountDown;
    StartCountdown SCD;
    public bool isDisplay;

    // Start is called before the first frame update
    void Start()
    {
        //���j���[��\��
        Menu.SetActive(false);
        //���������ʔ�\��
        Tutorial.SetActive(false);
        //Retry�m�F��ʔ�\��
        Retry.SetActive(false);
        //ReturnToTitle�m�F��ʔ�\��
        RTT.SetActive(false);
        //���ʒ��߉�ʔ�\��
        Sd.SetActive(false);
        //���ʒ��߉�ʔ�\��
        Cd.SetActive(false);

        SE = GameObject.Find("SEPlayer").GetComponent<Soundtest>();
        BGM = GameObject.Find("BGMPlayer").GetComponent<BGMPlayer>();
        SE.menuObj = Menu;
        SE.menuObj2 = Tutorial;
        SE.menuObj3 = RTT;
        SE.menuObj4 = Sd;// Cd������Ƃ�����
        SE.p = Player.GetComponent<PlayerSounds>();
        BGM.p = Player.GetComponent<PlayerSounds>();

        BGM.volumeSlider = BGMSlider.GetComponent<Slider>();
        SE.volumeSlider = SESlider.GetComponent<Slider>();

        BGM.volumeSlider.value = BGM.volumeSlider.maxValue = 1.0f;
        BGM.volumeSlider.minValue = 0.0f;

        SE.volumeSlider.value = SE.volumeSlider.maxValue = 1.0f;
        SE.volumeSlider.minValue = 0.0f;

        c = Boss.GetComponent<Clear>();
        mc.enabled = true;
        //�}�E�X�J�[�\����\���������ɌŒ�----------------�����ǉ�----------------
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SCD = StartCountDown.GetComponent<StartCountdown>();
        isDisplay = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Escape�L�[��1�x��������
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 9")) && MenuCount == 0)
        {
            //���j���[�\��
            Menu.SetActive(true);
            // ���ʉ�
            SE.positive1Player();
        }
        //Escape�L�[��2�x�������Ƃ�
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 9")) && MenuCount == 1)
        {
            //���j���[��\��
            Menu.SetActive(false);
            Time.timeScale = 1;
            //�}�E�X�J�[�\����\���������ɌŒ�----------------�����ǉ�----------------
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            // ���ʉ�
            SE.negative1Player();
        }
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 9")) && MenuCount == 2)
        {
            //���j���[�\��
            Menu.SetActive(true);
            
            //���������\��
            Tutorial.SetActive(false);
           
            //Retry�m�F��ʔ�\��
            Retry.SetActive(false);
           
            //ReturnToTitle�m�F��ʔ�\��
            RTT.SetActive(false);

            //���ʒ��߉�ʔ�\��
            Sd.SetActive(false);

            //�J�������߉�ʔ�\��
            Cd.SetActive(false);

            // ���ʉ�
            SE.negative1Player();
        }
        //���j���[���\������Ă���ꍇ
        if (Menu.activeSelf == true)
        {
            MenuCount = 1;
            Time.timeScale = 0;
            BGM.aisac2 = 1;
         
            //���������\��
            Tutorial.SetActive(false);
            //Retry�m�F��ʔ�\��
            Retry.SetActive(false);
            //ReturnToTitle�m�F��ʔ�\��
            RTT.SetActive(false);
            //ReturnToTitle�m�F��ʔ�\��
            Sd.SetActive(false);
            //ReturnToTitle�m�F��ʔ�\��
            Cd.SetActive(false);
            mc.enabled = false;
            //�}�E�X�J�[�\���\�������R�Ɉړ��\----------------�����ǉ�----------------
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        //���j���[��ʂ���h��������ʂ��\������Ă���ꍇ
        if (isDisplay == true || SCD.GameStart == false || Tutorial.activeSelf == true || Retry.activeSelf == true || RTT.activeSelf == true || Sd.activeSelf == true || Cd.activeSelf == true)
        {
            MenuCount = 2;
            BGM.aisac2 = 1;

            Time.timeScale = 0;
            mc.enabled = false;
            //�}�E�X�J�[�\���\�������R�Ɉړ��\----------------�����ǉ�----------------
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        
      
        //���j���[�Ƒ��������ʂǂ�����\������Ă��Ȃ��ꍇ
        if (isDisplay == false &&SCD.GameStart == true && Menu.activeSelf == false && Tutorial.activeSelf == false && Retry.activeSelf == false && RTT.activeSelf == false && Sd.activeSelf == false && Cd.activeSelf == false)
        {
            MenuCount = 0;
            BGM.aisac2 = 0;

            mc.enabled = true;
            //�}�E�X�J�[�\����\���������ɌŒ�----------------�����ǉ�----------------
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            if (c.isDead == false)
            {
                Time.timeScale = 1;
            }
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void UnPause()
    {
        Time.timeScale = 1;
    }
}
