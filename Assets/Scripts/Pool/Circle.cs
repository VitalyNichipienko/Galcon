using UnityEngine;

public class Circle : MonoBehaviour
{
    #region Methods

    public void HighlightPlanet(Planet planet)
    {
        gameObject.transform.position = planet.transform.position;
        gameObject.transform.SetParent(planet.transform);
    }


    public void DeHighlightPlanet(Planet planet)
    {
        ObjectPooler.Instance.ReturnToPool(ObjectPooler.Pool.ObjectType.Circle, planet.GetComponentInChildren<Circle>().gameObject);
    }

    #endregion
}