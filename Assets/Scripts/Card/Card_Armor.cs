using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Card_Armor : MonoBehaviour
{
    public GameObject m_player; //����
    public KeyCode itemKey = KeyCode.E; //Item�g�p�L�[

    [Header("ArmorDataBase")]
    public float armorAmount; //�t�^����A�[�}�[��
    public float damageReductionPercentage; // �A�[�}�[�̃_���[�W�y������

    public void Start()
    {

    }
    public  void Update()
    {
        if(this.gameObject.tag == "ActiveArea" && Input.GetKeyDown(itemKey)) //Active������g�p�L�[�������ꍇ
        {
           // GiveArmor();
        }
        
    }

    public void Use(GameObject target)
    {
        Armor armor = target.GetComponent<Armor>();
        if (armor == null)
        {
            armor = target.AddComponent<Armor>();
        }
        //armor.maxArmor += armorAmount;
        armor.currentArmor = armor.maxArmor;
        armor.damageReductionPercentage = damageReductionPercentage;

        // �@�ʖ�?��TANK�C?�p�?����
        //RoleManager roleManager = target.GetComponent<RoleManager>();
        //if (roleManager != null && roleManager.HasRoleTag(target, "Tank"))
        //{
            //ApplyArmorToAllies(target);
        //}
    }

    private void ApplyArmorToAllies(GameObject tank)
    {
        Collider[] colliders = Physics.OverlapSphere(tank.transform.position, 5f); // 5?���?
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Friendly"))
            {
                Armor armor = collider.GetComponent<Armor>();
                if (armor == null)
                {
                    armor = collider.gameObject.AddComponent<Armor>();
                }
                //armor.maxArmor += armorAmount;
                armor.currentArmor = armor.maxArmor;
                armor.damageReductionPercentage = damageReductionPercentage;
            }
        }
    }
}
