﻿// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-18 1:27 PM

namespace eagleboost.googledrive.Services
{
  using System.IO;
  using System.Threading;
  using System.Threading.Tasks;
  using Google.Apis.Auth.OAuth2;
  using Google.Apis.Util.Store;

  public interface IUserCredentialProvider
  {
    Task<UserCredential> GetUserCredentialAsync(string credentialFile, string credPath);
  }

  public class UserCredentialProvider
  {
    public async Task<UserCredential> GetUserCredentialAsync(string credentialFile, string credPath, string[] scopes)
    {
      using (var stream = new FileStream(credentialFile, FileMode.Open, FileAccess.Read))
      {
        var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
          GoogleClientSecrets.Load(stream).Secrets,
          scopes,
          "user",
          CancellationToken.None,
          new FileDataStore(credPath, true)).ConfigureAwait(false);

        return credential;
      }
    }
  }
}