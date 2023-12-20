using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D m_rigidbody = null;
    Animator m_animator = null;
    SpriteRenderer m_spriteRenderer = null;
    BoxCollider2D m_boxCollider = null;
    
    [SerializeField] Vector2 m_appliedMoveForce;
    [SerializeField] Vector2 m_appliedJumpForce;
    [SerializeField] float m_maxSpeed;
    [SerializeField] float m_inAirAcc;

    bool m_inAir;
    [SerializeField] bool m_leftAnim;
	// Start is called before the first frame update
	void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_boxCollider = GetComponent<BoxCollider2D>();
    }

    enum Direction
    {
        Left, Right
    }
    void HandleHorizonalMovement(Direction dir)
    {
		var velocity = m_rigidbody.velocity;

        float dirMod = (dir == Direction.Right) ? 1 : -1;
        float inAirSlowModifier = (m_inAir == false) ? 1.0f : 0.25f;

        m_inAir = false;

		if (velocity.magnitude < m_maxSpeed)
		{
			m_rigidbody.AddRelativeForce(m_appliedMoveForce * dirMod * inAirSlowModifier);
		}

		var vel = m_rigidbody.velocity;
        if (dir == Direction.Left && vel.x < 0.0f )
        {
            SwapSprite(dir);

		}

		if (dir == Direction.Right && vel.x > 0.0f)
		{
			SwapSprite(dir);
		}
	}

    void SwapSprite(Direction dir)
    {
		if (dir == Direction.Left)
		{
			m_boxCollider.offset = new Vector2(0.09f, m_boxCollider.offset.y);
            m_leftAnim = true;
		}
		else
		{
			m_boxCollider.offset = new Vector2(-0.08f, m_boxCollider.offset.y);
            m_leftAnim = false;
		}
	}

    void TryJump()
    {
        if (m_inAir) 
        { 
            return; 
        }

        m_rigidbody.AddRelativeForce(m_appliedJumpForce);
        m_animator.SetTrigger("jump");

        m_inAir = true;
        m_animator.SetBool("inAir", m_inAir);
    }

    void HandleInput()
    {
        if (Input.GetKey(KeyCode.D))
        {
            HandleHorizonalMovement(Direction.Right);
		}
        else if (Input.GetKey(KeyCode.A))
        {
            HandleHorizonalMovement(Direction.Left);
		}

        if (Input.GetKeyDown(KeyCode.W))
        {
            TryJump();
        }
	}
    // Update is called once per frame
    void Update()
    {
        HandleInput();

		m_animator.SetFloat("playerSpeed", Mathf.Abs(m_rigidbody.velocity.x));

		m_spriteRenderer.flipX = m_leftAnim;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		m_inAir = false;
		m_animator.SetBool("inAir", m_inAir);
	}
}
