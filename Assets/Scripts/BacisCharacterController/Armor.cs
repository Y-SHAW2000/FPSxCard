using UnityEngine;

public class Armor : MonoBehaviour
{
    public int maxArmor;
    public int currentArmor;
    public float damageReductionPercentage;

    void Start()
    {
        currentArmor = maxArmor;
    }

    public void TakeDamage(int damage)
    {
        //軽減した後のダメージ（reducedDamage）を計算
        int reducedDamage = Mathf.RoundToInt(damage * (1 - damageReductionPercentage / 100f));
        
        //アーマーがある場合、まずアーマーから防いだダメージ（reducedDamage）を割り
        if (currentArmor > 0)　
        {
            int armorDamage = Mathf.Min(currentArmor, reducedDamage);
            currentArmor -= armorDamage;
            reducedDamage -= armorDamage;
        }

        //もし
        if (reducedDamage > 0)
        {
            // Apply remaining damage to health (implement this in your health system)
            //Health health = GetComponent<Health>();
            //if (health != null)
            //{
            //health.TakeDamage(reducedDamage);
            //}
        }
    }
}
