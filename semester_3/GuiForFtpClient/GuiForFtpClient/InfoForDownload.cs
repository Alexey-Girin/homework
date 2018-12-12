namespace SimpleFTP_Client
{
    public class InfoForDownload
    {
        public string FixedHostName { get; }
        public string FixedHostPort { get; }
        public string FixedPathToDownload { get; }

        public InfoForDownload(string hostName, string hostPort, string pathToDownload)
        {
            FixedHostName = hostName;
            FixedHostPort = hostPort;
            FixedPathToDownload = pathToDownload;
        }
    }
}
