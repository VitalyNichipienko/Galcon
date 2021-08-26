using UnityEngine;

[CreateAssetMenu]
public class NeutralPlanetState : PlanetState
{
    public override void Init()
    {
        planet.countShips = Random.Range(0, 50);
    }
}
