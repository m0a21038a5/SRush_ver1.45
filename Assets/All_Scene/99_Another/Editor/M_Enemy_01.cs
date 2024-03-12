using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class M_Enemy_01 : MonoBehaviour
{
    // ���낢��M��̂͂������� ----------------------------------------------------------------------------------------------------
    // ��~��Ԃ̕b��
    [SerializeField] float stopSeconds;
    // �����Ԃ̕b��
    //[SerializeField] float patrolSeconds;
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

    // �o�H�̒��p�_
    [SerializeField] Vector3[] wayPoints;
    // �ǂ̂悤�ȏ��ԂŒ��p�_��ʂ邩
    [SerializeField] int[] patrolRoute;
    // ���p�_�Ԃ��ړ�����b��
    [SerializeField] float[] patrolSeconds;
    // ���p�_�łǂꂾ����~�����邩
    [SerializeField] float patrolStopSeconds;

    // ���G�͈�
    [SerializeField] float searchRange;
    // �U���J�n�͈�
    [SerializeField] float attackRange;
    // ���낢��M��̂͂����܂� ----------------------------------------------------------------------------------------------------

    // �����ʒu
    [SerializeField] Vector3 firstPos;// �C���X�y�N�^�Ō������̂�SerializeField
    // �s�����[�h
    [SerializeField] string state = "�s�����[�h";// �C���X�y�N�^�Ō������̂�SerializeField
    // ��Ԃ��J�ڂ��Ă���̌o�ߕb���i��ԑJ�ڃJ�E���g�j
    [SerializeField] float stateTransitionCount;// �C���X�y�N�^�Ō������̂�SerializeField
    // �p�g���[���̌o�ߕb��
    [SerializeField] float patrolCount;// �C���X�y�N�^�Ō������̂�SerializeField
    // �ڕW�n�̍��W
    [SerializeField] Vector3 targetPos;// �C���X�y�N�^�Ō������̂�SerializeField
    // �ڕW���p�_
    [SerializeField] int nextTargetNum;// �C���X�y�N�^�Ō������̂�SerializeField

    // �v���C���[GameObject
    private GameObject Player;
    // �v���C���[�̕��ʍ��W
    private Vector3 playerPos;
    // ������Rigidbody�R���|�[�l���g
    private Rigidbody RB;
    // �U������GameObject
    private GameObject AttackCollision;

    // ����̂ݏ���
    void Start()
    {
        // �����͒�~���
        state = "patrol";
        // ��ԑJ�ڃJ�E���g������
        stateTransitionCount = 0.0f;

        // �����ʒu�擾
        firstPos = transform.position;
        // �o�H�̒��p�_�𑊑΍��W�����΍��W��
        for (int i = 0; i < wayPoints.Length; i++)
        {
            wayPoints[i] += firstPos;
        }
        //
        targetPos = wayPoints[1];
        // �ڕW���p�_������
        nextTargetNum = 1;

        // �v���C���[�I�u�W�F�N�g�擾
        Player = GameObject.FindGameObjectWithTag("Player");
        // ������Rigidbody�R���|�[�l���g�擾
        RB = GetComponent<Rigidbody>();
        // �U������GameObject�擾
        AttackCollision = transform.Find("AttackCollision").gameObject;
    }

    // �t���[��������
    void Update()
    {

        // �v���C���[�̕��ʍ��W�̎擾
        playerPos.x = Player.transform.position.x;
        playerPos.y = transform.position.y;
        playerPos.z = Player.transform.position.z;

        // �^�[�Q�b�g�ʒu�̍X�V
        if (patrolCount >= patrolSeconds[LoopNumCal(nextTargetNum, -1, patrolSeconds.Length)])
        {
            patrolCount = 0.0f;
            nextTargetNum = LoopNumCal(nextTargetNum, +1, patrolSeconds.Length);
            targetPos = wayPoints[patrolRoute[nextTargetNum]];
        }
        
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
                    //transform.LookAt(targetPos);
                }

                break;

            // �����Ԃ̏��� ----------------------------------------------------------------------------------------------------
            case "patrol":

                // �ڕW���p�n�_���牓���Ԃ͖ڕW���p�n�_�Ɍ������Đi�s
                if ((transform.position - wayPoints[patrolRoute[nextTargetNum]]).magnitude > 0.5f)
                {
                    transform.LookAt(new Vector3(targetPos.x, transform.position.y, targetPos.z));
                    RB.AddForce(transform.forward * 2.0f, ForceMode.Acceleration);
                }
                // �ڕW���p�n�_�ɋ߂Â�����A�ڕW���p�n�_�����̒��p�n�_�ɍX�V���A��~��ԂɑJ��
                else
                {
                    RB.AddForce(-RB.velocity, ForceMode.Acceleration);
                    if (RB.velocity.magnitude < 0.01f)
                    {
                        RB.velocity = Vector3.zero;
                    }
                }

                // ����������͈͓��Ƀv���C���[�������Ă����ꍇ�A�ǔ���ԂɑJ��
                if ((transform.position - playerPos).magnitude <= searchRange)
                {
                    state = "chase";
                    stateTransitionCount = 0.0f;
                }

                break;

            // �ǔ���Ԃ̏��� ----------------------------------------------------------------------------------------------------
            case "chase":

                // �ǔ���ԂɑJ�ڂ��Ă����莞�Ԉȓ��̏ꍇ�A�v���C���[�̕��������Ȃ��猸������~����
                if (stateTransitionCount <= chaseStartSeconds)
                {
                    //transform.LookAt(playerPos);
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
                    state = "stop";
                    stateTransitionCount = 0.0f;
                }

                break;

            /*�ǔ���Ԍ�̏��� ----------------------------------------------------------------------------------------------------
            case "back to patrol":

                RB.velocity = Vector3.zero;
                transform.LookAt(firstPos);
                transform.position = Vector3.MoveTowards(transform.position, firstPos, backSpeed * Time.deltaTime);
                //�����ʒu�ɖ߂�
                if (transform.position == firstPos)
                {
                    state = "stop";
                    stateTransitionCount = 0.0f;
                }
                // ����������͈͓��Ƀv���C���[�������Ă����ꍇ�A�ǔ���ԂɑJ��
                if ((transform.position - playerPos).magnitude <= searchRange)
                {
                    state = "chase";
                    stateTransitionCount = 0.0f;
                }

                break;*/

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
                    AttackCollision.SetActive(true);
                }

                // �U������ON�����莞�Ԍo�߂����ꍇ�A�U������OFF�ɂ���ԑJ��
                else
                {
                    AttackCollision.SetActive(false);
                    // ����������͈͓��Ƀv���C���[������ꍇ�A�ǔ���ԂɑJ��
                    if ((transform.position - playerPos).magnitude <= searchRange)
                    {
                        state = "chase";
                        stateTransitionCount = 0.0f;
                    }
                    // ����������͈͓��Ƀv���C���[�����Ȃ��ꍇ�A��~��ԂɑJ��
                    else
                    {
                        state = "stop";
                        stateTransitionCount = 0.0f;
                    }
                }

                break;

        }

        // ��ԑJ�ڃJ�E���g�Ɍo�ߎ��Ԃ����Z
        stateTransitionCount += Time.deltaTime;
        // �p�g���[���̌o�ߕb���Ɍo�ߎ��Ԃ����Z
        patrolCount += Time.deltaTime;
    }




    /*
    private void OnCollisionEnter(Collision other)
    {
        //�v���C���[�ɐG�ꂽ��
        if (other.gameObject.tag == "Player")
        {
            if (ta.ismove_Statue == true)
            {

                //�t���ǉ�
                state = "Dead";
                //state = "Damage";
                //ta.ismove_Statue = false;
            }
        }
    }*/


    // 0��1��2���E�E�E��length-2��length-1�i�ő�l�j��0��1���E�E�E�ƃ��[�v����length�̐��̃`�F�[����
    // cur_num����add_num�i�񂾐����Ƃ郁�\�b�h
    //
    // �� 0��1��2��3��4��5��6��7��0��1��2���E�E�E�Ƃ������[�v�i�܂�length=8�j�ŁE�E�E
    //   5����4�i�񂾐���m�肽�� �� LoopNumCal(5, +4, 8) �� 1���Ԃ�
    //   2����7�߂�������m�肽�� �� LoopNumCal(2, -7, 8) �� 3���Ԃ�
    //
    // %���Z�ŏ�肭�����Ǝv�������A�}�C�i�X�l����肭�����Ȃ��̂ƋL�q�������Ȃ�̂ł��̃��\�b�h�������
    // ��Ɍo�H�ړ���nextTargetNum�p
    private int LoopNumCal(int cur_num, int add_num, int length)
    {
        cur_num += add_num;
        if (length <= cur_num)
        {
            cur_num = cur_num % length;
        }
        if (cur_num < 0)
        {
            cur_num = cur_num % length + length;
        }
        return cur_num;
    }

    // �M�Y���`��
    void OnDrawGizmosSelected()
    {
        var guiStyle = new GUIStyle { fontSize = 20, normal = { textColor = Color.blue } };
        // ���G�͈́��U���J�n�͈͕`��
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, searchRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // ���p�_���o�H�̕`��
        Gizmos.color = Color.cyan;
        for (int i = 0; i < wayPoints.Length; i++)
        {
            if (UnityEditor.EditorApplication.isPlaying)
            {
                Gizmos.DrawWireSphere(wayPoints[i], 0.5f);
                Handles.Label(wayPoints[i] + new Vector3(0, 1, 0), i + "", guiStyle);
            }
            else
            {
                Gizmos.DrawWireSphere(transform.position + wayPoints[i], 0.5f);
                Handles.Label(transform.position + wayPoints[i] + new Vector3(0, 1, 0), i + "", guiStyle);
            }
        }
        for (int i = 0; i < patrolRoute.Length; i++)
        {
            if (UnityEditor.EditorApplication.isPlaying)
            {
                Gizmos.DrawLine(wayPoints[patrolRoute[i]], wayPoints[patrolRoute[LoopNumCal(i,1, patrolRoute.Length)]]);
            }
            else
            {
                Gizmos.DrawLine(transform.position + wayPoints[patrolRoute[i]], transform.position + wayPoints[patrolRoute[LoopNumCal(i, 1, patrolRoute.Length)]]);
            }
        }

        // �ڕW�n�̕`��
        if (UnityEditor.EditorApplication.isPlaying)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(targetPos, 0.5f);
        }
    }


    /*

    // �C���X�y�N�^�[�g��
    // �Q�l�uhttps://qiita.com/sango/items/b705980ada56ba8ffa04�v
#if UNITY_EDITOR
    [CustomEditor(typeof(M_Enemy_01))]// �C���X�y�N�^�[�g�����̂��܂��Ȃ�
    public class CharacterEditor : Editor// �C���X�y�N�^�[�g�����̂��܂��Ȃ�
    {

        public override void OnInspectorGUI()
        {
            // �C���X�y�N�^�[�g�����̂��܂��Ȃ�
            M_Enemy_01 thisScr = target as M_Enemy_01;




        }
    }
#endif

    */

}
