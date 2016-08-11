using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Menu_Focus : MonoBehaviour, IPointerEnterHandler {

    public EventSystem eSys;

    public void OnPointerEnter(PointerEventData eventData)
    {
        eSys.SetSelectedGameObject(this.gameObject);
    }
}
