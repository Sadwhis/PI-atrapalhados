using UnityEngine;

public class GroundPool : ObjectPool
{
    public static GroundPool _groundPool;

    public override void Awake()
    {
        base.Awake();
        _groundPool = this;
        base.amountToPool = _fase1GameControl._groundNumber;
    }
}
