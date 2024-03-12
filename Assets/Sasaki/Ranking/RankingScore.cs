using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;  //StreamWriter�Ȃǂ��g�����߂ɒǉ�

public class RankingScore : MonoBehaviour
{//�����L���O�̃X�N���v�g�@���o�Ƃ��͂܂��@�\�������@
    public float[] ClearScoreList;
    public float[] ClearScoreRanking;
    public GameObject AllResultButton;
    List<float> RankingList = new List<float>();//���X�g
    List<float> RankingListTime = new List<float>();//�^�C�����X�g
    public float[] RankingListSave;//�����L���O�ۑ�
    public float[] RankingTimeSave;//�^�C���ۑ�
    private int PlayerRank;//�v���C���[�̏���
    public Text RankingText;
    public Text RankingTimeText;
    public Text RankingTimeTextPlayer;
    public Text PlayerText;
    private JsonType jsonType = new JsonType();
    public GameObject RankingAll;
    public GameObject Stafroll;
    public bool StafrollOn;
    private bool AllKeyUp;
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
        Debug.Log(jsonType.Key1);
        RankingListSave = jsonType.Key1;
        RankingTimeSave = jsonType.ClearScoreRanking;
        //JSON�t�@�C��������΃��[�h, �Ȃ���Ώ������֐���
        if (FindJsonfile())
        {
            jsonType= loadJsonData();
            Debug.Log(jsonType.Key1);
        }
        else
        {
            Initialize();
        }
        //�uSCORE�v�Ƃ����L�[�ŕۑ�����Ă���Float�l��ǂݍ���
        float ClearScoreNow = PlayerPrefs.GetFloat("SCORE");
        //�uClearTime�v�ŕۑ�����Ă���Float�l��ǂݍ���
        float ClearTimeNow = jsonType.ClearTime;
        // ClearScoreList�ɕۑ�����Ă��郉���L���O��ǂݎ����
        //ClearScoreRanking�z���ClearScoreNow��ClearScoreList������
        RankingList.AddRange(RankingListSave);
        RankingList.Add(ClearScoreNow);
        RankingListTime.AddRange(RankingTimeSave);
        RankingListTime.Add(ClearTimeNow);

        //���X�g���~���ɕ��בւ���
        RankingList.Sort();
        RankingList.Reverse();
        RankingListTime.Sort();

        //�X�N���[�������o�[�W�����A5�ʂ܂ŕ\�������邽�߂�6�ȉ���؂�̂�
        RankingList.RemoveAt(5);
        RankingListTime.RemoveAt(5);
        //���בւ����z���\��������
        //���X�g
        for (int i = 0; i < RankingList.Count; i++)
        {
            //RankingText.text =RankingText.text +"\n"+ (i+1) + "�ʁ@Score�F" + RankingList[i] + "!!"; //�e�L�X�g�̏㏑��
            RankingText.text = RankingText.text + "\n" + (i + 1) + "�@" + RankingList[i]; //�e�L�X�g�̏㏑��
        }
        for (int i = 0; i < RankingListTime.Count; i++)
        {
            //RankingTimeText.text = RankingTimeText.text + "\n" + (i + 1) + "�ʁ@Score�F" + RankingListTime[i] + "!!"; //�e�L�X�g�̏㏑��
            RankingTimeText.text = RankingTimeText.text + "\n" + (i + 1) + "�@"+((int)(RankingListTime[i] / 60)).ToString("00") + ":" + ((int)(RankingListTime[i] % 60)).ToString("00"); //�e�L�X�g�̏㏑��
         
        }

