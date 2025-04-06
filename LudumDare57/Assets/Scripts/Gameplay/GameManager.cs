using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MenuDataSource playId;
    [SerializeField] private MenuDataSource exitId;

    public event Action StartGame;
    public event Action EndGame;

    public void HandlePauseGame(bool isGamePaused)
    {
        if (isGamePaused)
            Time.timeScale = 0f;

        else
            Time.timeScale = 1f;
    }

    public void HandleMenuChange(string id)
    {
        if (id == playId.menuId)
            TriggerStartGame();

        if (id == exitId.menuId)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }

    [ContextMenu("Start")]
    private void TriggerStartGame()
    {
        Debug.Log("StartGame");
        StartGame?.Invoke();
    }
}
