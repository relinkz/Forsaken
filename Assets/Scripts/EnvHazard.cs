using Game.Entities;
using UnityEngine;

public class EnvHazard : MonoBehaviour
{
	[SerializeField] int m_damage = -1;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var player = collision.gameObject.GetComponent<IPlayer>();
		if (player != null)
		{
			player.HurtPlayer(m_damage, Vector2.zero);
		}
	}
}
