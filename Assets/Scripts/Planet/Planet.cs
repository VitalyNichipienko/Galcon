using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Galcon
{
	public class Planet : MonoBehaviour, IPointerClickHandler
	{
		#region Fields

		[Space]
		[SerializeField] private PlanetState NeutralPlanetState = default;
		[SerializeField] private PlanetState PlayerPlanetState = default;
		[SerializeField] private PlanetState EnemyPlanetState = default;
		private PlanetState CurrentState = default;
		[Space]
		[SerializeField] private float shipSpawnCooldown = 1.0f;
		[SerializeField] private int countShipsSpawnPerSecond = 5;

		private int countShips = 0;
		private bool spawnShipsIsStarted = false;

		private Text countShipsText = default;

		private Circle circleHighlightingPlanet = default;

		private PlayerController playerController = default;

		#endregion



		#region Properties

		public Circle CircleHighlightingPlanet => circleHighlightingPlanet;

		public PlanetState CurrentPlanetState => CurrentState;

		public int CountShips
		{
			get => countShips;
			set => countShips = Mathf.Clamp(value, 0, Int32.MaxValue);
		}

		#endregion



		#region Methods

		public void SetState(PlanetState state)
		{
			CurrentState = state;
			CurrentState.planet = this;
			CurrentState.Init();

			if (state != NeutralPlanetState && spawnShipsIsStarted == false)
			{
				StartCoroutine(SpawnShips());
			}
		}


		public void OnPointerClick(PointerEventData eventData)
		{
			if (CurrentState == PlayerPlanetState)
			{
				if (!playerController.SelectedPlanets.Contains(this))
				{
					circleHighlightingPlanet = ObjectPooler.Instance.SpawnFromPool(ObjectPooler.Pool.ObjectType.Circle).GetComponent<Circle>();
					circleHighlightingPlanet.HighlightPlanet(this);

					playerController.SelectPlanet(this, false);
				}
				else
				{
					circleHighlightingPlanet.DeHighlightPlanet(this);
					circleHighlightingPlanet = null;

					playerController.SelectPlanet(this, true);
				}
			}

			if (CurrentState != PlayerPlanetState && playerController.SelectedPlanets.Count != 0)
			{
				playerController.AttackPlanet(this);
			}
		}


		private void Awake()
		{
			SetState(NeutralPlanetState);

			countShipsText = GetComponentInChildren<Text>();
			playerController = FindObjectOfType<PlayerController>();
		}


		private void FixedUpdate()
		{
			countShipsText.text = countShips.ToString();
		}


		private IEnumerator SpawnShips()
		{
			spawnShipsIsStarted = true;

			while (true)
			{
				yield return new WaitForSeconds(shipSpawnCooldown);

				countShips += countShipsSpawnPerSecond;
			}
		}

		#endregion
	}
}
