using System;
using System.Collections;
using UnityEngine;

public class EnemySlide : MonoBehaviour
{
    [SerializeField] private float slideDuration = 1f;
    [SerializeField] private AnimationCurve slideCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

    private Rigidbody2D _rigidbody;
    private Coroutine _slideCoroutine;

    public event Action OnSlideComplete;

    public void Setup (Rigidbody2D rigidbody)
    {
        _rigidbody = rigidbody;
    }

    public void Slide(Vector2 initialVelocity)
    {
        if (_slideCoroutine != null)
            StopCoroutine(_slideCoroutine);

        _slideCoroutine = StartCoroutine(SlideRoutine(initialVelocity));
    }

    private IEnumerator SlideRoutine(Vector2 initialVelocity)
    {
        float elapsed = 0f;

        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            float t = slideCurve.Evaluate(elapsed / slideDuration);
            _rigidbody.velocity = initialVelocity * t;
            yield return null;
        }

        _rigidbody.velocity = Vector2.zero;
        OnSlideComplete?.Invoke();
    }

    public bool IsSliding => _slideCoroutine != null;
}