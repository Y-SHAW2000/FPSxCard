using UnityEngine;

public class HealingGranade : MonoBehaviour
{
    public float explosionRadius = 5f;    // �����̔��a
    public float healingAmount = 50f;     // ���×�
    public GameObject healingExplosionEffect;    // ��������

    private void OnCollisionEnter(Collision collision)
    {
        // ���̂ƂԂ������甚��
        HealingExplode();
    }

    void HealingExplode()
    {
        // �������ʐ���
        Instantiate(healingExplosionEffect, transform.position, transform.rotation);

        // �������a�̑S����Collider���l��
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            // Collider�͖������ǂ����𔻒f
            if (nearbyObject.CompareTag("Friendly"))
            {
                // ����
                AllyHealth friendlyHealth = nearbyObject.GetComponent<AllyHealth>();
                if (friendlyHealth != null)
                {
                    friendlyHealth.Heal(healingAmount);
                }
            }
        }

        // �O���l�[�hDestroy
        Destroy(gameObject);
    }
}

