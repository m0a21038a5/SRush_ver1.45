using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IsRendered : MonoBehaviour
{
    //�G�ɂ��̃X�N���v�g�t���Ă�������

    //���o�Ώۂ�Renderer
    [SerializeField] public MeshRenderer targetRenderer;
    //�ːi�ڕW��Renderer
    [SerializeField] public MeshRenderer StatueRenderer;
    //�ߐړG�Î~��Ԃ�Renderer
    [SerializeField] MeshRenderer StatueRenderer_01;
    //�ߐړG�����Ă����Ԃ�Renderer
    [SerializeField] MeshRenderer StatueRenderer_02;
    //���݂�Camera
    [SerializeField] Camera cam;
    //EnemyLayer�ɐݒ肵�Ă�������
    [SerializeField] LayerMask layerMask;
    //�ߐړG�Î~��Ԃ̃I�u�W�F�N�g
    [SerializeField] GameObject enemy_01;
    //�ߐړG�����Ă����Ԃ̃I�u�W�F�N�g
    [SerializeField] GameObject enemy_02;

    //��������
    public bool visible;
   
    //���o���\�b�h�p�Ɏ擾
    SingleTarget st;

    //�ːi���W�ݒ�p�Ɏ擾
    target t;



    void Start()
    {
        st = GameObject.FindGameObjectWithTag("Manager").GetComponent<SingleTarget>();

        //Boss�̂ݑ傫�����邽�ߌ��o�pRenderer�𓷑̕��Ɍ���
        if (this.gameObject.CompareTag("BOSS"))
        {
            GameObject first = this.transform.GetChild(3).gameObject;
            targetRenderer = first.transform.GetChild(0).GetComponent<MeshRenderer>();
        }
        else
        {
            targetRenderer = this.gameObject.GetComponent<MeshRenderer>();
        }

        //�ߐړG�͒��S������Ă��邽�߁A�ːi�ڕW��Renderer�𓷑̕��Ɍ���
        if(this.gameObject.CompareTag("Statue"))
        {
            GameObject first = this.transform.GetChild(5).gameObject;
            enemy_01 = first.transform.GetChild(0).gameObject;
            enemy_02 = first.transform.GetChild(1).gameObject;
            StatueRenderer_01 = enemy_01.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
            StatueRenderer_02 = enemy_02.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        }
        cam = Camera.main;
        t = GameObject.FindGameObjectWithTag("Player").GetComponent<target>();
    }

    void Update()
    {
        cam = Camera.main;

        //�ߐړG�̐؂�ւ��ɉ����āARenderer��ύX
        if (this.gameObject.CompareTag("Statue"))
        {
            if (enemy_01.activeSelf == true)
            {
                StatueRenderer = StatueRenderer_01;
            }
            if (enemy_02.activeSelf == true)
            {
                StatueRenderer = StatueRenderer_02;
            }
        }
       
        //��������
        st.IsVisibleFromWall(targetRenderer, StatueRenderer, cam, layerMask, this.gameObject);
        
        //Boss�A�ߐړG�̏ꍇ�̂ݓːi�ڕW��ύX
        if(this.gameObject.CompareTag("Statue") && t.TargetStatue == this.gameObject)
        {
          t.StatuePos2 = StatueRenderer.bounds.center;
        }
        if (this.gameObject.CompareTag("BOSS") && t.TargetBoss == this.gameObject)
        {
            t.BossPos = StatueRenderer.bounds.center;
        }

    }
}
