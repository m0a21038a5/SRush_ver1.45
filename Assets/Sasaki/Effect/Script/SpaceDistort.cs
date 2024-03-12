
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class SpaceDistort : MonoBehaviour
{ //�䂪�݂�����G�t�F�N�g�𐧌䂷��X�N���v�g�ł�
    //SpaceDistortProcessVolume Intensity�̍ő�A�ŏ��l�@
    //SpaceDistortTime�@�ω��ɂ����鎞�ԁ@�@SpaceDistortChangeValue�@�䂪�݂̕ω���
    public PostProcessVolume SpaceDistortProcessVolume;
    private LensDistortion SpaceDistortPostProcessLensDistortion;
    public float SpaceDistortValue = 90.0f;
    public float SpaceDistortTime = 0.2f;
    public float SpaceDistortChangeValue = 20.0f;
    public bool SpaceDistortStart;
    public bool SpaceDistortSecond;
    public bool SpaceDistortFinal;
    public GameObject player1;
    public target Target1; 
    void Start()
    {
        SpaceDistortStart = false;
        SpaceDistortSecond = false;
        SpaceDistortFinal = false;
        player1 = GameObject.Find("Player");
        Target1 = player1.GetComponent<target>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (PostProcessEffectSettings item in SpaceDistortProcessVolume.profile.settings)
        {
            if (item as LensDistortion)
            {
                SpaceDistortPostProcessLensDistortion = item as LensDistortion;
            };
        }
        //���N���b�N�������Ԃ��䂪�܂���
        //�^�[�Q�b�g�������Ă鎞�����E�N���b�N����?
        //(Target�̃X�N���v�g��������擾����?)
        //target��ismove_Statue = false;,public bool ismove_Beam = false;���擾
        //if(player1.GetComponent<target>().ismove_Statue || player1.GetComponent<target>().ismove_Beam)
        //if(Target1.ismove_Statue || Target1.ismove_Beam)
        //{
            if (Input.GetMouseButtonDown(0))
            {
                if (SpaceDistortPostProcessLensDistortion)
                {
                    //SpaceDistortStart = true;
                    StartCoroutine(SpaceDistortEffectCoroutine());
                    //SpaceDistortEffect();
                }
            }
        
        //}
        else
        {
            if (SpaceDistortPostProcessLensDistortion)
            {
                // TimeStopPostProcessColorGrading.Intensity.value = 50.0f;
            }
        }


        if (SpaceDistortStart == true)
        {
            SpaceDistortPostProcessLensDistortion.intensity.value -= SpaceDistortChangeValue;
            if (SpaceDistortPostProcessLensDistortion.intensity.value < -SpaceDistortValue)
            {
                SpaceDistortStart = false;
            }
        }
        else if (SpaceDistortStart == false)
        {

        }
        if (SpaceDistortSecond == true)
        {
            SpaceDistortPostProcessLensDistortion.intensity.value += SpaceDistortChangeValue;
            if (SpaceDistortPostProcessLensDistortion.intensity.value > SpaceDistortValue)
            {
                SpaceDistortSecond = false;
            }
        }
        else if (SpaceDistortSecond == false)
        {

        }

        if (SpaceDistortFinal == true)
        {
            SpaceDistortPostProcessLensDistortion.intensity.value -= SpaceDistortChangeValue;
            if (SpaceDistortPostProcessLensDistortion.intensity.value < 0)
            {
                SpaceDistortPostProcessLensDistortion.intensity.value = 0.0f;
            }
        }
        else if (SpaceDistortFinal == false)
        {
            SpaceDistortFinal = false;
        }
    }

    IEnumerator SpaceDistortEffectCoroutine()
    {
        //SpaceDistortPostProcessLensDistortion.intensity.value = -SpaceDistortValue;
        SpaceDistortStart = true;
        SpaceDistortSecond = false;
        SpaceDistortFinal = false;
        yield return new WaitForSecondsRealtime(SpaceDistortTime);
        // SpaceDistortPostProcessLensDistortion.intensity.value = SpaceDistortValue;
        SpaceDistortStart = false;
        SpaceDistortSecond = true;
        SpaceDistortFinal = false;
        yield return new WaitForSecondsRealtime(SpaceDistortTime);
        SpaceDistortStart = false;
        SpaceDistortSecond = false;
        SpaceDistortFinal = true;
        //SpaceDistortPostProcessLensDistortion.intensity.value = 0.0f;
    }

    void SpaceDistortEffect()
    {
        if (SpaceDistortPostProcessLensDistortion)
        {
            SpaceDistortPostProcessLensDistortion.intensity.value--;
            if (SpaceDistortPostProcessLensDistortion.intensity.value == -90.0f)
            {
                SpaceDistortPostProcessLensDistortion.intensity.value++;
                if (SpaceDistortPostProcessLensDistortion.intensity.value == 0.0f)
                {
                    SpaceDistortPostProcessLensDistortion.intensity.value--;
                }
            }
        }
    }
}
