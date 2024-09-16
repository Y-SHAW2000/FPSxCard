using UnityEngine;

public class AllyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth); //¡‚ÌHP‚ªÅ‘å’l‚É’´‚¦‚È‚¢‚æ‚¤‚É
        Debug.Log("Healed! Current Health: " + currentHealth);
    }
}
