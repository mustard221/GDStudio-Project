using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InCameraDetector : MonoBehaviour
{
    Camera cam;
    MeshRenderer render;
    Plane[] cameraFrustum;
    Collider collide;

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
            render.sharedMaterial.color = Color.green;
        }
        else
        {
            render.sharedMaterial.color = Color.red;
        }
    }
}
