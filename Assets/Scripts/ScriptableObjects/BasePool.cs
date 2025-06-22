using UnityEngine;
using System.Collections.Generic;

public abstract class BasePool : ScriptableObject
{
    protected Transform poolParent;

    public virtual void Initialize(Transform poolParent)
    {
        this.poolParent = poolParent;
    }
    
    // public abstract MonoBehaviour Get();
    
    public abstract void Return(MonoBehaviour instance);
    
    public abstract void Clear();
}