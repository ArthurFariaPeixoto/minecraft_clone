

using UnityEngine;

[CreateAssetMenu(fileName = "noiseData", menuName = "ScriptableObjects/noiseData")]
public class NoiseData : ScriptableObject
{
    public float noiseZoom;
    public int octaves;
    public Vector2Int offest;
    public Vector2Int worldOffset;
    public float persistance;
    public float redistributionModifier;
    public float exponent;
}