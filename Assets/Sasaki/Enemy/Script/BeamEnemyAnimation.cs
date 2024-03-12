using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamEnemyAnimation : MonoBehaviour
{ 
    //�ǉ��@�j��A�j���[�V�����Đ��p
    public float brokenAniTime = 2.0f;
    public bool brokenDrop;
    private Animator BeamAni;
    private string BrokenStr = "isBroken";
    [SerializeField]
    private BeamHPManager bhpm;
    public float DropParts;
    void Start()
    {
        bhpm = transform.root.gameObject.GetComponent<BeamHPManager>();
        this.BeamAni = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
       if(bhpm.BrokenAnibool == true)
        {
            StartCoroutine(BrokenCoroutine());
        }
        if (brokenDrop == true)
        {
            transform.position += new Vector3(0.0f, -DropParts * Time.deltaTime, 0.0f);         
        }
    }

    IEnumerator BrokenCoroutine()
    { //���X������Ondestroy�̏�����j��A�j���[�V������(����2�b)�x�点�Ă���J�n
        this.BeamAni.SetBool(BrokenStr, true);
        yield return new WaitForSecondsRealtime(brokenAniTime/2);
        brokenDrop = true;
        yield return new WaitForSecondsRealtime(brokenAniTime/2);
        //bhpm.BrokenDestroy = true;
        
        Destroy(transform.root.gameObject);
    }
}
