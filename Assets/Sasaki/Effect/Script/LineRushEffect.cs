using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRushEffect : MonoBehaviour
{
    public GameObject LineRushEffectObject;
    public float LineRushEffectTime=1.0f;

    //target����G�̃I�u�W�F�N�g���擾
    target t;
    //combo����ComboCount�̎擾
    Combo combo;
    //Combo��G�ɓ����������̈��̂ݒǉ������悤�ɂ��邽�߂̕ϐ�
    public int CountEnemy;
    void Start()
    {
        t = GameObject.FindGameObjectWithTag("Player").GetComponent<target>();
        combo = GameObject.FindGameObjectWithTag("Player").GetComponent<Combo>();
        LineRushEffectObject.SetActive(false);
    }

    void Update()
    {
        CountEnemy = combo.CountEnemyCombo;
      //  if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("joystick button 7") || Input.GetKeyDown("joystick button 0")) { 
            if (t.ismove_Statue == true|| t.ismove_Beam == true || t.isTarget_Boss == true)
            {
            //LineRushEffectObject.SetActive(true);
             StopCoroutine(SpaceDistortEffectCoroutine());
            StartCoroutine(SpaceDistortEffectCoroutine());
        }
        //}
    }

    IEnumerator SpaceDistortEffectCoroutine()
    {
        //Debug.Log("�W����");
        LineRushEffectObject.SetActive(true);
        yield return new WaitForSecondsRealtime(LineRushEffectTime);
        LineRushEffectObject.SetActive(false);

    }
}
