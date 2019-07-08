// このソースコードは私が書いたものではありません

using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    public bool IsInit { get; private set; }

    protected SingletonMonoBehaviour()
    {
        this.IsInit = false;
    }

    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }

            instance = FindObjectOfType(typeof(T)) as T;

            if (instance == null)
            {
                Debug.LogError("<color=#f00>" + typeof(T) + "</color> is nothing");
                Debug.Break();
            }

            if (instance != null && !instance.IsInit)
            {
                instance.Init();
            }

            return instance;
        }
    }

    /// <summary>
    /// 非明示的なoverrideをしないで!!
    /// </summary>
    private void Awake()
    {
        this.Init();
    }

    /// <summary>
    /// 初期化関数
    /// </summary>
    private void Init()
    {
        if (this == Instance)
        {
            if (this.IsInit) { return; }

            this.OnInit();
            this.IsInit = true;

            return;
        }

		Destroy(this);

		Debug.LogError("<color=#f00>" + typeof(T) + "</color> is duplicated");
        Debug.Break();
    }
    
    protected abstract void OnInit();
}