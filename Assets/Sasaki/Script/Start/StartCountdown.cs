using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;  //StreamWriter�Ȃǂ��g�����߂ɒǉ�

public class StartCountdown : MonoBehaviour
{//�ŏ��̃X�e�[�W�J�n�J�E���g�_�E��
    public float StarTextChangeTime;//�e�L�X�g���o�����鎞��
    public Text FirstStartText;
    public Text SecondStartText;
    public Text FirstStartTextEn;//�p��Ńe�L�X�g
    public Text SecondStartTextEn;//�p��Ńe�L�X�g
    public Text CountDownText;
    public int MouseClickNumber;//�}�E�X�N���b�N������
    private bool NextText;
    private bool CountdownStart;
    private GameObject time;
    private TimeCount1 ts1;
    public bool GameStart;//�Q�[���J�n����(�ǂ��ɂ��g���ĂȂ����Ƃ肠�����J�E���g�_�E�����I�������true�ɂ��Ă�)
    public GameObject StartBalliaobj;
    public GameObject CloseTextButton;
    public GameObject CloseTextButtonEn;
    private Soundtest sd;//���̃X�N���v�g�擾
    private GameObject playerse;//SEPlayer�̎擾
    public bool isSoundResult01;//true�ɂ���ƁA������񂾂��炷�@true�ύX�̓A�j���[�V�������Őݒ�
    public bool TimeStopStart;//�J�E���g�_�E�����̎��Ԓ�~�p

    //UI�̌����ύX������
    private string CurrentLanguage;//���ݑI�����Ă��錾��
    private JsonType jsonType = new JsonType();
    //�ۑ���
    string datapath;
    void Awake()
    { //�ۑ���̌v�Z������
        //datapath = Application.dataPath + "/RankingScoreSave.json";
        //StreamingAssets�t�H���_���w��. /�ȍ~�Ƀt�@�C����RankingScoreSave.json
        //StreamingAssets�t�H���_�Ɏw�肷�邱�ƂŃr���h���Ă��g�p�\
        datapath = Application.streamingAssetsPath + "/RankingScoreSave.json";
    }
    void Start()
    {
        jsonType = loadJsonData();
        CurrentLanguage = jsonType.Language;
        playerse = GameObject.Find("SEPlayer");
        sd = playerse.GetComponent<Soundtest>();
        NextText = false;
        GameStart = false;
        CountdownStart = false;
        TimeStopStart = true;
        ts1 = GameObject.Find("Time").GetComponent<TimeCount1>();
        if (CurrentLanguage == "Japanese")
        {
            FirstStartText.enabled = true;
            FirstStartTextEn.enabled = false;
            CloseTextButton.SetActive(true);
            CloseTextButtonEn.SetActive(false);
        }
        else if (CurrentLanguage == "English")
        {
            FirstStartText.enabled = false;
            FirstStartTextEn.enabled = true;
            CloseTextButton.SetActive(false);
            CloseTextButtonEn.SetActive(true);
        }
    }

    void Update()
    { 
        if(TimeStopStart == true)
        {
            Time.timeScale = 0;
        }else if(GameStart == true || GameObject.FindGameObjectWithTag("Player").GetComponent<target>().SpecialAttack)
        {
            Time.timeScale = 1;
        }
        if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Space))
        {
            MouseClickNumber++;
        }
        if (MouseClickNumber == 1)
        {
            
            if (NextText == false)
            {
                SoundStartClose();
                StartTextAnimation();
                if (CurrentLanguage == "Japanese")
                {
                    FirstStartText.enabled = false;
                    FirstStartTextEn.enabled = false;
                    SecondStartText.enabled = true;
                    SecondStartTextEn.enabled = false;
                }
                else if (CurrentLanguage == "English")
                {
                    FirstStartText.enabled = false;
                    FirstStartTextEn.enabled = false;
                    SecondStartText.enabled = false;
                    SecondStartTextEn.enabled = true;
                }
            }

        }
        if (MouseClickNumber == 2)
        {
            EndTextAnimation();
            FirstStartText.enabled = false;
            SecondStartText.enabled = false;
            SecondStartTextEn.enabled = false;
            CloseTextButton.SetActive(false);
            CloseTextButtonEn.SetActive(false);
            if (NextText == true)
            {
                CountdownStart = true;
            }
            if (CountdownStart == true)
            {
                NextText = false;
                StartCoroutine(ReadygoCoroutine());//��񂾂��J�E���g�_�E���̏���������
            }
        }
        if (MouseClickNumber == 3)
        {
          
        }
       
    }
    void StartTextAnimation()//�e�L�X�g��L�яk�݂�����
    {
        var sequence = DOTween.Sequence().SetUpdate(true);
        sequence.Append(transform.DOScale(new Vector3(0, 1, 1), StarTextChangeTime))
            .Append(transform.DOScale(new Vector3(1, 1, 1), StarTextChangeTime));
        NextText = true;
    }
   
    void EndTextAnimation()
    {
        transform.DOScale(new Vector3(1, 1, 1), StarTextChangeTime).SetUpdate(true);
    }
    void StartGame()//�Q�[���J�n
    {
        StartCoroutine(ReadygoCoroutine());
    }
    IEnumerator ReadygoCoroutine()//�Q�[���J�n�J�E���g�_�E��
    {
        SoundStartCountdown();
        CountdownStart = false;
        CountDownText.text = "3";
       Debug.Log("3");
        yield return new WaitForSecondsRealtime(1.0f);
        CountDownText.text = "2";
        Debug.Log("2");
        yield return new WaitForSecondsRealtime(1.0f);
        CountDownText.text = "1";
        Debug.Log("1");
        yield return new WaitForSecondsRealtime(1.0f);
        CountDownText.text = "Start!";
        Debug.Log("0");
        yield return new WaitForSecondsRealtime(0.5f);
        ts1.TimeClearStop = false;
        TimeStopStart = false;
        GameStart = true;
        StartBalliaobj.SetActive(false) ;
        this.gameObject.SetActive(false);
        MouseClickNumber++;
        
    }
    public void SoundStartCountdown()// SE_321()����炷���߂̊֐�
    {
        sd.SE_321Player();
    }
    public void SoundStartClose()// SE_321()����炷���߂̊֐�
    {
        sd.SE_PlayerAttack2Player();
    }

    //JSON�t�@�C����ǂݍ���, ���[�h���邽�߂̊֐�
    public JsonType loadJsonData()
    {
        string datastr = "";
        StreamReader reader;
        reader = new StreamReader(datapath);
        datastr = reader.ReadToEnd();
        reader.Close();

        return JsonConvert.DeserializeObject<JsonType>(datastr);
    }
}

