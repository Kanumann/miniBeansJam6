using UnityEngine;

public class VelocityControl : MonoBehaviour
{
    public float SlowVelocity = 2f;
    public float FastVelocity = 8f;
    public float Sustain = 60f;

    private Rigidbody TargetBody;

    public enum ControlType
    {
        None,
        Slowing,
        Accelerating
    }

    public ControlType ct { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        TargetBody = GetComponent<Rigidbody>();
        ct = ControlType.None;
    }

    // Update is called once per frame
    void Update()
    {
        if (ct == ControlType.None) return;

        float velo = TargetBody.velocity.magnitude;

        float target = velo;
        switch (ct)
        {
            case ControlType.Slowing:
                if (velo <= SlowVelocity) break;
                target = SlowVelocity;
                break;
            case ControlType.Accelerating:
                if (velo >= FastVelocity) break;
                target = FastVelocity;
                break;
        }

        float diff = (velo - target) * Sustain * Time.deltaTime;
        float mult = (target + diff) / velo;

        TargetBody.velocity *= mult;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Slowing") ct = ControlType.Slowing;
        if (collider.tag == "Accelerating") ct = ControlType.Accelerating;
    }

    void OnTriggerExit(Collider collider)
    {
        ct = ControlType.None;
    }
}
