using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class TitleScreenMusic : MonoBehaviour
{
    public static TitleScreenMusic instance;
 
    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;

            //If we want to keep music playing between scenes
            //DontDestroyOnLoad(this.gameObject);
        }
    }
}