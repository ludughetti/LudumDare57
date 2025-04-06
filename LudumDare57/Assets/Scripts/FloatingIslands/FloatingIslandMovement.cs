using UnityEngine;
using System.Collections;

public class FloatingIslandMovement : MonoBehaviour
{
    [SerializeField] private AnimationCurve movementCurve;
    [SerializeField] private float verticalAmplitude = 0.5f;
    [SerializeField] private float horizontalAmplitude = 0.2f;
    [SerializeField] private float movementSpeed = 0.3f;

    private Coroutine _movementCoroutine;

    private void OnEnable()
    {
        _movementCoroutine = StartCoroutine(MoveIslandLoop());
    }

    private void OnDisable()
    {
        if (_movementCoroutine != null)
        {
            StopCoroutine(_movementCoroutine);
        }
    }

    private IEnumerator MoveIslandLoop()
    {
        Vector3 currentBasePosition = transform.position;
        bool goingUp = true;

        float totalDistance = verticalAmplitude;

        while (true)
        {
            float distanceCovered = 0f;
            Vector3 start = currentBasePosition;
            Vector3 end = currentBasePosition + new Vector3(0f, totalDistance * (goingUp ? 1f : -1f), 0f);

            float duration = totalDistance / movementSpeed;

            while (distanceCovered < duration)
            {
                distanceCovered += Time.deltaTime;
                float t = Mathf.Clamp01(distanceCovered / duration);
                float curveValue = movementCurve.Evaluate(t);

                float y = Mathf.LerpUnclamped(start.y, end.y, curveValue);
                float x = currentBasePosition.x + horizontalAmplitude * Mathf.Sin(Time.time * Mathf.PI * 2f / (duration * 2f));

                transform.position = new Vector3(x, y, currentBasePosition.z);
                yield return null;
            }

            goingUp = !goingUp;
            currentBasePosition = end;
        }
    }
}