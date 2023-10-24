//Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class Client
{
    public bool isManager;
    public int id;
    public string label;
}

[Serializable]
public class Data
{
    [JsonProperty("1")]
    public DataItem Data1;

    [JsonProperty("2")]
    public DataItem Data2;

    [JsonProperty("3")]
    public DataItem Data3;
}

[Serializable]
public class DataItem
{
    public string address;
    public string name;
    public int points;
}

[Serializable]
public class Root
{
    public List<Client> clients;
    public Dictionary<string, DataItem> data;
    public string label;
}

