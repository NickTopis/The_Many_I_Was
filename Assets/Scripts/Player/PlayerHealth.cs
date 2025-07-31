using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public float hitPoints = 100f;
    [SerializeField] private float maxHitPoints = 100f;
    [SerializeField] private float regenRate = 0.5f; // health per second
    [SerializeField] private float regenDelay = 3f; // wait this many seconds before regen starts
    [SerializeField] TextMeshProUGUI healthText;

    private float lastDamageTime;

    private void Start()
    {
        UpdateHealthText();
    }

    private void Update()
    {
        // Check if enough time passed since last damage
        if (hitPoints > 0 && hitPoints < maxHitPoints)
        {
            if (Time.time - lastDamageTime >= regenDelay)
            {
                hitPoints += regenRate * Time.deltaTime;
                if (hitPoints > maxHitPoints) hitPoints = maxHitPoints;
                UpdateHealthText();
            }
        }
    }

    public void TakeDamage(int damage)
    {
        hitPoints -= damage;
        if (hitPoints < 0) hitPoints = 0;

        lastDamageTime = Time.time; // reset regen timer
        UpdateHealthText();

        if (hitPoints <= 0)
        {
            GetComponent<DeathHandler>().HandleDeath();
        }
    }

    private void UpdateHealthText()
    {
        healthText.text = "Health : " + Mathf.RoundToInt(hitPoints).ToString();
    }
}
