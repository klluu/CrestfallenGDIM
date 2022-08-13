using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BranchStoryUI : MonoBehaviour
{
    public void crypt()
    {
        SceneManager.LoadSceneAsync("Crypt");
    }

    public void boss()
    {
        SceneManager.LoadSceneAsync("BossLevel");
    }

    public void branch()
    {
        SceneManager.LoadSceneAsync("Branch");
    }
}
