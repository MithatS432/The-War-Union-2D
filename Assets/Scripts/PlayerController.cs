using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator panim;
    private SpriteRenderer psr;

    public float moveSpeed;
    public float hinput;
    public float vinput;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        panim = GetComponent<Animator>();
        psr = GetComponent<SpriteRenderer>();
    }
    private void FixedUpdate()
    {
        hinput = Input.GetAxis("Horizontal");
        vinput = Input.GetAxis("Vertical");
        rb.MovePosition(rb.position + new Vector2(hinput, vinput) * moveSpeed * Time.fixedDeltaTime);
        panim.SetFloat("Speed", Mathf.Abs(hinput) + Mathf.Abs(vinput));
        if (hinput > 0)
        {
            psr.flipX = false;
        }
        else if (hinput < 0)
        {
            psr.flipX = true;
        }
    }
}
