using UnityEngine;

public class Granade : MonoBehaviour
{
    public float delay = 3f;            // �����܂ł̎���
    public float radius = 5f;           // �����̔��a
    public float explosionForce = 700f; // ������
    public float damage = 100f;          // �����_���[�W
    public GameObject explosionEffect;  // ��������

    private float countdown;
    private bool hasExploded = false;

    void Start()
    {
        countdown = delay;
    }

    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }

    void Explode()
    {
        // �������ʐ���
        Instantiate(explosionEffect, transform.position, transform.rotation);

        // �������a�̑S����Collider���l��
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyObject in colliders)
        {
            // �����͎���
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, radius);
            }

            //�����Enemy�Ƀ_���[�W��^����
            EnemyHealth enemy = nearbyObject.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        // �O���l�[�hDestroy
        Destroy(gameObject);
    }

}
