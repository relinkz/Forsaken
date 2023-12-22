using Game.Entities;
using UnityEngine;

public class EnvHazard : MonoBehaviour
{
	[SerializeField] int m_damage = 0;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (m_damage == 0)
		{
			var killable = collision.gameObject.GetComponent<IKillable>();
			if (killable != null)
			{
				killable.Kill();
			}
		}
	}
}
