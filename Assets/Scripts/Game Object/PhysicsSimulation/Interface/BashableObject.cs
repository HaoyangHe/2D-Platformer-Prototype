using UnityEngine;

public interface BashableObject
{
    public Transform Transform { get; set; }
    public float Mass { get; set;}
    public void Lit();
    public void Unlit();
    public void BeforeBash();
    public void SetImpulse(Vector2 impulse);
    public void AfterBash();
}
