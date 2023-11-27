#nullable enable
using UnityEngine;
using UnityEngine.SceneManagement;

namespace assets.code
{
    public class LogicScript : MonoBehaviour
    {
        public GameObject? gameOverScreen;

        public void restartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void gameOver()
        {
            if (gameOverScreen != null)
            {
                gameOverScreen.SetActive(true);
            }
        }
    }
}