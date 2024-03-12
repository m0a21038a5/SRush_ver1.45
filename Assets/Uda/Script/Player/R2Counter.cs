using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R2Counter : MonoBehaviour
{
    public float PushCount;
    [SerializeField]
    float DashCount;
    target t;
    multipleTarget mt;
    multiplePlayer mp;
    bool isMultiple;
    Combo c;
    GameObject Player;
    
    // Start is called before the first frame update
    void Start()
    {
        PushCount = 0;
        Player = GameObject.FindGameObjectWithTag("Player");
        t = GameObject.FindGameObjectWithTag("Player").GetComponent<target>();
        c = GameObject.FindGameObjectWithTag("Player").GetComponent<Combo>();
        mt = GameObject.FindGameObjectWithTag("Manager").GetComponent<multipleTarget>();
        mp = GameObject.FindGameObjectWithTag("Player").GetComponent<multiplePlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("joystick button 0") && !c.SpecialMode && !t.isMoving && !t.SpecialAtStart)
        {
            if (t.isTarget_Statue)
            {
                t.ismove_Statue = true;
                t.FirstPosition = Player.transform.position;
                t.isMoving = true;
                t.SingleMovePosition = t.StatuePos2;
                t.Target = t.TargetStatue;
            }
            if (t.isTarget_Beam)
            {
                t.ismove_Beam = true;
                t.FirstPosition = Player.transform.position;
                t.isMoving = true;
                t.SingleMovePosition = t.TargetBeam.transform.position;
                t.Target = t.TargetBeam;
            }
            if (t.isTarget_Boss)
            {
                t.ismove_Boss = true;
                t.FirstPosition = Player.transform.position;
                t.isMoving = true;
                t.SingleMovePosition = t.BossPos;
                t.Target = t.TargetBoss;
            }
        }
        if((Input.GetKey("joystick button 7")  || Input.GetMouseButton(1)) && !t.isMoving && !t.SpecialAtStart)
        {
            mt.target = true;
        }
        if(Input.GetKeyUp("joystick button 7") || Input.GetMouseButtonUp(1))
        {
            if (mt.multipleTargetObject.Count > 1)
            {
                mp.At = true;
            }
            if (mt.multipleTargetObject.Count < 2)
            {
                mp.At = false;
                mt.multiple = false;
            }
            mt.target = false;
            mt.ChangeCamera = false;
        }
    }
}
