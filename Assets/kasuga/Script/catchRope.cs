using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class catchRope : MonoBehaviour
{
	//�v���C���[�̐e�����[�v�ɂ���X�N���v�g
	[SerializeField]
	private Transform arrivalPoint;

	public bool freezePos = false;
	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player")
		{
			//�@�L�����N�^�[�̐e�����[�v�ɂ���
			col.transform.SetParent(transform);
			//FreezePosition�̃I���ƃI�t��؂�ւ���ϐ�
			freezePos = true;
		}
	}
}
