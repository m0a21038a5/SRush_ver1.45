using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StatueEnemyMove : MonoBehaviour
{
    // ���낢��M��̂͂������� ----------------------------------------------------------------------------------------------------

    // �o�H�̒��p�_
    [SerializeField] public  Vector3[] wayPoints;
    // �ǂ̂悤�ȏ��ԂŒ��p�_��ʂ邩
    [SerializeField] public int[] patrolRoute;
    // ���p�_�Ԃ��ړ�����b��
    [SerializeField] public float[] patrolSeconds;

    // ���񎞑��x
    [SerializeField]public float patrolSpeed;
    // �ǔ������x
    [SerializeField] public float chaseSpeed;
    // ��]���x
    [SerializeField]public float rotateSpeed;
    // ������
    [SerializeField]public float accelPower;
    // ������
    [SerializeField]public float breakPower;

    // ��~��ԂɂȂ��Ă���~�܂��Ă���b��
    [SerializeField]public float stopSeconds;
    // �ǔ���ԂɂȂ��Ă���ǔ����J�n����b��
    [SerializeField]public float chaseStartSeconds;
    // �U�����蔭�����Ă���v���C���[��_���b��
    [SerializeField]public float attackAimSeconds;
    // �U����ԂɂȂ��Ă���U�����蔭���܂ł̕b��
    [SerializeField]public float attackStartSeconds;
    // �U�����肪�������Ă���I���܂ł̕b��
    [SerializeField]public float attackStopSeconds;
    // �p�g���[�����̖ڈ���~����
    [SerializeField]public float patrolStopSeconds;

    // ���G�͈�
    [SerializeField]public float searchRange;
    // �U���J�n�͈�
    [SerializeField]public float attackRange;
    // ���p�_�̔��a
    [SerializeField]public float wayPointsRadius;

    // ���낢��M��̂͂����܂� ----------------------------------------------------------------------------------------------------

    // �����ʒu
    [SerializeField]public Vector3 firstPos;
    // �s�����[�h
    [SerializeField] public string state = "�s�����[�h";
    // ��Ԃ��J�ڂ��Ă���̌o�ߕb���i��ԑJ�ڃJ�E���g�j
    [SerializeField] public float stateTransitionCount;
    // �ڕW���p�_
    [SerializeField] int nextTargetNum;
    // �ڕW�n�̍��W
    [SerializeField] Vector3 targetPos;
    // �p�g���[���̌o�ߕb��
    [SerializeField] float patrolCount;
    // �p�g���[�����~���邩�ǂ���
    [SerializeField] bool stopPatrol;
    // ���̌X��
    [SerializeField] Vector3 floorNormal;

    // �M�Y������ɕ\�����邩�ǂ���
    [SerializeField] bool isDrawPatrolRouteGizmos;
    [SerializeField] bool isDrawWayPointsNumberGizmos;
    [SerializeField] bool isDrawTargetGizmos;
    [SerializeField] bool isDrawStateGizmos;
    [SerializeField] bool isDrawRangeGizmos;
    [SerializeField] bool isDrawFloorNormalGizmos;

    // �v���C���[GameObject
    private GameObject Player;
    // �v���C���[�̍��W
    private Vector3 playerPos;
    // ������Rigidbody�R���|�[�l���g
    private Rigidbody RB;
    // �U������GameObject
    private GameObject AttackCollision;
    private GameObject enemyEffect;
    private GameObject EnemyEffect;

    // ����̂ݏ���
    void Start()
    {
        // �����͏�����
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
        // �ڕW���p�_�֌W��1�Ԗڂŏ�����
        nextTargetNum = 1;
        targetPos = wayPoints[1];
        patrolCount = patrolSeconds[1];

        // �p�g���[���t���O������
        stopPatrol = false;
        // ���̖@���x�N�g��������
        floorNormal = new Vector3(0.0f, -1.0f, 0.0f);

        // �v���C���[�I�u�W�F�N�g�擾
        Player = GameObject.FindGameObjectWithTag("Player");
        // ������Rigidbody�R���|�[�l���g�擾
        RB = GetComponent<Rigidbody>();
        // �U������GameObject�擾
        AttackCollision = transform.Find("AttackCollision").gameObject;
        EnemyEffect = transform.Find("da").gameObject;
        enemyEffect = transform.Find("dage").gameObject;
    }

    // ���t���[������
    void Update()
    {
        // �v���C���[�̍��W�̎擾
        playerPos = Player.transform.position;


        // �������p�_�̑J�ڂ܂ł̌o�ߕb�����߂��Ă���^�[�Q�b�g�����̒��p�_��
        if (patrolCount < 0.0f)
        {
            nextTargetNum = (nextTargetNum + 1) % patrolRoute.Length;
            patrolCount = patrolSeconds[nextTargetNum] - patrolCount;
            targetPos = wayPoints[patrolRoute[nextTargetNum]];
            stopPatrol = false;
        }

        // ��ԑJ�ڃJ�E���g�Ɍo�ߎ��Ԃ����Z
        stateTransitionCount -= Time.deltaTime;
        // �p�g���[���̌o�ߕb���Ɍo�ߎ��Ԃ����Z
        patrolCount -= Time.deltaTime;

        // ��Ԃɂ���ď�������i��ԑJ�ڂȂǁA�������Z���s�K�v�ȏ����i�������Z���͉���FixedUpdate�j�j
        switch (state)
        {
            // ��~��Ԃ̏��� ----------------------------------------------------------------------------------------------------
            case "stop":

                // ����������͈͓��Ƀv���C���[�������Ă����ꍇ�A�ǔ���ԂɑJ��
                if ((transform.position - playerPos).magnitude <= 15.0f)
                {
                    state = "chase";
                    stateTransitionCount = chaseStartSeconds;
                }

                // ��ԑJ�ڃJ�E���g�����ȏ�ɂȂ����ꍇ�A�����ԂɑJ��
                if (stateTransitionCount < 0.0f)
                {
                    state = "patrol";
                    stateTransitionCount = 0.0f;
                }

                break;

            // �����Ԃ̏��� ----------------------------------------------------------------------------------------------------
            case "patrol":

                // ����������͈͓��Ƀv���C���[�������Ă����ꍇ�A�ǔ���ԂɑJ��
                if ((transform.position - playerPos).magnitude <= searchRange)
                {
                    state = "chase";
                    stateTransitionCount = chaseStartSeconds;
                }

                break;

            // �ǔ���Ԃ̏��� ----------------------------------------------------------------------------------------------------
            case "chase":

                // ����������͈͊O�Ƀv���C���[���o���ꍇ�A�����ԂɑJ��
                if ((transform.position - playerPos).magnitude > searchRange)
                {
                    state = "stop";
                    stateTransitionCount = stopSeconds;
                }

                // ����������͈͓��Ƀv���C���[�������Ă����ꍇ�A�U����ԂɑJ��
                if ((transform.position - playerPos).magnitude <= attackRange)
                {
                    state = "attack";
                    stateTransitionCount = attackStartSeconds + attackStopSeconds;
                }

                break;

            // �U����Ԃ̏��� ----------------------------------------------------------------------------------------------------
            case "attack":

                // attackStartSeconds�b�o�߂�����U������ON
                if (0.0f <= stateTransitionCount && stateTransitionCount < attackStopSeconds)
                {
                    AttackCollision.SetActive(true);
                    EnemyEffect.SetActive(true);
                    enemyEffect.SetActive(true);
                }
                // attackStartSeconds+attackStopSeconds�b�o�߂�����i�J�E���g�_�E����0�ɂȂ�����j�U������OFF����ԑJ��
                else if (stateTransitionCount < 0.0f)
                {
                    AttackCollision.SetActive(false);
                    EnemyEffect.SetActive(false);
                    enemyEffect.SetActive(false);

                    // ����������͈͓��Ƀv���C���[������ꍇ�A�ĂэU��
                    if ((transform.position - playerPos).magnitude <= attackRange)
                    {
                        stateTransitionCount = attackStartSeconds + attackStopSeconds + attackAimSeconds;
                    }
                    // ����������͈͓��Ƀv���C���[������ꍇ�A�ǔ���ԂɑJ��
                    else if ((transform.position - playerPos).magnitude <= searchRange)
                    {
                        state = "chase";
                        stateTransitionCount = chaseStartSeconds;
                    }
                    // ����������͈͓��Ƀv���C���[�����Ȃ��ꍇ�A��~��ԂɑJ��
                    else
                    {
                        state = "stop";
                        stateTransitionCount = stopSeconds;
                    }
                }

                break;

        }
    }

    // �����֌W�̖��t���[������
    void FixedUpdate()
    {
        // �^�[�Q�b�g�ւ̃x�N�g��
        Vector3 ToTargetVec = wayPoints[patrolRoute[nextTargetNum]] - transform.position;
        // �v���C���[�ւ̃x�N�g��
        Vector3 ToPlayerVec = playerPos - transform.position;
        // �^�[�Q�b�g�ւ̊p�x�����v�Z�ipatrol�Ŏg�p�j
        float AngleToTargrt = Vector2.SignedAngle(new Vector2(transform.forward.x, transform.forward.z), new Vector2(ToTargetVec.x, ToTargetVec.z));
        // �v���C���[�ւ̊p�x�����v�Z�ichase��Attack�Ŏg�p�j
        float AngleToPlayer = Vector2.SignedAngle(new Vector2(transform.forward.x, transform.forward.z), new Vector2(ToPlayerVec.x, ToPlayerVec.z));

        // ���̌X���ɍ��킹���d�͕␳������
        RB.AddForce((Physics.gravity.magnitude * floorNormal) - Physics.gravity, ForceMode.Acceleration);

        // ��Ԃɂ���ď�������i�ړ��ȂǁA�������Z���K�v�ȏ����i��ԑJ�ړ��͏��Update�j�j
        switch (state)
        {
            // ��~��Ԃ̏��� ----------------------------------------------------------------------------------------------------
            case "stop":

                // ���x�����ȏ�̏ꍇ�A��R�������Ď~�߂�����
                RB.AddForce(-RB.velocity * breakPower, ForceMode.Acceleration);
                if (RB.velocity.magnitude < 0.01f)
                {
                    RB.velocity = Vector3.zero;
                }

                break;

            // �����Ԃ̏��� ----------------------------------------------------------------------------------------------------
            case "patrol":

                // �p�x���^�[�Q�b�g���ʂɍ����ĂȂ��ꍇ�͉�]
                if ( Mathf.Abs(AngleToTargrt) > 1.0f)
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0.0f, Mathf.Atan2(ToTargetVec.x, ToTargetVec.z) * Mathf.Rad2Deg, 0.0f), rotateSpeed);
                }
                // �p�x���^�[�Q�b�g���ʂɂ������������Ă�ꍇ�͈ړ�
                else
                {
                    // �ڕW���p�n�_���牓���Ԃ͖ڕW���p�n�_�Ɍ������Đi�s
                    if (new Vector2(ToTargetVec.x, ToTargetVec.z).magnitude > wayPointsRadius)
                    {
                        if (new Vector2(RB.velocity.x, RB.velocity.z).magnitude < patrolSpeed)
                        {
                            RB.AddForce(ToTargetVec.normalized * accelPower, ForceMode.Acceleration);
                        }
                    }
                    // �ڕW���p�n�_�ɋ߂Â�����p�g���[�����~
                    else
                    {
                        stopPatrol = true;
                    }
                    // �p�g���[����~��Ԃ������猸��������
                    if (stopPatrol == true)
                    {
                        RB.AddForce(-RB.velocity * breakPower, ForceMode.Acceleration);
                        if (RB.velocity.magnitude < 0.01f)
                        {
                            RB.velocity = Vector3.zero;
                        }
                    }
                }

                break;

            // �ǔ���Ԃ̏��� ----------------------------------------------------------------------------------------------------
            case "chase":

                // �v���C���[�����G�͈͂ɓ����Ă����莞�Ԃ͒�~���ăv���C���[�̕�������
                if (stateTransitionCount >= 0.0f)
                {
                    // �p�x���v���C���[���ʂɍ����ĂȂ��ꍇ�̓v���C���[���ʂɉ�]
                    if (Mathf.Abs(AngleToPlayer) > 1.0f)
                    {
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0.0f, Mathf.Atan2(ToPlayerVec.x, ToPlayerVec.z) * Mathf.Rad2Deg, 0.0f), rotateSpeed);
                    }
                }
                // ���ꂪ�߂�����v���C���[��ǂ�������
                else
                {
                    transform.LookAt(new Vector3(playerPos.x, transform.position.y, playerPos.z));
                    if (new Vector2(RB.velocity.x, RB.velocity.z).magnitude < chaseSpeed)
                    {
                        RB.AddForce(ToPlayerVec.normalized * accelPower, ForceMode.Acceleration);
                    }
                }

                break;

            // �U����Ԃ̏��� ----------------------------------------------------------------------------------------------------
            case "attack":

                // ���x�����ȏ�̏ꍇ�A��R�������Ď~�߂�����
                if (RB.velocity.magnitude >= 0.01f)
                {
                    RB.AddForce(-RB.velocity, ForceMode.Acceleration);
                }
                else
                {
                    RB.velocity = Vector3.zero;
                }
                // �ĂэU���ɂȂ������AattackAimSeconds�b�Ԃ̓v���C���[��_��
                if ( stateTransitionCount >= attackStartSeconds + attackStopSeconds)
                {
                    // �p�x���v���C���[���ʂɍ����ĂȂ��ꍇ�̓v���C���[���ʂɉ�]
                    if (Mathf.Abs(AngleToPlayer) > 1.0f)
                    {
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0.0f, Mathf.Atan2(ToPlayerVec.x, ToPlayerVec.z) * Mathf.Rad2Deg, 0.0f), rotateSpeed);
                    }
                }

                break;
        }
    }

    // �����ɐG��Ă���Ԃ̏���
    private void OnCollisionStay(Collision collision)
    {
        // �Ƃ肠�����ڐG�_�̐������񂷁i�ڐG�_�͔z��ł����擾�ł��Ȃ������̂Łj
        foreach (ContactPoint contact in collision.contacts)
        {
            // �������ƐG��Ă�����A���̖@���x�N�g�����擾
            if (collision.transform.tag == "Floor")
            {
                floorNormal = - contact.normal;
            }
        }
    }

    // ��������M�Y���`�恕�C���X�y�N�^�[�g���֘A�i�����Ɋ֌W�Ȃ��j ----------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
    // �M�Y���`��i��Ɂj
    void OnDrawGizmos()
    {
        // �G�f�B�^���s��
        if (UnityEditor.EditorApplication.isPlaying)
        {
            if (isDrawPatrolRouteGizmos == true)
            {
                DrawPatrolRouteGizmos(Vector3.zero);
            }
            if (isDrawWayPointsNumberGizmos == true)
            {
                DrawWayPointsNumberGizmos(Vector3.zero);
            }
            if (isDrawTargetGizmos == true)
            {
                DrawTargetGizmos();
            }
            if (isDrawStateGizmos == true)
            {
                DrawStateGizmos();
            }
        }
        // �G�f�B�^���s���łȂ��Ƃ�
        else
        {
            if (isDrawPatrolRouteGizmos == true)
            {
                DrawPatrolRouteGizmos(transform.position);
            }
            if (isDrawWayPointsNumberGizmos == true)
            {
                DrawWayPointsNumberGizmos(transform.position);
            }
        }
        // ���ł�
        if (isDrawRangeGizmos == true)
        {
            DrawRangeGizmos();
        }
        if (isDrawFloorNormalGizmos == true)
        {
            DrawFloorNormalGizmos();
        }
    }
    // �M�Y���`��i�I�𒆁j
    void OnDrawGizmosSelected()
    {
        // �G�f�B�^���s��
        if (UnityEditor.EditorApplication.isPlaying)
        {
            if (isDrawPatrolRouteGizmos == false)
            {
                DrawPatrolRouteGizmos(Vector3.zero);
            }
            if (isDrawWayPointsNumberGizmos == false)
            {
                DrawWayPointsNumberGizmos(Vector3.zero);
            }
            if (isDrawTargetGizmos == false)
            {
                DrawTargetGizmos();
            }
            if (isDrawStateGizmos == false)
            {
                DrawStateGizmos();
            }
        }
        // �G�f�B�^���s���łȂ��Ƃ�
        else
        {
            if (isDrawPatrolRouteGizmos == false)
            {
                DrawPatrolRouteGizmos(transform.position);
            }
            if (isDrawWayPointsNumberGizmos == false)
            {
                DrawWayPointsNumberGizmos(transform.position);
            }
        }
        // ���ł�
        if (isDrawRangeGizmos == false)
        {
            DrawRangeGizmos();
        }
        if (isDrawFloorNormalGizmos == false)
        {
            DrawFloorNormalGizmos();
        }
    }

    // ���p�_&�o�H�`�惁�\�b�h
    void DrawPatrolRouteGizmos(Vector3 offset)
    {
        Gizmos.color = Color.cyan;
        for (int i = 0; i < wayPoints.Length; i++)
        {
            Gizmos.DrawWireSphere(offset + wayPoints[i], wayPointsRadius);
        }
        for (int i = 0; i < patrolRoute.Length; i++)
        {
            Gizmos.DrawLine(offset + wayPoints[patrolRoute[i]], offset + wayPoints[patrolRoute[(i + 1) % patrolRoute.Length]]);
        }
    }

    // ���p�_�ԍ��`�惁�\�b�h
    void DrawWayPointsNumberGizmos(Vector3 offset)
    {
        var guiStyle = new GUIStyle { fontSize = 20, normal = { textColor = Color.blue } };
        for (int i = 0; i < wayPoints.Length; i++)
        {
            Handles.Label(offset + wayPoints[i] + new Vector3(0.0f, 1.0f, 0.0f), i + "", guiStyle);
        }
    }

    // �ڕW�_�`�惁�\�b�h
    void DrawTargetGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(targetPos, 0.5f);
    }

    // ��ԕ`�惁�\�b�h
    void DrawStateGizmos()
    {
        var guiStyle = new GUIStyle { fontSize = 20, normal = { textColor = Color.blue } };
        Handles.Label(transform.position + new Vector3(0.0f, -1.0f, 0.0f), state, guiStyle);
    }

    // ���G�͈́��U���J�n�͈͕`�惁�\�b�h
    void DrawRangeGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, searchRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    // �@���x�N�g���`�惁�\�b�h
    void DrawFloorNormalGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position - (floorNormal * 10.0f));
    }

    // �C���X�y�N�^�[�g��
    [CustomEditor(typeof(StatueEnemyMove))]// ���܂��Ȃ�
    public class StatueEnemyMoveEditor : Editor// Editor���p���i�悭�킩���j
    {
        // �܂��݂̃t���O
        bool individualFolding = true;
        bool basicFolding = false;
        bool debugFolding = false;

        // GUI�`�惁�\�b�h
        public override void OnInspectorGUI()
        {
            StatueEnemyMove thisScript = target as StatueEnemyMove;
            // �V���A���C�Y�J�n�i�悭�킩��񂯂�CustomEditor�ɕK�v�j
            serializedObject.Update();

            // �ʐݒ�`��
            if (individualFolding = EditorGUILayout.Foldout(individualFolding, "�ʐݒ�"))
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(serializedObject.FindProperty("wayPoints"), new GUIContent("���p�_�̍��W"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("patrolRoute"), new GUIContent("���p�_��ʂ鏇��"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("patrolSeconds"), new GUIContent("���p�_��ʂ�b��"));
                thisScript.isWarning();
                if (GUILayout.Button("�b���̎����ݒ�"))
                {
                    Undo.RecordObject(thisScript, "�b���̎����ݒ�");
                    thisScript.AutoSecondsSetting();
                    EditorUtility.SetDirty(target);
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.Space();

            // ��{�ݒ�`��
            if (basicFolding = EditorGUILayout.Foldout(basicFolding, "���ʐݒ�"))
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(serializedObject.FindProperty("patrolSpeed"), new GUIContent("���񒆂̃X�s�[�h"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("chaseSpeed"), new GUIContent("�ǔ����̃X�s�[�h"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("rotateSpeed"), new GUIContent("��]����X�s�[�h"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("accelPower"), new GUIContent("�������̃p���["));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("breakPower"), new GUIContent("�������̃p���["));
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("stopSeconds"), new GUIContent("��~��Ԃ̕b��"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("chaseStartSeconds"), new GUIContent("�ǔ���Ԃ̒ǔ��J�n�b��"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("attackAimSeconds"), new GUIContent("�U����Ԃ̑_���b��"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("attackStartSeconds"), new GUIContent("�U���J�n�܂ł̕b��"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("attackStopSeconds"), new GUIContent("�U���Ɋ|����b��"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("patrolStopSeconds"), new GUIContent("���񒆂̋x�e�ڈ��b��"));
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("searchRange"), new GUIContent("���G�͈�"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("attackRange"), new GUIContent("�U���J�n�͈�"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("wayPointsRadius"), new GUIContent("���p�_�̔��a"));
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.Space();

            // �f�o�b�O���`��
            if (debugFolding = EditorGUILayout.Foldout(debugFolding, "�f�o�b�N���"))
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(serializedObject.FindProperty("firstPos"), new GUIContent("�����ʒu"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("state"), new GUIContent("�s�����[�h"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("stateTransitionCount"), new GUIContent("��ԑJ�ڃJ�E���g"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("nextTargetNum"), new GUIContent("�ڕW���p�n�_"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("targetPos"), new GUIContent("�ڕW���p�n�_�̍��W"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("patrolCount"), new GUIContent("�p�g���[���o�߃J�E���g"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("floorNormal"), new GUIContent("���̖@���x�N�g��"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("stopPatrol"), new GUIContent("�p�g���[����~�t���O"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("isDrawPatrolRouteGizmos"), new GUIContent("�펞�o�H�M�Y���\��"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("isDrawWayPointsNumberGizmos"), new GUIContent("�펞�ԍ��M�Y���\��"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("isDrawTargetGizmos"), new GUIContent("�펞�ڕW�_�M�Y���\��"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("isDrawStateGizmos"), new GUIContent("�펞��ԃM�Y���\��"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("isDrawRangeGizmos"), new GUIContent("�펞�͈̓M�Y���\��"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("isDrawFloorNormalGizmos"), new GUIContent("�펞���@���M�Y���\��"));
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.Space();

            // �V���A���C�Y�I���i�悭�킩��񂯂�CustomEditor�ɕK�v�j
            serializedObject.ApplyModifiedProperties();
        }
    }

    // �����b���ݒ胁�\�b�h
    public void AutoSecondsSetting()
    {
        patrolSeconds = new float[patrolRoute.Length];
        patrolSeconds[0] = (wayPoints[patrolRoute[patrolRoute.Length-1]] - wayPoints[patrolRoute[0]]).magnitude / patrolSpeed + patrolStopSeconds;
        for (int i=1; i< patrolSeconds.Length; i++)
        {
            patrolSeconds[i] = (wayPoints[patrolRoute[i]] - wayPoints[patrolRoute[i-1]]).magnitude / patrolSpeed + patrolStopSeconds;
        }
    }

    // �x���\�����\�b�h
    public void isWarning()
    {
        for (int i=0; i<patrolRoute.Length; i++)
        {
            if (patrolRoute[i] <= -1 || wayPoints.Length <= patrolRoute[i]  )
            {
                EditorGUILayout.HelpBox($"�u���p�_��ʂ鏇�ԁv�ő��݂��Ȃ����p�_���g�����Ƃ��Ă��܂�\n���̏ꍇ 0�`{wayPoints.Length-1} �͈̔͂Ŏw�肷�邩���p�_�𑝂₵�Ă�", MessageType.Warning, true);
            }
        }
        if (patrolRoute.Length != patrolSeconds.Length)
        {
            EditorGUILayout.HelpBox("�u���p�_��ʂ鏇�ԁv�Ɓu���p�_��ʂ�b���v�͓������ɂ��Ă�������\n�Ƃ肠�������́u�b���̎����ݒ�v�{�^���������Ɖ����ł��邩��", MessageType.Warning, true);
        }
    }
#endif

}
