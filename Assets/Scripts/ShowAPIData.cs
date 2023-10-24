using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowAPIData : MonoBehaviour, IPointerClickHandler
{
    GameObject ClientmanagerObject;
    private void Start()
    {
        ClientmanagerObject = GameObject.Find("ClientManager").gameObject;

    }
    // This method is called when the UI element is clicked by a pointer
    public void OnPointerClick(PointerEventData eventData)
    {
        // Handle the click event Dynamically for ClientData GameObject
        ClientmanagerObject.GetComponent<ClientManager>().ShowData();
    }
}