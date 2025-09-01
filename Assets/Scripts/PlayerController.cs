using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator panim;
    private SpriteRenderer psr;
    private AudioSource audioSource;

    public float moveSpeed;
    private float hinput;
    private float vinput;

    private float xRange = -16.2f, yRange = -16.7f;

    [Header("Health")]
    private int maxHealth = 500;
    private int currentHealth;
    public Image healthBarImage;
    public GameObject isDead;
    public GameObject deadText;
    public GameObject restartButton;

    [Header("XP")]
    private int toLevelUp = 100;
    private int currentXP = 0;
    public Image emptyXPBar;
    public Image fullXPBar;

    [Header("Attack")]
    public GameObject auraPrefab;
    private float attackCooldown = 3f;
    private float attackTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        panim = GetComponent<Animator>();
        psr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        UpdateHealthBar();
    }
    private void Update()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackCooldown)
        {
            Attack();
            attackTimer = 0f;
        }
    }
    void Attack()
    {
        GameObject aura = Instantiate(auraPrefab, transform.position, Quaternion.identity);
        Destroy(aura, 1f);
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
        audioSource.Play();
        UpdateHealthBar();
        if (currentHealth <= 0)
        {
            panim.SetTrigger("Die");
            isDead.SetActive(true);
            deadText.SetActive(true);
            restartButton.SetActive(true);
            if (restartButton.GetComponent<Button>() != null)
            {
                restartButton.GetComponent<Button>().onClick.AddListener(RestartGame);
            }
        }
    }
    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }



    public void GainXP(int xp)
    {
        currentXP += xp;
        UpdateXPBar();
        if (currentXP >= toLevelUp)
        {
            currentXP = 0;
        }
    }
    public void UpdateXPBar()
    {
        float fillValue = (float)currentXP / toLevelUp;
        fullXPBar.fillAmount = fillValue;
        emptyXPBar.fillAmount = 1 - fillValue;
    }
}
