using UnityEngine;

public class HealingGranade : MonoBehaviour
{
    public float explosionRadius = 5f;    // 爆発の半径
    public float healingAmount = 50f;     // 治療量
    public GameObject healingExplosionEffect;    // 爆発効果

    private void OnCollisionEnter(Collision collision)
    {
        // 物体とぶつかったら爆発
        HealingExplode();
    }

    void HealingExplode()
    {
        // 爆発効果生成
        Instantiate(healingExplosionEffect, transform.position, transform.rotation);

        // 爆発半径の全部のColliderを獲得
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            // Colliderは味方かどうかを判断
            if (nearbyObject.CompareTag("Friendly"))
            {
                // 治療
                AllyHealth friendlyHealth = nearbyObject.GetComponent<AllyHealth>();
                if (friendlyHealth != null)
                {
                    friendlyHealth.Heal(healingAmount);
                }
            }
        }

        // グレネードDestroy
        Destroy(gameObject);
    }
}

