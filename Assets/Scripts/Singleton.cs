using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    static Singleton instance;

    public Singleton GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        ManageSingleton();
    }

    private void ManageSingleton()
    {


        if (instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
