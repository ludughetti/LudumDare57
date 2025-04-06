using UnityEngine;
using TMPro;

public class EndgameController : MonoBehaviour
{
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private TMP_Text _resultText;
    [SerializeField] private string _winMessage = "YOU WIN!";
    [SerializeField] private string _loseMessage = "GAME OVER";

    private void OnEnable()
    {
        _levelManager.triggerEndgame += HandleEndgame;
    }

    private void OnDisable()
    {
        _levelManager.triggerEndgame -= HandleEndgame;
    }

    private void HandleEndgame(bool isWin)
    {
        if (isWin)
            _resultText.SetText(_winMessage);
        else
            _resultText.SetText(_loseMessage);
    }
}
