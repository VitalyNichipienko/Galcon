using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    #region Fields

    [SerializeField] protected PlanetState planetState;
    [SerializeField] protected ObjectPooler.Pool.ObjectType shipsType;

    [SerializeField] protected List<Planet> capturedPlanets;
    [SerializeField] protected List<Planet> selectedPlanets;

    protected Planet targetPlanet;

    #endregion


    #region Methods

    protected void SendShips(Planet currentPlanet, Planet targetPlanet)
    {
        int countShipsSent = currentPlanet.countShips / 2;

        currentPlanet.countShips -= countShipsSent;

        for (int i = 0; i < countShipsSent; i++)
        {
            GameObject ship = ObjectPooler.Instance.SpawnFromPool(shipsType);
            ship.transform.position = new Vector3(currentPlanet.transform.position.x + Random.Range(-0.5f, 0.5f),
                                                  currentPlanet.transform.position.y + Random.Range(-0.5f, 0.5f),
                                                  currentPlanet.transform.position.z);
            ship.GetComponent<Ship>().MoveToPlanet(targetPlanet);
        }

        if (targetPlanet.CurrentState != planetState)
        {
            targetPlanet.countShips -= countShipsSent;

            if (targetPlanet.countShips <= 0)
            {
                targetPlanet.SetState(planetState);
                targetPlanet.countShips *= -1;

                if (targetPlanet.CurrentState == targetPlanet.PlayerPlanetState)
                {
                    FindObjectOfType<OpponentController>().capturedPlanets.Remove(targetPlanet);
                }
                if (targetPlanet.CurrentState == targetPlanet.EnemyPlanetState)
                {
                    FindObjectOfType<PlayerController>().capturedPlanets.Remove(targetPlanet);
                }


                capturedPlanets.Add(targetPlanet);
            }
        }
        else
        {
            targetPlanet.countShips += countShipsSent;
        }
    }

    #endregion
}
