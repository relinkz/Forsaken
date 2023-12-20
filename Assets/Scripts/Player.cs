using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D m_rigidbody = null;
    Animator m_animator = null;
    bool m_playerLostControl;
    
    [SerializeField] Vector2 m_appliedMoveForce;
    [SerializeField] Vector2 m_appliedJumpForce;
    [SerializeField] float m_maxSpeed;
    [SerializeField] float m_inAirAcc;
    [SerializeField] Transform m_spawnPoint;

    bool m_inAir;
    [SerializeField] bool m_leftAnim;
	// Start is called before the first frame update
	void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_playerLostControl = false;
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
		if (dir == Direction.Left && m_leftAnim == false)
		{
            gameObject.transform.Rotate(Vector3.up, -180);
            m_leftAnim = true;
		}
		else if (dir == Direction.Right && m_leftAnim == true)
		{
			gameObject.transform.Rotate(Vector3.up, 180);
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
        if (m_playerLostControl)
        {
            return;
        }

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
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		m_inAir = false;
		m_animator.SetBool("inAir", m_inAir);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
        StartCoroutine(DisablePlayerAndStartAtRespawn());
	}

    IEnumerator DisablePlayerAndStartAtRespawn()
    {
        gameObject.GetComponent<Rigidbody2D>().Sleep();
		m_playerLostControl = true;

		yield return new WaitForSeconds(2.0f);

        gameObject.transform.position = m_spawnPoint.position;

		gameObject.GetComponent<Rigidbody2D>().WakeUp();

		yield return new WaitForSeconds(1.0f);
        m_playerLostControl = false;
	}
}
