using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    // ���� ������ �� dontdestroyonload
    #region Singleton
    public static DontDestroyOnLoad instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    #endregion Singleton
}