        //�z���ۑ�����
        //listRankingList��z��ɕϊ���RankingListSave�ɕۑ�
        //RankingList��ۑ�
        RankingListSave = RankingList.ToArray();
        jsonType.Key1 = RankingListSave;
        Debug.Log(jsonType.Key1 + "�ۑ�����0!");
        /*var jsonBody = JsonConvert.SerializeObject(jsonType);
        Debug.Log(jsonBody+"�ۑ����̂P!");
        JsonType outPut = JsonConvert.DeserializeObject<JsonType>(jsonBody);
        Debug.Log(outPut.Key1+"�ۑ����̂Q!");
        saveJsonData(jsonBody);*/
        //�z���ۑ�����
        //listRankingList��z��ɕϊ���RankingListSave�ɕۑ�
        //RankingList��ۑ�
        RankingTimeSave = RankingListTime.ToArray();
        jsonType.ClearScoreRanking = RankingTimeSave;
        var jsonBody = JsonConvert.SerializeObject(jsonType);
        JsonType outPut = JsonConvert.DeserializeObject<JsonType>(jsonBody);
        saveJsonData(jsonBody);
        PlayerText.text = ClearScoreNow.ToString();
        //RankingTimeTextPlayer.text = ClearTimeNow.ToString();
        RankingTimeTextPlayer.text = ((int)(ClearTimeNow / 60)).ToString("00") + ":" + ((int)(ClearTimeNow % 60)).ToString("00");
        /*
       //�uSCORE�v�Ƃ����L�[�ŕۑ�����Ă���Float�l��RankingList�Ɠ����X�R�A��T����
       //���Ԃ��擾,IndexOf�͑��݂��Ȃ���-1��Ԃ�
       PlayerRank = RankingList.IndexOf(ClearScoreNow) + 1;//���X�g�̏�����
       //���Ȃ��̏��ʂ́�����!�ƕ\��������A�����L���O���O�̏ꍇ�̓X�R�A��\������
       if (PlayerRank >= 1)
       {
           PlayerText.text = "���Ȃ��̏��ʂ�" + PlayerRank + "��!!";
       }
       else if (PlayerRank == 0){
           PlayerText.text = "���Ȃ��̃X�R�A��" + ClearScoreNow + "!!";
       }*/
    }

    void Update()
    {/*
        if (StafrollOn == false)
        {//�{�^�����N���b�N������A�X�^�b�t���[����\��������
            if (Input.anyKey)
            {
                RankingAll.SetActive(false);
                Stafroll.SetActive(true);
                AllKeyUp = true;
            }
            if (!Input.anyKey&& AllKeyUp==true)
            {
                StafrollOn = true;
            }
        }
        if (StafrollOn == true)
        {//�{�^�����N���b�N������A���U���g�{�^����\��������
            if (Input.anyKey)
            {
                AllResultButton.SetActive(true);
                RankingAll.SetActive(false);
                Stafroll.SetActive(false);
            }
        }*/
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
    //JSON�t�@�C�����Ȃ��ꍇ�ɌĂяo���������֐�
    //�����l���Z�[�u��, JSON�t�@�C���𐶐�����
    public void Initialize()
    {
        jsonType.Key1 = RankingListSave;
        jsonType.ClearScoreRanking = RankingTimeSave;
        var jsonBody = JsonConvert.SerializeObject(jsonType);

        saveJsonData(jsonBody);
    }
    //JSON�t�@�C���̗L���𔻒肷�邽�߂̊֐�
    public bool FindJsonfile()
    {//StreamingAssets�t�H���_���w��. /�ȍ~�Ƀt�@�C����RankingScoreSave.json
        string filePath = Application.streamingAssetsPath + "/RankingScoreSave.json";
        return File.Exists(filePath);
    }
}


public class JsonType
{
    [JsonProperty("Key1")]
    public float[] Key1 { get; set; }
    [JsonProperty("ClearTime")]//�N���A�^�C���ۑ��p
    public float ClearTime { get; set; }
    [JsonProperty("ClearScore")]//�N���A�X�R�A�ۑ��p
    public float ClearScore { get; set; }
    [JsonProperty("ClearScoreRanking")]//�N���A�^�C�������L���O�ۑ��p
    public float[] ClearScoreRanking { get; set; }
    [JsonProperty("ClearCoin")]
    public int[] ClearCoin { get; set; }
    [JsonProperty("Language")]
    public string Language { get; set; }
}
