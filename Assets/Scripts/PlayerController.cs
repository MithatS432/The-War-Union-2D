using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator panim;
    private SpriteRenderer psr;

    public float moveSpeed;
    private float hinput;
    private float vinput;

    private float xRange = -16.2f, yRange = -16.7f;

    [Header("Health")]
    public int maxHealth = 500;
    public int currentHealth;
    public Image healthBarImage;

    [Header("XP")]
    public int toLevelUp = 100;
    public int currentXP = 0;
    public Image emptyXPBar;
    public Image fullXPBar;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        panim = GetComponent<Animator>();
        psr = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        UpdateHealthBar();
    }
    private void FixedUpdate()
    {
        hinput = Input.GetAxis("Horizontal");
        vinput = Input.GetAxis("Vertical");
        rb.MovePosition(rb.position + new Vector2(hinput, vinput) * moveSpeed * Time.fixedDeltaTime);
        panim.SetFloat("Speed", Mathf.Abs(hinput) + Mathf.Abs(vinput));
        Flip();
        PositionReset();
    }
    void Flip()
    {
        if (hinput > 0)
        {
            psr.flipX = false;
        }
        else if (hinput < 0)
        {
            psr.flipX = true;
        }
    }
    void PositionReset()
    {
        if (transform.position.x < xRange)
        {
            transform.position = new Vector2(xRange, transform.position.y);
        }
        else if (transform.position.x > -xRange)
        {
            transform.position = new Vector2(-xRange, transform.position.y);
        }
        if (transform.position.y < yRange)
        {
            transform.position = new Vector2(transform.position.x, yRange);
        }
        else if (transform.position.y > -yRange)
        {
            transform.position = new Vector2(transform.position.x, -yRange);
        }
    }
    public void UpdateHealthBar()
    {
        float fillValue = (float)currentHealth / maxHealth;
        healthBarImage.fillAmount = fillValue;
    }
    public void GetDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthBar();
    }
    public void GainXP(int xp)
    {
        currentXP += xp;
        if (currentXP >= toLevelUp)
        {
            currentXP = 0;
        }
    }
    public void UpdateXPBar()
    {

    }
}
