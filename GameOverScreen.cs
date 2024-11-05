using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField]
    private AudioSource GameOverMusic;

    public void Awake()
    {
        GameOverMusic.Play();
    }
    public void RestartButton()
    {
        GameOverMusic.Stop();
        SceneManager.LoadScene("CombatScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
