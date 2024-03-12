using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    private GameObject _player;
    public string _earthTag;
    public float _moveSpeeds;

    Vector3 V_e = new Vector3(0, 0, 0);

    private Rigidbody RB;

    bool Crash = false;


    void Start ()
    {
        RB = GetComponent<Rigidbody>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (V_e.sqrMagnitude * (_moveSpeeds - 1) < (this.transform.position-_player.transform.position).sqrMagnitude)
        {
            Crash = false;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (Crash == false)
        {
            if (target.instance.isAtacked == true)
            {
                if (collision.gameObject == _player)
                {
                    // GetAngleVec�𒷂����P�̃x�N�g��V_e�ɕϊ�
                    V_e = GetAngleVec(_player, this.gameObject).normalized;


                    RB.velocity = Vector3.zero;
                    RB.AddForce(V_e * _moveSpeeds, ForceMode.VelocityChange);
                    Debug.Log(V_e * _moveSpeeds);
                    Crash = true;

                    target.instance.isAtacked = false;
                }
            }
        }
    }

    Vector3 GetAngleVec(GameObject _from, GameObject _to)
    {
        //�����̊T�O���Ȃ��x�N�g�������
        Vector3 fromVec = new Vector3(_from.transform.position.x, 0, _from.transform.position.z);
        Vector3 toVec = new Vector3(_to.transform.position.x, 0, _to.transform.position.z);

        return Vector3.Normalize(toVec - fromVec);
    }

    // Start is called before the first frame update
}
