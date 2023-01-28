using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRController))]
public class XRInputEvents : MonoBehaviour
{
    public UnityEvent OnTriggerPress;
    public UnityEvent OnTriggerRelease;

    [SerializeField] XRController xrController;
    private float previousTriggerAxis;
    private float triggerThresholdValue;

    private void Awake()
    {
        xrController = this.gameObject.GetComponent<XRController>();
        triggerThresholdValue = xrController.axisToPressThreshold;
    }
    void Update()
    {
        //float triggerAxis = xrController.ReadValue(InputHelpers.Button.Trigger);
        float pressedTrigger;
        bool triggerAxis = xrController.inputDevice.TryGetFeatureValue(CommonUsages.trigger, out pressedTrigger);
        if (pressedTrigger > triggerThresholdValue && previousTriggerAxis <= triggerThresholdValue)
        {
            Debug.Log("input press started");
            OnTriggerPress.Invoke();
        }
        if (pressedTrigger <= triggerThresholdValue && previousTriggerAxis > triggerThresholdValue)
        {
            Debug.Log("input press ended");
            OnTriggerRelease.Invoke();
        }
        previousTriggerAxis = pressedTrigger;
    }
}
