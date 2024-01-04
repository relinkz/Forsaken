
using UnityEngine;

namespace Game
{
	namespace Entities
	{
		public interface IDamageOnTouch
		{
			void ReceiveDamageOnTouch(int dmg);
		}

		public interface IPlayer
		{
			void HurtPlayer(int dmg, Vector2 colDir);
		}
	}
}
