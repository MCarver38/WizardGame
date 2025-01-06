using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    [SerializeField] private int damageAmount = 10;

    private void OnTriggerEnter(Collider other)
    {
        Health targetHealth = other.gameObject.GetComponent<Health>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(damageAmount);
        }
        
        Debug.Log(other.gameObject.name);
    }
}
