using UnityEngine;

public class GroundPool : ObjectPool
{
    public static GroundPool _groundPool;

    public override void Awake()
    {
        base.Awake();
        base.amountToPool = _gameControl._groundNumber;
        _groundPool = this;
    }
}
