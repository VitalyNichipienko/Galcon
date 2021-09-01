using UnityEngine;

[CreateAssetMenu]
public class PlayerPlanetState : PlanetState
{
    public override void Init()
    {
        planet.GetComponent<Renderer>().material.color = new Color(0, 0, 255);
    }
}
