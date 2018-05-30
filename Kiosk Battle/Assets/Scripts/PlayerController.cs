using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PlayerController : MonoBehaviour {

	public float speed;
	Vector3 m_EulerAngleVelocity;
    bool isHolding = false;

	public XboxController controller;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        m_EulerAngleVelocity = new Vector3(0, 0, -200);
    }

    void FixedUpdate()
    {
        if (XCI.GetAxis(XboxAxis.LeftStickY, controller) != 0 || XCI.GetAxis(XboxAxis.LeftStickX, controller) != 0)
        {
            Vector3 move = Vector3.zero = new Vector3(XCI.GetAxis(XboxAxis.LeftStickX, controller), transform.position.y, XCI.GetAxis(XboxAxis.LeftStickY, controller));
            rb.MovePosition(transform.position + move.normalized * Time.deltaTime * speed);
        }
        if (XCI.GetAxis(XboxAxis.RightStickX, controller) != 0)
        {
            Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.deltaTime * (XCI.GetAxis(XboxAxis.RightStickX, controller)));
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
    }

    void Update()
    {
        if (XCI.GetAxis(XboxAxis.LeftTrigger) > 0.1f && XCI.GetAxis(XboxAxis.RightTrigger) > 0.1f && !isHolding)
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);
            Debug.DrawRay(pos, transform.up, Color.green);
            RaycastHit hit;
            if (Physics.Raycast(pos, transform.up, out hit, 2.5f))
            {
                if (hit.transform.tag == "Box")
                {
                    isHolding = true;
                    hit.transform.position = transform.GetChild(0).transform.position;
                    hit.transform.rotation = transform.GetChild(0).transform.rotation;
                    hit.transform.SetParent(transform.GetChild(0).transform);
                }
            }
        }
        if (isHolding && XCI.GetAxis(XboxAxis.LeftTrigger) <= 0.1f && XCI.GetAxis(XboxAxis.RightTrigger) <= 0.1f)
        {
            isHolding = false;
            transform.GetChild(0).GetChild(0).SetParent(null);
        }
    }
}
