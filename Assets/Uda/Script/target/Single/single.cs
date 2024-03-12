using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class single : MonoBehaviour
{
    
    //突進可能な敵格納用List
    [SerializeField]
    public List<GameObject> targetList;

    
    private target ta;



    [SerializeField]
    public GameObject Playerobj;

    //ソートするタイミングを図る際に使用
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
          
            //敵の種類に応じて、targetに突進可能な敵を受け渡す
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

        
       
        
     
        //List内の要素が0の場合、ソート可能に
        if (!(targetList.Count > 0))
        {
            Sort = true;
        }

        //Listの先頭のオブジェクトが消えた場合、ソート
        if (targetList[0] == null || targetList[0].activeSelf == false|| !ta.isMoving)
        {
            SortVectorList();
        }

       
    }

    //targetListの要素を削除
    public void ListClear()
    {
        targetList.Clear(); //Listすべての要素を削除
    }

    //targetListをPlayerに近い順にソート
    public void SortVectorList()
    {

        Vector3 characterPosition = Playerobj.transform.position;

        targetList.RemoveAll(item => item == null || !item.activeSelf);

        targetList.Sort((a, b) => Vector3.Distance(a.transform.position, characterPosition).CompareTo(Vector3.Distance(b.transform.position, characterPosition)));
       
    }

   
}
