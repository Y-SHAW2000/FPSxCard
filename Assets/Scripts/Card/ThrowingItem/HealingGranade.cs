using UnityEngine;

public class HealingGranade : MonoBehaviour
{
    public float explosionRadius = 5f;    // ”š”­‚Ì”¼Œa
    public float healingAmount = 50f;     // ¡—Ã—Ê
    public GameObject healingExplosionEffect;    // ”š”­Œø‰Ê

    private void OnCollisionEnter(Collision collision)
    {
        // •¨‘Ì‚Æ‚Ô‚Â‚©‚Á‚½‚ç”š”­
        HealingExplode();
    }

    void HealingExplode()
    {
        // ”š”­Œø‰Ê¶¬
        Instantiate(healingExplosionEffect, transform.position, transform.rotation);

        // ”š”­”¼Œa‚Ì‘S•”‚ÌCollider‚ğŠl“¾
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            // Collider‚Í–¡•û‚©‚Ç‚¤‚©‚ğ”»’f
            if (nearbyObject.CompareTag("Friendly"))
            {
                // ¡—Ã
                AllyHealth friendlyHealth = nearbyObject.GetComponent<AllyHealth>();
                if (friendlyHealth != null)
                {
                    friendlyHealth.Heal(healingAmount);
                }
            }
        }

        // ƒOƒŒƒl[ƒhDestroy
        Destroy(gameObject);
    }
}

