using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamPos : MonoBehaviour
{
    private GameObject area;
    public GameObject EnemyObject;
    public GameObject enemyPos;
    public bool isEmemy = false;

    target t;

    // Start is called before the first frame update
    void Start()
    {
        area = GameObject.Find("Player");
        EnemyObject = GameObject.Find("Player");
        enemyPos = GameObject.Find("Player");
        t = GameObject.FindGameObjectWithTag("Player").GetComponent<target>();
    }

    // Update is called once per frame
    void Update()
    {
        //�G�̈ʒu��ړI�̈ʒu�Ɏw��
        if (isEmemy == true)
        {
            //EnemyObject.GetComponent<target>().TargetBeam = this.gameObject;
            //enemyPos.GetComponent<target>().BeamPos = this.transform.position;
        }
    }

    //�G���G���A�ɓ����Ă��邩
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "PlayerArea")
        {
            isEmemy = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "PlayerArea")
        {
            isEmemy = false;
        }
    }

    private void OnDestroy()
    {
        t.isTarget_Beam = false;
        t.TargetBeam = null;
    }
}
