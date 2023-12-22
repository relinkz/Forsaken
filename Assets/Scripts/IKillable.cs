
namespace Game
{
	namespace Entities
	{
		public interface IKillable
		{
			void Kill();
			void DoDamage(int dmg);
		}
	}
}
