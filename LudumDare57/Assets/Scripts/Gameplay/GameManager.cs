using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MenuDataSource playId;
    [SerializeField] private MenuDataSource exitId;

    public event Action StartGame;
    public event Action EndGame;

    public void HandleMenuChange(string id)
    {
        if (id == playId.name)
            TriggerStartGame();

        if (id == exitId.name)
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
        StartGame?.Invoke();
    }
}
