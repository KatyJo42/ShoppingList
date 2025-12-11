

namespace ShoppingList.Models;

public class UserDataCollection
{
    public UserData[] UserDataItems { get; set; }
}

public class UserData
{
    public string userKey { get; set; }
    public string dataValue { get; set; }
    public string dataID { get; set; }

    public UserData(string userKey, string dataValue, string dataID)
    {
        this.userKey = userKey;
        this.dataValue = dataValue;
        this.dataID = dataID;
    }
}