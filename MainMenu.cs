using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private AudioSource MenuMusic;

    public void Awake()
    {
        MenuMusic.Play();
    }
    public void PlayGame()
    {
        MenuMusic.Stop();
        SceneManager.LoadSceneAsync("CombatScene");
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
