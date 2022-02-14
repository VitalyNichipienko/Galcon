using UnityEngine;


namespace Galcon
{
	public abstract class PlanetState : ScriptableObject
	{
		public Planet planet;


		public abstract void Init();
	}
}
