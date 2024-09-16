using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeParticleGenerate : MonoBehaviour
{
    public ParticleSystem smokeParticle;


    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(smokeParticle, collision.transform.position, Quaternion.identity);
        Destroy(this.gameObject, 1f);
    }
}
