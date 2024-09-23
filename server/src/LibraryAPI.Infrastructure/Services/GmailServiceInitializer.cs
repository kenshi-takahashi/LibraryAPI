// using Google.Apis.Auth.OAuth2;
// using Google.Apis.Gmail.v1;
// using Google.Apis.Services;
// using Google.Apis.Util.Store;
// using Newtonsoft.Json.Linq;

// public class GmailServiceInitializer
// {
//     private static string[] Scopes = { GmailService.Scope.GmailSend };
//     private static string ApplicationName = "LibraryAPI";

//     public static async Task<GmailService> GetGmailServiceAsync()
//     {
//         UserCredential credential;

//         var clientSecretPath = "../LibraryAPI.Infrastructure/Configuration/google_client_secret.json";
//         var json = await File.ReadAllTextAsync(clientSecretPath);
//         var secrets = JObject.Parse(json)["web"];

//         var clientSecrets = new ClientSecrets
//         {
//             ClientId = secrets["client_id"].ToString(),
//             ClientSecret = secrets["client_secret"].ToString()
//         };

//         var tokenPath = "../LibraryAPI.Infrastructure/Configuration/token.json";
//         using (var stream = new FileStream(tokenPath, FileMode.OpenOrCreate, FileAccess.Read))
//         {
//             credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
//                 clientSecrets,
//                 Scopes,
//                 "user",
//                 CancellationToken.None,
//                 new FileDataStore("token.json", true));
//         }

//         return new GmailService(new BaseClientService.Initializer()
//         {
//             HttpClientInitializer = credential,
//             ApplicationName = ApplicationName,
//         });
//     }
// }
