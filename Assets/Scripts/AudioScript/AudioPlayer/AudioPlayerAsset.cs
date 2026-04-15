using UnityEngine;

[CreateAssetMenu(fileName = "Som Player", menuName = "SomPlayer")]
public class SomInimigo : ScriptableObject
{
    public AudioClip hit;
    public AudioClip morte;
    public AudioClip explo;
}
