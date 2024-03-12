using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamEnemy : MonoBehaviour
{
    public BeamBody beamBodyEnemy;
    public float BeamSpeed;
    [SerializeField]
    private GameObject BeamPrefab;

    //�t���ǉ�
    private target ta;


    #region// �i���ǉ�
    // ��������
    public float Move_Dist;
    // �㉺�A�O��A���E�ǂ̕����ɓ�����
    public bool Patrol_UPDOWN;
    public bool Patrol_FRONTBACK;
    public bool Patrol_LEFTRIGHT;
    // �ŏ��̓���
    public bool First;

    public bool isTrigger_Player = false;

    float Count;
    // �������x
    public float Move_Speed;
    #endregion




    public enum BeamEnemyStatus
    {
        WaitStop,//�~�܂��Ă���ҋ@
        WaitWalk,//���񂵂Ă���ҋ@
        ChasePlayerWalk,//�v���C���[��ǂ�������
        Beam,//�r�[������
        DamegePlayerAttack,//�v���C���[�̍U�����󂯂�
        KnockDown//�r�[���̓G���|�����
    }
    public BeamEnemyStatus beamEnemyStatus;
    void Start()
    {
        //�t���ǉ�
        GameObject Playerobj = GameObject.Find("Player");
        ta = Playerobj.GetComponent<target>();

        #region // �i���ǉ� 
        Count = Move_Dist / (Move_Speed * Time.deltaTime * 2);
        #endregion

    }
    void Update()
    {
        switch (beamEnemyStatus)
        {
            case BeamEnemyStatus.WaitStop:
                //Debug.Log("�ҋ@��");
                EnemyWait();
                break;
            case BeamEnemyStatus.WaitWalk:
                //Debug.Log("����");
                EnemyWalk();
                break;
            case BeamEnemyStatus.ChasePlayerWalk:
                //Debug.Log("�v���C���[��ǂ�������!");
                EnemyChase();
                break;
            case BeamEnemyStatus.Beam:
               // Debug.Log("�r�[������!");
                EnemyBeam();
                break;
            case BeamEnemyStatus.DamegePlayerAttack:
                //Debug.Log("�v���C���[�̍U�����󂯂�!");
                EnemyDamage();
                break;
            case BeamEnemyStatus.KnockDown:
                //Debug.Log("�r�[���̓G���|���ꂽ!");
                EnemyKnockDown();
                break;
        }
    }
    void EnemyWait()
    {
        beamEnemyStatus = BeamEnemyStatus.ChasePlayerWalk;
    }
    void EnemyWalk()
    {
        //BeamBody�X�N���v�g�𖳌��ɂ���
        beamBodyEnemy.enabled = false;

        if (First == true)
        {
            if (Count * Move_Speed * Time.deltaTime < Move_Dist)
            {
                if (Patrol_UPDOWN == true)
                {
                    // �㏸����
                    transform.position += transform.up * Move_Speed * Time.deltaTime;
                }

                if (Patrol_FRONTBACK == true)
                {
                    // �O�ɐi��
                    transform.position += transform.forward * Move_Speed * Time.deltaTime;
                }

                if (Patrol_LEFTRIGHT == true)
                {
                    // �E�ɐi��
                    transform.position += transform.right * Move_Speed * Time.deltaTime;
                }
                Count++;
            }
            else First = false;
        }
        else
        {
            if (0 < Count * Move_Speed * Time.deltaTime)
            {
                if (Patrol_UPDOWN == true)
                {
                    // ���~����
                    transform.position -= transform.up * Move_Speed * Time.deltaTime;
                }

                if (Patrol_FRONTBACK == true)
                {
                    // ���ɐi��
                    transform.position -= transform.forward * Move_Speed * Time.deltaTime;
                }

                if (Patrol_LEFTRIGHT == true)
                {
                    // ���ɐi��
                    transform.position -= transform.right * Move_Speed * Time.deltaTime;
                }
                Count--;
            }
            else First = true;
        }
    }
    void EnemyChase()
    {
        //BeamBody�X�N���v�g�𖳌��ɂ���
        beamBodyEnemy.enabled = false;
        //�Ƃ肠�����v���C���[�̕���������
        GameObject PlayerObject = GameObject.FindGameObjectWithTag("Player");
        transform.LookAt(PlayerObject.transform);
        
        // StartCoroutine(BeamCoroutine());
    }
    void EnemyBeam()
    {
        //�Ƃ肠�����v���C���[�̕���������
        GameObject PlayerObject = GameObject.FindGameObjectWithTag("Player");
        transform.LookAt(PlayerObject.transform);
        //BeamBody�X�N���v�g��L���ɂ���
        beamBodyEnemy.enabled=true;
    }
    void EnemyDamage()
    {

    }

    void EnemyKnockDown()
    {
        //�v���C���[�ɓ���������j�󂷂�
        Destroy(this.gameObject);

    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {//�v���C���[�ɓ���������KnockDown�ɂ���
            beamEnemyStatus = BeamEnemyStatus.KnockDown;

            //�t���ǉ�
            ta.isTarget_Beam = false;
            ta.ismove_Beam = false;
        }

    }
    IEnumerator BeamCoroutine()
    {
        yield return new WaitForSecondsRealtime(3.0f);
        GameObject shell = Instantiate(BeamPrefab, transform.position, Quaternion.identity);
        Rigidbody shellRb = shell.GetComponent<Rigidbody>();
        shellRb.AddForce(transform.forward * BeamSpeed);
        Destroy(shell, 3.0f);
    }
}
