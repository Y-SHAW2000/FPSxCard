using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Card_Armor : MonoBehaviour
{
    public GameObject m_player; //自分
    public KeyCode itemKey = KeyCode.E; //Item使用キー

    [Header("ArmorDataBase")]
    public float armorAmount; //付与するアーマー量
    public float damageReductionPercentage; // アーマーのダメージ軽減割合

    public void Start()
    {

    }
    public  void Update()
    {
        if(this.gameObject.tag == "ActiveArea" && Input.GetKeyDown(itemKey)) //Activeかつ道具使用キーを押す場合
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

        // 如果目?是TANK，?用范?效果
        //RoleManager roleManager = target.GetComponent<RoleManager>();
        //if (roleManager != null && roleManager.HasRoleTag(target, "Tank"))
        //{
            //ApplyArmorToAllies(target);
        //}
    }

    private void ApplyArmorToAllies(GameObject tank)
    {
        Collider[] colliders = Physics.OverlapSphere(tank.transform.position, 5f); // 5?位范?
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
