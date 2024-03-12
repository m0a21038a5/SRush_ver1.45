using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy02AniOnOff : MonoBehaviour
{
    // �n��G�A�j���[�V�����Đ��A�ړ��̂݃��f����؂�ւ���
    public GameObject WalkAniObject;
    public GameObject IdleAniObject;
    public StatueEnemyMove sem;
    public StatueHPManager shpm;
    void Start()
    {
        sem = transform.root.gameObject.GetComponent<StatueEnemyMove>();
        shpm = transform.root.gameObject.GetComponent<StatueHPManager>();
    }

    void Update()
    {
        if (sem.state == "stop")
        {
            WalkAniObject.SetActive(false);
            IdleAniObject.SetActive(true);
        }
        if (sem.state == "patrol" || sem.state == "chase")
        {
            WalkAniObject.SetActive(true);
            IdleAniObject.SetActive(false);
        }
        if (sem.state == "attack")
        {
            WalkAniObject.SetActive(true);
            IdleAniObject.SetActive(false);
        }
    }
}