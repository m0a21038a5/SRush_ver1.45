using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamage : MonoBehaviour
{
    // HP�����炷���߂̃X�N���v�g�ł�
    //�U����������ꏊ�ɂ��Ă�������(�r�[���G�Ȃ�r�[���ɂ��āA�ߐړG�Ȃ�r�̐�Ƃ�)
    //Slider��MaxValue���ő�HP�ɂ��Ă�������
    // DamageValue�̓_���[�W�̑傫���ł�

    public PlayerHP pHP;
    public StatueEnemyMove SEM;
    public bool isDamaged = false;
    
    //private GameObject PlayerHPSlider;
    void Start()
    {
        pHP = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHP>();
        SEM = GetComponentInParent<StatueEnemyMove>();
    }

    private void Update()
    {
        if (SEM.stateTransitionCount < 0.1f)
        {
            isDamaged = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && isDamaged == false && SEM.stateTransitionCount > 0.1f)
        {//�v���C���[�ɓ���������DamageValue�̕��������炷

            pHP.Damage = true;
            isDamaged = true;
            pHP.DamageCount = 0;
        }
    }


    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && pHP.DamageCount == 0)
        {//�v���C���[�ɓ���������DamageValue�̕��������炷
           
            pHP.Damage = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
           pHP.DamageCount = 0;
        }
    }*/
}
