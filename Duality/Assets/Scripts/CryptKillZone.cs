using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryptKillZone : MonoBehaviour
{
    [SerializeField] private GameObject _gameOver;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.SetActive(false);
            _gameOver.SetActive(true);
        }
    }
}
