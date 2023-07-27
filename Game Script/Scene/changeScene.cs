using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changeScene : MonoBehaviour
{
    public Animator animator;

    public void FadeOut()
    {
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete(string sceneName)
    {
        // Load the specified scene.
        SceneManager.LoadScene(sceneName);
    }

    public void changeTo(string sceneName) { 
        SceneManager.LoadScene(sceneName);
    }
}
