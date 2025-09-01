using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D erb;

    public GameObject target;
    public float moveSpeed = 3f;

    public float health = 20f;
    public float damage = 10f;
    public float xpValue = 30f;

    void Start()
    {
        erb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (target == null) return;

        Vector2 direction = (target.transform.position - transform.position).normalized;
        erb.linearVelocity = direction * moveSpeed;
        if (direction.x != 0)
        {
            Vector3 localScale = transform.localScale;
            localScale.x = Mathf.Sign(direction.x) * Mathf.Abs(localScale.x);
            transform.localScale = localScale;
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = Object.FindAnyObjectByType<PlayerController>();
            if (player != null)
            {
                player.GetDamage((int)damage);
                Destroy(gameObject);
            }
        }
    }
    public void GetDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
            PlayerController player = Object.FindAnyObjectByType<PlayerController>();
            if (player != null)
            {
                player.GainXP((int)xpValue);
            }
        }
    }
}
