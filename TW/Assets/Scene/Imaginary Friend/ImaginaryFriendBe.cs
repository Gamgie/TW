using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class ImaginaryFriendBe : MonoBehaviour
{
    [System.Serializable]
    public struct BrownianData {public float frequency;public float amplitude; }

    [SerializeField] bool followMouse;
    [SerializeField] float smoothFollow;
    [Range(0,1)]
    [SerializeField] float shaking;
    [SerializeField] BrownianData minShaking;
    [SerializeField] BrownianData maxShaking;
    [Range(0, 3)]
    [SerializeField] float sizeMultiplier = 1.0f;
    [SerializeField] bool breath;
    [SerializeField] float breathAmplitude = 1.0f;
    [SerializeField] float breathSpeed = 1.0f;

    private Camera _cam;
    private Vector3 velocity = Vector3.zero;
    private GameObject _sphere = null;
    private Klak.Motion.BrownianMotion _brownianScript;
    private float _internalSize;

    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
        _brownianScript = GetComponentInChildren<Klak.Motion.BrownianMotion>();
        _sphere = _brownianScript.gameObject;
        _internalSize = _sphere.transform.localScale.x;
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

        if(Keyboard.current.enterKey.wasPressedThisFrame)
        {
            BreathAnimation();
        }

        UpdateBrownianNoise();
        UpdateSize();
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
        if(!breath && !DOTween.IsTweening("Breath"))
        {
            _sphere.transform.localScale = new Vector3(_internalSize * sizeMultiplier, _internalSize * sizeMultiplier, _internalSize * sizeMultiplier);
        }
    }

    public void BreathAnimation()
    {
        breath = !breath;
        /*DOTween.Kill("Breath");

        if (breath)
        {
            _sphere.transform.DOScale(_internalSize + breathAmplitude,breathSpeed).SetSpeedBased(true).SetLoops(-1, LoopType.Yoyo).SetId("Breath").SetEase(Ease.InOutQuad);
        }
        else
        {
            _sphere.transform.DOScale(_internalSize, breathSpeed).SetSpeedBased(true).SetId("Breath");
        }*/

    }

    void UpdateBreath()
    {
        if(breath)
        {

        }
    }

    float easeInOutQuad(float progress) {
        return progress < 0.5 ? 2 * progress * progress : 1 - Mathf.Pow(-2 * progress + 2, 2) / 2;

    }
}
