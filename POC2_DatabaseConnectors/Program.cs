using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System;
//using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.IO;
using System.Text.RegularExpressions;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace excelTOMysql
{
    class DBConnect
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;
        //private object MessageBox;

        //Constructor
        public DBConnect()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            server = "localhost";
            database = "connectcsharptomysql";
            uid = "root";
            password = "asadbhatti";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //Insert statement
        public void Insert(string p1, string p2, string p3, string p4, string p5, string p6, string p7)
        {
            //INSERT INTO taskData ( firstName,lastName,gender,country,age,date1,id) VALUES('Arshad','Ali','M','Pakistan',40,str_to_date('17-09-2010','%d-%m-%Y'),312); 
            string query = "INSERT INTO taskData ( firstName,lastName,gender,country,age,date1,id) VALUES("+ p1+","+  p2 + "," +p3+ ","+ p4 + "," + p5 + "," + p6 + "," +p7+ ");";
            //string query = "INSERT INTO taskData(firstName, lastName, gender, country, age, date1, id) VALUES('Arshad', 'Ali', 'M', 'Pakistan', 40, str_to_date('17-09-2010', '%d-%m-%Y'), 312);";
            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //Update statement
        public void Update()
        {
            string query = "UPDATE tableinfo SET name='Joe', age='22' WHERE name='John Smith'";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //Delete statement
        public void Delete()
        {
            string query = "DELETE FROM tableinfo WHERE name='John Smith'";

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }
        //Select statement
        public List<string>[] Select()
        {
            string query = "SELECT * FROM taskdata";

            //Create a list to store the result
            List<string>[] list = new List<string>[3];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["id"] + "");
                    list[1].Add(dataReader["name"] + "");
                    list[2].Add(dataReader["age"] + "");
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }

        //Count statement
        public int Count()
        {
            string query = "SELECT Count(*) FROM tableinfo";
            int Count = -1;

            //Open Connection
            if (this.OpenConnection() == true)
            {
                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //ExecuteScalar will return one value
                Count = int.Parse(cmd.ExecuteScalar() + "");

                //close Connection
                this.CloseConnection();

                return Count;
            }
            else
            {
                return Count;
            }
        }

        //Backup
        public void Backup()
        {
        }

        //Restore
        public void Restore()
        {
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            DBConnect db1 = new DBConnect();
            //create a list to hold all the values
            List<string> excelData = new List<string>();

            //read the Excel file as byte array
            byte[] bin = File.ReadAllBytes("C:\\sampleData.xlsx");

            //or if you use asp.net, get the relative path
            // byte[] bin = File.ReadAllBytes(Server.MapPath("ExcelDemo.xlsx"));

            //create a new Excel package in a memorystream
            using (MemoryStream stream = new MemoryStream(bin))
            using (ExcelPackage excelPackage = new ExcelPackage(stream))
            {
                //loop all worksheets
                foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
                {
                    //loop all rows excep first
                    for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                    {
                        //loop all columns in a row
                        for (int j = worksheet.Dimension.Start.Column; j <= worksheet.Dimension.End.Column; j++)
                        {
                            //add the cell data to the List
                            if (worksheet.Cells[i, j].Value != null)
                            {
                                excelData.Add(worksheet.Cells[i, j].Value.ToString());
                            }
                        }
                        string p7, p1, p2, p3, p4, p5, p6;
                        p1 = "'" + excelData[1] + "'";//fn
                        p2 = "'" + excelData[2] + "'";//ln
                        p3 = "'" + excelData[3] + "'"; // gender
                        p4 = "'" + excelData[4] + "'";//country
                        p5 = excelData[5];//int type age

                        //str_to_date('17-09-2010','%d-%m-%Y')
                        p6 = "str_to_date('" + excelData[6] + "','%d/%m/%Y')";//datetype
                        p7 = excelData[7];//int type id

                        //printing on screen ith row
                        excelData.ForEach(Console.WriteLine);

                        excelData.Clear();
                        db1.Insert(p1, p2, p3, p4, p5, p6, p7);



                    }
                }
            }








        }
    }
}
