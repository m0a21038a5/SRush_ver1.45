using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


[System.Serializable]
public class Data
{
    public Vector3 position;

    // その他のデータを追加
}

public class target : MonoBehaviour
{
    //突進対象の近接敵
    public GameObject TargetStatue;
    //突進対象のビーム敵
    public GameObject TargetBeam;
    //突進対象のBoss
    public GameObject TargetBoss;
    //突進の速度
    public float RushSpeed = 0f;
   
    //突進対象の近接敵の座標
    public Vector3 StatuePos2;
    //突進対象のビーム敵の座標
    public Vector3 BeamPos;
    //突進対象のBossの座標
    public Vector3 BossPos;
    public Rigidbody rb;
    //敵の種類に応じて突進中かターゲット中か判定
    public bool isTarget_Statue = false;
    public bool isTarget_Beam = false;
    public bool ismove_Statue = false;
    public bool ismove_Beam = false;
    public bool isTarget_Boss = false;
    public bool ismove_Boss = false;
    public bool clear = false;

   

    //当たった時の重力変更と当たった時のはね
    public float jump = 3f;
    public float JumpGravityY = -10;

    public Vector3 surfacePoint;

    private single si;

    public List<Data> dataList = new List<Data>();

    public Vector2 targetPosition;

    //攻撃時のフラグ
    public bool Attack;

    #region 突進による吹っ飛ばし
    public static target instance;
    public bool isAtacked = false;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

    // PlayerSoundsスクリプト--------村岡追加--------
    private PlayerSounds ps;
    private Soundtest st;
    private bool isPlaySPChargeSound;

    [SerializeField]
    private float center;

    public bool jumpwait = false;

    public Vector3 nowPos;
    public float backKnockBackForce;
    public float upKnockBackForce;
    public float knockBackPower;

    multipleTarget mt;
    multiplePlayer mp;

    Combo c;

    public bool SpecialAttack;
    public bool SpecialAtStart;
    public Vector3 SpecialPosition;
    public Vector3 FinalSpecialPosition;

    public bool isMoving = false;

    [SerializeField] float slowMotionScale = 0.2f;
    public Vector3 FirstPosition;
    Vector3 SurfaceNormal;

    [SerializeField] GameObject SpecialEffect;
    float originalTimeScale = 1.0f;


    int enemyLayerMask;
    int BossLayerMask;

    public Vector3 SingleMovePosition;
    public GameObject Target;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //TargetImage.GetComponent<Image>().enabled = false;

        mt = GameObject.FindGameObjectWithTag("Manager").GetComponent<multipleTarget>();
        mp = GameObject.FindGameObjectWithTag("Player").GetComponent<multiplePlayer>();

        c = this.gameObject.GetComponent<Combo>();

