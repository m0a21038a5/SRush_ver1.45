using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_ver2 : MonoBehaviour
{
    GameObject Player;
    target ta;
    private void Start()
    {
        //�v���C���[�I�u�W�F�N�g�擾
        Player = GameObject.FindGameObjectWithTag("Player");
        ta = Player.GetComponent<target>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
        if(collision.gameObject.name == "Player")
        {
            ta.ismove_Statue = false;
        }
        
    }
}
