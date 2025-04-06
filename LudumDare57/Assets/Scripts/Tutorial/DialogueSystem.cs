using Audio;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct DialogueStruct
{
    public string character;
    public string message;
}

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private InputHandler _input;
    [SerializeField] private GameObject _ducky;
    [SerializeField] private GameObject _mermaid;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private DialogueData _dialogue;
    [SerializeField] private float _textSpeed = 0.05f;

    [SerializeField] private AudioDataSource audioManager;
    [SerializeField] private AudioConfig[] mermaidVoices;
    [SerializeField] private AudioConfig[] duckVoices;

    private int _currentLineIndex = 0;
    private bool _isTyping = false;
    private Coroutine _typingCoroutine;

    public Action DialogueEnd;

    private void OnEnable()
    {
        _currentLineIndex = 0;
        ShowNextLine();

        _input.OnClickEvent += OnUserClick;
    }

    private void OnDisable()
    {
        _input.OnClickEvent -= OnUserClick;
    }

    [ContextMenu("Next")]
    public void OnUserClick()
    {
        if (_isTyping)
        {
            StopCoroutine(_typingCoroutine);
            _dialogueText.text = _dialogue.Dialogue[_currentLineIndex].message;
            _isTyping = false;
        }
        else
        {
            _currentLineIndex++;
            if (_currentLineIndex < _dialogue.Dialogue.Count)
            {
                ShowNextLine();
            }
            else
            {
                DialogueEnd?.Invoke();
            }
        }
    }

    private void ShowNextLine()
    {
        if (_dialogue.Dialogue[_currentLineIndex].character == "Ducky")
        {
            _typingCoroutine = StartCoroutine(TypeText(_dialogue.Dialogue[_currentLineIndex].message, "Ducky"));

            _ducky.SetActive(true);
            _mermaid.SetActive(false);
        }
        else
        {
            _typingCoroutine = StartCoroutine(TypeText(_dialogue.Dialogue[_currentLineIndex].message, "Mermaid"));

            _ducky.SetActive(false);
            _mermaid.SetActive(true);
        }
    }

    private IEnumerator TypeText(string line, string character)
    {
        _dialogueText.text = "";
        _isTyping = true;

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (i % 2 == 0)
            {
                if (audioManager.DataInstance != null)
                {
                    AudioConfig voiceToPlay = null;

                    if (character == "Ducky" && duckVoices.Length > 0)
                    {
                        int randomIndex = Random.Range(0, duckVoices.Length);
                        voiceToPlay = duckVoices[randomIndex];
                    }
                    else if (character != "Ducky" && mermaidVoices.Length > 0)
                    {
                        int randomIndex = Random.Range(0, mermaidVoices.Length);
                        voiceToPlay = mermaidVoices[randomIndex];
                    }

                    if (voiceToPlay != null)
                    {
                        audioManager.DataInstance.PlayAudio(voiceToPlay);
                    }
                }
            }

            _dialogueText.text += c;
            yield return new WaitForSeconds(_textSpeed);
        }

        _isTyping = false;
    }


}
