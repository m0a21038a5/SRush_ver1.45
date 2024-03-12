using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class multiplePlayer : MonoBehaviour
{

    multipleTarget mt;
    target t;
    [SerializeField]
    float RushSpeed;

    single s;

    public bool At;

    private PlayerSounds ps;
    private int next_i = 0;
    Rigidbody rb; 
    Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        mt = GameObject.FindGameObjectWithTag("Manager").GetComponent<multipleTarget>();
        s = GameObject.FindGameObjectWithTag("Manager").GetComponent<single>();
        At = false;
        t = gameObject.GetComponent<target>();
        rb = GetComponent<Rigidbody>();
        ps = GetComponent<PlayerSounds>();
    }

    // Update is called once per frame
    void Update()
    {
        if (At && mt.multipleTargetObject.Count > 1)
        {
            MoveTarget();
            t.isMoving = true;
        }
        
    }

    private void MoveTarget()
    {
        // 移動処理のループはUpdate内ではなく、コルーチンを使用
        StartCoroutine(MoveTargetsCoroutine());
    }

    //複数ターゲット
    private IEnumerator MoveTargetsCoroutine()
    {
        Vector3 lastTargetPosition = Vector3.zero; // 最後の目標の初期位置
       

        for (int i = 0; i < mt.multipleTargetObject.Count; i++)
        {
            GameObject targetObject = mt.multipleTargetObject[i];

            if (targetObject == null)
            {
                continue;
            }

           

            Vector3 targetPosition = mt.multipleTargetObject[i].GetComponent<IsRendered>().StatueRenderer.bounds.center;

            direction = lastTargetPosition - targetPosition;

           



            Tween moveTween = transform.DOMove(targetPosition, RushSpeed).SetEase(Ease.Linear);

            if (i == next_i)
            {
                ps.isPlayAttackHitSound = true;
                next_i++;
            }

            yield return new WaitForSeconds(0.5f);

            // 最後の目標座標を更新
            lastTargetPosition = targetPosition;
        }

       

        // すべてのターゲットに到達したらリセットします。
        mt.multipleTargetObject.Clear();
        mt.multiple = false;
        s.targetList.Clear();
        At = false;
        
        next_i = 0;
        t.isMoving = false;
        //ノックバック
        Vector3 incidentVector = direction.normalized;
        float remainingDistance = 10f;
        Vector3 newPosition = this.transform.position + incidentVector * remainingDistance;
        this.transform.position = Vector3.Lerp(this.transform.position, newPosition, Time.deltaTime * RushSpeed);
    }

}
