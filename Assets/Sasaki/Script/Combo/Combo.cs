using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combo : MonoBehaviour
{/*�R���{�̃X�N���v�g�ł�
�@�R���{�̎d�l��
    �G�ɘA�����čU������Ɣ�������
    ���ʂ̓R���{���ɉ����čU���͋����A���ʉ��̕ω��A�X�R�A�{���̏㏸�A�G�t�F�N�g�̕ω�(��)�A�R���{�\�L�̕ω�
    �R���{���������������͒n�ʂɒ��n�����Ƃ�
    �U���͏㏸�̔{��(�X�R�A?)
    �R���{�{��
�P�R���{�F1.0�{
�Q�R���{�F1.2�{
�R�R���{�F1.4�{
�S�R���{�F1.6�{
5�R���{�F1.8�{
�U�R���{�F2.0�{
�V�R���{�F2.3�{
�W�R���{�F2.6�{
�X�R���{�F2.9�{
10�R���{�F3.0�{
  */
    public float ComboResetTime = 5.0f;//�R���{�����Z�b�g����鎞��
    [SerializeField]
    [HeaderAttribute("���݂̃R���{")]
    public int ComboCount = 0;//���݂̃R���{
    [SerializeField]
    [HeaderAttribute("���݂̃R���{�{��")]
    public float ComboAttackCurrentMagnification;//���݂̃R���{�{��
    [SerializeField]
    [HeaderAttribute("�ő�̃R���{��")]
    public int ComboAttackMaxMagnification = 10;//�ő�̃R���{�{��
    [SerializeField]
    [HeaderAttribute("���͂����Ȃ��ŃR���{���r�؂��b��")]
    public float ChainUnlocksec = 5.0f;//�v���C���[�����͂����Ȃ��œr�؂��b��
    [SerializeField]
    [HeaderAttribute("�R���{�̍U���͏㏸�{��")]
    public float[] ComboAttackMagnification ;//�R���{�̍U���͏㏸�{��
    [SerializeField]
    [HeaderAttribute("�K�E�Z�̍U���͏㏸�{��")]
    public float SpecialAttackMagnification;//�K�E�Z�̍U���͏㏸�{��

    [SerializeField]
    [HeaderAttribute("�R���{�Q�[�W")]
    private Slider Gage_ChainGage;
    [SerializeField]
    private GameObject Obj_ChainGage;


    //Combo��G�ɓ����������̈��̂ݒǉ������悤�ɂ��邽�߂̕ϐ�
    public int CountEnemyCombo;

    //target����G�̃I�u�W�F�N�g���擾
    target t;

    //�R���{�̃e�L�X�g
    public Text ComboNumberText;
    public Text ComboText;

    //�K�E�Z�p�̕ϐ�
    public bool Special;
    public bool isPushed;

    //�R���{����
    public bool ChainUnlock;
    //�R���{�����̎���
    public float ChainUnlocktime;

    //�����^�[�Q�b�g
    multiplePlayer mp;

    public bool a;

    public bool isDeathblow = false;

    public bool isFloor;
    public bool isCombo;
    public bool SpecialMode;

    int EnemyCount;

    [SerializeField] GameObject UIParent;
    [SerializeField] Image SpecialGauge;

    void Start()
    {
        t = GameObject.FindGameObjectWithTag("Player").GetComponent<target>();
        ComboText.enabled = false;
        ComboNumberText.enabled = false;
        ChainUnlock = false;
        ChainUnlocktime = 3.0f;
        //ChainUnlocksec�b���Ƃ�Chain��0�ɂ��鏈�������Ԋu�ŌĂяo��
        // InvokeRepeating(nameof(ChainUnlock0), ChainUnlocksec, ChainUnlocksec);

        mp = GameObject.FindGameObjectWithTag("Player").GetComponent<multiplePlayer>();
        Special = false;
        isPushed = false;
        a = false;
        EnemyCount = 0;

        Obj_ChainGage.SetActive(true);
        SpecialGauge.fillAmount = 0.1f;
    }

    void LateUpdate()
    {
        ChainUnlocktime += Time.deltaTime;

        Gage_ChainGage.value = (ChainUnlocksec - ChainUnlocktime) / ChainUnlocksec;
        //SpecialGauge.fillAmount = (ChainUnlocksec - ChainUnlocktime) / ChainUnlocksec;

        if (ComboCount == 0 || SpecialMode)
        {
            ComboAttackCurrentMagnification = ComboAttackMagnification[0];
            ComboText.enabled = false;
            ComboNumberText.enabled = false;
            isCombo = false;
        }
      
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown("joystick button 7") || Input.GetKeyDown("joystick button 0") || mp.At || Input.GetKeyDown("joystick button 2") || Input.GetKeyDown(KeyCode.LeftShift)) && (t.ismove_Statue || t.ismove_Beam || t.ismove_Boss))
        {
            //ChainUnlock = false;       �G�ɓ��������ꏊ�Ɉڍs�iOnCollisionEnter�j
            //ChainUnlocktime = 0.0f;
            //CountEnemyCombo = 0;
            // StartCoroutine(ChainUnlock1());
        }
        if (ChainUnlock == true)
        {
            ComboCount = 0;
            ComboNumberText.GetComponent<Text>().text = ComboCount.ToString();
        }
        if (ChainUnlock == false)
        {
            //Debug.Log("Chain�p����!");
            isCombo = true;
        }
        if(ChainUnlocktime >= ChainUnlocksec || a)
        {
            ChainUnlock = true;
            //ChainUnlocktime = 0.0f;
            a = false;
            //EnemyCount = 0;
            //SpecialGauge.fillAmount = 0;
            //Special = false;
        }

        if (t.SpecialAttack)
        {
            isDeathblow = true;
            //ComboAttackCurrentMagnification = SpecialAttackMagnification;
            SpecialGauge.fillAmount = 0;
        }
        else
        {
            isDeathblow = false;
            //ComboAttackCurrentMagnification = ComboAttackMagnification[ComboCount - 1];
        }

        if(t.SpecialAtStart)
        {
            EnemyCount = 0;
        }

        /*
        if(isPushed)
        {
            ComboAttackCurrentMagnification = SpecialAttackMagnification;
            isDeathblow = true;
        }
        */
        if(EnemyCount >= 10)
        {
            Special = true;
        }
        else
        {
            Special = false;
        }

        if (SpecialMode || GameObject.FindGameObjectWithTag("Manager").GetComponent<multipleTarget>().ChangeCamera)
        {
            UIParent.SetActive(false);
        }
        else
        {
            UIParent.SetActive(true);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {//�^�OBeam���^�OStatue�̕t�����G�ɓ����������AComboProcess�����s
        //if ((collision.gameObject == t.TargetBeam || collision.gameObject == t.TargetStatue) && Count == 0)
        if ((collision.gameObject.CompareTag("Beam") || collision.gameObject.CompareTag("Statue") || collision.gameObject.CompareTag("BOSS")) && (t.Attack || mp.At))
        {
            // �ʏ�U���Ń`�F�C�����Ȃ��邩�ǂ���
            bool isChain = true;
            if (collision.gameObject.CompareTag("Beam"))
            {
                if (collision.gameObject.GetComponent<BeamHPManager>().DamagedValue <= 0.0f)
                {
                    isChain = false;
                }
            }

            // �U���������ꍇ�̂ݏ���
            if (isChain || t.SpecialAttack)
            {
                ComboProcess();
                ComboText.enabled = true;
                ComboNumberText.enabled = true;
                //CountEnemyCombo++;
                ChainUnlock = false;
                ChainUnlocktime = 0.0f;
                //CountEnemyCombo = 0;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isFloor = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Beam") || collision.gameObject.CompareTag("Statue") || collision.gameObject.CompareTag("BOSS"))
        {
            //CountEnemyCombo = 0;
            //Debug.Log("Out");
        }

        if (collision.gameObject.tag == "Floor")
        {
            isFloor = false;
        }
    }

    
    public void ComboProcess()
    {//�R���{�̏���
        //ComboCount��1���₷
      
        ComboCount++;
        Gage_ChainGage.value = 1;
        EnemyCount++;
        if (EnemyCount <= 9)
        {
            SpecialGauge.fillAmount += 0.06f;
        }
        else
        {
            SpecialGauge.fillAmount += 1 - SpecialGauge.fillAmount;
        }


        //�e�L�X�g�����݂̃R���{�ɂ���(�v�ύX)
        ComboNumberText.GetComponent<Text>().text = ComboCount.ToString();
        ComboText.GetComponent<Text>().text = "Chain";
        //�ő�R���{��菬�������A���݂̃R���{�ɉ������{�������݂̃R���{�{���ł���ComboAttackCurrentMagnification�ɓ����
        //�ő�R���{�ȏ�ɂȂ�����{�����ő�̔{���ɌŒ�
        if (ComboCount <= ComboAttackMaxMagnification)
        {
            ComboAttackCurrentMagnification = ComboAttackMagnification[ComboCount-1];
        }else if(ComboCount > ComboAttackMaxMagnification && !t.SpecialAttack)
        {
            ComboAttackCurrentMagnification = ComboAttackMagnification[ComboAttackMaxMagnification-1];
        }
    }
    void ChainUnlock0()
    {
        ChainUnlock = true;
        Debug.Log("Chain��0�ɂ���!");
    }

   

    /*IEnumerator ChainUnlock1()
    {
        yield return new WaitForSecondsRealtime(ChainUnlocksec);
        ChainUnlock = true;
        Debug.Log("Chain��0�ɂ���!");

    }
    private void OnDestroy()
    {
        // Destroy���ɓo�^����Invoke�����ׂăL�����Z��
        CancelInvoke();
    }*/
}
