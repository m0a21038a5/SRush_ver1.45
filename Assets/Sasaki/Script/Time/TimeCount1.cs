using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;  //StreamWriter�Ȃǂ��g�����߂ɒǉ�
public class TimeCount1 : MonoBehaviour
{
    //�o�ߎ��Ԃ��v������X�N���v�g�A�v���������Ԃ�RankingScoreSave.json��ClearTime�Ƃ��ĕۑ�
    //���Ԃ͕�����(string)�ŕۑ�����
    public Text timeLabel;
    public float timeCount;//timeCount �o�ߎ���
    public string timeCountstr;//timecountstr �o�ߎ��ԕ����� 
    public string timeCountstrrd;//�؂�̂ėp
    public float timeCountflo;
    public GameObject Bossobj;
    public Clear ClearSclipt;
    private JsonType jsonType = new JsonType();
    public Animator ClearUIAni;
    //�ۑ���
    string datapath;
    public bool TimeClearStop;//���Ԓ�~�pbool,�Q�[���J�n���J�E���g�_�E���ɂ��g�p���܂�
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
        timeLabel.text = "Time :" + timeCount;
        Bossobj = GameObject.Find("BOSS");
        ClearSclipt = Bossobj.GetComponent<Clear>();
        TimeClearStop = true;
        //JSON�t�@�C��������΃��[�h, �Ȃ���Ώ������֐���
       
            jsonType = loadJsonData();
            Debug.Log(jsonType.Key1);
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeClearStop == false)
        {
            timeCount += Time.deltaTime;//�o�ߎ��Ԃ𑝂₷
        }
        else if (TimeClearStop==true)//�{�X��|������^�C�}�[���~�߂�
        {
            //�o�ߎ��Ԃ𑝂₷�̂��~�߂�
        }
        timeLabel.text = "Time :" + ((int)(timeCount / 60)).ToString("00") + ":" + ((int)(timeCount % 60)).ToString("00"); //timeCount.ToString("00.00"); //������Ƃ��ĕ\��������
        timeCountstr = ((int)(timeCount / 60)).ToString("00") + ":" + ((int)(timeCount % 60)).ToString("00"); //timeCount.ToString("00.00");//������Ƃ��ĕۑ�
        timeCountstrrd = timeCount.ToString("0.00");
        timeCountflo = float.Parse(timeCountstrrd);//float�ɕϊ��ł��ĂȂ�
        if (ClearSclipt.isDead == true)//�{�X���|���ꂽ��
        {
            jsonType = loadJsonData();
            ClearUIAni.enabled=true;
            TimeClearStop = true;
            //RankingScoreSave.json������������
            jsonType.ClearTime = timeCountflo;
            var jsonBody = JsonConvert.SerializeObject(jsonType);
            JsonType outPut = JsonConvert.DeserializeObject<JsonType>(jsonBody);
            Debug.Log(outPut.ClearTime + "�ۑ����̂Q!");
            saveJsonData(jsonBody);
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
    public void Initialize()
    {
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
