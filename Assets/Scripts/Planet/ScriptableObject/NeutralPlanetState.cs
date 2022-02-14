using UnityEngine;


namespace Galcon
{
	[CreateAssetMenu]
	public class NeutralPlanetState : PlanetState
	{
		public override void Init()
		{
			planet.CountShips = Random.Range(0, 50);
		}
	}
}
