using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private string id;
    private Button button;

    public event Action<string> OnClick;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(HandleButtonClick);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(HandleButtonClick);
    }

    public void Setup(string label, string id, Action<string> onClick)
    {
        _text.SetText(label);
        this.id = id;
        OnClick = onClick;
    }

    private void HandleButtonClick()
    {
        OnClick?.Invoke(id);
    }
}
