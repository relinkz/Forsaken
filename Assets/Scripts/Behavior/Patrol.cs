using Game.Entities;
using System.Collections;
using UnityEngine;

public class Patrol : MonoBehaviour
{
	[SerializeField] GameObject m_pointA;
	[SerializeField] GameObject m_pointB;
	[SerializeField] float m_speed;
	[SerializeField] float m_trackOffset = 0.1f;
	[SerializeField] float m_idleTimer = 1.0f;
	[SerializeField] int m_playerDamage = 1;

	Rigidbody2D m_rb;
	Animator m_anim;
	Transform m_currentPoint;
	SpriteRenderer m_spriteRenderer;
	bool m_isIdle;

	// Start is called before the first frame update
	void Start()
	{
		m_rb = GetComponent<Rigidbody2D>();
		m_anim = GetComponent<Animator>();
		m_spriteRenderer = GetComponent<SpriteRenderer>();

		m_currentPoint = m_pointA.transform;
		m_anim.SetBool("moving", true);
		m_isIdle = false;
	}

	IEnumerator IdleThenWalkToOtherSpot()
	{
		m_isIdle = true;
		m_anim.SetBool("moving", false);
		yield return new WaitForSeconds(m_idleTimer);
		m_spriteRenderer.flipX = !m_spriteRenderer.flipX;
		m_anim.SetBool("moving", true);
		m_isIdle = false;
	}

	IEnumerator IdleThenContinue()
	{
		m_isIdle = true;
		m_anim.SetBool("moving", false);
		yield return new WaitForSeconds(m_idleTimer);
		m_anim.SetBool("moving", true);
		m_isIdle = false;
	}

	// wait in 3 seconds
	// move to other point
	void ApplyVelocity()
	{
		if (m_currentPoint == m_pointB.transform)
		{
			m_rb.velocity = new Vector2(m_speed, 0);
			
		}
		else
		{
			m_rb.velocity = new Vector2(-m_speed, 0);
		}
	}

	void ChangeTargetMovePosition()
	{
		var dist = Vector2.Distance(transform.position, m_currentPoint.position);
		if (dist < m_trackOffset && m_currentPoint == m_pointB.transform)
		{
			// reach point B
			m_currentPoint = m_pointA.transform;
			StartCoroutine(IdleThenWalkToOtherSpot());
			return;
		}

		if (dist < m_trackOffset && m_currentPoint == m_pointA.transform)
		{
			// reach point A
			m_currentPoint = m_pointB.transform;
			StartCoroutine(IdleThenWalkToOtherSpot());
			return;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (m_isIdle)
		{
			return;
		}

		ApplyVelocity();

		ChangeTargetMovePosition();
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		var player = collision.gameObject.GetComponent<IPlayer>();
		if (player != null)
		{
			Vector2 dir = collision.collider.transform.position - collision.gameObject.transform.position;
			Vector2 knockBackDir = Vector2.zero;
			if (dir.x < 0)
			{
				knockBackDir = Vector2.left + Vector2.up;
			}
			else
			{
				knockBackDir = Vector2.right + Vector2.up;
			}

			player.HurtPlayer(m_playerDamage, knockBackDir.normalized);
			StartCoroutine(IdleThenContinue());
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(m_pointA.transform.position, 0.3f);
		Gizmos.DrawWireSphere(m_pointB.transform.position, 0.3f);
		Gizmos.DrawLine(m_pointA.transform.position, m_pointB.transform.position);
	}
}
