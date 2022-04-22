using UnityEngine;

[CreateAssetMenu(fileName = "New Player Data", menuName = "Data/Presets/Player Data")]
public class PresetPlayerData : ScriptableObject
{
    [Header("Player Movement")]
    public float maxHorizontalSpeed = 10.0f;
    public float jumpVelocity = 10.0f;

    [Header("Player Health")]
    public int maxHealth = 100;

    [Header("Constant")]
    public float MINRECONISEDMOVEMENT = 0.001f;
}
