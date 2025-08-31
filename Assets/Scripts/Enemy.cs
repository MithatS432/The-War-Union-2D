using UnityEngine;

public class Enemy : MonoBehaviour
{
    void Start()
    {

    }

    void FixedUpdate()
    {

    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = Object.FindAnyObjectByType<PlayerController>();
            if (player != null)
            {
                player.GetDamage(500);
            }
        }
    }
}
