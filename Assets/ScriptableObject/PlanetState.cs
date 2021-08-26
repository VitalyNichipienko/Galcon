using UnityEngine;

public abstract class PlanetState : ScriptableObject
{
    public Planet planet;

    public abstract void Init();

    public virtual void RespawnShips() { }
}
