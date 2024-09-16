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
        //�y��������̃_���[�W�ireducedDamage�j���v�Z
        int reducedDamage = Mathf.RoundToInt(damage * (1 - damageReductionPercentage / 100f));
        
        //�A�[�}�[������ꍇ�A�܂��A�[�}�[����h�����_���[�W�ireducedDamage�j������
        if (currentArmor > 0)�@
        {
            int armorDamage = Mathf.Min(currentArmor, reducedDamage);
            currentArmor -= armorDamage;
            reducedDamage -= armorDamage;
        }

        //����
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
