using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfetImagePlayer : MonoBehaviour
{
    // �v���C���[�ɂ̂ݎc����\�������܂�
    public bool OnlyPlayerAfterImageEffect;//����Ƀ`�F�b�N������ƃv���C���[�ɂ̂ݎc���G�t�F�N�g���t���܂�
    
    public GameObject AfterImageEffectObject;
    public float PlayerAfterImageEffectTime;
    public GameObject player3;
    public target Target3;
    void Start()
    {
        AfterImageEffectObject = GameObject.Find("AfetImagePlayer");
        player3 = GameObject.Find("Player");
        Target3 = player3.GetComponent<target>();
        AfterImageEffectObject.SetActive(false);
    }
    void Update()
    {
        if(OnlyPlayerAfterImageEffect == true)
        {
            if (Target3.ismove_Statue || Target3.ismove_Beam)
            {
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(AfterImageEffectCoroutine());
            } 
            }
        }
        else
        {
            AfterImageEffectObject.SetActive(false);
        }
    }

    IEnumerator AfterImageEffectCoroutine()
    {
        AfterImageEffectObject.SetActive(true);
        yield return new WaitForSecondsRealtime(PlayerAfterImageEffectTime);
        AfterImageEffectObject.SetActive(false);

    }
}
