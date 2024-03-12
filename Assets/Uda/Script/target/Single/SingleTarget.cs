using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTarget : MonoBehaviour
{
    
    MeshRenderer Player;
    single s;
    targetChange tc;
    IsRendered R;
    Combo c;
    multipleTarget mt;
    target t;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<MeshRenderer>();
        s = this.gameObject.GetComponent<single>();
        tc = this.gameObject.GetComponent<targetChange>();
        c = Player.GetComponent<Combo>();
        t = Player.GetComponent<target>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //カメラの描画範囲内かつ遮蔽物に隠れていない場合targetListに追加
    public void IsVisibleFromWall(MeshRenderer renderer,MeshRenderer StatueRenderer ,  Camera camera, LayerMask layerMask, GameObject targetObject)
    {
        //カメラの描画範囲取得
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);

        //検出に使用するRenderer
        Bounds bounds = renderer.bounds;

        //対象オブジェクトの座標
        Vector3 targetPosition;
       
        targetPosition = StatueRenderer.bounds.center;
      
        //対象とPlayerの距離計算
        float dis = Vector3.Distance(Player.bounds.center, targetPosition) + Vector3.Distance(camera.transform.position, Player.bounds.center);

        //カメラの描画範囲にRendererがあり、Playerとの距離が120f以内の場合のみ遮蔽物判定を行う
        if(GeometryUtility.TestPlanesAABB(planes, bounds) == true && dis < 120f)
        {
            RaycastHit hit;

            //Rayの開始位置
            Vector3 origin;

            //カメラが特殊演出中出ない場合は、Rayの開始位置を変更
            if (!c.SpecialMode || !mt.ChangeCamera)
            {
                origin = camera.transform.position + new Vector3(0.0f, 5.0f, 0.0f);
            }
            else
            {
                origin = camera.transform.position;
            }
            //Rayの方向
            Vector3 rayDirection = targetPosition - origin;
            //Rayの最大距離
            float rayDistance = 70f;
            Physics.Raycast(origin , rayDirection, out hit, rayDistance, layerMask);
            //Rayのデバッグ
            Debug.DrawRay(origin, rayDirection * rayDistance, Color.red, 0.1f, false);
            //可視化可能であることを敵のIsRenderdへ
            R = targetObject.GetComponent<IsRendered>();
            R.visible = true;

            //Rayが対象オブジェクトにヒットした時のみtargetListへ追加
            if(hit.collider.gameObject == targetObject)
            {
                if (!s.targetList.Contains(targetObject))
                {
                    //敵の種類に応じて処理を変更
                    if(targetObject.GetComponent<BeamHPManager>() != null)
                    {
                        if(!targetObject.GetComponent<BeamHPManager>().Boss && !targetObject.GetComponent<BeamHPManager>().isDefeat)
                        {
                            s.targetList.Add(targetObject);
                            s.Sort = true;
                            tc.Change = true;
                        }
                    }
                    else if (targetObject.GetComponent<StatueHPManager>() != null)
                    {
                        if (!targetObject.GetComponent<StatueHPManager>().Boss)
                        {
                            s.targetList.Add(targetObject);
                            s.Sort = true;
                            tc.Change = true;
                        }
                    }
                    else if (targetObject.CompareTag("BOSS"))
                    {
                        s.targetList.Add(targetObject);
                        s.Sort = true;
                        tc.Change = true;
                    }
                }               
            }
            else
            {
                //Playerが突進状態出なく、対象が遮蔽物に隠れた場合は、targetListから削除
                if (!t.isMoving)
                {
                    s.targetList.Remove(targetObject);
                    R.visible = false;
                }
            }
        }
        else
        {
            //Playerが突進状態出なく、対象がカメラの描画範囲内からいなくなった場合は、targetListから削除
            if (!t.isMoving)
            {
                s.targetList.Remove(targetObject);
                R.visible = false;
            }
        }
    }

  
}
