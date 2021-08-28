using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Fields

    [SerializeField] private List<Planet> selectedPlanets;
    [SerializeField] private Planet targetPlanet;

    public List<GameObject> playerPlanets;


    #endregion



    #region Methods

    private void Start()
    {
        playerPlanets.Add(FindObjectsOfType<Planet>()[0].gameObject);
        playerPlanets[0].GetComponent<Planet>().SetState(playerPlanets[0].GetComponent<Planet>().PlayerPlanetState);
        playerPlanets[0].GetComponent<Planet>().countShips = 50;
    }


    private void Update()
    {
        SelectionPlanet();
    }


    private void SelectionPlanet()
    {
        //        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        //        {
        //            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        //        }
        //#if UNITY_EDITOR || UNITY_STANDALONE
        //        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //#else
        //                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        //                {
        //                    ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        //                }
        //#endif

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.GetComponent<Planet>())
        {
            Planet newSelectablePlanet = hit.collider.gameObject.GetComponent<Planet>();

            if (newSelectablePlanet.CurrentState == newSelectablePlanet.PlayerPlanetState)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) || Input.touchCount > 0)
                {
                    if (!selectedPlanets.Contains(newSelectablePlanet))
                    {
                        HighlightPlanet(newSelectablePlanet);
                        selectedPlanets.Add(newSelectablePlanet);
                    }
                    else
                    {
                        DeHighlightPlanet(newSelectablePlanet);
                        selectedPlanets.Remove(newSelectablePlanet);
                    }
                }
            }

            if (newSelectablePlanet.CurrentState != newSelectablePlanet.PlayerPlanetState && selectedPlanets.Count != 0)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) || Input.touchCount > 0)
                {
                    targetPlanet = newSelectablePlanet;

                    foreach (var planet in selectedPlanets)
                    {
                        SendShips(planet, targetPlanet);
                    }

                    selectedPlanets.Clear();
                    targetPlanet = null;
                }
            }
        }
    }


    private void HighlightPlanet(Planet planet)
    {
        GameObject cirlce = ObjectPooler.Instance.SpawnFromPool(ObjectPooler.Pool.ObjectType.Circle);

        cirlce.transform.position = planet.transform.position;
        cirlce.transform.SetParent(planet.transform);
    }


    private void DeHighlightPlanet(Planet planet)
    {
        ObjectPooler.Instance.ReturnToPool(ObjectPooler.Pool.ObjectType.Circle, planet.GetComponentInChildren<Circle>().gameObject);
    }


    private void SendShips(Planet currentPlanet, Planet targetPlanet)
    {
        GameObject line = ObjectPooler.Instance.SpawnFromPool(ObjectPooler.Pool.ObjectType.Line);
        line.GetComponent<Line>().DrawLine(currentPlanet, targetPlanet);

        DeHighlightPlanet(currentPlanet);

        int countShipsSent = currentPlanet.countShips / 2;
        
        currentPlanet.countShips -= countShipsSent;

        for (int i = 0; i < countShipsSent; i++)
        {
            GameObject ship = ObjectPooler.Instance.SpawnFromPool(ObjectPooler.Pool.ObjectType.PlayerShip);
            ship.transform.position = currentPlanet.transform.position;
            ship.GetComponent<Ship>().MoveToPlanet(targetPlanet);
        }

        if (targetPlanet.CurrentState != targetPlanet.PlayerPlanetState)
        {
            targetPlanet.countShips -= countShipsSent;

            if (targetPlanet.countShips <= 0)
            {
                targetPlanet.SetState(targetPlanet.PlayerPlanetState);                
                targetPlanet.countShips *= -1;
                playerPlanets.Add(targetPlanet.gameObject);
            }
        }
        else
        {
            targetPlanet.countShips += countShipsSent;
        }
    }

#endregion
}