using System.Collections;
using UnityEngine;

public class Line : MonoBehaviour
{
    #region Fields

    public Vector3 offset;

    #endregion


    #region Methods

    public void DrawLine(Planet currentPlanet, Planet targetPlanet)
    {
        transform.rotation = Quaternion.identity;
        transform.SetParent(currentPlanet.transform);

        Vector3 aPos;
        Vector3 bPos;

        float rA = currentPlanet.transform.localScale.x / 2;
        float rB = targetPlanet.transform.localScale.x / 2;

        Vector3 direction = (targetPlanet.transform.position - currentPlanet.transform.position).normalized;


        aPos = currentPlanet.transform.position + direction * rA;
        bPos = targetPlanet.transform.position - direction * rB;

        float distance = Vector3.Distance(aPos, bPos);


        Vector3 middlePoint = aPos + direction * distance / 2;

        transform.localScale = new Vector3(transform.localScale.x, distance / 2);
        transform.position = middlePoint;

        offset.z = Vector3.SignedAngle(direction, transform.up, Vector3.forward) * -1;

        transform.eulerAngles = direction + offset;

        StartCoroutine(ReturnLineToPool());
    }


    private IEnumerator ReturnLineToPool()
    {
        yield return new WaitForSeconds(1.0f);

        ObjectPooler.Instance.ReturnToPool(ObjectPooler.Pool.ObjectType.Line, gameObject);
    }

    #endregion
}