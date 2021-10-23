using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class ImaginaryFriendBe : MonoBehaviour
{
    [System.Serializable]
    public struct BrownianData {public float frequency;public float amplitude; }

    public bool followMouse;
    public float smoothFollow;
    [Range(0,1)]
    public float shaking;
    public BrownianData minShaking;
    public BrownianData maxShaking;
    [Range(0, 1)]
    public float size = 1.0f;
    public bool breath;
    public float breathAmplitude = 1.0f;
    public float breathSpeed = 1.0f;

    [Header("Perlin Wobble")]
    public float perlinAmplitude;
    public float perlinSpeed;

    [Header("Emitter1")]
    public float rate1;
    public float life1;
    public Gradient[] colorOverLife;

    [Header("Emitter2")]
    public float rate2;
    public float life2;

    [Header("Forces")]
    public float turbulence;
    public float gravity;

    private Camera _cam;
    private Vector3 velocity = Vector3.zero;
    private GameObject _sphere = null;
    private Klak.Motion.BrownianMotion _brownianScript;
    private float _internalSize;
    private float _breathTimer;
    private VisualEffect _vfx;


    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
        _brownianScript = GetComponentInChildren<Klak.Motion.BrownianMotion>();
        _sphere = _brownianScript.gameObject;
        _internalSize = 0;

        _vfx = GetComponentInChildren<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        if(followMouse)
        {
            
            // Check if we are in the screen. We add an slight offset so IF can go offscreen sometime.
            float absoluteMouseX = Mouse.current.position.ReadValue().x / Screen.width;
            float absoluteMouseY = Mouse.current.position.ReadValue().y / Screen.height;

            if (absoluteMouseX >= -0.2f && absoluteMouseX <= 1.2f 
                &&
                absoluteMouseY >= -0.2f && absoluteMouseY <= 1.2f)
            {
                // Check if left button is pressed
                if (Mouse.current.leftButton.isPressed && _cam != null)
                {
                    Ray r = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());
                    RaycastHit hit;

                    if (Physics.Raycast(r, out hit))
                    {
                        transform.position = Vector3.SmoothDamp(transform.position, hit.point, ref velocity, smoothFollow);
                    }
                }
            }

        }

        UpdateBrownianNoise();
        UpdateBreath();
        UpdateSize();
        UpdateVFX();
    }

    void UpdateBrownianNoise()
    {
        if(_brownianScript != null)
        {
            _brownianScript.frequency = Mathf.Lerp(minShaking.frequency, maxShaking.frequency, shaking);
            float amplitude = Mathf.Lerp(minShaking.amplitude, maxShaking.amplitude, shaking);
            _brownianScript.positionAmount = new Vector3(amplitude, amplitude, 0);
        }
    }

    void UpdateSize()
    {
        _sphere.transform.localScale = new Vector3(_internalSize + size, _internalSize + size, _internalSize + size);
    }

    void UpdateBreath()
    {
        if(breath)
        {
            // progress is the wave going back and forth between 0 and 1
            Debug.Log((Time.time - _breathTimer) * breathSpeed);
            float progress = (1 - Mathf.Cos((Time.time - _breathTimer) * breathSpeed)) * 0.5f;

            // Compute breath value with ease quad
            float breathValue = breathAmplitude * easeInOutQuad(progress);

            // update internal size
            _internalSize = Mathf.Lerp(_internalSize, breathValue,0.2f);
        }

        // Reset timer every frame when not breathing
        if(!breath)
        {
            _breathTimer = Time.time;
            _internalSize = Mathf.Lerp(_internalSize,0,0.1f);
        }
    }

    float easeInOutQuad(float progress) {
        return progress < 0.5 ? 2 * progress * progress : 1 - Mathf.Pow(-2 * progress + 2, 2) / 2;
    }

    void UpdateVFX()
    {
        if (_vfx == null)
            return;

        if (_vfx.HasFloat("Emitter Size"))
            _vfx.SetFloat("Emitter Size", _internalSize + size);

        if (_vfx.HasFloat("Perlin Amplitude"))
            _vfx.SetFloat("Perlin Amplitude", perlinAmplitude);

        if (_vfx.HasFloat("Perlin Speed"))
            _vfx.SetFloat("Perlin Speed", perlinSpeed);

        if (_vfx.HasFloat("Rate1"))
            _vfx.SetFloat("Rate1", rate1);

        if (_vfx.HasFloat("LifeTime1"))
            _vfx.SetFloat("LifeTime1", life1);

        if (_vfx.HasFloat("Rate2"))
            _vfx.SetFloat("Rate2", rate2);

        if (_vfx.HasFloat("LifeTime2"))
            _vfx.SetFloat("LifeTime2", life2);

        if (_vfx.HasFloat("Gravity"))
            _vfx.SetFloat("Gravity", gravity);

        if (_vfx.HasFloat("Turbulence"))
            _vfx.SetFloat("Turbulence", turbulence);
    }
}
