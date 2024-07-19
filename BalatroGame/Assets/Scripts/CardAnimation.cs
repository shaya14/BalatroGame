using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAnimation : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _rotationSpeedZ;
    [SerializeField] private float _duration;

    void Start()
    {
        StartCoroutine(AnimateCard());
    }

    private IEnumerator AnimateCard()
    {
        if (_duration == 0)
        {
            Debug.LogWarning("Duration is set to 0. Animation will be skipped to avoid crashing.");
            yield break;
        }

        while (true)
        {
            float time = 0;
            float segmentDuration = _duration / 4;

            // Move up and rotate left
            while (time < segmentDuration)
            {
                time += Time.deltaTime;
                transform.position += Vector3.up * _moveSpeed * Time.deltaTime;
                transform.Rotate(Vector3.up, -_rotationSpeed * Time.deltaTime);
                transform.Rotate(Vector3.forward, _rotationSpeedZ * Time.deltaTime); // Rotate left
                yield return null;
            }

            time = 0;

            // Move down and reset rotation
            while (time < segmentDuration)
            {
                time += Time.deltaTime;
                transform.position -= Vector3.up * _moveSpeed * Time.deltaTime;
                transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
                transform.Rotate(Vector3.forward, -_rotationSpeedZ * Time.deltaTime); // Rotate right
                yield return null;
            }

            time = 0;

            // Move up and rotate right
            while (time < segmentDuration)
            {
                time += Time.deltaTime;
                transform.position += Vector3.up * _moveSpeed * Time.deltaTime;
                transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
                transform.Rotate(Vector3.forward, -_rotationSpeedZ * Time.deltaTime); // Rotate left
                yield return null;
            }

            time = 0;

            // Move down and reset rotation
            while (time < segmentDuration)
            {
                time += Time.deltaTime;
                transform.position -= Vector3.up * _moveSpeed * Time.deltaTime;
                transform.Rotate(Vector3.up, -_rotationSpeed * Time.deltaTime);
                transform.Rotate(Vector3.forward, _rotationSpeedZ * Time.deltaTime); // Rotate right
                yield return null;
            }
        }
    }
}
