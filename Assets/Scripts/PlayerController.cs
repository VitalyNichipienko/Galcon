using System.Collections;
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
        //Ray ray;
        //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        //{
        //    ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        //}

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.GetComponent<Planet>())
        {
            Planet newSelectablePlanet = hit.collider.gameObject.GetComponent<Planet>();

            if (newSelectablePlanet.CurrentState == newSelectablePlanet.PlayerPlanetState)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (!selectedPlanets.Contains(newSelectablePlanet)) // Если планеты нет в списке выделеных
                    {
                        HighlightPlanet(newSelectablePlanet);
                        selectedPlanets.Add(newSelectablePlanet);
                    }
                    else // Если есть в списке выделенных
                    {
                        DeHighlightPlanet(newSelectablePlanet);
                        selectedPlanets.Remove(newSelectablePlanet);
                    }
                }
            }

            if (newSelectablePlanet.CurrentState != newSelectablePlanet.PlayerPlanetState && selectedPlanets.Count != 0)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    targetPlanet = newSelectablePlanet;

                    foreach (var planet in selectedPlanets)
                    {
                        SendShips(planet, targetPlanet);
                    }

                    newSelectablePlanet.SetState(newSelectablePlanet.PlayerPlanetState);
                    playerPlanets.Add(newSelectablePlanet.gameObject);

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


        for (int i = 0; i < currentPlanet.countShips / 2; i++)
        {
            GameObject ship = ObjectPooler.Instance.SpawnFromPool(ObjectPooler.Pool.ObjectType.PlayerShip);
            ship.transform.position = currentPlanet.transform.position;
            ship.GetComponent<Ship>().MoveToPlanet(currentPlanet, targetPlanet);
        }

        currentPlanet.countShips /= 2;
    }

    #endregion
}