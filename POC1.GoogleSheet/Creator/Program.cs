using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace Creator
{
     class Program
     {
          static string[] Scopes = { SheetsService.Scope.Spreadsheets }; // Giving all access
          static string ApplicationName = "Google Sheets API .NET Quickstart";

          static void Main(string[] args)
          {
               UserCredential credential;
               // Create your own credentials from here https://developers.google.com/sheets/api/quickstart/dotnet

               using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
               {
                    string credPath = System.Environment.GetFolderPath(
                        System.Environment.SpecialFolder.Personal);
                    credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-dotnet-quickstart.json");

                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                    Console.WriteLine("Credential file saved to: " + credPath);
               }

               // Create Google Sheets API service.
               var service = new SheetsService(new BaseClientService.Initializer()
               {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
               });

               var myNewSheet = new Google.Apis.Sheets.v4.Data.Spreadsheet();
               myNewSheet.Properties = new SpreadsheetProperties();
               myNewSheet.Properties.Title = "Sample Sheet"; // Name of worksheet
               var sheet = new Sheet();
               sheet.Properties = new SheetProperties();
               sheet.Properties.Title = "Sheet1"; // Name of one sheet
               var sheet2 = new Sheet();
               sheet2.Properties = new SheetProperties();
               sheet2.Properties.Title = "Sheet2"; // Name of second sheet
               myNewSheet.Sheets = new List<Sheet>() { sheet, sheet2 }; // Add as many sheets as you want in this list

               var newSheet = service.Spreadsheets.Create(myNewSheet).Execute();

               
          }
     }
}
