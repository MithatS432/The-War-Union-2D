using UnityEngine;

public class EffectDamage : MonoBehaviour
{
    private int damage = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().GetDamage(damage);
        }
    }
}
