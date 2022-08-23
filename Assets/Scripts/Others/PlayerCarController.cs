using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerCarController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float maxVelocity = 3;
    [SerializeField] private float turnSpeed = 1;
    [SerializeField] private float maxTrackdOffset = 3;

    private Rigidbody rb;
    private Joystick joystick;

    //Cache to optimize Dynamic memory allocation
    private Vector3 inputDir;
    private Vector3 moveDir;
    private Vector3 velocity;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        joystick = FindObjectOfType <Joystick>();
    }

    void FixedUpdate()
    {
        if (joystick == null || rb == null)
            return;

        velocity = rb.velocity;
        if (joystick.Horizontal == 0 && joystick.Vertical == 0)
        {
            moveDir = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
            velocity.y = 0;
            if (Vector3.Dot(velocity.normalized, moveDir) > 0.01f)
            {
                velocity = moveDir * velocity.magnitude;
                velocity.y = rb.velocity.y;
                rb.velocity = velocity;
            }
            var friction = 1 - Time.fixedDeltaTime * 5;
            rb.velocity = new Vector3(rb.velocity.x * friction, rb.velocity.y, rb.velocity.z * friction);
            rb.angularVelocity *= friction;
        }
        else
        { 
            inputDir = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
            rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.LookRotation(inputDir), turnSpeed * Time.fixedDeltaTime);

            moveDir = new Vector3(transform.forward.x, 0, transform.forward.z);
            rb.AddForce(moveDir * inputDir.magnitude * moveSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);

            velocity = rb.velocity;
            velocity.y = 0;
            if (Vector3.Dot(velocity.normalized, moveDir) > 0.01f)
            {
                velocity = moveDir * velocity.magnitude;
                velocity.y = rb.velocity.y;
                rb.velocity = velocity;
            }
            velocity.y = 0;
            if (velocity.magnitude > maxVelocity)
            {
                velocity = velocity.normalized * maxVelocity;
                velocity.y = rb.velocity.y;
                rb.velocity = velocity;
            }
        }

    }
}
