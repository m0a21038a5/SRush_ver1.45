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

    //�J�����̕`��͈͓����Օ����ɉB��Ă��Ȃ��ꍇtargetList�ɒǉ�
    public void IsVisibleFromWall(MeshRenderer renderer,MeshRenderer StatueRenderer ,  Camera camera, LayerMask layerMask, GameObject targetObject)
    {
        //�J�����̕`��͈͎擾
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);

        //���o�Ɏg�p����Renderer
        Bounds bounds = renderer.bounds;

        //�ΏۃI�u�W�F�N�g�̍��W
        Vector3 targetPosition;
       
        targetPosition = StatueRenderer.bounds.center;
      
        //�Ώۂ�Player�̋����v�Z
        float dis = Vector3.Distance(Player.bounds.center, targetPosition) + Vector3.Distance(camera.transform.position, Player.bounds.center);

        //�J�����̕`��͈͂�Renderer������APlayer�Ƃ̋�����120f�ȓ��̏ꍇ�̂ݎՕ���������s��
        if(GeometryUtility.TestPlanesAABB(planes, bounds) == true && dis < 120f)
        {
            RaycastHit hit;

            //Ray�̊J�n�ʒu
            Vector3 origin;

            //�J���������ꉉ�o���o�Ȃ��ꍇ�́ARay�̊J�n�ʒu��ύX
            if (!c.SpecialMode || !mt.ChangeCamera)
            {
                origin = camera.transform.position + new Vector3(0.0f, 5.0f, 0.0f);
            }
            else
            {
                origin = camera.transform.position;
            }
            //Ray�̕���
            Vector3 rayDirection = targetPosition - origin;
            //Ray�̍ő勗��
            float rayDistance = 70f;
            Physics.Raycast(origin , rayDirection, out hit, rayDistance, layerMask);
            //Ray�̃f�o�b�O
            Debug.DrawRay(origin, rayDirection * rayDistance, Color.red, 0.1f, false);
            //�����\�ł��邱�Ƃ�G��IsRenderd��
            R = targetObject.GetComponent<IsRendered>();
            R.visible = true;

            //Ray���ΏۃI�u�W�F�N�g�Ƀq�b�g�������̂�targetList�֒ǉ�
            if(hit.collider.gameObject == targetObject)
            {
                if (!s.targetList.Contains(targetObject))
                {
                    //�G�̎�ނɉ����ď�����ύX
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
                //Player���ːi��ԏo�Ȃ��A�Ώۂ��Օ����ɉB�ꂽ�ꍇ�́AtargetList����폜
                if (!t.isMoving)
                {
                    s.targetList.Remove(targetObject);
                    R.visible = false;
                }
            }
        }
        else
        {
            //Player���ːi��ԏo�Ȃ��A�Ώۂ��J�����̕`��͈͓����炢�Ȃ��Ȃ����ꍇ�́AtargetList����폜
            if (!t.isMoving)
            {
                s.targetList.Remove(targetObject);
                R.visible = false;
            }
        }
    }

  
}
