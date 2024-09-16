using UnityEngine;

public class Granade : MonoBehaviour
{
    public float delay = 3f;            // 爆発までの時間
    public float radius = 5f;           // 爆発の半径
    public float explosionForce = 700f; // 爆発力
    public float damage = 100f;          // 爆発ダメージ
    public GameObject explosionEffect;  // 爆発効果

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
        // 爆発効果生成
        Instantiate(explosionEffect, transform.position, transform.rotation);

        // 爆発半径の全部のColliderを獲得
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyObject in colliders)
        {
            // 爆発力実現
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, radius);
            }

            //周りのEnemyにダメージを与える
            EnemyHealth enemy = nearbyObject.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        // グレネードDestroy
        Destroy(gameObject);
    }

}
