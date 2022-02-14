using UnityEngine;


namespace Galcon
{
	[CreateAssetMenu]
	public class EnemyPlanetState : PlanetState
	{
		public override void Init()
		{
			planet.GetComponent<Renderer>().material.color = new Color(255, 0, 0);
		}
	}
}
