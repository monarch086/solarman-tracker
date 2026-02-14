namespace SolarmanTracker.Core.DataModel
{
    public sealed class Config
    {
        public string StationId { get; set; }

        public string AppId { get; set; }

        public string AppSecret { get; set; }

        public bool IsActive { get; set; }

        public string AccessToken { get; set; }

        public DateTime AccessTokenExpDate { get; set; }

        public string RefreshToken { get; set; }

        public string DeviceSn { get; set; }

        public string ChatId { get; set; }
    }
}
