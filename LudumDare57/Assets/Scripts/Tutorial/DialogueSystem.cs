using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        _typingCoroutine = StartCoroutine(TypeText(_dialogue.Dialogue[_currentLineIndex].message));

        if (_dialogue.Dialogue[_currentLineIndex].character == "Ducky")
        {
            _ducky.SetActive(true);
            _mermaid.SetActive(false);
        }
        else
        {
            _ducky.SetActive(false);
            _mermaid.SetActive(true);
        }
    }

    private IEnumerator TypeText(string line)
    {
        _dialogueText.text = "";
        _isTyping = true;

        foreach (char c in line)
        {
            _dialogueText.text += c;
            yield return new WaitForSeconds(_textSpeed);
        }

        _isTyping = false;
    }
}
