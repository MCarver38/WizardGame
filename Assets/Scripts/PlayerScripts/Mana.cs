using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Mana : MonoBehaviour
{
    public int maxMana = 50;
    [SerializeField] private float manaRegenRate = 2f;
    [SerializeField] private float regenDelay = 2f;
    
    public int currentMana;
    private Coroutine manaRegenCoroutine;

    public UnityEvent<int, int> OnManaChanged; // Current and max mana

    private void Awake()
    {
        currentMana = maxMana;
    }

    public void UseMana(int amount)
    {
        currentMana = Mathf.Clamp(currentMana - amount, 0, maxMana);
        OnManaChanged?.Invoke(currentMana, maxMana);

        RestartManaRegeneration();
    }

    private void RestartManaRegeneration()
    {
        // Stop the current coroutine if it's running
        if (manaRegenCoroutine != null)
        {
            StopCoroutine(manaRegenCoroutine);
        }

        // Start the coroutine anew
        manaRegenCoroutine = StartCoroutine(RegenerateMana());
    }

    private IEnumerator RegenerateMana()
    {
        yield return new WaitForSeconds(regenDelay);

        while (currentMana < maxMana)
        {
            currentMana += Mathf.FloorToInt(manaRegenRate);
            currentMana = Mathf.Clamp(currentMana, 0, maxMana);
            OnManaChanged?.Invoke(currentMana, maxMana);

            // Wait for 1 second
            yield return new WaitForSeconds(1f);
        }
        
        manaRegenCoroutine = null;
    }

    public void GainMana(int amount)
    {
        currentMana = Mathf.Clamp(currentMana + amount, 0, maxMana);
        OnManaChanged?.Invoke(currentMana, currentMana);
    }

    public void SetMaxMana(int newMaxMana, bool resetMana = true)
    {
        maxMana = newMaxMana;
        if (resetMana)
        {
            currentMana = maxMana;
        }
        OnManaChanged?.Invoke(currentMana, currentMana);
    }

    public bool IsEnoughManaToUse(int amount)
    {
        return currentMana >= amount;
    }
}
