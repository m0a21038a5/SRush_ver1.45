using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeamHPManager : MonoBehaviour
{

    public GameObject BeamHP;
    public float DamageValue;
    public float MaxHP;
    public float HP;
    private Slider slider;
    public GameObject hp;
    private Canvas canvas;

    private bool Damage;

    target t;
    Combo c;
    public float height;
    multiplePlayer mp;

    public Vector3 firstPos;

    //�ǉ��@�j��A�j���[�V�����Đ��p�@���X��
    public bool BrokenAnibool;
    public bool BrokenDestroy;
    private SphereCollider sc;

    public bool DeadDamage;

    single s;

    //�t���ǋL
    [SerializeField]
    public GameObject hit;
    [SerializeField]
    public GameObject Deathblow;
    Combo Combo;
    public float EffectHeight = 0;
    public float DefencePower;

    // �����ǉ�
    private PlayerSounds ps;
    private Soundtest st;

    public float DamagedValue;

    public bool Boss;
    public bool isDefeat;
    // ���X�؁@�ǉ��@
    [HeaderAttribute("�`���[�g���A���p�̒��{�X�ł���ꍇ�ɂ̂݃`�F�b�N")]
    public bool isTutorial;//�`���[�g���A���p�̃{�X�ł��邩����
    public GameObject cai;//�R�A��UI�̕\��

    GameObject Zone;

    float ComboAttackMagnification;
    bool CanCollide;


    multipleTarget mt;
    // Start is called before the first frame update
    void Start()
    {
        //�t���ǋL
        Combo = GameObject.FindGameObjectWithTag("Player").GetComponent<Combo>();

        firstPos = this.gameObject.transform.position;
        canvas = GameObject.FindGameObjectWithTag("HP").GetComponent<Canvas>();
        hp = Instantiate(BeamHP, canvas.transform);
        slider = hp.GetComponent<Slider>();
        slider.maxValue = MaxHP;
        slider.value = slider.maxValue;
        HP = MaxHP;
        hp.SetActive(false);
        Damage = false;
        t = GameObject.FindGameObjectWithTag("Player").GetComponent<target>();
        c = GameObject.FindGameObjectWithTag("Player").GetComponent<Combo>();
        //�ǉ��@�j��A�j���[�V�����Đ��p �R���C�_�[�擾�@���X��
        sc = this.gameObject.GetComponent<SphereCollider>();
        cai = GameObject.Find("CoreAllUI");
        mp = GameObject.FindGameObjectWithTag("Player").GetComponent<multiplePlayer>();
        s = GameObject.FindGameObjectWithTag("Manager").GetComponent<single>();
        mt = GameObject.FindGameObjectWithTag("Manager").GetComponent<multipleTarget>();

        // �����ǉ� ���ʉ��֌W
        ps = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSounds>();
        st = GameObject.Find("SEPlayer").GetComponent<Soundtest>();// �����͈�ԉ��̍s�ɂ��Ă�������
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new Vector3(t.BeamPos.x, t.BeamPos.y + height, t.BeamPos.z);
        Vector2 position = RectTransformUtility.WorldToScreenPoint(Camera.main, pos);
        hp.transform.position = new Vector3(position.x, position.y, 0);

        if(t.SpecialAttack)
        {
            ComboAttackMagnification = c.SpecialAttackMagnification;
        }
        else
        {
            ComboAttackMagnification = c.ComboAttackCurrentMagnification;
        }

      
        if(DamageValue * ComboAttackMagnification - DefencePower <= 0)
        {
            DamagedValue = 0;
        }
        else
        {
            DamagedValue = DamageValue * ComboAttackMagnification - DefencePower;
        }

        if (Damage == true)
        {
            Debug.Log(DamagedValue);
            HP = HP - DamagedValue; 
            slider.value = HP;
            Damage = false;
            t.Attack = false;

            // �����ǉ�
            if (HP > 0 && DefencePower >= 30.0f)
            {
                st.SE_CantAttackPlayer();
            }
            else if (HP > 0)
            {
                st.SE_CantAttackPlayer();
                ps.isPlayAttackHitSound = true;
            }
            else
            {
                ps.isPlayAttackHitSound = true;
            }
        }

        if (HP <= 0)
        {
            Destroy(hp);
            //�ǉ����ύX�@�j��A�j���[�V�����Đ��p�@���X�� �R���C�_�[����
            BrokenAnibool = true;
            sc.enabled = false;
            isDefeat = true;

            if (isTutorial == true)
            {
                st.SE_getCoinPlayer();
            }
            else if (isTutorial == false)
            {
                if (GetComponent<MiniBossCoin>() != null)
                {
                    GetComponent<MiniBossCoin>().getCoin();
                    cai.GetComponent<CoreAllImage>().coreUIsave();
                    st.SE_getCoinPlayer();
                }
            }
        }

        /*
        if (BrokenDestroy)
        {
            Destroy(this.gameObject);
        }
        */



        if (t.isTarget_Beam == true && t.TargetBeam == this.gameObject)
        {
            hp.SetActive(true);
        }
        else
        {
            hp.SetActive(false);
        }

        if(DamagedValue >= HP)
        {
            DeadDamage = true;
        }

        if(isDefeat || CanCollide)
        {
            s.targetList.Remove(this.gameObject);
            s.SortVectorList();
        }

      
    }

   
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && (mp.At || t.SpecialAttack))
        {
            //�t���ǋL
            if (Combo.isDeathblow == true)
            {
                var obj = Instantiate(Deathblow);
                obj.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + EffectHeight, this.transform.position.z);
            }
            else
            {
                var obj = Instantiate(hit);
                obj.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + EffectHeight, this.transform.position.z);
            }
            Damage = true;
            //sc.enabled = false;
           

            if (DeadDamage)
            {
                isDefeat = true;
            }

            if (mt.multipleTargetObject.Contains(this.gameObject) && mp.At)
            {
                mt.multipleTargetObject.Remove(this.gameObject);
            }
        }
        if (collision.gameObject.tag == "DangerZone")
        {
            Zone = collision.gameObject;
        }

        
    }

    /*
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Damage = false;
        }
    }
    */

    private void OnDestroy()
    {
        if (!this.gameObject.name.Contains("Variant") && !Boss)
        {
            BeamManager bm = GameObject.FindGameObjectWithTag("Manager").GetComponent<BeamManager>();
            bm.AddBeamData(this.gameObject);
            bm.RequestsRespawn(gameObject);
        }
        if(Zone != null)
        {
            if (this.gameObject.name.Contains("Variant"))
            {
                Zone.GetComponent<DangerArea>().Boss = null;
            }
            else
            {
                Zone.GetComponent<DangerArea>().DangerList.Remove(this.gameObject);
            }
        }
         s.targetList.Clear();
        targetChange tc = GameObject.FindGameObjectWithTag("Manager").GetComponent<targetChange>();
        tc.right.Clear();
        tc.left.Clear();
    }
    public void SingleDamage()
    {
        HP = HP - DamagedValue;
        slider.value = HP;
        if (t.ismove_Beam)
        {
            t.SingleKnockBack(100f, 60f);
            ps.isPlayAttackHitSound = true;
        }
        if (HP > 0 && !mp.At)
        {
            t.DenfensiveKnockBack();
            st.SE_CantAttackPlayer();
        }
        if (Combo.isDeathblow == true)
        {
            var obj = Instantiate(Deathblow);
            obj.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + EffectHeight, this.transform.position.z);
        }
        else
        {
            var obj = Instantiate(hit);
            obj.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + EffectHeight, this.transform.position.z);
        }
    }
    private IEnumerator EnableCollisionAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        CanCollide = false;
        Boss = false;
    }
}
