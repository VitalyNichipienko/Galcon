using UnityEngine;

[CreateAssetMenu]
public class EnemyPlanetState : PlanetState
{
    public override void Init()
    {
        planet.GetComponent<Renderer>().material.color = new Color(255, 0, 0);
    }


    public override void RespawnShips()
    {
        planet.seconds += 0.02f;

        if (planet.seconds >= 1)
        {
            planet.seconds = 0;
            planet.countShips += 5;
        }
    }
}
