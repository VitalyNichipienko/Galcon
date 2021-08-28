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

            if (capturedPlanets.Count == 0)
            {
                gameObject.SetActive(false);
                break;
            }

            selectedPlanets.Add(capturedPlanets[Random.Range(0, capturedPlanets.Count - 1)]);

            while (true)
            {
                if (capturedPlanets.Count >= FindObjectsOfType<Planet>().Length)
                    break;

                targetPlanet = FindObjectsOfType<Planet>()[Random.Range(0, FindObjectsOfType<Planet>().Length)];
                if (targetPlanet.CurrentState != planetState)
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
