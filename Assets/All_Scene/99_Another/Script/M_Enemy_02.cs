using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M_Enemy_02 : MonoBehaviour
{
    // ���낢��M��̂͂������� ----------------------------------------------------------------------------------------------------
    // ��~��Ԃ̕b��
    [SerializeField] float stopSeconds;
    // �����Ԃ̕b��
    [SerializeField] float patrolSeconds;
    // �ǔ���ԂɂȂ��Ă���ǔ����J�n����b��
    [SerializeField] float chaseStartSeconds;
    // �U����ԂɂȂ��Ă���U�����蔭���܂ł̕b��
    [SerializeField] float attackStartSeconds;
    // �U�����蔭�����Ă���I���܂ł̕b��
    [SerializeField] float attackStopSeconds;

    // ���񑬓x
    [SerializeField] float patrolSpeed;
    // �ǔ����x
    [SerializeField] float chaseSpeed;
    //�����ʒu�ɖ߂鑬�x
    [SerializeField] float backSpeed;

    // ���G�͈�
    [SerializeField] float searchRange;
    // �U���J�n�͈�
    [SerializeField] float attackRange;
    // ���낢��M��̂͂����܂� ----------------------------------------------------------------------------------------------------

    // �s�����[�h
    [SerializeField] string state = "�s�����[�h";// �C���X�y�N�^�Ō������̂�SerializeField
    // ��Ԃ��J�ڂ��Ă���̌o�ߕb���i��ԑJ�ڃJ�E���g�j
    [SerializeField] float stateTransitionCount;// �C���X�y�N�^�Ō������̂�SerializeField

    // �����|�W�V����
    private Vector3 firstPos;
    // �^�[�Q�b�g�̃|�W�V����
    private Vector3 targetPos;
    private Vector3 targetPos_x;
    private Vector3 targetPos_z;

    //x��������񂷂�ꍇ
    [SerializeField] bool Patrol_x;
    //y��������񂷂�ꍇ
    [SerializeField] bool Patrol_z;
    //�����_���ȕ����ɐi�ޏꍇ
    [SerializeField] bool Patrol_random;

    //x����𓮂����ꍇ�̏����̌���
    [SerializeField] float Direction;
    private int Count;


    // �v���C���[GameObject
    GameObject Player;
    // �v���C���[�̕��ʍ��W
    private Vector3 playerPos;
    // ������Rigidbody�R���|�[�l���g
    private Rigidbody RB;
    // �U������GameObject
    [SerializeField] GameObject AttackColiison;

    //HP�o�[�֘A
    [SerializeField]
    public GameObject HPUI;
    private Slider hpSlider;
    public bool Damage;
    public float DamageSpeed;

    //target�X�N���v�g�擾
    private target ta;
    //�v���C���[��rigidbody�擾
    private Rigidbody rb;

    //�d�͂���߂�^�C�~���O
    public bool gravity_A;

    //�ːi��Ƀv���C���[���������
    public float Fly;

    


    // ����̂ݏ���
    void Start()
    {
        // �����͒�~���
        state = "stop";
        stateTransitionCount = 0.0f;

        // �����|�W�V������ݒ�
        firstPos = transform.position;

        //�v���C���[�I�u�W�F�N�g�擾
        Player = GameObject.FindGameObjectWithTag("Player");

        //target�X�N���v�g�擾
        ta = Player.GetComponent<target>();


        // ������Rigidbody�R���|�[�l���g�擾
        RB = GetComponent<Rigidbody>();

        //HP�o�[�̎擾
        hpSlider = HPUI.GetComponent<Slider>();
        hpSlider.value = 1f;
        Damage = false;
        ta = Player.GetComponent<target>();
        rb = Player.GetComponent<Rigidbody>();
        gravity_A = false;

        //�����̌����w��
        Count = 0;
    }

    // �t���[��������
    void Update()
    {

        // �v���C���[�̕��ʍ��W�̎擾
        playerPos.x = Player.transform.position.x;
        playerPos.y = Player.transform.position.y;
        playerPos.z = Player.transform.position.z;

        // ��Ԃɂ���ď�������
        switch (state)
        {
            // ��~��Ԃ̏��� ----------------------------------------------------------------------------------------------------
            case "stop":

                // ���x�����ȏ�̏ꍇ�A��R�������Ď~�߂�����
                if (RB.velocity.magnitude > 0.01f)
                {
                    RB.AddForce(-RB.velocity, ForceMode.Acceleration);
                }
                else
                {
                    RB.velocity = Vector3.zero;
                }

                // ����������͈͓��Ƀv���C���[�������Ă����ꍇ�A�ǔ���ԂɑJ��
                if ((transform.position - playerPos).magnitude <= 15.0f)
                {
                    state = "chase";
                    stateTransitionCount = 0.0f;
                }

                // ��ԑJ�ڃJ�E���g�����ȏ�ɂȂ����ꍇ�A�����ԂɑJ��
                if (stateTransitionCount >= stopSeconds)
                {
                    state = "patrol";
                    stateTransitionCount = 0.0f;
                    // �p�g���[����ԏ����ݒ�
                    if (Patrol_random == true)
                    {
                        targetPos = firstPos + new Vector3(Random.Range(-5.0f, 5.0f), 0.0f, Random.Range(-5.0f, 5.0f));
                        //patrolVec = (targetPos - transform.position).normalized;
                        transform.LookAt(targetPos);
                    }
                    //x����𓮂����ꍇ�̏����̌���
                    if (Patrol_x == true && Count == 0)
                    {
                        targetPos_x = firstPos + new Vector3(-Direction, 0.0f, 0.0f);
                        transform.LookAt(targetPos_x);
                        Count++;
                    }
                    //z����𓮂����ꍇ�̏����̌���
                    if (Patrol_z == true && Count == 0)
                    {
                        targetPos_z = firstPos + new Vector3(0.0f, 0.0f, -Direction);
                        transform.LookAt(targetPos_z);
                        Count++;
                    }
                    if (Patrol_x == true || Patrol_z == true)
                    {
                            transform.Rotate(new Vector3(0, 180, 0));
                    }
                }

                break;

            // �����Ԃ̏��� ----------------------------------------------------------------------------------------------------
            case "patrol":

                // ���x�����ȉ��̏ꍇ�A�O�����ɗ͂�������
                if (RB.velocity.magnitude <= patrolSpeed)
                {
                    RB.AddForce(transform.forward, ForceMode.Acceleration);
                }

                // ����������͈͓��Ƀv���C���[�������Ă����ꍇ�A�ǔ���ԂɑJ��
                if ((transform.position - playerPos).magnitude <= searchRange)
                {
                    state = "chase";
                    stateTransitionCount = 0.0f;
                }

                // ��ԑJ�ڃJ�E���g�����ȏ�ɂȂ����ꍇ�A��~��ԂɑJ��
                if (stateTransitionCount >= patrolSeconds)
                {
                    state = "stop";
                    stateTransitionCount = 0.0f;
                   
                }

                break;

            // �ǔ���Ԃ̏��� ----------------------------------------------------------------------------------------------------
            case "chase":

                // �ǔ���ԂɑJ�ڂ��Ă����莞�Ԉȓ��̏ꍇ�A�v���C���[�̕��������Ȃ��猸������~����
                if (stateTransitionCount <= chaseStartSeconds)
                {
                    transform.LookAt(playerPos);
                    if (RB.velocity.magnitude > 0.01f)
                    {
                        RB.AddForce(-RB.velocity, ForceMode.Acceleration);
                    }
                    else
                    {
                        RB.velocity = Vector3.zero;
                    }
                }
                // �ǔ���ԂɑJ�ڂ��Ă����莞�Ԍo�߂����ꍇ�A�v���C���[�̕��������A�ǂ������Ă���
                else
                {
                    transform.LookAt(playerPos);
                    RB.velocity = (playerPos - transform.position).normalized * chaseSpeed;
                }

                // ����������͈͓��Ƀv���C���[�������Ă����ꍇ�A�U����ԂɑJ��
                if ((transform.position - playerPos).magnitude < attackRange)
                {
                    state = "attack";
                    stateTransitionCount = 0.0f;
                }
                // �����̈��͈͓�����v���C���[���o���ꍇ�A��~��ԂɑJ��
                else if ((transform.position - playerPos).magnitude > searchRange)
                {
                    state = "back to patrol";
                    stateTransitionCount = 0.0f;
                }

                break;

            //�ǔ���Ԍ�̏���
            case "back to patrol":

                RB.velocity = Vector3.zero;
                transform.LookAt(firstPos);
                transform.position = Vector3.MoveTowards(transform.position, firstPos, backSpeed * Time.deltaTime);
                //�����ʒu�ɖ߂�
                if (transform.position == firstPos)
                {
                    state = "stop";
                    stateTransitionCount = 0.0f;
                    Count = 0;
                }
                // ����������͈͓��Ƀv���C���[�������Ă����ꍇ�A�ǔ���ԂɑJ��
                if ((transform.position - playerPos).magnitude <= searchRange)
                {
                    state = "chase";
                    stateTransitionCount = 0.0f;
                }

                break;

            // �U����Ԃ̏��� ----------------------------------------------------------------------------------------------------
            case "attack":

                // �U����ԂɑJ�ڂ��Ă����莞�Ԉȓ��̏ꍇ�A����
                if (stateTransitionCount <= attackStartSeconds)
                {
                    if (RB.velocity.magnitude >= 0.01f)
                    {
                        RB.AddForce(-RB.velocity, ForceMode.Acceleration);
                    }
                    else
                    {
                        RB.velocity = Vector3.zero;
                    }
                }

                // �������I����Ă����莞�Ԍo�߂����ꍇ�A�U������ON
                else if (stateTransitionCount <= (attackStartSeconds + attackStopSeconds))
                {
                    AttackColiison.SetActive(true);
                }

                // �U������ON�����莞�Ԍo�߂����ꍇ�A�U������OFF�ɂ���ԑJ��
                else
                {
                    AttackColiison.SetActive(false);
                    // ����������͈͓��Ƀv���C���[������ꍇ�A�ǔ���ԂɑJ��
                    if ((transform.position - playerPos).magnitude <= searchRange)
                    {
                        state = "chase";
                        stateTransitionCount = 0.0f;
                    }
                    // ����������͈͓��Ƀv���C���[�����Ȃ��ꍇ�A�����ʒu�Ɉړ�
                    else
                    {
                        state = "back to patrol";
                        stateTransitionCount = 0.0f;
                    }
                }

                break;

            //���񂾂Ƃ��̏���
            case "Dead":
                Destroy(this.gameObject);
                rb.constraints = RigidbodyConstraints.None;
                Player.transform.position = new Vector3(Player.transform.position.x,Player.transform.position.y + Fly, Player.transform.position.z);
                gravity_A = true;
                ta.ismove_Statue = false;
                ta.isTarget_Statue = false;
                //rb.isKinematic = false;

                break;

            //�v���C���[�ɓ����������̏���
            case "Damage":

                Damage = true;

                break;
        }

        // ��ԑJ�ڃJ�E���g�Ɍo�ߎ��Ԃ����Z
        stateTransitionCount += Time.deltaTime;

        //HP����
        if (Damage == true)
        {
            hpSlider.value -= DamageSpeed * Time.deltaTime;
            if (hpSlider.value <= 0)
            {
                state = "Dead";
            }
        }
       
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (ta.ismove_Statue == true)
            {
                state = "Damage";
                ta.ismove_Statue = false;
                //rb.isKinematic = false;
            }
        }
    }

   
    // �M�Y���`��
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, searchRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
