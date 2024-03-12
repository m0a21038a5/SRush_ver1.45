using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IsRendered : MonoBehaviour
{
    //敵にこのスクリプト付けてください

    //検出対象のRenderer
    [SerializeField] public MeshRenderer targetRenderer;
    //突進目標のRenderer
    [SerializeField] public MeshRenderer StatueRenderer;
    //近接敵静止状態のRenderer
    [SerializeField] MeshRenderer StatueRenderer_01;
    //近接敵動いている状態のRenderer
    [SerializeField] MeshRenderer StatueRenderer_02;
    //現在のCamera
    [SerializeField] Camera cam;
    //EnemyLayerに設定してください
    [SerializeField] LayerMask layerMask;
    //近接敵静止状態のオブジェクト
    [SerializeField] GameObject enemy_01;
    //近接敵動いている状態のオブジェクト
    [SerializeField] GameObject enemy_02;

    //可視化判定
    public bool visible;
   
    //検出メソッド用に取得
    SingleTarget st;

    //突進座標設定用に取得
    target t;



    void Start()
    {
        st = GameObject.FindGameObjectWithTag("Manager").GetComponent<SingleTarget>();

        //Bossのみ大きすぎるため検出用Rendererを胴体部に限定
        if (this.gameObject.CompareTag("BOSS"))
        {
            GameObject first = this.transform.GetChild(3).gameObject;
            targetRenderer = first.transform.GetChild(0).GetComponent<MeshRenderer>();
        }
        else
        {
            targetRenderer = this.gameObject.GetComponent<MeshRenderer>();
        }

        //近接敵は中心がずれているため、突進目標のRendererを胴体部に限定
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

        //近接敵の切り替えに応じて、Rendererを変更
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
       
        //可視化判定
        st.IsVisibleFromWall(targetRenderer, StatueRenderer, cam, layerMask, this.gameObject);
        
        //Boss、近接敵の場合のみ突進目標を変更
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
