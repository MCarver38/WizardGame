using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    [SerializeField] private Mana manaComponent;
    [SerializeField] private Slider manaSlider;

    private void OnEnable()
    {
        manaComponent.OnManaChanged.AddListener(UpdateManaBar);
    }

    private void OnDisable()
    {
        manaComponent.OnManaChanged.RemoveListener(UpdateManaBar);
    }

    private void UpdateManaBar(int currentMana, int maxMana)
    {
        manaSlider.maxValue = maxMana;
        manaSlider.value = currentMana;
    }
}
