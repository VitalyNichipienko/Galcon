using System.Collections.Generic;
using UnityEngine;


namespace Galcon
{
	public class PlanetGenerator : MonoBehaviour
	{
		#region Fields

		[SerializeField] private GameObject prefab;

		private int x;
		private int y;
		private int countPlanet = 10;
		private List<GameObject> planets = new List<GameObject>();

		#endregion



		#region Methods

		private void Awake()
		{
			x = Screen.width / 2 / 100 - 1;
			y = Screen.height / 2 / 100 - 1;

			Debug.Log("Width = " + x + "   Height = " + y);

			for (int i = 0; i < countPlanet; i++)
			{
				GameObject newPlanet = Instantiate(prefab, GetPosition(x, y, prefab.transform.localScale.x / 2), Quaternion.identity);

				newPlanet.name = "Planet " + i;

				planets.Add(newPlanet);
			}
		}


		private Vector3 GetPosition(int x, int y, float radius)
		{
			Vector3 position = new Vector3(Random.Range(-x, x), Random.Range(-y, y));
			bool isFreePosition = false;

			if (planets.Count == 0)
			{
				return position;
			}

			while (isFreePosition == false)
			{
				isFreePosition = true;
				position = new Vector3(Random.Range(-x, x), Random.Range(-y, y));

				foreach (var planet in planets)
				{
					if (Vector2.Distance(position, planet.transform.position) <= radius + planet.transform.localScale.x / 2)
					{
						isFreePosition = false;
						break;
					}
				}
			}

			return position;
		}

		#endregion
	}
}
