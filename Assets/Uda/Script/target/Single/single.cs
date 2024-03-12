using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class single : MonoBehaviour
{
    
    //�ːi�\�ȓG�i�[�pList
    [SerializeField]
    public List<GameObject> targetList;

    
    private target ta;



    [SerializeField]
    public GameObject Playerobj;

    //�\�[�g����^�C�~���O��}��ۂɎg�p
    public bool Sort = false;
    bool Change;



    void Start()
    {
        Playerobj = GameObject.FindGameObjectWithTag("Player");
        ta = GameObject.FindGameObjectWithTag("Player").GetComponent<target>();

    }
    void Update()
    {
        if (Sort == true)
        {
          
            //�G�̎�ނɉ����āAtarget�ɓːi�\�ȓG���󂯓n��
            if (targetList[0].CompareTag("Statue"))
            {
                ta.TargetStatue = targetList[0];
                ta.TargetBeam = null;
                ta.TargetBoss = null;
                ta.isTarget_Statue = true;
                ta.isTarget_Beam = false;
                ta.isTarget_Boss = false;
            }
            if (targetList[0].CompareTag("Beam"))
            {
                ta.TargetBeam = targetList[0];
                ta.TargetStatue = null;
                ta.TargetBoss = null;
                ta.isTarget_Beam = true;
                ta.isTarget_Statue = false;
                ta.isTarget_Boss = false;
            }
            if (targetList[0].CompareTag("BOSS"))
            {
                ta.TargetBoss = targetList[0];
                ta.TargetStatue = null;
                ta.TargetBeam = null;
                ta.isTarget_Boss = true;
                ta.isTarget_Statue = false;
                ta.isTarget_Beam = false;
            }
        }

        
       
        
     
        //List���̗v�f��0�̏ꍇ�A�\�[�g�\��
        if (!(targetList.Count > 0))
        {
            Sort = true;
        }

        //List�̐擪�̃I�u�W�F�N�g���������ꍇ�A�\�[�g
        if (targetList[0] == null || targetList[0].activeSelf == false|| !ta.isMoving)
        {
            SortVectorList();
        }

       
    }

    //targetList�̗v�f���폜
    public void ListClear()
    {
        targetList.Clear(); //List���ׂĂ̗v�f���폜
    }

    //targetList��Player�ɋ߂����Ƀ\�[�g
    public void SortVectorList()
    {

        Vector3 characterPosition = Playerobj.transform.position;

        targetList.RemoveAll(item => item == null || !item.activeSelf);

        targetList.Sort((a, b) => Vector3.Distance(a.transform.position, characterPosition).CompareTo(Vector3.Distance(b.transform.position, characterPosition)));
       
    }

   
}
