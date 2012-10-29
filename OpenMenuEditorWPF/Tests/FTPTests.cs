using NUnit.Framework;

namespace OpenMenuEditorWPF.Tests {
  [TestFixture]
  public class FTPTests {
    [Test]
    public void FTP_Download_CreateLocalFile() {
      FTPTools.Download("dotob.de", "test.xml", "test.xml", "fringshaus", "fringshaus");
    }

    [Test]
    public void FTP_Upload_CreateRemoteFile() {
      FTPTools.Upload("dotob.de", @"c:\tmp\testup.xml", "testup.xml", "fringshaus", "fringshaus");
    }

  }
}