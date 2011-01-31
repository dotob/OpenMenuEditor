using NUnit.Framework;

namespace OpenMenuEditorWPF.Tests {
  [TestFixture]
  public class FTPTests {
    [Test]
    public void FTP_Download_CreateLocalFile() {
      FTPTools.Download("dotob.de", 21, "test.xml", "test.xml", "fringshaus", "fringshaus", false);
    }

    [Test]
    public void FTP_Upload_CreateRemoteFile() {
      FTPTools.Upload("dotob.de", 21, @"c:\tmp\testup.xml", "testup.xml", "fringshaus", "fringshaus", false);
    }

  }
}