using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CriWare;
using CriWare.Assets;

public class Soundtest : MonoBehaviour
{
    public GameObject BGMPlayer;
    public CriAtomAcfAsset acf;
    public CriAtomAcbAsset acb;
    public CriAtomExPlayer player;
    private CriAtomExPlayback playback;//�v���C�o�b�N
    // Start is called before the first frame update
    [SerializeField]
    public PlayerSounds p;
    GameManager gm;
    public GameObject menuObj;
    public GameObject menuObj2;
    public GameObject menuObj3;
    public GameObject menuObj4;
    public Slider volumeSlider;
    public float playerVol;
    public GameObject BGMObj;
    BGMPlayer Code_bgmplayer;


    void Start()
    { 
        DontDestroyOnLoad(this.gameObject);
        acf.Register();
        CriAtomEx.AttachDspBusSetting("DspBusSetting_0");
        acb.LoadImmediate();
        player = new CriAtomExPlayer();
        volumeSlider.value = volumeSlider.maxValue = 1.0f;
        volumeSlider.minValue = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (p.isPlayWalkLSound == true)//������
        {
            player.SetCue(acb.Handle, "SE_FootSteps_Left");
            player.Start();
            p.isPlayWalkLSound = false;
        }
        if (p.isPlayWalkRSound == true)//�E����
        {
            player.SetCue(acb.Handle, "SE_FootSteps_Right");
            player.Start();
            p.isPlayWalkRSound = false;
        }
        if (p.isPlayRushSound == true)//�ːi���؂艹
        {
            player.SetCue(acb.Handle, "SE_PlayerRush");
            player.Start();
            p.isPlayRushSound = false;
        }
        if (p.isPlayAttackSound == true)//�U����
        {
            player.SetCue(acb.Handle, "");
            player.Start();
            p.isPlayAttackSound = false;
        }
        if (p.isPlayAttackHitSound == true)//�U������������
        {
            player.SetCue(acb.Handle, "SE_PlayerAttack");
            player.Start();
            p.isPlayAttackHitSound = false;
        }
        if (p.isPlayDamageSound)//�_���[�W
        {
            player.SetCue(acb.Handle, "SE_Damaged");
            player.Start();
            p.isPlayDamageSound = false;
        }
        if (p.isPlayJumpSound == true)//�W�����v
        {
            player.SetCue(acb.Handle, "SE_Jump");
            player.Start();
            p.isPlayJumpSound = false;
        }
        if (p.isPlayFallSound == true)//���ɗ�����
        {
            player.SetCue(acb.Handle, "SE_FallToHall");
            player.Start();
            p.isPlayFallSound = false;
        }
        if (p.isPlayFallWaterSound == true)//���ɗ�����
        {
            player.SetCue(acb.Handle, "SE_FallToWater");
            player.Start();
            p.isPlayFallWaterSound = false;
        }

        /*
        if(menuObj.activeSelf == false)//���j���[��ʊJ���Ă��Ȃ��� [ GameManager�Ɉړ����܂��� by���� ]
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                positive1Player();              
            }
            BGMObj.GetComponent<BGMPlayer>().aisac2 = 0.0f;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                negative1Player();               
            }
            BGMObj.GetComponent<BGMPlayer>().aisac2 = 1.0f;
        }*/

        playerVol = volumeSlider.value;
        SetVolume(playerVol);
        
    }
    public void positive1Player()
    {
        player.SetCue(acb.Handle, "positive1");
        player.Start();
    }
    public void positive2Player()
    {
        player.SetCue(acb.Handle, "positive2");
        player.Start();
    }
    public void negative1Player()
    {
        player.SetCue(acb.Handle, "negative1");
        player.Start();
    }
    public void negative2Player()
    {
        player.SetCue(acb.Handle, "negative2");
        player.Start();
    }
    public void SE_ButtonPlayer()
    {
        player.SetCue(acb.Handle, "SE_Button");
        player.Start();
    }
    public void SE_MenuOpenPlayer()
    {
        player.SetCue(acb.Handle, "SE_MenuOpen");
        player.Start();
    }
    public void SE_MenuClosePlayer()
    {
        player.SetCue(acb.Handle, "SE_MenuClose");
        player.Start();
    }
    public void SE_PsoundPlayer()
    {
        player.SetCue(acb.Handle, "SE_Psound");
        player.Start();
    }

    public void SE_CantAttackPlayer() //�U�����e����鉹
    {
        player.SetCue(acb.Handle, "SE_CantAttack");
        player.Start();
    }

    public void SE_SPButtonPlayer()//�K�E�Z�{�^���̉�
    {
        player.SetCue(acb.Handle, "SE_SPButton");
        player.Start();
    }

    public void SE_SuperAttackPlayer()//�K�E�Z�������鉹
    {
        player.SetCue(acb.Handle, "SE_SuperAttack");
        player.Start();
    }

    public void SE_getCoinPlayer()//�R�C���擾�̉�
    {
        player.SetCue(acb.Handle, "SE_GetCoin");
        player.Start();
    }

    public void SE_TargetLockedPlayer()//�^�[�Q�b�g�̉�
    {
        player.SetCue(acb.Handle, "SE_TargetLocked");
        player.Start();
    }
    public void SE_EnemyDead1Player()//�d���G�����񂾂Ƃ��̉�
    {
        player.SetCue(acb.Handle, "SE_EnemyDead1");
        player.Start();
    }

    public void SE_InSPPlayer()
    {
        player.SetCue(acb.Handle, "SE_InSP");
        player.Start();
    }

    public void SE_PlayerAttackPlayer()//���c�p�e�X�g�U����
    {
        player.SetCue(acb.Handle, "SE_PlayerAttack");
        player.Start();
    }

    public void SE_PlayerAttack2Player()//�O�̍U����
    {
        player.SetCue(acb.Handle, "SE_PlayerAttack2");
        player.Start();
    }

    public void result1Player()//���U���g��ʂ̌��ʉ��@
    {
        player.SetCue(acb.Handle, "result1");
        player.Start();
    }
    public void result2Player()//���U���g��ʂ̌��ʉ��A
    {
        player.SetCue(acb.Handle, "result2");
        player.Start();
    }
    public void kazePlayer()
    {
        player.SetCue(acb.Handle, "SE_PlayerRush");
        player.Start();
    }
    public void SE_SPChargePlayer()
    {
        player.SetCue(acb.Handle, "SE_SPCharge");
        player.Start();
    }
    public void SE_321Player()
    {
        player.SetCue(acb.Handle, "SE_321");
        player.Start();
    }

    public void SE_HeartbeatPlayer()
    {
        player.SetCue(acb.Handle, "SE_Heartbeat");
        playback = player.Start();
    }

    public void SE_HeartbeatStopper()
    {
        playback.Stop();
    }
    public void SetVolume(float vol)
    {
        /* (19) �{�����[���̐ݒ� */
        player.SetVolume(vol);

        /* (20) �p�����[�^�[�̍X�V */
        player.UpdateAll();
    }
}
