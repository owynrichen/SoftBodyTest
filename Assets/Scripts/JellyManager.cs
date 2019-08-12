using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyManager : MonoBehaviour
{
    public Camera cam;
    public Jellyfier jellyfier;
    public UnityEngine.UI.Slider bounceSpeed;
    public UnityEngine.UI.Slider fallForce;
    public UnityEngine.UI.Slider stiffness;
    public UnityEngine.UI.Slider pressure;
    public UnityEngine.UI.Slider rotationSpeed;
    public UnityEngine.UI.Slider height;
    public UnityEngine.UI.Text rotationSpeedValue;
    public UnityEngine.UI.Text bounceSpeedValue;
    public UnityEngine.UI.Text fallForceValue;
    public UnityEngine.UI.Text stiffnessValue;
    public UnityEngine.UI.Text pressureValue;
    public UnityEngine.UI.Text heightValue;

    private Vector3 startingPosition;
    private Quaternion startingRotation;
    
    // Start is called before the first frame update
    void Start()
    {
        jellyfier.bounceSpeed = bounceSpeed.value;
        jellyfier.fallForce = fallForce.value;
        jellyfier.stiffness = stiffness.value;
        height.value = jellyfier.transform.position.y;

        startingPosition = jellyfier.transform.position;
        startingRotation = jellyfier.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        jellyfier.bounceSpeed = bounceSpeed.value;
        jellyfier.fallForce = fallForce.value;
        jellyfier.stiffness = stiffness.value;

        if (!jellyfier.GetComponent<Rigidbody>().useGravity)
        {
            jellyfier.transform.position = new Vector3(jellyfier.transform.position.x, height.value, jellyfier.transform.position.z);
            jellyfier.gameObject.transform.Rotate(Vector3.up, rotationSpeed.value);
        }

        bounceSpeedValue.text = bounceSpeed.value.ToString();
        rotationSpeedValue.text = rotationSpeed.value.ToString();
        fallForceValue.text = fallForce.value.ToString();
        stiffnessValue.text = stiffness.value.ToString();
        pressureValue.text = pressure.value.ToString();
        heightValue.text = jellyfier.transform.position.y.ToString();


        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject == jellyfier.gameObject) {
                    jellyfier.ApplyPressureToPoint(hit.point, pressure.value);
                }
            }
        }
    }

    public void BounceObject()
    {
        jellyfier.GetComponent<Rigidbody>().useGravity = true;
    }

    public void ResetObject()
    {
        jellyfier.GetComponent<Rigidbody>().useGravity = false;
        jellyfier.transform.SetPositionAndRotation(startingPosition, startingRotation);
        jellyfier.transform.TransformDirection(Vector3.up);
    }

}