        // PlayerSoundsスクリプト取得--------M追加--------
        ps = GetComponent<PlayerSounds>();
        st = GameObject.Find("SEPlayer").GetComponent<Soundtest>();// ここは一番下の行にしてください
        SpecialEffect.SetActive(false);
        originalTimeScale = 1.0f;
        enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
        BossLayerMask = 1 << LayerMask.NameToLayer("Boss");
    }

    // Update is called once per frame
    void Update()
    {
        //突進可能なオブジェクトがnullでない場合は、敵に応じてflagを変更
        if (TargetBeam != null)
        {
            isTarget_Beam = true;
            isTarget_Statue = false;
            isTarget_Boss = false;
        }
        else if (TargetStatue != null)
        {
            isTarget_Beam = false;
            isTarget_Statue = true;
            isTarget_Boss = false;
        }
        else if (TargetBoss != null)
        {
            isTarget_Beam = false;
            isTarget_Statue = false;
            isTarget_Boss = true;
        }

        //敵に応じて突進の処理を変更
        if ((TargetStatue == null || !TargetStatue.activeSelf) && ismove_Statue)
        {
            ismove_Statue = false;
            isMoving = false;
            isAtacked = false;
            Attack = false;
        }
        else if ((TargetBeam == null || !TargetBeam.activeSelf)&& ismove_Beam)
        {
            ismove_Beam = false;
            isMoving = false;
            isAtacked = false;
            Attack = false;
        }

        //近接敵への突進処理
        if (isTarget_Statue == true && !mt.multiple)
        {
            if (Input.GetMouseButtonDown(0) && !isMoving)
            {
                ismove_Statue = true;
                isMoving = true;
                SingleMovePosition = StatuePos2;
                Target = TargetStatue;
                // プレイヤーの突進音をON--------村岡追加--------
                ps.isPlayRushSound = true;
            }        
        }

        //ビーム敵への突進処理
        if (isTarget_Beam == true)
        {
             BeamPos = TargetBeam.transform.position;
             if (Input.GetMouseButtonDown(0) && !isMoving)
             {
                  ismove_Beam = true;
                  isMoving = true;
                  SingleMovePosition = TargetBeam.transform.position;
                  Target = TargetBeam;
                   // プレイヤーの突進音をON--------M追加--------
                   ps.isPlayRushSound = true;
             }
        }
        //Bossへの突進処理
        if (isTarget_Boss == true)
        {
             if (Input.GetMouseButtonDown(0))
             {
                 ismove_Boss = true;
                 isMoving = true;
                 SingleMovePosition = BossPos;
                 Target = TargetBoss;
                 // プレイヤーの突進音をON--------M追加--------
                 ps.isPlayRushSound = true;
             }
        }
           
        //必殺技演出
            if (c.Special == true && isPlaySPChargeSound == false)
            {
                st.SE_SPChargePlayer();
                isPlaySPChargeSound = true;
            }
            if (c.Special == false && isPlaySPChargeSound == true)
            {
                isPlaySPChargeSound = false;
            }

            //近接敵への突進、突進完了後の処理
            if(ismove_Statue)
           {
            transform.position = Vector3.MoveTowards(this.transform.position, SingleMovePosition, RushSpeed * Time.deltaTime);
            rb.isKinematic = true;
            Attack = true;
            if ((Mathf.Approximately(this.transform.position.x, SingleMovePosition.x) && Mathf.Approximately(this.transform.position.y, SingleMovePosition.y) && Mathf.Approximately(this.transform.position.z, SingleMovePosition.z)))
            {
                if (Target != null && Target.activeSelf)
                {
                    //対象にダメージを与える
                    Target.GetComponent<StatueHPManager>().SingleDamage();
                }
                isTarget_Statue = false;
                ismove_Statue = false;
                isMoving = false;
                rb.isKinematic = false;
                Attack = false;
                TargetStatue = null;
                // プレイヤーの攻撃をした時の音をON（アニメーションが出来次第移動）--------M追加--------
                ps.isPlayAttackSound = true;
                // プレイヤーの攻撃が当たった時の音をON--------M追加--------
                //ps.isPlayAttackHitSound = true;
            }
            }

        //ビーム敵への突進、突進完了後の処理
        if (ismove_Beam)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, SingleMovePosition, RushSpeed * Time.deltaTime);
            rb.isKinematic = true;
            Attack = true;
            if ((Mathf.Approximately(this.transform.position.x, SingleMovePosition.x) && Mathf.Approximately(this.transform.position.y, SingleMovePosition.y) && Mathf.Approximately(this.transform.position.z, SingleMovePosition.z)))
            {
                if (Target != null && Target.activeSelf)
                {
                    //対象にダメージを与える
                    Target.GetComponent<BeamHPManager>().SingleDamage();
                }
                ismove_Beam = false;
                isMoving = false;
                rb.isKinematic = false;
                Attack = false;
                isTarget_Beam = false;
                TargetBeam = null;
                // プレイヤーの攻撃をした時の音をON（アニメーションが出来次第移動）--------村岡追加--------
                ps.isPlayAttackSound = true;
                // プレイヤーの攻撃が当たった時の音をON--------村岡追加--------
                //ps.isPlayAttackHitSound = true;
            }
        }
        //Bossへの突進、突進完了後の処理
        if (ismove_Boss)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, SingleMovePosition, RushSpeed * Time.deltaTime);
            rb.isKinematic = true;
            Attack = true;
            if ((Mathf.Approximately(this.transform.position.x, SingleMovePosition.x) && Mathf.Approximately(this.transform.position.y, SingleMovePosition.y) && Mathf.Approximately(this.transform.position.z, SingleMovePosition.z)))
            {
                if (Target != null && Target.activeSelf)
                {
                    //対象へダメージを与える
                    Target.GetComponent<Clear>().SingleDamage();
                }
                ismove_Boss = false;
                isMoving = false;
                rb.isKinematic = false;
                Attack = false;
                isTarget_Boss = false;
                TargetBoss = null;
                // プレイヤーの攻撃をした時の音をON（アニメーションが出来次第移動）--------村岡追加--------
                ps.isPlayAttackSound = true;
                // プレイヤーの攻撃が当たった時の音をON--------村岡追加--------
                //ps.isPlayAttackHitSound = true;
            }
        }
    }

      
         

    //突進後のノックバック
        public void KnockBack(Collision collision)
        {
            nowPos = this.transform.position;
            var boundVec = (nowPos - collision.transform.position);
            boundVec.y = 0.0f;
            boundVec = boundVec.normalized * backKnockBackForce;
            boundVec.y = 1.0f * upKnockBackForce;
            boundVec = boundVec.normalized;
            rb.velocity = Vector3.zero;
            rb.AddForce(boundVec * knockBackPower, ForceMode.Impulse);
        }

    //敵に近づき過ぎた場合に後ろに下がる
        private void GetBack()
        {
           Vector3 First = new Vector3(FirstPosition.x, this.transform.position.y, FirstPosition.z);
           Vector3 movePoint = new Vector3(surfacePoint.x, this.transform.position.y, surfacePoint.z);
           float moveDistance = (movePoint - First).magnitude;
           float moveAmount = 5.0f;
           Vector3 moveDirection = (First - movePoint).normalized;
           this.transform.position = Vector3.Lerp(this.transform.position, this.transform.position + moveDirection * moveAmount,Time.deltaTime * RushSpeed);
        }

    //ノックバック処理
    public void SingleKnockBack(float MoveDistance, float updistance)
    {
        FirstPosition.y = SingleMovePosition.y;
        Vector3 Direction = (Target.transform.position - FirstPosition).normalized;
        Vector3 upOffset = new Vector3(0f, updistance, 0f);
        Vector3 newPosition = transform.position - Direction * MoveDistance + upOffset;
        transform.position = Vector3.Lerp(transform.position, newPosition, 2f * Time.deltaTime);
        rb.velocity = Vector3.zero;
        Debug.Log("Back");
    }
    private void OnCollisionEnter(Collision other)
        {
            if ((other.gameObject.tag == "Statue" || other.gameObject.tag == "Beam") && SpecialAttack)
            {
                //必殺技使用時のSE
                st.SE_SuperAttackPlayer();
            }
        }

    
    //敵の防御力がPlayerの攻撃力よりも高かった場合のノックバック処理
        public void DenfensiveKnockBack()
        {
            Vector3 incidentVector = (FirstPosition - SingleMovePosition).normalized;
            float KnockBackDistance = 15f;
            Vector3 moveDirection = transform.position + incidentVector * KnockBackDistance;
            rb.velocity = Vector3.zero;
            this.transform.position = Vector3.Lerp(this.transform.position,moveDirection, Time.deltaTime * RushSpeed);
        }

        IEnumerator ResetCollisionFlag()
        {
            yield return new WaitForSeconds(1f);
            jumpwait = false;
        }


        public void AddData(Data data)
        {
            dataList.Add(data);
        }

    //必殺技発動コルーチン
        public IEnumerator DelayedStart(float delayInSeconds)
        {
            SpecialAtStart = true;
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            //必殺技の開始地点
            Vector3 FirstPoint = transform.position;    
            //必殺技発動前のスローモーションの秒数
            Time.timeScale = slowMotionScale;
        　　//必殺技専用エフェクト出現　
            SpecialEffect.SetActive(true);
            transform.LookAt(SpecialPosition);

            yield return new WaitForSecondsRealtime(delayInSeconds);
            //必殺技処理開始

            // プレイヤーの突進音をON--------M追加--------
            ps.isPlayRushSound = true;
        　　//時間を元に戻す
            Time.timeScale = originalTimeScale;
            SpecialEffect.SetActive(false);
            SpecialAttack = true;
            rb.isKinematic = false;

            if (!isMoving)
            {
                int layerMask = 1 << LayerMask.NameToLayer("Default"); 
                int enemyLayerMask = 1 << LayerMask.NameToLayer("Boss");

                Ray ray = new Ray(transform.position, SpecialPosition - transform.position);
                RaycastHit hit;
                

　　　　　　　　　
            　　//突進完了位置を事前に計算し、Boss、Variantの場合は、衝突した地点で停止、それ以外の場合は反射
                if (Physics.Raycast(ray, out hit, Vector3.Distance(transform.position, SpecialPosition), (layerMask | enemyLayerMask)))
                {
                    if ((layerMask & (1 << hit.collider.gameObject.layer)) != 0)
                    {
                        Vector3 normal = hit.normal;


                        // 法線ベクトルの向きによって壁か床かを判別する
                        if (Mathf.Abs(normal.y) > Mathf.Abs(normal.x) && Mathf.Abs(normal.y) > Mathf.Abs(normal.z))
                        {
                            // 法線ベクトルが上向き（床の法線ベクトル）の場合は床と判定
                            // 床に対する処理を行う
                            // 衝突地点を取得
                            Vector3 collisionPoint = hit.point;
                            Debug.Log("床");
                            FinalSpecialPosition = collisionPoint;
                            // 新しい位置にTweenアニメーションを設定（床に移動）
                            transform.DOMove(collisionPoint, 0.2f)
                                .SetEase(Ease.Linear)
                                .OnStart(() =>
                                {
                                    isMoving = true;
                                })
                                .OnComplete(() =>
                                {
                                // 床への移動が完了したら、元々移動していた方向に床と水平な角度で移動
                                Vector3 incidentVector = (SpecialPosition - FirstPoint).normalized;
                                    Vector3 floorNormal = hit.normal;
                                    Vector3 reflectionVector = Vector3.Reflect(incidentVector, floorNormal);
                                    Vector3 moveDirection = Vector3.Cross(Vector3.Cross(incidentVector, floorNormal), floorNormal).normalized;

                                // 移動距離を計算
                                float remainingDistance = 15f;

                                // 新しい位置を計算
                                Vector3 newPosition = collisionPoint + reflectionVector * remainingDistance;
                                    isMoving = false;
                                    SpecialAttack = false;
                                    SpecialAtStart = false;
                                    rb.useGravity = true;
                                // 新しい位置にTweenアニメーションを設定（床と水平な角度で移動）
                                rb.velocity = Vector3.zero;
                                    this.transform.position = Vector3.Lerp(this.transform.position, newPosition, Time.deltaTime * RushSpeed);
                                });
                        }
                        else
                        {
                            // 法線ベクトルが水平方向（壁の法線ベクトル）の場合は壁と判定
                            // 壁に対する処理を行う
                            // 壁の法線ベクトルを取得
                            Vector3 wallNormal = hit.normal;
                            Debug.Log("壁");
                            FinalSpecialPosition = hit.point;
                            // 新しい位置にTweenアニメーションを設定（壁に移動）
                            transform.DOMove(hit.point, 0.2f)
                                .SetEase(Ease.Linear)
                                .OnStart(() =>
                                {
                                    isMoving = true;
                                })
                                .OnComplete(() =>
                                {
                                // 壁への移動が完了したら、反射を実行
                                Vector3 incidentVector = (SpecialPosition - FirstPoint).normalized;
                                    float remainingDistance = 15f;
                                    Vector3 reflectionVector = Vector3.Reflect(incidentVector, wallNormal);

                                    Vector3 newPosition = hit.point + reflectionVector * remainingDistance;
                                    isMoving = false;
                                    SpecialAttack = false;
                                    rb.isKinematic = false;
                                    SpecialAtStart = false;
                                    rb.useGravity = true;
                                // 新しい位置にTweenアニメーションを設定（反射）
                                rb.velocity = Vector3.zero;
                                    this.transform.position = Vector3.Lerp(this.transform.position, newPosition, Time.deltaTime * RushSpeed);
                                });
                        }
                    }
                    else if (hit.collider.CompareTag("BOSS") || hit.collider.gameObject.name.Contains("Variant"))
                    {
                        FinalSpecialPosition = hit.collider.gameObject.GetComponent<IsRendered>().StatueRenderer.bounds.center;
                        transform.DOMove(hit.collider.gameObject.GetComponent<IsRendered>().StatueRenderer.bounds.center, 0.3f)
                                      .SetEase(Ease.Linear)
                                      .OnComplete(() =>
                                      {
                                          isMoving = false;
                                          SpecialAttack = false;
                                          SpecialAtStart = false;
                                          rb.useGravity = true;
                                          Vector3 incidentVector = (SpecialPosition - FirstPoint).normalized;
                                          float remainingDistance = 15f;
                                          Vector3 newPosition = hit.point + incidentVector * remainingDistance;
                                          this.transform.position = Vector3.Lerp(this.transform.position, newPosition, Time.deltaTime * RushSpeed);
                                      });
                    }
                }
                else
                {
                    FinalSpecialPosition = SpecialPosition;
                    // 衝突しない場合は通常のTweenアニメーションを実行
                    transform.DOMove(SpecialPosition, 0.3f)
                        .SetEase(Ease.Linear)
                        .OnComplete(() =>
                        {
                            isMoving = false;
                            SpecialAttack = false;
                            SpecialAtStart = false;
                            rb.useGravity = true;
                        });
                }
            }
        }

    }

