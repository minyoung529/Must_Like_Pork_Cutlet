using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool shuttingDown = false;
    private static object locker = new object();
    private static T instance = null;
    public T Instance
    {
        get
        {
            if (shuttingDown)
            {
                Debug.LogWarning("[Singleton]" + typeof(T) + "is already destroyed. returning null.");
                return null;
            }

            lock (locker)
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    if (instance == null)
                    {
                        instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
                        DontDestroyOnLoad(instance);
                    }
                }
            }

            return instance;
        }
    }

    private void OnApplicationQuit()
    {
        shuttingDown = true;
    }

    private void OnDestroy()
    {
        shuttingDown = true;
    }

    ///
    /// 오늘 점심 리스트
    /// 
    /// 1. 비빔면
    /// 2. 비빔면
    /// 3. 비빔면
    /// 4. 밥...??
    /// 5. 계란
    /// 6. ㅂ비비비ㅣ비비비비ㅣ비비비비ㅣ비ㅣ며면
    ///ㄴ

}