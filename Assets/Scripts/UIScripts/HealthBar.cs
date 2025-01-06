using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health healthComponent;
    [SerializeField] private Slider healthSlider;

    private void OnEnable()
    {
        healthComponent.OnHealthChanged.AddListener(UpdateHealthBar);
    }

    private void OnDisable()
    {
        healthComponent.OnHealthChanged.RemoveListener(UpdateHealthBar);
    }

    private void Start()
    {
        healthSlider.maxValue = healthComponent.maxHealth;
        healthSlider.value = healthComponent.currentHealth;
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
}
