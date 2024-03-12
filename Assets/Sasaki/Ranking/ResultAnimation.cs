using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;  //StreamWriter�Ȃǂ��g�����߂ɒǉ�

public class ResultAnimation : MonoBehaviour
{
    //���U���g��ʃA�j���[�V����
    private string TextRankingScoreStr = "isTextRankingScore";
    private string TextRankingTimeStr = "isTextRankingTime";
    private string TextStaffStr = "isTextStaff";
    private string TextButtonStr = "isButtonTItle";
    public GameObject RightBoxAll;
    public GameObject RightBoxRankingScore;
    public GameObject RightBoxRankingTime;
    public GameObject RightBoxStaff;
    public GameObject RightTitleButton;
    public Image[] CoinAllImage;
    public Image[] CoinDottLineAllImage;
    private Animator ResultAni;
    public int MouseClickNumber;//�}�E�X�N���b�N������
    public bool isMouseClick;
    public int[] MiniBossAllcoin;
    private Soundtest sd;//���̃X�N���v�g�擾
    private GameObject playerse;//SEPlayer�̎擾
    public bool isSoundResult01;//true�ɂ���ƁA������񂾂��炷�@true�ύX�̓A�j���[�V�������Őݒ�
    public bool isSoundResult02;//������񂾂��炷
    public bool isSoundResultCoin;//������񂾂��炷
    public bool isSoundResultRush;//������񂾂��炷
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
    {//�}�E�X�����ɂ��āA�O���͎����œ������A
        this.ResultAni = GetComponent<Animator>();
        playerse = GameObject.Find("SEPlayer");
        sd = playerse.GetComponent<Soundtest>();
        RightBoxRankingScore.SetActive(false);
        RightBoxRankingTime.SetActive(false);
        RightBoxStaff.SetActive(false);
        RightTitleButton.SetActive(false);
        isMouseClick = false;
        jsonType = loadJsonData();
        MiniBossAllcoin = jsonType.ClearCoin;
        //�R�C���̖����ɉ����ĕ\��������
        for (int i = 0; i < MiniBossAllcoin.Length; i++)
        {
            if (MiniBossAllcoin[i] == 1)
            {
                CoinAllImage[i].enabled = true;
                CoinDottLineAllImage[i].enabled = false;
            }
            else if (MiniBossAllcoin[i] == 0)
            {
                CoinAllImage[i].enabled = false;
                CoinDottLineAllImage[i].enabled = true;
            }
           
        }
    }

    void Update()
    {
        if (isMouseClick == true)
        {
            if (Input.GetKeyDown("joystick button 2") || Input.GetMouseButtonDown(0)|| Input.GetKeyDown("joystick button 1"))
            {
                MouseClickNumber++;
            }
        }
        if (MouseClickNumber == 1)
        {
            isMouseClick = true;
            this.ResultAni.SetBool(TextRankingScoreStr, true);
            RightBoxAll.SetActive(false);
            RightBoxRankingScore.SetActive(true);

        }
        if (MouseClickNumber == 2)
        {
            isMouseClick = true;
            this.ResultAni.SetBool(TextRankingTimeStr, true);
            RightBoxRankingScore.SetActive(false);
            RightBoxRankingTime.SetActive(true);
        }
        if (MouseClickNumber == 3)
        {
            isMouseClick = true;
            this.ResultAni.SetBool(TextStaffStr, true);
            RightBoxRankingTime.SetActive(false);
            RightBoxStaff.SetActive(true);
        }
        if (MouseClickNumber == 4)
        {
            isMouseClick = true;
            this.ResultAni.SetBool(TextButtonStr, true);
            RightBoxStaff.SetActive(false);
            RightTitleButton.SetActive(true);
            //�R�C���̖��������Z�b�g����
            for (int i = 0; i < MiniBossAllcoin.Length; i++)
            {
                MiniBossAllcoin[i]=0;
            }
            jsonType.ClearCoin = MiniBossAllcoin;
            var jsonBody = JsonConvert.SerializeObject(jsonType);
            JsonType outPut = JsonConvert.DeserializeObject<JsonType>(jsonBody);
            saveJsonData(jsonBody);
        }
        if (isSoundResult01 == true) //result1Player()����炷
        {
            SoundResult01();
            isSoundResult01 = false;
        }
        if (isSoundResult02 == true)// result2Player()����炷���߂̊֐�
        {
            SoundResult02();
            isSoundResult02 = false;
        }
        if (isSoundResultCoin == true)//SE_getCoinPlayer()����炷
        {
            SoundResultCoin();
            isSoundResultCoin = false;
        }
        if (isSoundResultRush == true)// kazePlayer()����炷
        {
            SoundResultRush();
            isSoundResultRush = false;
        }
    }
    //�Z�[�u���邽�߂̊֐�
    public void saveJsonData(string text)
    {
        StreamWriter writer;

        //JSON�t�@�C���ɏ�������
        writer = new StreamWriter(datapath, false);
        writer.Write(text);
        writer.Flush();
        writer.Close();
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
    public void SoundResult01()// result1Player()����炷���߂̊֐�(�A�j���[�V�������Ŏ��s�����ق�����������)
    {
        sd.result1Player();
    }
    public void SoundResult02()// result2Player()����炷���߂̊֐�(�A�j���[�V�������Ŏ��s�����ق�����������)
    {
        sd.result2Player();
    }
    public void SoundResultCoin()//SE_getCoinPlayer()����炷���߂̊֐�(�A�j���[�V�������Ŏ��s�����ق�����������)
    {
        sd.SE_getCoinPlayer();
    }
    public void SoundResultRush()// kazePlayer()����炷���߂̊֐�(�A�j���[�V�������Ŏ��s�����ق�����������)
    {
        sd.kazePlayer();
    }

}
