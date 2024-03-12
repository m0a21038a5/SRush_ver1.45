using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class singleBeam : MonoBehaviour
{
    public LayerMask raycastLayer; // Ray���΂��Ώۂ̃��C���[�}�X�N
    public float raycastDistance = 10f; // Ray�̔�΂�����



    [SerializeField]
    private List<GameObject> targetList;

    public Transform character;// �L�����N�^�[��Transform

    private target ta;
    private single si;

    public bool SortB;

    [SerializeField]
    public GameObject Playerobj;

    [SerializeField] public GameObject TargetCamera;

    void Start()
    {
        Playerobj = GameObject.Find("Player");
        GameObject ConeObj = GameObject.Find("Cone");
        ta = Playerobj.GetComponent<target>();
        si = ConeObj.GetComponent<single>();
        SortB = false;
    }
    void Update()
    {
        if (SortB == false)
        {
            if (ta.isTarget_Beam == true)
            {
                //ta.BeamPos = targetList[0].transform.position;
                ta.TargetBeam = targetList[0];
            }
            else
            {
                ta.BeamPos = Vector3.zero;
                ta.TargetBeam = null;
            }
        }
        if (ta.clear == true)
        {
            ListClear();
            ta.clear = false;
        }

    }





    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Beam")) // �R���C�_�[�ɓ������I�u�W�F�N�g��Beam�^�O�������Ă��邩�m�F
        {
            ta.isTarget_Beam = true;
            Vector3 raycastDirection = other.transform.position - transform.position; // �v���C���[�̈ʒu���玩���̈ʒu���������x�N�g����Ray�̕����Ƃ���
            Ray ray = new Ray(TargetCamera.transform.position, raycastDirection); // �����̈ʒu���N�_��Ray���쐬
            Debug.DrawRay(ray.origin, raycastDirection, Color.red, 3, false);
            RaycastHit[] hits = Physics.RaycastAll(ray, raycastDistance, raycastLayer); // Ray���΂��đΏۂ̃��C���[�}�X�N�Ƀq�b�g�����S�ẴI�u�W�F�N�g���擾

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("Beam")) // �q�b�g�����I�u�W�F�N�g��Enemy�^�O�������Ă��邩�m�F
                {

                    if (!targetList.Contains(other.gameObject)) // List �Ɋ܂܂�Ă��Ȃ��ꍇ�̂ݒǉ�����
                    {
                        targetList.Add(other.gameObject);

                    }
                    if (SortB == false)
                    {
                        si.SortVectorList();
                    }
                }
            }


        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Beam")) // �R���C�_�[����o���I�u�W�F�N�g��Player�^�O�������Ă��邩�m�F
        {
            si.ListClear();
            ta.isTarget_Beam = false;
        }
    }
    public void ListClear()
    {
        targetList.Clear(); //List���ׂĂ̗v�f���폜

    }
}
