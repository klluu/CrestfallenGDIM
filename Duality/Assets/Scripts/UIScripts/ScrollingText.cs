using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScrollingText : MonoBehaviour
{
    [SerializeField] float endPos = 1600;
    [SerializeField] int mode = 0;
    [SerializeField] GameObject buttons;
    [SerializeField] float time = 15;
    float accTime = 0;
    private Vector3 start;
    private bool _stop = false;
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        start = rectTransform.anchoredPosition;
    }

    private void Update()
    {
        if (!_stop)
        {
            accTime += Time.deltaTime;

            rectTransform.anchoredPosition = Vector3.Lerp(start, new Vector3(0, endPos, 0), accTime / time);

            if (accTime > time)
            {
                _stop = true;

                if (mode == 0)
                {
                    SceneManager.LoadSceneAsync("Level1");
                }
                else
                {
                    buttons.SetActive(true);
                }
            }
        }
    }
}
