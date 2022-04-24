using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreComponent : MonoBehaviour
{
    protected CoreComponentsManager core;

    protected virtual void Awake()
    {
        core = transform.parent.GetComponent<CoreComponentsManager>();

        if (core == null) { Debug.LogError("There is no CoreComponentsManager on the parent."); }
    }
}
