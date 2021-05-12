using Core.Demo.Scripts.Model;

namespace Core.Demo.Scripts.EventTypes
{
    public class CustomEventTypes : Cross.Events.EventTypes
    {
        public delegate void IsValidDataParsedDelegate(bool isValid);
        public delegate void DataParsedDelegate(DataTransfer data);
    }
}
