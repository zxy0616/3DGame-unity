using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class SSAction : ScriptableObject
{

    public bool enable = true;
    public bool destroy = false;

    public GameObject gameObject;
    public Transform transform;
    public SSActionCallback CallBack;

    public virtual void Start()
    {
        throw new System.NotImplementedException();
    }

    public virtual void Update()
    {
        throw new System.NotImplementedException();
    }
}