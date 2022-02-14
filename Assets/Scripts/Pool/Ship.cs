using System.Collections;
using UnityEngine;


namespace Galcon
{
	public class Ship : MonoBehaviour
	{
		#region Fields

		[SerializeField] private float speed = default;
		[SerializeField] private ObjectPooler.Pool.ObjectType type = default;

		#endregion



		#region Methods

		public IEnumerator MoveToPlanet(Planet targetPlanet)
		{
			while (transform.position != targetPlanet.transform.position)
			{
				yield return null;

				transform.position = Vector2.MoveTowards(transform.position,
					targetPlanet.transform.position, speed * Time.deltaTime);
			}

			ObjectPooler.Instance.ReturnToPool(type, gameObject);
		}

		#endregion
	}
}
