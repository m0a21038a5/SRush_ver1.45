using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoruriCutin : MonoBehaviour
{//�K�E�Z�̃J�b�g�C������X�N���v�g
    public target t;
    public Image Cutin;
    private Animator CutinAnimator;
    private bool isSECutin;//true�ɂ���ƁA������񂾂��炷
    private GameObject playerse;//SEPlayer�̎擾
    private Soundtest sd;//���̃X�N���v�g�擾

    private string CutinAni = "isCutin";
    void Start()
    {
        this.CutinAnimator = GetComponent<Animator>();
        playerse = GameObject.Find("SEPlayer");
        sd = playerse.GetComponent<Soundtest>();
        t = GameObject.FindGameObjectWithTag("Player").GetComponent<target>();
        isSECutin = true;
    }

    void Update()
    {
        if (t.SpecialAtStart == true)
        {
            Cutin.enabled = true;
            this.CutinAnimator.SetBool(CutinAni, true);
            if (isSECutin == true)
            {
                SECutin();
                isSECutin = false;
            }
        }
        else
        {
            isSECutin = true;
            Cutin.enabled = false;
            this.CutinAnimator.SetBool(CutinAni, false);
        }
    }
    public void SECutin()//�J�b�g�C���̉����Đ�����
    {
        sd.SE_PlayerAttack2Player();
    }
}
