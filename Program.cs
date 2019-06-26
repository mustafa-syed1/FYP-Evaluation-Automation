using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using CsvHelper;



namespace SheetsQuickstart
{
    class Program
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "Google Sheets API .NET Quickstart";

        static void Main(string[] args)
        {
            UserCredential credential;
            // Create your own credentials from here https://developers.google.com/sheets/api/quickstart/dotnet

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

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define request parameters.
            // Make spreadsheet public to access this
            String spreadsheetId = "1hxfUK40tFSZe7wz5p2sJpafnllolfRS4r_UybgHuYGI"; // Change the ID to your desired spreadsheet
            String range = "Sheet1"; // Define the columns and sheets tabs
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);

            // Prints the columns in sheet
            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;

            var filename = Directory.GetCurrentDirectory() + @"\file.csv";
            TextWriter textWriter = File.CreateText(filename);
            var csvWriter = new CsvWriter(textWriter);
            
            if (values != null && values.Count > 0)
            {
                Console.WriteLine("Columns");
                foreach (var row in values)
                {
                    // Print columns A and E, which correspond to indices 0 and 6.
                    Console.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}, {6}", row[0], row[1], row[2], row[3], row[4], row[5], row[6]); // Enter the columns here to read
                    for (int i = 0; i < 4; ++i ) // Select the rows to write in output csv file
                    {
                        csvWriter.WriteField(row[i]);
                    }
                    csvWriter.NextRecord();
                }
            }
            else
            {
                Console.WriteLine("No data found.");
            }
            textWriter.Close();
            Console.Read();
        }
    }

}