using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Planet : MonoBehaviour, IPointerClickHandler
{
    #region Fields

    public int countShips = 0;
    [Space]
    [SerializeField] private PlanetState NeutralPlanetState;
    [SerializeField] private PlanetState PlayerPlanetState;
    [SerializeField] private PlanetState EnemyPlanetState;
    private PlanetState CurrentState;
    [Space]
    [SerializeField] private float shipSpawnCooldown = 1.0f;
    [SerializeField] private int countShipsSpawnPerSecond = 5;
    private bool spawnShipsIsStarted = false;

    private Text countShipsText;
        
    private Circle circleHighlightingPlanet;

    private PlayerController playerController;

    #endregion



    #region Properties

    public Circle CircleHighlightingPlanet => circleHighlightingPlanet;

    public PlanetState CurrentPlanetState => CurrentState;

    #endregion



    #region Methods

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


    private IEnumerator SpawnShips()
    {
        spawnShipsIsStarted = true;

        while (true)
        {
            yield return new WaitForSeconds(shipSpawnCooldown);

            countShips += countShipsSpawnPerSecond;
        }        
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (CurrentState == PlayerPlanetState)
        {
            if (!playerController.SelectedPlanets.Contains(this))
            {
                GameObject obj = ObjectPooler.Instance.SpawnFromPool(ObjectPooler.Pool.ObjectType.Circle);
                circleHighlightingPlanet = obj.GetComponent<Circle>();
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

    #endregion
}
