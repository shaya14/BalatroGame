using System.Collections;
using UnityEngine;

public class CardAnimation : MonoBehaviour
{
    // CR: no defaults in the code!
    [SerializeField] private float _moveSpeed = 1.0f;
    [SerializeField] private float _rotationSpeed = 1.0f;
    [SerializeField] private float _rotationSpeedZ = 1.0f;
    [SerializeField] private float _duration = 4.0f;

    private void Start()
    {
        if (_duration <= 0)
        {
            Debug.LogWarning("Duration must be greater than 0. Animation will not start.");
        }
        else
        {
            StartCoroutine(AnimateCard());
        }
    }

    private IEnumerator AnimateCard()
    {
        float segmentDuration = _duration / 4;

        while (true)
        {
            // Move up and rotate left
            yield return AnimateSegment(Vector3.up, -_rotationSpeed, _rotationSpeedZ, segmentDuration);

            // Move down and reset rotation
            yield return AnimateSegment(Vector3.down, _rotationSpeed, -_rotationSpeedZ, segmentDuration);

            // Move up and rotate right
            yield return AnimateSegment(Vector3.up, _rotationSpeed, -_rotationSpeedZ, segmentDuration);

            // Move down and reset rotation
            yield return AnimateSegment(Vector3.down, -_rotationSpeed, _rotationSpeedZ, segmentDuration);
        }
    }

    private IEnumerator AnimateSegment(Vector3 direction, float rotationY, float rotationZ, float segmentDuration)
    {
        float time = 0;

        while (time < segmentDuration)
        {
            time += Time.deltaTime;
            transform.position += direction * _moveSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up, rotationY * Time.deltaTime);
            transform.Rotate(Vector3.forward, rotationZ * Time.deltaTime);
            yield return null;
        }
    }
}
