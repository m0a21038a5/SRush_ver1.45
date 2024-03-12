using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamArea_ver2 : MonoBehaviour
{
    public GameObject EnemyGameObject;
    private BeamEnemy_ver2 EnemyStatus;
    void Start()
    {
        EnemyStatus = EnemyGameObject.GetComponent<BeamEnemy_ver2>();
        
    }

    void Update()
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {//�v���C���[�ɓ���������EnemyBeam�ɂ���
           EnemyStatus.beamEnemyStatus = BeamEnemy_ver2.BeamEnemyStatus.Beam;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {//�v���C���[�����ꂽ��EnamyChase�ɂ���
            EnemyStatus.beamEnemyStatus = BeamEnemy_ver2.BeamEnemyStatus.ChasePlayerWalk;
        }
    }
}
