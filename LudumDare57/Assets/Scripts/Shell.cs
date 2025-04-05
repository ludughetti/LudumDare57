using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _oxigenIncrementPerSecond = 2f;
    [SerializeField] private float _timeClosed = 1f;
    [SerializeField] private float _timeOpen = 1f;
    [SerializeField] private List<GameObject> _bubbles;

    [SerializeField] private string _isOpenParameter = "IsOpen";

    private bool _isOpen = false;

    private void Start()
    {
        StartCoroutine(WaitingToOpen());
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!_isOpen)
            return;

        if (collision.TryGetComponent(out OxigenLogic player))
            player.AddOxigen(_oxigenIncrementPerSecond * Time.deltaTime);
    }

    private IEnumerator WaitingToOpen()
    {
        yield return new WaitForSeconds(_timeClosed);
        _animator.SetBool(_isOpenParameter,true);
        _isOpen = true;
        ActivateBubbles();
        StartCoroutine(WaitingToClose());
    }

    private IEnumerator WaitingToClose()
    {
        yield return new WaitForSeconds(_timeOpen);
        _animator.SetBool(_isOpenParameter, false);
        _isOpen = false;
        DesactivateBubbles();
        StartCoroutine(WaitingToOpen());
    }

    [ContextMenu("Activate Bubbles")]
    private void ActivateBubbles()
    {
        StartCoroutine(HandleBubbles(true));
    }

    [ContextMenu("Desactivate Bubbles")]
    private void DesactivateBubbles()
    {
        StartCoroutine(HandleBubbles(false));
    }

    private IEnumerator HandleBubbles(bool isActive)
    {
        for (int i = 0; i < _bubbles.Count; i++)
        {
            yield return new WaitForSeconds(.5f);
            _bubbles[i].SetActive(isActive);
        }
    }
}
