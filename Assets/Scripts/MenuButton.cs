using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
public class MenuButton : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public GameObject infoWindow;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Button clicked: " + gameObject.name);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (infoWindow != null)
        {
            infoWindow.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        }
        Debug.Log("Pointer entered: " + gameObject.name);
    }
    public void OnPointerExit(PointerEventData eventData)
    {

        if (infoWindow != null)
        {
            infoWindow.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
        }
        Debug.Log("Pointer exited: " + gameObject.name);
    }
}
