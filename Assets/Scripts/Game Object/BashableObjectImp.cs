using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BashableObjectImp : MonoBehaviour, BashableObject
{
    [SerializeField] private float objectMass = 1.0f;
    
    public Transform Transform { get; set; }
    public float Mass { get; set;}

    private void Awake() 
    {
        Transform = this.transform;  
        Mass = objectMass;  
    }
    
    public void Lit()
    {
        transform.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
    }
    
    public void Unlit()
    {
        transform.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void BeforeBash()
    {
        transform.localScale *= new Vector2(1.4f, 1.4f);
    }

    public void SetImpulse(Vector2 impulse)
    {
       transform.parent.gameObject.GetComponent<BashableObjectOwner>().OnTriggered(impulse);
    }

    public void AfterBash()
    {
        transform.localScale /= new Vector2(1.4f, 1.4f);
    }

    public Transform GetTranform()
    {
        return transform;
    }
}
