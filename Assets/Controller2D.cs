using UnityEngine;

public class Controller2D : MonoBehaviour
{
    public float m_gravityScale;
    public float m_accelerationForce;
    public float m_particleAccelerationForce;
    public float m_minAccelerationInput;
    public Vector3 m_InputAcceleration { get { return new Vector3(Input.acceleration.x, Input.acceleration.y, -Input.acceleration.z) * Mathf.Max(m_minAccelerationInput, Input.acceleration.magnitude); } }

    void Start()
    {
        Input.gyro.enabled = true;
    }

    void Update()
    {
        //Update gravity
        if (SystemInfo.supportsGyroscope)
        {
            Physics2D.gravity = Input.gyro.gravity;
        }
#if UNITY_EDITOR
        else
        {
            int BoolToInt(bool _bool) { return _bool ? 1 : 0; }

            Physics2D.gravity = new Vector3
                (
                BoolToInt(Input.GetKey(KeyCode.D)) - BoolToInt(Input.GetKey(KeyCode.A)),
                BoolToInt(Input.GetKey(KeyCode.W)) - BoolToInt(Input.GetKey(KeyCode.S)),
                BoolToInt(Input.GetKey(KeyCode.Q)) - BoolToInt(Input.GetKey(KeyCode.E))
                ).normalized;
        }
#endif

        Physics2D.gravity *= m_gravityScale;// * gyro.gravity;
        Physics2D.gravity = new Vector3(Physics2D.gravity.x, Physics2D.gravity.y) + (m_InputAcceleration * m_accelerationForce);
    }
}
