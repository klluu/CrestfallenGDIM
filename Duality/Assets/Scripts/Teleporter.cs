using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    [SerializeField] string scenenName;
    private bool teleported = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !teleported)
        {
            teleported = true;

            SceneManager.LoadSceneAsync(scenenName);
        }
    }
}
