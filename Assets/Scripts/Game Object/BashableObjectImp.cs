using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BashableObjectImp : MonoBehaviour, BashableObject
{
    [SerializeField] private float objectMass = 1.0f;
    [SerializeField] private Color colorLit = Color.yellow;

    public Transform Transform { get; set; }
    public float Mass { get; set;}

    private Color originColor;

    private void Awake() 
    {
        Transform = this.transform;  
        Mass = objectMass;
        originColor = GetComponent<SpriteRenderer>().color;
    }
    
    public void Lit()
    {
        transform.gameObject.GetComponent<SpriteRenderer>().color = colorLit;
    }
    
    public void Unlit()
    {
        transform.gameObject.GetComponent<SpriteRenderer>().color = originColor;
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
