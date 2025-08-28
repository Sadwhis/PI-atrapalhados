using JetBrains.Annotations;
using UnityEngine;

public class InimigoPool : ObjectPool
{
    public static InimigoPool _InimigoPool;

    public override void Awake()
    {
        base.Awake();
        _InimigoPool = this;
        _GCMJ1= GameObject.FindWithTag("GameControllerMJ1").GetComponent<GameContolerMJ1>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
