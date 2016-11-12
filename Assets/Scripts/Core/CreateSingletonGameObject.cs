using UnityEngine;

public class CreateSingletonGameObject<T> : MonoBehaviour where T : CreateSingletonGameObject<T>
{
    private static T s_instance = null;

    public static T Instance
    {
        get
        {
            if (s_instance == null)
            {
                var holder = new GameObject("Singleton_" + typeof(T).ToString());
                s_instance = holder.AddComponent<T>();
            }
            return s_instance;
        }
    }
}
