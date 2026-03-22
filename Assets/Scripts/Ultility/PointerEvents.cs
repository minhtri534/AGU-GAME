using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PointerEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent onPointerEnter = new();
    public UnityEvent onPointerExit = new();
    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnter.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onPointerExit.Invoke();
    }
}