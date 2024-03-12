
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [HeaderAttribute("���݂̃X�R�A")]
    public float ScoreCount = 0;//���݂̃X�R�A
    [SerializeField]
    [HeaderAttribute("�n��G��|�������ɖႦ��X�R�A")]
    public float StatueEnemyScore; //�n��G��|�������A1�R���{�̏�ԂŖႦ��X�R�A
    [SerializeField]
    [HeaderAttribute("�r�[���G��|�������ɖႦ��X�R�A")]
    public float BeamEnemyScore; //�r�[���G��|�������A1�R���{�̏�ԂŖႦ��X�R�A
    [SerializeField]
    [HeaderAttribute("�{�X��|�������ɖႦ��X�R�A")]
    public float BossEnemyScore; //�{�X��|�������A1�R���{�̏�ԂŖႦ��X�R�A
    [SerializeField]
    [HeaderAttribute("�R���{�̃X�R�A�㏸�{��")]
    public float[] ComboScoreMultiplier; //�R���{�̃X�R�A�㏸�{��
    //�X�R�A�̃e�L�X�g
    public Text ScoreText;
    //�R���{�̎擾
    [HeaderAttribute("���݂̃R���{")]
    public int ComboScoreCount = 0;//���݂̃R���{
                                   //�v���C���[�擾
    private GameObject PlayerCombo;
    //�X�N���v�g�擾
    Combo comboScript;
    //target�X�N���v�g�擾
    target t;

    //�ő�X�R�A����
    public bool MaxScoreMultiplier;
    //�ő�X�R�A
    public int CAMM;




    void Start()
    {
        PlayerCombo = GameObject.Find("Player");
        comboScript = PlayerCombo.GetComponent<Combo>();
        t = GameObject.FindGameObjectWithTag("Player").GetComponent<target>();
        CAMM = comboScript.ComboAttackMaxMagnification;
       

    }

    void Update()
    {
        ScoreText.GetComponent<Text>().text = "Score:" + ScoreCount;
       


        if (comboScript.SpecialMode)
        {
            ScoreText.GetComponent<Text>().enabled = false;
        }
        else
        {
            ScoreText.GetComponent<Text>().enabled = true;
        }
    }



    public void OnCollisionEnter(Collision collision)
    {
        //�^�OStatue�̕t�����G�ɓ����������A�X�R�A�𑝂₷�����s
        if (collision.gameObject.tag == "Statue" && t.Attack)
        {
            if (comboScript.ComboCount < (CAMM - 1))
            {
                ScoreCount = ScoreCount + StatueEnemyScore * ComboScoreMultiplier[comboScript.ComboCount];
                //�uSCORE�v�Ƃ����L�[�ŁAInt�l�́u ScoreCount �v��ۑ�
                PlayerPrefs.SetFloat("SCORE", ScoreCount);
                PlayerPrefs.Save();
            }
            else if (comboScript.ComboCount >= (CAMM - 1))
            {
                ScoreCount = ScoreCount + StatueEnemyScore * ComboScoreMultiplier[CAMM - 1];
                //�uSCORE�v�Ƃ����L�[�ŁAInt�l�́u ScoreCount �v��ۑ�
                PlayerPrefs.SetFloat("SCORE", ScoreCount);
                PlayerPrefs.Save();
            }
           
        }
        //�^�OBeam�̕t�����G�ɓ����������A�X�R�A�𑝂₷�����s
        if (collision.gameObject.tag == "Beam" && t.Attack)
        {
            if (comboScript.ComboCount < (CAMM - 1))
            {
                ScoreCount = ScoreCount + StatueEnemyScore * ComboScoreMultiplier[comboScript.ComboCount];
                //�uSCORE�v�Ƃ����L�[�ŁAInt�l�́u ScoreCount �v��ۑ�
                PlayerPrefs.SetFloat("SCORE", ScoreCount);
                PlayerPrefs.Save();
            }
            else if (comboScript.ComboCount >= (CAMM - 1))
            {
                ScoreCount = ScoreCount + StatueEnemyScore * ComboScoreMultiplier[(CAMM - 1)];
                //�uSCORE�v�Ƃ����L�[�ŁAInt�l�́u ScoreCount �v��ۑ�
                PlayerPrefs.SetFloat("SCORE", ScoreCount);
                PlayerPrefs.Save();
            }
           
        }

        if (collision.gameObject.tag == "BOSS" && t.Attack)
        {
            if (comboScript.ComboCount < (CAMM - 1))
            {
                ScoreCount = ScoreCount + StatueEnemyScore * ComboScoreMultiplier[comboScript.ComboCount];
                //�uSCORE�v�Ƃ����L�[�ŁAInt�l�́u ScoreCount �v��ۑ�
                PlayerPrefs.SetFloat("SCORE", ScoreCount);
                PlayerPrefs.Save();
            }
            else if (comboScript.ComboCount >= (CAMM - 1))
            {
                ScoreCount = ScoreCount + StatueEnemyScore * ComboScoreMultiplier[CAMM - 1];
                //�uSCORE�v�Ƃ����L�[�ŁAInt�l�́u ScoreCount �v��ۑ�
                PlayerPrefs.SetFloat("SCORE", ScoreCount);
                PlayerPrefs.Save();
            }
           
        }
    }
}
