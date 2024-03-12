using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;  //StreamWriter�Ȃǂ��g�����߂ɒǉ�

public class MiniBossCoinStatueEnemy : MonoBehaviour
{//���{�X��|���ƃR�C������ɓ���
    [HeaderAttribute("���{�X�̏���")]
    public int MiniBossNumber;
    public int[] MiniBossAllcoin;
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
        MiniBossAllcoin = jsonType.ClearCoin;
    }

    public void getCoin()
    {
        jsonType = loadJsonData();
        MiniBossAllcoin = jsonType.ClearCoin;
        MiniBossAllcoin[MiniBossNumber] = 1;
        jsonType.ClearCoin = MiniBossAllcoin;//json�̂ق��ɔ��f�@���ꖳ���ł��܂܂łǂ�����ĕۑ����Ă���?
        var jsonBody = JsonConvert.SerializeObject(jsonType);
        JsonType outPut = JsonConvert.DeserializeObject<JsonType>(jsonBody);
        saveJsonData(jsonBody);
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
