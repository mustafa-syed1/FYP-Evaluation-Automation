using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Access
{
     class Program
     {
          public static Permission InsertPermission(DriveService service, String fileId,
      String type, String role)
          {
               Permission newPermission = new Permission();
               //newPermission.Value = value;
               newPermission.Type = type;
               newPermission.Role = role;
               try
               {
                    return service.Permissions.Create(newPermission, fileId).Execute();
               }
               catch (Exception e)
               {
                    Console.WriteLine("An error occurred: " + e.Message);
               }
               return null;
          }
          static string[] Scopes = { DriveService.Scope.Drive };
          static string ApplicationName = "Drive API .NET Quickstart";
          static void Main(string[] args)
          {
               UserCredential credential;

               using (var stream =
                   new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
               {
                    // The file token.json stores the user's access and refresh tokens, and is created
                    // automatically when the authorization flow completes for the first time.
                    string credPath = "token.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                    Console.WriteLine("Credential file saved to: " + credPath);
               }

               // Create Drive API service.
               var service = new DriveService(new BaseClientService.Initializer()
               {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
               });

               // Define parameters of request.
               FilesResource.ListRequest listRequest = service.Files.List();
               listRequest.PageSize = 10;
               listRequest.Fields = "nextPageToken, files(id, name)";

               // List files.
               IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
                   .Files;
               Console.WriteLine("Files:");
               if (files != null && files.Count > 0)
               {
                    foreach (var file in files)
                    {
                         Console.WriteLine("{0} ({1})", file.Name, file.Id);
                    }
               }
               else
               {
                    Console.WriteLine("No files found.");
               }

               /*
                * role	string	
                * The primary role for this user:
                    owner
                    organizer
                    fileOrganizer
                    writer
                    reader
                    writable
                    type	string
                    	
                    The account type. Allowed values are:
                    user
                    group
                    domain
                    anyone
                * 
                * */
                // Give File ID and permission to grant access
               InsertPermission(service, "101Mh1py2egrjEDVqkTba6N4FAhhjLJsKu4Rn8e5eFio", "anyone", "reader");
               Console.WriteLine("Permission Granted");
               Console.Read();

          }
     }
}
