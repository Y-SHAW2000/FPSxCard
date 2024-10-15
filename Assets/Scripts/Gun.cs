using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//èeÇÃScript
public class Gun : MonoBehaviour
{
    public int BulletCount = 10; //íeä€êî

    public GameObject bulletPrefab;
    public GameObject casingPrefab;

    public Transform bulletTf;
    public Transform casingTf;


    void Start()
    {
        
    }

    public void Attack()
    {
        GameObject bulletObj = Instantiate(bulletPrefab);
        bulletObj.transform.position = bulletTf.transform.position;
        bulletObj.GetComponent<Rigidbody>().AddForce(transform.forward * 500, ForceMode.Impulse);

        GameObject casingObj = Instantiate(casingPrefab);
        casingObj.transform.position = casingTf.transform.position;
    }

}
