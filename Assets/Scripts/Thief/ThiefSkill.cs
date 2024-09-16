using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ThiefSkill : MonoBehaviour
{
    private CharacterController m_CharacterController;
    private Camera m_camera;
    private Transform player;
    [Header("Smoke Grade")]
    public GameObject smoke;
    private Rigidbody smoke_rb;
    public Transform smokeGeneratePosition; //アニメーションによってプレイヤーの手の位置のところに生成
    public Vector3 smokeAddForceTransform; //どの方向に力を加える、投げるアニメーションとプレイヤーの 視角によって計算する
    public float throwForce; //前に加える力
    public float throwUpwardForce; //上に加える力
    [Header("SkillKey")]
    public KeyCode skill1Key;
    public KeyCode skill2Key;
    [Header("SkillCoolDownTime")]
    public float skill1CD = 100;
    private float skill1Count = 0;
    public float skill2CD = 100;
    private float skill2Count = 0;



    void Start()
    {
        gameObject.AddComponent<Attacker>();
        m_CharacterController = GetComponent<CharacterController>();
        m_camera = GetComponentInChildren<Camera>();
        smoke_rb = smoke.GetComponent<Rigidbody>();
        player = GetComponent<Transform>();
    }

    void Update()
    {
        skill1Count -= 1;
        skill2Count -= 1;
        Smoke();
        Teleport();
    }

    private void Smoke()
    {
        if (Input.GetKeyDown(skill1Key))
        {
            
            //instantiate object to throw
            GameObject projectile = Instantiate(smoke, smokeGeneratePosition.position, m_camera.transform.rotation);
            //get rigidbody component
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

            //アニメーションで生成位置が必要、今アニメーションなしで
            //calculate direction

            Vector3 forceDirection = m_camera.transform.forward;

            RaycastHit hit;

            if(Physics.Raycast(m_camera.transform.position, m_camera.transform.forward, out hit, 500f))
            {
                forceDirection = (hit.point - smokeGeneratePosition.position).normalized;
            }

            //add force
            Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

            projectileRb.AddForce(forceToAdd, ForceMode.Impulse);
        }

    }

    private void Teleport()
    {
        if (Input.GetKeyDown(skill2Key))
        {
            if (skill2Count >= 0)
            {
                return;
            }
            else
            {
                GetComponent<CharacterController>().enabled = false;
                //Debug.Log(transform.position);
                if (m_CharacterController.velocity == new Vector3(0, 0, 0))
                {
                    player.position += player.forward * 5.0f;
                }
                else if (m_CharacterController.velocity.y >= 0)
                {
                    player.position += m_CharacterController.velocity.normalized * 5.0f;
                }
                else if (m_CharacterController.velocity.y < 0)
                {
                    player.position += new Vector3(m_CharacterController.velocity.normalized.x * 5.0f, player.position.y, m_CharacterController.velocity.normalized.z * 5.0f);
                }

                //Debug.Log(transform.position);
                GetComponent<CharacterController>().enabled = true;
                skill2Count = skill2CD;
            }
        }

    }
}
