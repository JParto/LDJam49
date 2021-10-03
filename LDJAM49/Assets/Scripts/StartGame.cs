using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PathCreation.Drawables;

public class StartGame : MonoBehaviour
{
    [SerializeField] private DrawablePlatform platform;
    private IEnumerator coroutine;

    void Start(){
        if (platform){
            platform.reachedEndEvent.AddListener(FadeOut);
        } else {
            Debug.LogError("Cannot start the game, no start drawable defined");
        }
    }

    void FadeOut(){
        coroutine = waitSeconds();
        StartCoroutine(coroutine);
    }

    void StartTheGame(){
        SceneManager.LoadScene("MainScene");
        SceneManager.LoadScene("Managers", LoadSceneMode.Additive);
        SceneManager.LoadScene("UI", LoadSceneMode.Additive);
        SceneManager.LoadScene("DarkScene", LoadSceneMode.Additive);
        SceneManager.LoadScene("LightScene", LoadSceneMode.Additive);
        SceneManager.LoadScene("Portals", LoadSceneMode.Additive);
    }

    IEnumerator waitSeconds(){
        yield return new WaitForSeconds(3f);
        StartTheGame();
        
    }
}
