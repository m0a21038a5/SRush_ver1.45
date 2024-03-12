using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;  //StreamWriter�Ȃǂ��g�����߂ɒǉ�


public class TitleUIButton : MonoBehaviour
{
    //�^�C�g����UI�p�X�N���v�g
    //�ݒ�{�^���N���ƌ���ύX
    public GameObject ConfigAll;//�ݒ�{�^�����ׂ�
    public GameObject FirstTitleButton;//�ŏ��ɕ\�������{�^��
    public GameObject StuffAll;//�X�^�b�t�N���W�b�g�S��
    public GameObject LanguageAll;//����ݒ�S��
    public Button FirstSelectButton;//�ŏ��ɑI�������{�^��
    public Button ConfigSelectButton;//�ݒ��ʂőI�������{�^��
    public Button LangageButton;//����I����ʂőI�������{�^��
    public Button StaffCloseButton;//�X�^�b�t�N���W�b�g�őI�������{�^��
    public GameObject[] JapaneseObjects;//���{��e�L�X�g
    public Text[] JapaneseTexts;//���{��e�L�X�g
    public GameObject[] EnglishObjects;//�p��Ńe�L�X�g
    public Text[] EnglishTexts;//�p��Ńe�L�X�g
    private bool StuffPanelClose;//�X�^�b�t�N���W�b�g���\����������
    private bool StuffPanel;//�X�^�b�t�N���W�b�g���\����������
    public string CurrentLanguage;//���ݑI�����Ă��錾��
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
        //AcquisitionEnglish();
        //AcquistionJapanese();
        jsonType = loadJsonData();
        CurrentLanguage = jsonType.Language;
    }
    void Update()
    {
        if(CurrentLanguage== "Japanese")
        {
            for (int i = 0; i < JapaneseObjects.Length; i++)
            {
                JapaneseTexts[i].enabled = true;
                EnglishTexts[i].enabled = false;
            }
        }
        else if (CurrentLanguage == "English")
        {
            for (int i = 0; i < EnglishObjects.Length; i++)
            {
                JapaneseTexts[i].enabled = false;
                EnglishTexts[i].enabled = true;
            }
        }
        
    }
    //�ݒ�{�^�����N���b�N�����Ƃ��̏���
    public void OnClickConfigButton() 
    {
        FirstTitleButton.SetActive(false);
        LanguageAll.SetActive(false);
        ConfigAll.SetActive(true);
        ConfigSelectButton.Select();
    }
    //����ݒ�{�^�����N���b�N�����Ƃ��̐ݒ�
    public void OnClickLanguageButton()
    {
        ConfigAll.SetActive(false);
        LanguageAll.SetActive(true);
        LangageButton.Select();
    }
    //�J���҃{�^�����N���b�N�����Ƃ��̏���
    public void OnClickStuffButton()
    {
        StuffPanel = true;
        ConfigAll.SetActive(false);
        StuffAll.SetActive(true);
        StaffCloseButton.Select();
    }
    //�߂�{�^�����������Ƃ��̏���
    public void OnClickReturnButton()
    {
        ConfigAll.SetActive(false);
        FirstTitleButton.SetActive(true);
        FirstSelectButton.Select();
    }
    //�X�^�b�t�N���W�b�g����鏈��
    public void CloseStuff()
    {
        StuffAll.SetActive(false);
        ConfigAll.SetActive(true);
        StuffPanel = false;
        StuffPanelClose = false;
        ConfigSelectButton.Select();
    }
    //���{��ɕύX�����Ƃ��̏���
    public void ChangeLanguageJapanese()
    {
        jsonType = loadJsonData();
        CurrentLanguage = "Japanese";
        jsonType.Language = CurrentLanguage;
        var jsonBody = JsonConvert.SerializeObject(jsonType);
        JsonType outPut = JsonConvert.DeserializeObject<JsonType>(jsonBody);
        saveJsonData(jsonBody);
        for (int i = 0; i < JapaneseObjects.Length; i++)
        {
            JapaneseTexts[i].enabled = true;
            EnglishTexts[i].enabled = false;
        }
    }
    //�p��ɕύX�����Ƃ��̏���
    public void ChangeLanguageEnglish()
    {
        jsonType = loadJsonData();
        CurrentLanguage = "English";
        jsonType.Language = CurrentLanguage;
        var jsonBody = JsonConvert.SerializeObject(jsonType);
        JsonType outPut = JsonConvert.DeserializeObject<JsonType>(jsonBody);
        saveJsonData(jsonBody);
    }
    //TextJapanese�^�O�̂����e�L�X�g���擾 �擾�ł��Ȃ�? ���Ԃ��Ȃ��̂Ŏ蓮��
    void AcquistionJapanese()
    {
        JapaneseObjects = GameObject.FindGameObjectsWithTag("TextJapanese");
        for (int i = 0; i < JapaneseObjects.Length; i++)
        {
            JapaneseTexts[i] = JapaneseObjects[i].GetComponent<Text>();
        }
        
    }
    //TextJapanese�^�O�̂����e�L�X�g���擾
    void AcquisitionEnglish()
    {
        EnglishObjects = GameObject.FindGameObjectsWithTag("TextEnglish");
        for (int i = 0; i < EnglishObjects.Length; i++)
        {
            EnglishTexts[i] = EnglishObjects[i].GetComponent<Text>();
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
}
