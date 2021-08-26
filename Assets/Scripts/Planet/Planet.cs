using UnityEngine;
using UnityEngine.UI;

public class Planet : MonoBehaviour
{
    #region Fields

    public int countShips = 0;

    public PlanetState NeutralPlanetState;
    public PlanetState PlayerPlanetState;
    public PlanetState EnemyPlanetState;

    public PlanetState CurrentState;

    public float seconds = 0;

    private Text countShipsText;

    #endregion


    #region Methods

    private void Start()
    {
        SetState(NeutralPlanetState);

        countShipsText = GetComponentInChildren<Text>();
    }


    private void FixedUpdate()
    {
        CurrentState.RespawnShips();
        countShipsText.text = countShips.ToString();
    }


    public void SetState(PlanetState state)
    {
        CurrentState = state;
        CurrentState.planet = this;
        CurrentState.Init();
    }

    #endregion
}
