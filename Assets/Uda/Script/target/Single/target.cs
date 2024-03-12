using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


[System.Serializable]
public class Data
{
    public Vector3 position;

    // ���̑��̃f�[�^��ǉ�
}

public class target : MonoBehaviour
{
    //�ːi�Ώۂ̋ߐړG
    public GameObject TargetStatue;
    //�ːi�Ώۂ̃r�[���G
    public GameObject TargetBeam;
    //�ːi�Ώۂ�Boss
    public GameObject TargetBoss;
    //�ːi�̑��x
    public float RushSpeed = 0f;
   
    //�ːi�Ώۂ̋ߐړG�̍��W
    public Vector3 StatuePos2;
    //�ːi�Ώۂ̃r�[���G�̍��W
    public Vector3 BeamPos;
    //�ːi�Ώۂ�Boss�̍��W
    public Vector3 BossPos;
    public Rigidbody rb;
    //�G�̎�ނɉ����ēːi�����^�[�Q�b�g��������
    public bool isTarget_Statue = false;
    public bool isTarget_Beam = false;
    public bool ismove_Statue = false;
    public bool ismove_Beam = false;
    public bool isTarget_Boss = false;
    public bool ismove_Boss = false;
    public bool clear = false;

   

    //�����������̏d�͕ύX�Ɠ����������̂͂�
    public float jump = 3f;
    public float JumpGravityY = -10;

    public Vector3 surfacePoint;

    private single si;

    public List<Data> dataList = new List<Data>();

    public Vector2 targetPosition;

    //�U�����̃t���O
    public bool Attack;

    #region �ːi�ɂ�鐁����΂�
    public static target instance;
    public bool isAtacked = false;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

    // PlayerSounds�X�N���v�g--------�����ǉ�--------
    private PlayerSounds ps;
    private Soundtest st;
    private bool isPlaySPChargeSound;

    [SerializeField]
    private float center;

    public bool jumpwait = false;

    public Vector3 nowPos;
    public float backKnockBackForce;
    public float upKnockBackForce;
    public float knockBackPower;

    multipleTarget mt;
    multiplePlayer mp;

    Combo c;

    public bool SpecialAttack;
    public bool SpecialAtStart;
    public Vector3 SpecialPosition;
    public Vector3 FinalSpecialPosition;

    public bool isMoving = false;

    [SerializeField] float slowMotionScale = 0.2f;
    public Vector3 FirstPosition;
    Vector3 SurfaceNormal;

    [SerializeField] GameObject SpecialEffect;
    float originalTimeScale = 1.0f;


    int enemyLayerMask;
    int BossLayerMask;

    public Vector3 SingleMovePosition;
    public GameObject Target;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //TargetImage.GetComponent<Image>().enabled = false;

        mt = GameObject.FindGameObjectWithTag("Manager").GetComponent<multipleTarget>();
        mp = GameObject.FindGameObjectWithTag("Player").GetComponent<multiplePlayer>();

        c = this.gameObject.GetComponent<Combo>();

