using UnityEngine;

public interface BashableObjectOwner
{
    public bool IsTriggered { get; set; }
    public BashableObject bashableObject { get; set; }
    public void OnTriggered(Vector2 impulse);
}
