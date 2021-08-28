using UnityEngine;

public class PlayerController : Controller
{
    #region Methods

    private void Start()
    {
        capturedPlanets.Add(FindObjectsOfType<Planet>()[0]);
        capturedPlanets[0].SetState(planetState);
        capturedPlanets[0].countShips = 50;
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

            if (newSelectablePlanet.CurrentState == planetState)
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

            if (newSelectablePlanet.CurrentState != planetState && selectedPlanets.Count != 0)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) || Input.touchCount > 0)
                {
                    targetPlanet = newSelectablePlanet;

                    foreach (var planet in selectedPlanets)
                    {
                        DeHighlightPlanet(planet);
                        HighlightPath(planet, targetPlanet);
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


    private void HighlightPath(Planet currentPlanet, Planet targetPlanet)
    {
        GameObject line = ObjectPooler.Instance.SpawnFromPool(ObjectPooler.Pool.ObjectType.Line);
        line.GetComponent<Line>().DrawLine(currentPlanet, targetPlanet);
    }

    #endregion
}