        // PlayerSounds�X�N���v�g�擾--------M�ǉ�--------
        ps = GetComponent<PlayerSounds>();
        st = GameObject.Find("SEPlayer").GetComponent<Soundtest>();// �����͈�ԉ��̍s�ɂ��Ă�������
        SpecialEffect.SetActive(false);
        originalTimeScale = 1.0f;
        enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
        BossLayerMask = 1 << LayerMask.NameToLayer("Boss");
    }

    // Update is called once per frame
    void Update()
    {
        //�ːi�\�ȃI�u�W�F�N�g��null�łȂ��ꍇ�́A�G�ɉ�����flag��ύX
        if (TargetBeam != null)
        {
            isTarget_Beam = true;
            isTarget_Statue = false;
            isTarget_Boss = false;
        }
        else if (TargetStatue != null)
        {
            isTarget_Beam = false;
            isTarget_Statue = true;
            isTarget_Boss = false;
        }
        else if (TargetBoss != null)
        {
            isTarget_Beam = false;
            isTarget_Statue = false;
            isTarget_Boss = true;
        }

        //�G�ɉ����ēːi�̏�����ύX
        if ((TargetStatue == null || !TargetStatue.activeSelf) && ismove_Statue)
        {
            ismove_Statue = false;
            isMoving = false;
            isAtacked = false;
            Attack = false;
        }
        else if ((TargetBeam == null || !TargetBeam.activeSelf)&& ismove_Beam)
        {
            ismove_Beam = false;
            isMoving = false;
            isAtacked = false;
            Attack = false;
        }

        //�ߐړG�ւ̓ːi����
        if (isTarget_Statue == true && !mt.multiple)
        {
            if (Input.GetMouseButtonDown(0) && !isMoving)
            {
                ismove_Statue = true;
                isMoving = true;
                SingleMovePosition = StatuePos2;
                Target = TargetStatue;
                // �v���C���[�̓ːi����ON--------�����ǉ�--------
                ps.isPlayRushSound = true;
            }        
        }

        //�r�[���G�ւ̓ːi����
        if (isTarget_Beam == true)
        {
             BeamPos = TargetBeam.transform.position;
             if (Input.GetMouseButtonDown(0) && !isMoving)
             {
                  ismove_Beam = true;
                  isMoving = true;
                  SingleMovePosition = TargetBeam.transform.position;
                  Target = TargetBeam;
                   // �v���C���[�̓ːi����ON--------M�ǉ�--------
                   ps.isPlayRushSound = true;
             }
        }
        //Boss�ւ̓ːi����
        if (isTarget_Boss == true)
        {
             if (Input.GetMouseButtonDown(0))
             {
                 ismove_Boss = true;
                 isMoving = true;
                 SingleMovePosition = BossPos;
                 Target = TargetBoss;
                 // �v���C���[�̓ːi����ON--------M�ǉ�--------
                 ps.isPlayRushSound = true;
             }
        }
           
        //�K�E�Z���o
            if (c.Special == true && isPlaySPChargeSound == false)
            {
                st.SE_SPChargePlayer();
                isPlaySPChargeSound = true;
            }
            if (c.Special == false && isPlaySPChargeSound == true)
            {
                isPlaySPChargeSound = false;
            }

            //�ߐړG�ւ̓ːi�A�ːi������̏���
            if(ismove_Statue)
           {
            transform.position = Vector3.MoveTowards(this.transform.position, SingleMovePosition, RushSpeed * Time.deltaTime);
            rb.isKinematic = true;
            Attack = true;
            if ((Mathf.Approximately(this.transform.position.x, SingleMovePosition.x) && Mathf.Approximately(this.transform.position.y, SingleMovePosition.y) && Mathf.Approximately(this.transform.position.z, SingleMovePosition.z)))
            {
                if (Target != null && Target.activeSelf)
                {
                    //�ΏۂɃ_���[�W��^����
                    Target.GetComponent<StatueHPManager>().SingleDamage();
                }
                isTarget_Statue = false;
                ismove_Statue = false;
                isMoving = false;
                rb.isKinematic = false;
                Attack = false;
                TargetStatue = null;
                // �v���C���[�̍U�����������̉���ON�i�A�j���[�V�������o������ړ��j--------M�ǉ�--------
                ps.isPlayAttackSound = true;
                // �v���C���[�̍U���������������̉���ON--------M�ǉ�--------
                //ps.isPlayAttackHitSound = true;
            }
            }

        //�r�[���G�ւ̓ːi�A�ːi������̏���
        if (ismove_Beam)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, SingleMovePosition, RushSpeed * Time.deltaTime);
            rb.isKinematic = true;
            Attack = true;
            if ((Mathf.Approximately(this.transform.position.x, SingleMovePosition.x) && Mathf.Approximately(this.transform.position.y, SingleMovePosition.y) && Mathf.Approximately(this.transform.position.z, SingleMovePosition.z)))
            {
                if (Target != null && Target.activeSelf)
                {
                    //�ΏۂɃ_���[�W��^����
                    Target.GetComponent<BeamHPManager>().SingleDamage();
                }
                ismove_Beam = false;
                isMoving = false;
                rb.isKinematic = false;
                Attack = false;
                isTarget_Beam = false;
                TargetBeam = null;
                // �v���C���[�̍U�����������̉���ON�i�A�j���[�V�������o������ړ��j--------�����ǉ�--------
                ps.isPlayAttackSound = true;
                // �v���C���[�̍U���������������̉���ON--------�����ǉ�--------
                //ps.isPlayAttackHitSound = true;
            }
        }
        //Boss�ւ̓ːi�A�ːi������̏���
        if (ismove_Boss)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, SingleMovePosition, RushSpeed * Time.deltaTime);
            rb.isKinematic = true;
            Attack = true;
            if ((Mathf.Approximately(this.transform.position.x, SingleMovePosition.x) && Mathf.Approximately(this.transform.position.y, SingleMovePosition.y) && Mathf.Approximately(this.transform.position.z, SingleMovePosition.z)))
            {
                if (Target != null && Target.activeSelf)
                {
                    //�Ώۂփ_���[�W��^����
                    Target.GetComponent<Clear>().SingleDamage();
                }
                ismove_Boss = false;
                isMoving = false;
                rb.isKinematic = false;
                Attack = false;
                isTarget_Boss = false;
                TargetBoss = null;
                // �v���C���[�̍U�����������̉���ON�i�A�j���[�V�������o������ړ��j--------�����ǉ�--------
                ps.isPlayAttackSound = true;
                // �v���C���[�̍U���������������̉���ON--------�����ǉ�--------
                //ps.isPlayAttackHitSound = true;
            }
        }
    }

      
         

    //�ːi��̃m�b�N�o�b�N
        public void KnockBack(Collision collision)
        {
            nowPos = this.transform.position;
            var boundVec = (nowPos - collision.transform.position);
            boundVec.y = 0.0f;
            boundVec = boundVec.normalized * backKnockBackForce;
            boundVec.y = 1.0f * upKnockBackForce;
            boundVec = boundVec.normalized;
            rb.velocity = Vector3.zero;
            rb.AddForce(boundVec * knockBackPower, ForceMode.Impulse);
        }

    //�G�ɋ߂Â��߂����ꍇ�Ɍ��ɉ�����
        private void GetBack()
        {
           Vector3 First = new Vector3(FirstPosition.x, this.transform.position.y, FirstPosition.z);
           Vector3 movePoint = new Vector3(surfacePoint.x, this.transform.position.y, surfacePoint.z);
           float moveDistance = (movePoint - First).magnitude;
           float moveAmount = 5.0f;
           Vector3 moveDirection = (First - movePoint).normalized;
           this.transform.position = Vector3.Lerp(this.transform.position, this.transform.position + moveDirection * moveAmount,Time.deltaTime * RushSpeed);
        }

    //�m�b�N�o�b�N����
    public void SingleKnockBack(float MoveDistance, float updistance)
    {
        FirstPosition.y = SingleMovePosition.y;
        Vector3 Direction = (Target.transform.position - FirstPosition).normalized;
        Vector3 upOffset = new Vector3(0f, updistance, 0f);
        Vector3 newPosition = transform.position - Direction * MoveDistance + upOffset;
        transform.position = Vector3.Lerp(transform.position, newPosition, 2f * Time.deltaTime);
        rb.velocity = Vector3.zero;
        Debug.Log("Back");
    }
    private void OnCollisionEnter(Collision other)
        {
            if ((other.gameObject.tag == "Statue" || other.gameObject.tag == "Beam") && SpecialAttack)
            {
                //�K�E�Z�g�p����SE
                st.SE_SuperAttackPlayer();
            }
        }

    
    //�G�̖h��͂�Player�̍U���͂������������ꍇ�̃m�b�N�o�b�N����
        public void DenfensiveKnockBack()
        {
            Vector3 incidentVector = (FirstPosition - SingleMovePosition).normalized;
            float KnockBackDistance = 15f;
            Vector3 moveDirection = transform.position + incidentVector * KnockBackDistance;
            rb.velocity = Vector3.zero;
            this.transform.position = Vector3.Lerp(this.transform.position,moveDirection, Time.deltaTime * RushSpeed);
        }

        IEnumerator ResetCollisionFlag()
        {
            yield return new WaitForSeconds(1f);
            jumpwait = false;
        }


        public void AddData(Data data)
        {
            dataList.Add(data);
        }

    //�K�E�Z�����R���[�`��
        public IEnumerator DelayedStart(float delayInSeconds)
        {
            SpecialAtStart = true;
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            //�K�E�Z�̊J�n�n�_
            Vector3 FirstPoint = transform.position;    
            //�K�E�Z�����O�̃X���[���[�V�����̕b��
            Time.timeScale = slowMotionScale;
        �@�@//�K�E�Z��p�G�t�F�N�g�o���@
            SpecialEffect.SetActive(true);
            transform.LookAt(SpecialPosition);

            yield return new WaitForSecondsRealtime(delayInSeconds);
            //�K�E�Z�����J�n

            // �v���C���[�̓ːi����ON--------M�ǉ�--------
            ps.isPlayRushSound = true;
        �@�@//���Ԃ����ɖ߂�
            Time.timeScale = originalTimeScale;
            SpecialEffect.SetActive(false);
            SpecialAttack = true;
            rb.isKinematic = false;

            if (!isMoving)
            {
                int layerMask = 1 << LayerMask.NameToLayer("Default"); 
                int enemyLayerMask = 1 << LayerMask.NameToLayer("Boss");

                Ray ray = new Ray(transform.position, SpecialPosition - transform.position);
                RaycastHit hit;
                

�@�@�@�@�@�@�@�@�@
            �@�@//�ːi�����ʒu�����O�Ɍv�Z���ABoss�AVariant�̏ꍇ�́A�Փ˂����n�_�Œ�~�A����ȊO�̏ꍇ�͔���
                if (Physics.Raycast(ray, out hit, Vector3.Distance(transform.position, SpecialPosition), (layerMask | enemyLayerMask)))
                {
                    if ((layerMask & (1 << hit.collider.gameObject.layer)) != 0)
                    {
                        Vector3 normal = hit.normal;


                        // �@���x�N�g���̌����ɂ���ĕǂ������𔻕ʂ���
                        if (Mathf.Abs(normal.y) > Mathf.Abs(normal.x) && Mathf.Abs(normal.y) > Mathf.Abs(normal.z))
                        {
                            // �@���x�N�g����������i���̖@���x�N�g���j�̏ꍇ�͏��Ɣ���
                            // ���ɑ΂��鏈�����s��
                            // �Փ˒n�_���擾
                            Vector3 collisionPoint = hit.point;
                            Debug.Log("��");
                            FinalSpecialPosition = collisionPoint;
                            // �V�����ʒu��Tween�A�j���[�V������ݒ�i���Ɉړ��j
                            transform.DOMove(collisionPoint, 0.2f)
                                .SetEase(Ease.Linear)
                                .OnStart(() =>
                                {
                                    isMoving = true;
                                })
                                .OnComplete(() =>
                                {
                                // ���ւ̈ړ�������������A���X�ړ����Ă��������ɏ��Ɛ����Ȋp�x�ňړ�
                                Vector3 incidentVector = (SpecialPosition - FirstPoint).normalized;
                                    Vector3 floorNormal = hit.normal;
                                    Vector3 reflectionVector = Vector3.Reflect(incidentVector, floorNormal);
                                    Vector3 moveDirection = Vector3.Cross(Vector3.Cross(incidentVector, floorNormal), floorNormal).normalized;

                                // �ړ��������v�Z
                                float remainingDistance = 15f;

                                // �V�����ʒu���v�Z
                                Vector3 newPosition = collisionPoint + reflectionVector * remainingDistance;
                                    isMoving = false;
                                    SpecialAttack = false;
                                    SpecialAtStart = false;
                                    rb.useGravity = true;
                                // �V�����ʒu��Tween�A�j���[�V������ݒ�i���Ɛ����Ȋp�x�ňړ��j
                                rb.velocity = Vector3.zero;
                                    this.transform.position = Vector3.Lerp(this.transform.position, newPosition, Time.deltaTime * RushSpeed);
                                });
                        }
                        else
                        {
                            // �@���x�N�g�������������i�ǂ̖@���x�N�g���j�̏ꍇ�͕ǂƔ���
                            // �ǂɑ΂��鏈�����s��
                            // �ǂ̖@���x�N�g�����擾
                            Vector3 wallNormal = hit.normal;
                            Debug.Log("��");
                            FinalSpecialPosition = hit.point;
                            // �V�����ʒu��Tween�A�j���[�V������ݒ�i�ǂɈړ��j
                            transform.DOMove(hit.point, 0.2f)
                                .SetEase(Ease.Linear)
                                .OnStart(() =>
                                {
                                    isMoving = true;
                                })
                                .OnComplete(() =>
                                {
                                // �ǂւ̈ړ�������������A���˂����s
                                Vector3 incidentVector = (SpecialPosition - FirstPoint).normalized;
                                    float remainingDistance = 15f;
                                    Vector3 reflectionVector = Vector3.Reflect(incidentVector, wallNormal);

                                    Vector3 newPosition = hit.point + reflectionVector * remainingDistance;
                                    isMoving = false;
                                    SpecialAttack = false;
                                    rb.isKinematic = false;
                                    SpecialAtStart = false;
                                    rb.useGravity = true;
                                // �V�����ʒu��Tween�A�j���[�V������ݒ�i���ˁj
                                rb.velocity = Vector3.zero;
                                    this.transform.position = Vector3.Lerp(this.transform.position, newPosition, Time.deltaTime * RushSpeed);
                                });
                        }
                    }
                    else if (hit.collider.CompareTag("BOSS") || hit.collider.gameObject.name.Contains("Variant"))
                    {
                        FinalSpecialPosition = hit.collider.gameObject.GetComponent<IsRendered>().StatueRenderer.bounds.center;
                        transform.DOMove(hit.collider.gameObject.GetComponent<IsRendered>().StatueRenderer.bounds.center, 0.3f)
                                      .SetEase(Ease.Linear)
                                      .OnComplete(() =>
                                      {
                                          isMoving = false;
                                          SpecialAttack = false;
                                          SpecialAtStart = false;
                                          rb.useGravity = true;
                                          Vector3 incidentVector = (SpecialPosition - FirstPoint).normalized;
                                          float remainingDistance = 15f;
                                          Vector3 newPosition = hit.point + incidentVector * remainingDistance;
                                          this.transform.position = Vector3.Lerp(this.transform.position, newPosition, Time.deltaTime * RushSpeed);
                                      });
                    }
                }
                else
                {
                    FinalSpecialPosition = SpecialPosition;
                    // �Փ˂��Ȃ��ꍇ�͒ʏ��Tween�A�j���[�V���������s
                    transform.DOMove(SpecialPosition, 0.3f)
                        .SetEase(Ease.Linear)
                        .OnComplete(() =>
                        {
                            isMoving = false;
                            SpecialAttack = false;
                            SpecialAtStart = false;
                            rb.useGravity = true;
                        });
                }
            }
        }

    }

