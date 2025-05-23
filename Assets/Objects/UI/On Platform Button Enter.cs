using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnPlatformButtonEnter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Scriptable Object")]
    [SerializeField] PlatformScriptableObject platformScriptableObject;
    
    [Header("Component")] 
    [SerializeField] private GameObject descriptionWindow;
    [SerializeField] TextMeshProUGUI resourceText;
    [SerializeField] TextMeshProUGUI descriptionText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        descriptionWindow.SetActive(true);
        SetResourceText();
        SetDescriptionText();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionWindow.SetActive(false);
    }

    void SetResourceText()
    {
        resourceText.text = platformScriptableObject.resourceCost.ToString();
    }

    void SetDescriptionText()
    {
        descriptionText.text = platformScriptableObject.platformDescription.ToString();
    }
}
