using UnityEditor;

/// <summary>
/// Persisted (to EditorPrefs) int value for edit mode that survives assembly reloads.
/// Use for making editors not reset when you recompile scripts. 
/// 
/// Uses the edited element's instanceID (or any int, but you know) to match value with object.
/// </summary>
public abstract class PersistedVal<T>
{
    private readonly string key;
    private T cachedVal;

    protected PersistedVal(string key, int instanceID)
    {
        this.key = key + instanceID;
        cachedVal = Get();
    }

    public void SetTo(T value)
    {
        if (ToInt(value) != ToInt(cachedVal))
        {
            EditorPrefs.SetInt(key, ToInt(value));
            cachedVal = value;
        }
    }

    public T Get()
    {
        return ToType(EditorPrefs.GetInt(key, 0));
    }

    public static implicit operator int(PersistedVal<T> p)
    {
        return p.ToInt(p.Get());
    }
    
    protected abstract int ToInt(T val);

    protected abstract T ToType(int i);
}

public class PersistedInt : PersistedVal<int>
{

    public PersistedInt(string key, int instanceID) : base(key, instanceID)
    { }

    protected override int ToInt(int val)
    {
        return val;
    }

    protected override int ToType(int i)
    {
        return i;
    }
}
