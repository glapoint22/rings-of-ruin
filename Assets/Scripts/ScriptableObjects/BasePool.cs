using UnityEngine;

public abstract class BasePool : ScriptableObject
{
    protected Transform poolParent;

    public virtual void Initialize(Transform poolParent)
    {
        this.poolParent = poolParent;
    }
    
    
    public abstract void Return(GameObject instance);
    
    public abstract void Clear();
}