using UnityEngine;

namespace Kaleidoscope
{
    public class KaleidoscopeRotator : MonoBehaviour
    {
        Transform _transform;
        new Transform transform { get { return _transform ? _transform : _transform = GetComponent<Transform>(); } }

        [SerializeField] public float speed = 90f;
        float count;

        void Update()
        {
            count = Mathf.Repeat(count + Time.deltaTime * speed, 360f);
            transform.rotation = Quaternion.Euler(0f, 0f, count);
        }
    }
}