using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D m_rigidbody = null;
    
    [SerializeField] Vector2 m_appliedMoveForce;
    [SerializeField] Vector2 m_appliedJumpForce;

    [SerializeField] float m_maxSpeed;
	// Start is called before the first frame update
	void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    enum Direction
    {
        Left, Right
    }
    void HandleHorizonalMovement(Direction dir)
    {
        Debug.Log(dir);
		var velocity = m_rigidbody.velocity;
        float dirMod = (dir == Direction.Right) ? 1 : -1;
		if (velocity.magnitude < m_maxSpeed)
		{
			m_rigidbody.AddRelativeForce(m_appliedMoveForce * dirMod);
            return;
		}
	}

    void TryJump()
    {
        m_rigidbody.AddRelativeForce(m_appliedJumpForce);
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
	}
}
