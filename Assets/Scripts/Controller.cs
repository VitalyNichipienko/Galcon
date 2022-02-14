using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Galcon
{
	public abstract class Controller : MonoBehaviour
	{
		#region Fields

		protected const int STARTING_NUMBER_SHIPS = 50;

		[SerializeField] protected PlanetState planetState = default;
		[SerializeField] protected ObjectPooler.Pool.ObjectType shipsType = default;

		[SerializeField] protected List<Planet> capturedPlanets = default;
		[SerializeField] protected List<Planet> selectedPlanets = default;

		protected Planet targetPlanet = default;

		#endregion



		#region Properties

		public List<Planet> SelectedPlanets => selectedPlanets;

		#endregion



		#region Methods

		public IEnumerator CapturePlanet(Planet targetPlanet, Ship ship)
		{
			yield return StartCoroutine(ship.MoveToPlanet(targetPlanet));

			if (targetPlanet.CurrentPlanetState != planetState)
			{
				targetPlanet.CountShips--;
				if (targetPlanet.CountShips <= 0)
				{
					targetPlanet.SetState(planetState);

					foreach (var controller in FindObjectsOfType<Controller>())
					{
						if (controller != this)
						{
							if (targetPlanet.GetComponentInChildren<Circle>())
							{
								targetPlanet.GetComponentInChildren<Circle>().DeHighlightPlanet(targetPlanet);
							}
							controller.capturedPlanets.Remove(targetPlanet);
							controller.selectedPlanets.Remove(targetPlanet);
						}
					}

					capturedPlanets.Add(targetPlanet);
				}
			}
			else
			{
				targetPlanet.CountShips++;
			}
		}


		protected void SendShips(Planet currentPlanet, Planet targetPlanet)
		{
			int countShipsSent = currentPlanet.CountShips / 2;

			currentPlanet.CountShips -= countShipsSent;

			for (int i = 0; i < countShipsSent; i++)
			{
				GameObject ship = ObjectPooler.Instance.SpawnFromPool(shipsType);
				ship.transform.position = new Vector3(
					currentPlanet.transform.position.x + Random.Range(-currentPlanet.transform.localScale.x / 2, currentPlanet.transform.localScale.x / 2),
					currentPlanet.transform.position.y + Random.Range(-currentPlanet.transform.localScale.y / 2, currentPlanet.transform.localScale.y / 2), currentPlanet.transform.position.z);

				StartCoroutine(CapturePlanet(targetPlanet, ship.GetComponent<Ship>()));
			}
		}

		#endregion
	}
}
