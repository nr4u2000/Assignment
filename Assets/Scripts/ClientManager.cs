using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using System;
using Newtonsoft.Json;
using UnityEngine.UI;

public class ClientManager : MonoBehaviour
{
    private string apiUrl = "https://qa2.sunbasedata.com/sunbase/portal/api/assignment.jsp?cmd=client_data";
    [SerializeField] GameObject ClientDataObject;
    [SerializeField] GameObject List;
    [SerializeField] List<GameObject> ClientList = new List<GameObject>();
    [SerializeField] GameObject WaitText;
    [SerializeField] GameObject DataTextarea;

    void Start()
    {

        StartCoroutine(GetData());  // To Fetch data Contents

        WaitText.SetActive(false);
        DataTextarea.SetActive(false);

        //Make dropdown filter Contents and store it in the list
        var dropDown = transform.GetComponent<Dropdown>();
        dropDown.options.Clear();
        List<string> list = new List<string>();
        list.Add("All-Clients");
        list.Add("Manager Only");
        list.Add("Non-Manager");

        foreach(var item in list)
        {
            dropDown.options.Add(new Dropdown.OptionData() { text = item });
        }

        //It is Only use to Show index value i.e. All-Client Because for first Time its not showing in dropdown
        dropDown.value = 1;
        dropDown.value = 0;

        SelectedItem(dropDown); // To Fetch Data according to first index of Dropdown content i.e. All-Client

        dropDown.onValueChanged.AddListener(delegate { SelectedItem(dropDown); });
    }

    private void Update()
    {
        //Check When all contents of client data loaded, for a while it should show WaitText
        if(ClientList.Count==0)
        {
            WaitText.SetActive(true);
        }
        else
        {
            WaitText.SetActive(false);
        }
    }
    #region Public Functions
    void SelectedItem(Dropdown dropdown)
    {
        // Select DropDown Contecnt and Refresh the ClienttData list
        int index = dropdown.value;
        string filterVal = dropdown.options[index].text;
        Debug.Log(filterVal);
        StartCoroutine(GetClient(filterVal));
    }

    public void ShowData()
    {
        // Call this function in OnPointerDown Method in Script ShowAPIData
        DataTextarea.SetActive(true);
    }

    public void Back()
    {
        // Call this function in back button of API DATA Panel
        DataTextarea.SetActive(false);
    }
    #endregion

    #region API IEnumerators
    IEnumerator GetClient(string filterValue)
    {

            // Destroy all previous list of ClientData GameObject
            foreach (var item in ClientList)
            {
                Destroy(item);
            }
            ClientList.Clear(); // Clear all previous list of ClientData GameObject from list

        // API Fetching Method
        UnityWebRequest request = UnityWebRequest.Get(apiUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            Root clientData = JsonUtility.FromJson<Root>(json);

            // Here we Sort The data According to Filter
            foreach (var client in clientData.clients)
            {
                if (filterValue == "Non-Manager")
                {
                    if(!client.isManager)
                    {
                        GameObject tempClientInstance = Instantiate(ClientDataObject, List.transform.position, Quaternion.identity, List.transform);
                        tempClientInstance.transform.GetChild(0).GetComponent<Text>().text = client.isManager.ToString();
                        tempClientInstance.transform.GetChild(1).GetComponent<Text>().text = client.id.ToString();
                        tempClientInstance.transform.GetChild(2).GetComponent<Text>().text = client.label;
                        ClientList.Add(tempClientInstance);
                    } 
                }

                if (filterValue == "Manager Only")
                {
                    if(client.isManager)
                    {
                        GameObject tempClientInstance = Instantiate(ClientDataObject, List.transform.position, Quaternion.identity, List.transform);
                        tempClientInstance.transform.GetChild(0).GetComponent<Text>().text = client.isManager.ToString();
                        tempClientInstance.transform.GetChild(1).GetComponent<Text>().text = client.id.ToString();
                        tempClientInstance.transform.GetChild(2).GetComponent<Text>().text = client.label;
                        ClientList.Add(tempClientInstance);
                    }
                }

                if(filterValue == "All-Clients")
                {
                    GameObject tempClientInstance = Instantiate(ClientDataObject,List.transform.position,Quaternion.identity,List.transform);
                    tempClientInstance.transform.GetChild(0).GetComponent<Text>().text = client.isManager.ToString();
                    tempClientInstance.transform.GetChild(1).GetComponent<Text>().text = client.id.ToString();
                    tempClientInstance.transform.GetChild(2).GetComponent<Text>().text = client.label;
                    ClientList.Add(tempClientInstance);
                }
            }
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }


    IEnumerator GetData()
    {
        // API Fetching Method
        UnityWebRequest request = UnityWebRequest.Get(apiUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(json);

            DataItem item1 = myDeserializedClass.data["1"];
            DataItem item2 = myDeserializedClass.data["2"];
            DataItem item3 = myDeserializedClass.data["3"];

            DataTextarea.transform.GetChild(1).GetComponent<Text>().text = item1.address + "\n" + item2.address + "\n" + item3.address;
            DataTextarea.transform.GetChild(2).GetComponent<Text>().text = item1.name + "\n" + item2.name + "\n" + item3.name;
            DataTextarea.transform.GetChild(3).GetComponent<Text>().text = item1.points.ToString() + "\n" + item2.points.ToString() + "\n" + item3.points.ToString();
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }
    #endregion

}
