using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[ExecuteInEditMode]
public class ForceController : MonoBehaviour
{

    public int forceID = 0;
    [Tooltip("If the vfx is not a child of this script. Add here the parent of the controlled VFX")]
    public GameObject vfxObjectParent;

    protected VisualEffect[] m_vfxs;
    protected string suffix = "";

    private void OnEnable()
    {
        if(vfxObjectParent == null)
        {
            m_vfxs = GetComponentsInChildren<VisualEffect>();
        }
        else
        {
            m_vfxs = vfxObjectParent.GetComponentsInChildren<VisualEffect>();
        }
        

        // Set suffix to handle same multiple force on the same object.
        if (forceID != 0)
        {
            suffix = " " + forceID.ToString();
        }
    }

}
