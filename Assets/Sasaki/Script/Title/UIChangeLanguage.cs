using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;  //StreamWriter�Ȃǂ��g�����߂ɒǉ�

public class UIChangeLanguage : MonoBehaviour
{
    //UI�̌����ύX������
    public Text[] JapaneseTexts;//���{��e�L�X�g
    public Text[] EnglishTexts;//�p��Ńe�L�X�g
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
        jsonType = loadJsonData();
        CurrentLanguage = jsonType.Language;
    }

    void Update()
    {
        if (CurrentLanguage == "Japanese")
        {
            for (int i = 0; i < JapaneseTexts.Length; i++)
            {
                JapaneseTexts[i].enabled = true;
                EnglishTexts[i].enabled = false;
            }
        }
        else if (CurrentLanguage == "English")
        {
            for (int i = 0; i < EnglishTexts.Length; i++)
            {
                JapaneseTexts[i].enabled = false;
                EnglishTexts[i].enabled = true;
            }
        }
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
