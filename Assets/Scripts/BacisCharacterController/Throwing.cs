using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwing : MonoBehaviour
{
    public KeyCode throwKey = KeyCode.G;
    private Camera m_camera;
    public GameObject granade;
    public GameObject healingGranade;
    private GameObject throwingItem;
    public Transform throwItemGeneratePosition; //�A�j���[�V�����ɂ���ăv���C���[�̎�̈ʒu�̂Ƃ���ɐ���
    public float throwForce; //�O�ɉ������
    public float throwUpwardForce; //��ɉ������

    void Start()
    {
        m_camera = GetComponentInChildren<Camera>();
    }


    void Update()
    {
        if(throwingItem != null)
        {
            Throw();
        }

    }
    private void Throw()
    {
        if (Input.GetKeyDown(throwKey))
        {

            //instantiate object to throw
            GameObject projectile = Instantiate(throwingItem, throwItemGeneratePosition.position, m_camera.transform.rotation);
            //get rigidbody component
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

            //�A�j���[�V�����Ő����ʒu���K�v�A���A�j���[�V�����Ȃ���
            //calculate direction

            Vector3 forceDirection = m_camera.transform.forward;

            RaycastHit hit;

            if (Physics.Raycast(m_camera.transform.position, m_camera.transform.forward, out hit, 500f))
            {
                forceDirection = (hit.point - throwItemGeneratePosition.position).normalized;
            }

            //add force
            Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

            projectileRb.AddForce(forceToAdd, ForceMode.Impulse);
        }

    }
    public void ReplaceThrowingItem(GameObject newThrowingItem)
    {
        throwingItem = newThrowingItem;
    }
}
