using UnityEngine;

public class Ship : MonoBehaviour
{
    #region Fields       

    public Planet currentPlanet;
    public Planet targetPlanet;

    public bool isMove = false;
    [SerializeField] private float speed;

    #endregion



    #region Methods

    private void FixedUpdate()
    {
        if (isMove == false)
        {
            return;
        }            

        transform.position = Vector2.MoveTowards(transform.position, targetPlanet.transform.position, speed * Time.deltaTime);

        if (transform.position == targetPlanet.transform.position)
        {
            ObjectPooler.Instance.ReturnToPool(ObjectPooler.Pool.ObjectType.PlayerShip, gameObject);
        }
    }


    public void MoveToPlanet(Planet currentPlanet, Planet targetPlanet)
    {
        this.currentPlanet = currentPlanet;
        this.targetPlanet = targetPlanet;

        isMove = true;
    }

    #endregion
}
