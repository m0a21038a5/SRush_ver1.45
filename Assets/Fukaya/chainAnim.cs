using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class chainAnim : MonoBehaviour
{
    //�`�F�C����
    public int chainCount;
    //�R���{�̃e�L�X�g
    public Text ComboText;
    //�g�嗦
    public float scaleFactor = 1.3f;
    // X�����ɓ���������
    public float xOffset = -10f;
    // Y�����ɓ���������
    public float yOffset = 10f;
    //�`�F�C��UI�̌��̈ʒu
    private Vector3 originalPosition;
    //�v���C���[�擾
    private GameObject PlayerCombo;
    //�R���{�X�N���v�g�擾
    Combo comboScript;
    private bool isScaling = false;
    private Vector3 originalScale;
    //�g��ɂ����鎞��
    public float scalingDuration = 0.1f; 
    private float scaleStartTime;

    // Start is called before the first frame update
    void Start()
    {
        PlayerCombo = GameObject.Find("Player");
        comboScript = PlayerCombo.GetComponent<Combo>();
        
        originalScale = ComboText.transform.localScale;
        originalPosition = ComboText.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        chainCount = comboScript.ComboCount;

        if (chainCount % 10 == 0�@&&  !isScaling)
        {
            isScaling = true;
            scaleStartTime = Time.time;

            // StartCoroutine(ScaleOverTime(originalScale * scaleFactor, originalPosition + new Vector3(xOffset, yOffset, 0f),scalingDuration, 0.3f, 1.5f));
            StartCoroutine(ScaleOverTime(originalScale * scaleFactor, originalPosition + new Vector3(-10, 10, 0f), scalingDuration, 0.3f, 1.5f));


        }
        else if(chainCount % 10 != 0)
        {
            ComboText.transform.localScale = originalScale;
            ComboText.transform.localPosition = originalPosition;
        }
    }




    private IEnumerator ScaleOverTime(Vector3 targetScale, Vector3 targetPosition, float duration, float delay, float wai)
    {
        float startTime = Time.time;
        Vector3 startScale = ComboText.transform.localScale;
        Vector3 startPosition = ComboText.transform.localPosition;

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            ComboText.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            ComboText.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        ComboText.transform.localScale = targetScale;
        ComboText.transform.localPosition = targetPosition;


        yield return new WaitForSeconds(delay);
        ComboText.transform.localScale = originalScale;
        ComboText.transform.localPosition = originalPosition;

        yield return new WaitForSeconds(wai);

        isScaling = false;
    }


}
    