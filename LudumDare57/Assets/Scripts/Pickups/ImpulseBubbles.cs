using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpulseBubbles : MonoBehaviour
{
    [SerializeField] private List<GameObject> _bubbles;
    [SerializeField] private float _firtsImpulse = 10f;
    [SerializeField] private float _impulse = 2.5f;

    private bool _hasJustEntered = false;

    private void Start()
    {
        ActivateBubbles();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController player))
        {
            player.SetExternalForce(transform.up * _impulse);
            if (!_hasJustEntered)
            {
                player.SetExternalForce(transform.up * _firtsImpulse);
                StartCoroutine(ResetEnterFlag());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController player))
        {
            player.ClearExternalForce();
        }
    }

    private IEnumerator ResetEnterFlag()
    {
        _hasJustEntered = true;
        yield return new WaitForSeconds(1f);
        _hasJustEntered = false;
    }

    private void ActivateBubbles()
    {
        StartCoroutine(HandleBubbles(true));
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
