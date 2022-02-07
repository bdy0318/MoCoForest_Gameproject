using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    // 생성 아이템 용 dontdestroyonload
    #region Singleton
    public static DontDestroyOnLoad instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    #endregion Singleton
}
