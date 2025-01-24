
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float health = 100f;
    public float maxHealth = 100f;
    
    public Image healthBarImage;

    // Update is called once per frame
    void Update()
    {
        healthBarImage.fillAmount = health / maxHealth;
        
        health = Mathf.Clamp(health, 0f, maxHealth);
    }

    public void DamageButton(int damageAmount)
    {
        health -= damageAmount;
    }

    public void HealButton(int healAmount)
    {
        health += healAmount;
    }
}
