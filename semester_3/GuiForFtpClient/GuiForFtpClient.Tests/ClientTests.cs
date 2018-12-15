using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleFTP_Client;

namespace GuiForFtpClient.Tests
{
    [TestClass]
    public class ClientTests
    {
        [TestMethod]
        public async void IfServerOffClientShouldNotCrash()
        {
            var clientViewModel = new ClientViewModel();

            clientViewModel.HostName = "localhost";
            clientViewModel.HostPort = "8888";

            await clientViewModel.GetDirectory(@"...\");
            clientViewModel.DownloadFiles(new FileInfo(null, false));
        }
    }
}
