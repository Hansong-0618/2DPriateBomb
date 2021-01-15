using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private PlayerController player;
    private Door doorexit;

    public bool gameOver;

    public List<Enemy> enemies = new List<Enemy>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        //player = FindObjectOfType<PlayerController>();
        //doorexit = FindObjectOfType<Door>();
    }
    public void Update()
    {
        if(player != null)
            gameOver = player.isDead;
        UIManager.instance.GameOverUI(gameOver);
    }
    public void EnterNewGame()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(1);
    }
    public void ContinueGame()
    {
        if (PlayerPrefs.HasKey("sceneIndex"))
            SceneManager.LoadScene(PlayerPrefs.GetInt("sceneIndex"));
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        PlayerPrefs.DeleteKey("playerHealth");
    }
    public void GameToNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void IsEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }
    public void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
        if (enemies.Count == 0)
        {
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                UIManager.instance.WinMenu.SetActive(true);
                return;
            }
            doorexit.OpenDoor();
            SaveData();
        }
    }
    public void IsPlayer(PlayerController playerController)
    {
        player = playerController;
    }
    public void IsExitDoor(Door door)
    {
        doorexit = door;
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public float LoadHealth()
    {
        if (!PlayerPrefs.HasKey("playerHealth"))
        {
            PlayerPrefs.SetFloat("playerHealth", 3f);
        }

        float currentHealth = PlayerPrefs.GetFloat("playerHealth");

        return currentHealth;
    }
    public void SaveData()
    {
        PlayerPrefs.SetFloat("playerHealth", player.health);
        PlayerPrefs.SetInt("sceneIndex", SceneManager.GetActiveScene().buildIndex + 1);
        PlayerPrefs.Save();
    }
}
