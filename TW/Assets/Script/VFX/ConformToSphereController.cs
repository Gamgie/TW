using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ConformToSphereController : ForceController
{
    public Transform sphereCenter;
    public float radius = 1;
    public float attractionSpeed = 5;
    public float attractionForce = 20;
    public float stickDistance = 0.1f;
    public float stickForce = 50;
    public float forceAreaSize = 20;
    


    // Update is called once per frame
    void Update()
    {
        if (m_vfxs == null)
            return;

        foreach (VisualEffect visualEffect in m_vfxs)
        {
            if (visualEffect.HasVector3("CTS Center" + suffix))
                visualEffect.SetVector3("CTS Center" + suffix, sphereCenter.position);

            if (visualEffect.HasFloat("CTS Radius" + suffix))
                visualEffect.SetFloat("CTS Radius" + suffix, radius);

            if (visualEffect.HasFloat("CTS Attraction Speed" + suffix))
                visualEffect.SetFloat("CTS Attraction Speed" + suffix, attractionSpeed);

            if (visualEffect.HasFloat("CTS Attraction Force" + suffix))
                visualEffect.SetFloat("CTS Attraction Force" + suffix, attractionForce);

            if (visualEffect.HasFloat("CTS Stick Distance" + suffix))
                visualEffect.SetFloat("CTS Stick Distance" + suffix, stickDistance);

            if (visualEffect.HasFloat("CTS Stick Force" + suffix))
                visualEffect.SetFloat("CTS Stick Force" + suffix, stickForce);
            
            if (visualEffect.HasFloat("CTS Force Area Size" + suffix))
                visualEffect.SetFloat("CTS Force Area Size" + suffix, forceAreaSize);
        }
    }
}
