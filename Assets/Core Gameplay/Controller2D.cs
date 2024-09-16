using UnityEngine;

public class Controller2D : MonoBehaviour
{
    [Header("Gravity")]
    public bool m_gyroGravityEnabled = true;
    public float m_gravityScale;
    public float m_particleForceScale;
    public float m_accelerationForce;
    public float m_particleAccelerationForce;
    public float m_minAccelerationInput;
    public Vector3 m_InputAcceleration { get { return new Vector3(Input.acceleration.x, Input.acceleration.y, -Input.acceleration.z) * Mathf.Max(m_minAccelerationInput, Input.acceleration.magnitude); } }

    [Header("Grab")]
    public bool m_pointerGrabEnabled = true;
    [SerializeField] LayerMask m_grabLayerMask;
    [SerializeField] Rigidbody2D m_grabbedRigidbody;
    [SerializeField] Vector2 m_grabPoint;
    [SerializeField] float m_grabSpringStrength;
    [SerializeField] float m_grabSpringDamping;
    [SerializeField] float m_grabMaxSpeed;


    void Start()
    {
        Input.gyro.enabled = true;
    }

    void Update()
    {
        //Update gravity
        if (m_gyroGravityEnabled)
        {
            //Physics2D.gravity = Vector2.zero;
            //
            ////Control with gyro
            //if (SystemInfo.supportsGyroscope) Physics2D.gravity = Input.gyro.gravity;
            ////Control with controller
            //else Physics2D.gravity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
            Physics2D.gravity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

            Physics2D.gravity *= m_gravityScale;// * gyro.gravity;
        }

        //Grabbing
        if (m_pointerGrabEnabled)
        {
            //Grab Objects
            if (Input.GetMouseButtonDown(0))
            {
                //Cast a ray from the mouse position
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, m_grabLayerMask);

                //Check whether the hit object has a rigidbody that can be grabbed
                if (hit.collider != null)
                {
                    Rigidbody2D hitRigidbody = hit.collider.GetComponent<Rigidbody2D>();
                    if (hitRigidbody != null)
                    {
                        //Record grabbed rigidbody and grabbed position realitve to the rigibody origin. 
                        m_grabbedRigidbody = hitRigidbody;
                        m_grabPoint = hitRigidbody.transform.InverseTransformPoint(hit.point);
                    }
                }
            }

            //Drag Objects
            if (m_grabbedRigidbody)
            {
                Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 grabbedWorldPoint = m_grabbedRigidbody.transform.TransformPoint(m_grabPoint);
                Vector2 grabOffset = mouseWorldPos - grabbedWorldPoint;
                Vector2 dragForce = (m_grabSpringStrength * grabOffset) - (m_grabbedRigidbody.velocity * m_grabSpringDamping);

                m_grabbedRigidbody.AddForceAtPosition((m_grabSpringStrength * grabOffset) - (m_grabbedRigidbody.velocity * m_grabSpringDamping), grabbedWorldPoint);
                
                //Clamp Speed
                if (m_grabbedRigidbody.velocity.sqrMagnitude > m_grabMaxSpeed * m_grabMaxSpeed) m_grabbedRigidbody.velocity = m_grabbedRigidbody.velocity.normalized * m_grabMaxSpeed;
            }

            //Release Objects
            if (Input.GetMouseButtonUp(0) && m_grabbedRigidbody != null)
            {
                m_grabbedRigidbody = null;
            }
        }
    }
}
