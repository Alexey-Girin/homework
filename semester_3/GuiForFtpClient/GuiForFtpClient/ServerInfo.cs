namespace SimpleFTP_Client
{
    public class ServerInfo
    {
        public string HostName { get; }
        public int HostPort { get; }
        public string PathToDownload { get; } = null;

        public ServerInfo(string hostName, string hostPort)
        {
            HostName = hostName;
            HostPort = int.Parse(hostPort);
        }

        public ServerInfo(string hostName, string hostPort, string pathToDownload)
        {
            HostName = hostName;
            HostPort = int.Parse(hostPort);
            PathToDownload = pathToDownload;
        }
    }
}
