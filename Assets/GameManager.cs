using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //[SerializeField] Transform m_transform;
    Gyroscope gyro;
    public float m_gravityScale;
    public float m_accelerationForce;
    public float m_particleAccelerationForce;
    public float m_minAccelerationInput;
    public Vector3 m_InputAcceleration { get { return new Vector3(Input.acceleration.x, Input.acceleration.y, -Input.acceleration.z) * Mathf.Max(m_minAccelerationInput, Input.acceleration.magnitude); } }
    //[SerializeField] Vector3 offset;
    //public Quaternion m_gyroWorldRotation;

    void Start()
    {
        //Screen.orientation = ScreenOrientation.Portrait;
        gyro = Input.gyro;
        Input.gyro.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Update gyro world rotation
        //m_gyroWorldRotation = Quaternion.Inverse(gyro.attitude); m_gyroWorldRotation = new Quaternion(m_gyroWorldRotation.x, m_gyroWorldRotation.z, m_gyroWorldRotation.y, m_gyroWorldRotation.w);
        //m_gyroWorldRotation.SetLookRotation(gyro.gravity);
        //m_transform.rotation = m_gyroWorldRotation;

        
        //Update gravity
        Physics.gravity = m_gravityScale * gyro.gravity;
        Physics.gravity = new Vector3(Physics.gravity.x, Physics.gravity.y, -Physics.gravity.z * 5.0f) + (m_InputAcceleration * m_accelerationForce);

        foreach (var particleSystem in FindObjectsOfType<ParticleSystem>())
        {
            ParticleSystem.Particle[] sss = new ParticleSystem.Particle[particleSystem.particleCount];
            particleSystem.GetParticles(sss);
            for (int i = 0; i < sss.Length; i++)
            {
                sss[i].velocity += Vector3.zero;

            }

        }
    }

    void FixedUpdate()
    {
        //foreach (var rigidbody in FindObjectsOfType<Rigidbody>()) { rigidbody.AddForce(m_InputAcceleration * m_accelerationForce, ForceMode.Impulse); }
        
    }
}
