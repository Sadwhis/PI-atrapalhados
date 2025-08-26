using JetBrains.Annotations;
using UnityEngine;

public class InimigoPool : ObjectPool
{
    public static InimigoPool _InimigoPool;

    protected override void Awake()
    {
        base.Awake();
        _InimigoPool = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
