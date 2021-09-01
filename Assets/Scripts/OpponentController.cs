using System.Collections;
using UnityEngine;

public class OpponentController : Controller
{
    #region

    [SerializeField] private float decisionSpeed = 5.0f;

    #endregion



    #region Methods

    private void Start()
    {
        capturedPlanets.Add(FindObjectsOfType<Planet>()[1]);
        capturedPlanets[0].SetState(planetState);
        capturedPlanets[0].countShips = 50;

        StartCoroutine(SelectionPlanet());
    }


    private IEnumerator SelectionPlanet()
    {
        while (true)
        {
            yield return new WaitForSeconds(decisionSpeed);

            if (capturedPlanets.Count == 0 || capturedPlanets.Count >= FindObjectsOfType<Planet>().Length)
            {
                gameObject.SetActive(false);
                break;
            }

            selectedPlanets.Add(capturedPlanets[Random.Range(0, capturedPlanets.Count)]);

            for (int i = 0; i < capturedPlanets.Count; i++)
            {
                float rand = Random.Range(0.0f, 1.0f);
                if (rand >= 0.8 && !selectedPlanets.Contains(capturedPlanets[i]))
                {
                    selectedPlanets.Add(capturedPlanets[i]);
                }
            }

            while (true)
            {
                targetPlanet = FindObjectsOfType<Planet>()[Random.Range(0, FindObjectsOfType<Planet>().Length)];
                if (targetPlanet.CurrentPlanetState != planetState)
                {
                    foreach (var planet in selectedPlanets)
                    {
                        SendShips(planet, targetPlanet);
                    }

                    selectedPlanets.Clear();
                    targetPlanet = null;

                    break;
                }
            }
        }
    }

    #endregion
}
