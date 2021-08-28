using UnityEngine;

public class Ship : MonoBehaviour
{
    #region Fields       
        
    private bool isMove = false;
    private Planet targetPlanet;
    [SerializeField] private float speed;
    [SerializeField] private ObjectPooler.Pool.ObjectType type;

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
            ObjectPooler.Instance.ReturnToPool(type, gameObject);
        }
    }


    public void MoveToPlanet(Planet targetPlanet)
    {
        this.targetPlanet = targetPlanet;

        isMove = true;
    }

    #endregion
}
