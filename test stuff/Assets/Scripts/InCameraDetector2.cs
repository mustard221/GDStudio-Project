using UnityEngine;
using UnityEngine.Events;

public class InCameraDetector2 : MonoBehaviour
{
    public UnityEvent enteredTrigger;

    Camera cam;
    MeshRenderer render;
    Plane[] cameraFrustum;
    Collider collide;

    private bool insideTrigger;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        render = GetComponent<MeshRenderer>();
        collide = GetComponent<Collider>();
    }


    // Update is called once per frame
    void Update()
    {
        var bounds = collide.bounds;
        cameraFrustum = GeometryUtility.CalculateFrustumPlanes(cam);
        if (GeometryUtility.TestPlanesAABB(cameraFrustum, bounds))
        {
            TriggerEntered();
        }
        /* else
         {
             print("POO");
         } */
    }

    public void TriggerEntered()
    {
        enteredTrigger.Invoke();
      // insideTrigger = true;
    }
}
