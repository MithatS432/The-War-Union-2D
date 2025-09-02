using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D erb;
    private Animator eanim;
    private AudioSource eAudio;

    public GameObject target;
    public GameObject bloodEffectPrefab;
    public float moveSpeed = 3f;

    public float health = 20f;
    public float damage = 10f;
    public float xpValue = 30f;

    void Start()
    {
        erb = GetComponent<Rigidbody2D>();
        eanim = GetComponent<Animator>();
        eAudio = GetComponent<AudioSource>();
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
            eanim.SetTrigger("Die");
            eAudio.Play();
            GameObject bloodEffect = Instantiate(bloodEffectPrefab, transform.position, Quaternion.identity);
            Destroy(bloodEffect, 2.5f);
            this.enabled = false;
            StartCoroutine(DieAfterDelay(1f));
        }
    }

    private IEnumerator DieAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayerController player = Object.FindAnyObjectByType<PlayerController>();
        if (player != null)
        {
            player.GainXP((int)xpValue);
        }
        Destroy(gameObject);
    }

}
