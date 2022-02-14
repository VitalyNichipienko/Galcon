using UnityEngine;


namespace Galcon
{
	public class PlayerController : Controller
	{
		#region Methods

		public void SelectPlanet(Planet planet, bool isSelect)
		{
			if (isSelect == false)
			{
				selectedPlanets.Add(planet);
			}
			else
			{
				selectedPlanets.Remove(planet);
			}
		}


		public void AttackPlanet(Planet planet)
		{
			targetPlanet = planet;

			foreach (var selectedPlanet in selectedPlanets)
			{
				ObjectPooler.Instance.SpawnFromPool(ObjectPooler.Pool.ObjectType.Line)
							.GetComponent<Line>().DrawLine(selectedPlanet, targetPlanet);

				selectedPlanet.CircleHighlightingPlanet.DeHighlightPlanet(selectedPlanet);

				SendShips(selectedPlanet, targetPlanet);
			}

			selectedPlanets.Clear();
			targetPlanet = null;
		}


		private void Start()
		{
			capturedPlanets.Add(FindObjectsOfType<Planet>()[0]);
			capturedPlanets[0].SetState(planetState);
			capturedPlanets[0].CountShips = 50;
		}

		#endregion
	}
}
