using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerCarController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float maxVelocity = 3;
    [SerializeField] private float turnSpeed = 1;

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
        if (joystick.Horizontal == 0 && joystick.Vertical == 0)
        {
            rb.velocity *= 0.8f;
            return;
        }


        inputDir = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.LookRotation(inputDir), turnSpeed * Time.fixedDeltaTime);

        velocity = rb.velocity;
        moveDir.x = transform.forward.x;
        moveDir.y = 0;
        moveDir.z = transform.forward.z;
        velocity = moveDir * inputDir.magnitude * moveSpeed * Time.fixedDeltaTime;
        rb.velocity = velocity;

        velocity.y = 0;
        if (velocity.magnitude > maxVelocity)
        {
            velocity = velocity.normalized * maxVelocity;
            velocity.y = rb.velocity.y;
            rb.velocity = velocity;
        }

    }
}
