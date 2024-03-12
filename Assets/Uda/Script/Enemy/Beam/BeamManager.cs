using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamManager : MonoBehaviour
{
    [System.Serializable]
    public class BeamData
    {
        //ビーム敵の初期情報
        public GameObject BeamPrefab;
        public Vector3 BeamPosition;

        public Vector3 BeamSize;

        //BeamEnemyBodyの変数
        public float BeamSpeed;
        public GameObject Beam;

        //BeamHPManagerの変数
        public float DamageValue;
        public float MaxHP;
        public float DefencePower;

        //リポップの秒数
        public float respawnInterval_Beam;
    }
    public List<BeamData> beamDataList = new List<BeamData>();
    public GameObject Beam;
    public float respawnInterval;

    [SerializeField]
    GameObject particle;

    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RespawnBeam(BeamData beamData)
    {
        StartCoroutine(RespawnCoroutine(beamData));
    }

    private IEnumerator RespawnCoroutine(BeamData beamData)
    {

        yield return new WaitForSecondsRealtime(beamData.respawnInterval_Beam - 0.3f);

        Instantiate(particle, beamData.BeamPosition, Quaternion.identity);

        yield return new WaitForSecondsRealtime(0.3f);

        GameObject newBeam = Instantiate(Beam, beamData.BeamPosition, Quaternion.identity);
        newBeam.transform.localScale = beamData.BeamSize;

        //BeamEnemyBodyの変数
        newBeam.GetComponent<BeamEnemy_ver2>().BeamSpeed = beamData.BeamSpeed;
        newBeam.GetComponent<BeamEnemy_ver2>().BeamPrefab = beamData.Beam;

        //BeamHPManagerの変数
        newBeam.GetComponent<BeamHPManager>().DamageValue = beamData.DamageValue;
        newBeam.GetComponent<BeamHPManager>().MaxHP = beamData.MaxHP;
        newBeam.GetComponent<BeamHPManager>().DefencePower = beamData.DefencePower;

        RemoveData(beamData);
    }

    private void RemoveData(BeamData beamData)
    {
        beamDataList.Remove(beamData);
    }

    public void AddBeamData(GameObject enemyObject)
    {
        BeamData beamData = new BeamData();
        beamData.BeamPrefab = enemyObject;
        beamData.BeamSize = enemyObject.transform.localScale;

        BeamEnemy_ver2 b = enemyObject.GetComponent<BeamEnemy_ver2>();

        //BeamEnemy_ver2の変数
        beamData.BeamSpeed = b.BeamSpeed;
        beamData.Beam = b.BeamPrefab;

        BeamHPManager bHP = enemyObject.GetComponent<BeamHPManager>();

        //BeamHPManagerの変数
        beamData.DamageValue = bHP.DamageValue;
        beamData.MaxHP = bHP.MaxHP;
        beamData.BeamPosition = bHP.firstPos;
        beamData.DefencePower = bHP.DefencePower;

        //リポップ時間
        beamData.respawnInterval_Beam = respawnInterval;

        beamDataList.Add(beamData);
    }

    public void RequestsRespawn(GameObject beamObject)
    {
        foreach(BeamData beamData in beamDataList)
        {
            if(beamData.BeamPrefab == beamObject)
            {
                RespawnBeam(beamData);
                break;
            }
        }
    }
}
