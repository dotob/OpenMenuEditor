using System.IO;
using System.Net;

namespace OpenMenuEditorWPF {
  public class FTPTools {
    public static void Upload(string server, int port, string localFile, string remoteFile, string username, string password, bool isActive) {
      string url = string.Format("ftp://{0}:{1}/{2}", server, port, remoteFile);
      var ftp = (FtpWebRequest) WebRequest.Create(url);
      ftp.Credentials = new NetworkCredential(username, password);
      ftp.KeepAlive = false;
      ftp.UseBinary = true;
      ftp.Method = WebRequestMethods.Ftp.UploadFile;
      if (isActive) {
        ftp.UsePassive = false;
      }

      using (var writer = new BinaryWriter(ftp.GetRequestStream())) {
        var allBytes = File.ReadAllBytes(localFile);
        writer.Write(allBytes);
      }
    }

    public static void Download(string server, int port, string remoteFile, string localFile, string username, string password, bool isActive) {
      // Get the object used to communicate with the server.
      string url = string.Format("ftp://{0}:{1}/{2}", server, port, remoteFile);
      var ftpWebRequest = (FtpWebRequest) WebRequest.Create(url);
      ftpWebRequest.Method = WebRequestMethods.Ftp.DownloadFile;
      ftpWebRequest.KeepAlive = false;
      ftpWebRequest.UseBinary = true;
      if (isActive) {
        ftpWebRequest.UsePassive = false;
      }

      // This example assumes the FTP site uses anonymous logon.
      ftpWebRequest.Credentials = new NetworkCredential(username, password);

      var response = (FtpWebResponse) ftpWebRequest.GetResponse();

      using (var reader = new StreamReader(response.GetResponseStream())) {
        string fileContent = reader.ReadToEnd();
        File.WriteAllText(localFile, fileContent);
      }
    }
  }
}