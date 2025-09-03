using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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
    public TextMeshProUGUI levelUpText;
    private int currentLevel = 1;
    public Button damageLevelButton;
    public Button healthLevelButton;

    [Header("Attack")]
    public GameObject auraPrefab;
    public ParticleSystem lightningPrefab;
    public GameObject firePrefab;
    public GameObject icePrefab;
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
        levelUpText.text = "Level: " + currentLevel;
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

        EffectDamage auraDamage = aura.GetComponent<EffectDamage>();
        if (auraDamage != null)
        {
            auraDamage.damage = GetComponent<EffectDamage>() != null ? GetComponent<EffectDamage>().damage : auraDamage.damage;
        }

        Destroy(aura, 1f);

        if (currentLevel >= 3 && lightningPrefab != null)
        {
            GameObject lightning = Instantiate(lightningPrefab.gameObject, transform.position, Quaternion.identity);
            EffectDamage lightningDamage = lightning.GetComponent<EffectDamage>();
            if (lightningDamage != null)
                lightningDamage.damage = GetComponent<EffectDamage>() != null ? GetComponent<EffectDamage>().damage : lightningDamage.damage;

            Destroy(lightning, 3f);
        }

        // Level 5 ve üstü: Fire & Ice
        if (currentLevel >= 5)
        {
            if (firePrefab != null)
            {
                GameObject fire = Instantiate(firePrefab, transform.position, Quaternion.identity);
                EffectDamage fireDamage = fire.GetComponent<EffectDamage>();
                if (fireDamage != null)
                    fireDamage.damage = GetComponent<EffectDamage>() != null ? GetComponent<EffectDamage>().damage : fireDamage.damage;
                Destroy(fire, 3f);
            }

            if (icePrefab != null)
            {
                GameObject ice = Instantiate(icePrefab, transform.position, Quaternion.identity);
                EffectDamage iceDamage = ice.GetComponent<EffectDamage>();
                if (iceDamage != null)
                    iceDamage.damage = GetComponent<EffectDamage>() != null ? GetComponent<EffectDamage>().damage : iceDamage.damage;
                Destroy(ice, 3f);
            }
        }
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
            psr.flipX = false;
        else if (hinput < 0)
            psr.flipX = true;
    }

    void PositionReset()
    {
        float clampedX = Mathf.Clamp(transform.position.x, xRange, -xRange);
        float clampedY = Mathf.Clamp(transform.position.y, yRange, -yRange);

        transform.position = new Vector2(clampedX, clampedY);
    }



    // ---------------- HEALTH ----------------
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
                restartButton.GetComponent<Button>().onClick.RemoveAllListeners();
                restartButton.GetComponent<Button>().onClick.AddListener(RestartGame);
            }
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }




    // ---------------- XP & LEVEL ----------------
    public void GainXP(int xp)
    {
        currentXP += xp;
        UpdateXPBar();

        if (currentXP >= toLevelUp)
        {
            currentXP = 0;
            LevelUp();
        }
    }

    public void UpdateXPBar()
    {
        float fillValue = (float)currentXP / toLevelUp;
        fullXPBar.fillAmount = fillValue;
        emptyXPBar.fillAmount = 1 - fillValue;
    }

    public void LevelUp()
    {
        currentLevel++;
        toLevelUp += 50;
        currentXP = 0;
        UpdateXPBar();

        levelUpText.text = "Level: " + currentLevel;
        Time.timeScale = 0;

        // Tüm butonları başta kapatalım
        damageLevelButton.gameObject.SetActive(false);
        healthLevelButton.gameObject.SetActive(false);

        damageLevelButton.onClick.RemoveAllListeners();
        healthLevelButton.onClick.RemoveAllListeners();

        switch (currentLevel)
        {
            case 2:
                damageLevelButton.gameObject.SetActive(true);
                healthLevelButton.gameObject.SetActive(true);
                damageLevelButton.GetComponentInChildren<TextMeshProUGUI>().text = "Damage +10";
                healthLevelButton.GetComponentInChildren<TextMeshProUGUI>().text = "Max Health +50";
                damageLevelButton.onClick.AddListener(UpgradeDamage);
                healthLevelButton.onClick.AddListener(UpgradeHealth);
                break;

            case 3:
                damageLevelButton.gameObject.SetActive(true);
                healthLevelButton.gameObject.SetActive(true);
                damageLevelButton.GetComponentInChildren<TextMeshProUGUI>().text = "Move Speed +1";
                healthLevelButton.GetComponentInChildren<TextMeshProUGUI>().text = "Lightning Damage";
                damageLevelButton.onClick.AddListener(UpgradeSpeed);
                healthLevelButton.onClick.AddListener(AddLightningDamage);
                break;

            case 4:
                damageLevelButton.gameObject.SetActive(true);
                healthLevelButton.gameObject.SetActive(true);
                damageLevelButton.GetComponentInChildren<TextMeshProUGUI>().text = "Max Health +50";
                healthLevelButton.GetComponentInChildren<TextMeshProUGUI>().text = "Move Speed +1";
                damageLevelButton.onClick.AddListener(UpgradeHealth);
                healthLevelButton.onClick.AddListener(UpgradeSpeed);
                break;

            case 5:
                damageLevelButton.gameObject.SetActive(true);
                healthLevelButton.gameObject.SetActive(true);
                damageLevelButton.GetComponentInChildren<TextMeshProUGUI>().text = "Fire Damage";
                healthLevelButton.GetComponentInChildren<TextMeshProUGUI>().text = "Ice Damage";
                damageLevelButton.onClick.AddListener(AddFireDamage);
                healthLevelButton.onClick.AddListener(AddIceDamage);
                break;

            default:
                LevelUpComplete();
                break;
        }
    }

    // Yeni metodlar
    public void UpgradeSpeed()
    {
        moveSpeed += 1f;
        CloseLevelUpUI();
    }

    public void AddLightningDamage()
    {
        ParticleSystem lightning = Instantiate(lightningPrefab, transform.position, Quaternion.identity);
        Destroy(lightning.gameObject, 1f);
        CloseLevelUpUI();
    }

    public void AddFireDamage()
    {
        GameObject fire = Instantiate(firePrefab, transform.position, Quaternion.identity);
        Destroy(fire, 1f);
        CloseLevelUpUI();
    }

    public void AddIceDamage()
    {
        GameObject ice = Instantiate(icePrefab, transform.position, Quaternion.identity);
        Destroy(ice, 1f);
        CloseLevelUpUI();
    }

    private void CloseLevelUpUI()
    {
        damageLevelButton.gameObject.SetActive(false);
        healthLevelButton.gameObject.SetActive(false);
        LevelUpComplete();
    }


    public void LevelUpComplete()
    {
        Time.timeScale = 1;
    }

    public void UpgradeDamage()
    {
        EffectDamage effectDamage = GetComponent<EffectDamage>();
        if (effectDamage != null)
        {
            effectDamage.damage += 10;
        }

        damageLevelButton.gameObject.SetActive(false);
        healthLevelButton.gameObject.SetActive(false);
        LevelUpComplete();
    }

    public void UpgradeHealth()
    {
        maxHealth += 50;
        currentHealth = maxHealth;
        UpdateHealthBar();
        damageLevelButton.gameObject.SetActive(false);
        healthLevelButton.gameObject.SetActive(false);
        LevelUpComplete();
    }
}