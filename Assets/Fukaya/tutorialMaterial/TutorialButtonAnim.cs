using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButtonAnim : MonoBehaviour
{
    public float pulseSpeed = 2.0f; // �p���X�̑��x
    public float minScale = 0.9f;   // �ŏ��X�P�[��
    public float maxScale = 1.5f;   // �ő�X�P�[��

    private Vector3 initialScale;
    private bool scalingUp = true;


    // Start is called before the first frame update
    void Start()
    {
        initialScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {

        // ���݂̃X�P�[��
        Vector3 currentScale = transform.localScale;

        // �p���X�̓���
        float scaleFactor = Mathf.PingPong(Time.time * pulseSpeed, 1);
        float newScale = Mathf.Lerp(minScale, maxScale, scaleFactor);

        // �X�P�[�����X�V
        transform.localScale = initialScale * newScale;
    }
}
