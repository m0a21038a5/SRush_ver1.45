using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialSkip : MonoBehaviour
{
    // �`���[�g���A���������邢��N�L�[�������ŃX�L�b�v����@�����ĉ���?
    public  int framenumber = 180;
    private int frameKey = 0;
    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKey("joystick button 3") || Input.GetKey(KeyCode.N))
        {
            frameKey = frameKey + 1;
        }
        if (Input.GetKeyUp("joystick button 3") && Input.GetKeyUp(KeyCode.N))
        {
            frameKey = 0;
        }
        if(frameKey == framenumber)
        {
            SceneManager.LoadScene("Map 1");
        }
    }
}
