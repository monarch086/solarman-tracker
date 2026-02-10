namespace SolarmanTracker.Core.DataModel
{
    public sealed class Config
    {
        public string AppId { get; set; }

        public string AppSecret { get; set; }

        public string DeviceSn { get; set; }

        public bool IsActive { get; set; }

        public string ChatId { get; set; }
    }
}
