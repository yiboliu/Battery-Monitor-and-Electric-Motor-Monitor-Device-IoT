using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using BatteryMotorMonitor.Models;

namespace BatteryMotorMonitor.HelperClasses
{
    public class BatteryMLService
    {
        private static string username = "RunFranks525";
        private static string password = "1048Boston";
        private static string datasource = "iotbatterydatabase.database.windows.net";
        public static async Task<string> GetMLResponse()
        {
            SqlConnectionStringBuilder connectionString = buildConnectionString();
            List<BatteryTable> dataPoints = getDatabaseValues(connectionString);
            //var scoreRequest = buildScoreRequest(dataPoints);
            var scoreRequest = new
            {
                Inputs = new Dictionary<string, List<Dictionary<string, string>>>() {
                        {
                            "input1",
                            new List<Dictionary<string, string>>(){new Dictionary<string, string>(){
                                            {
                                                "DateStamp", "12/2/2016"
                                            },
                                            {
                                                "BatteryTemperature", "1"
                                            },
                                            {
                                                "BatteryCurrent", "1"
                                            },
                                            {
                                                "BatteryVoltage", "1"
                                            },
                                            {
                                                "BatteryPower", "1"
                                            },
                                }
                            }
                        },
                    },
                GlobalParameters = new Dictionary<string, string>()
                {
                }
            };
            using (var client = new HttpClient())
            {
                const string apiKey = "sZ3f8vVE0iUD0FPHC7lhAZ2sBf1zPkeJWYQwqgTKy1a1zQjlQljdnk9tQOzfu9kdYCqBdPN0Q4+5FgrkBjub4A==";
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/subscriptions/4769386f1cdf4fb6bc3250b9336fc5bc/services/70ccf66e2ea8486baf7919364135d381/execute?api-version=2.0&format=swagger");

                HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return result;
                }
                else
                {
                    return string.Format("The request failed with status code: {0}", response.StatusCode);

                    //// Print the headers - they include the request ID and the timestamp,
                    //// which are useful for debugging the failure
                    //Console.WriteLine(response.Headers.ToString());

                    //string responseContent = await response.Content.ReadAsStringAsync();
                    //Console.WriteLine(responseContent);
                }
            }
        }

        private static SqlConnectionStringBuilder buildConnectionString()
        {
            SqlConnectionStringBuilder connString1Builder = new SqlConnectionStringBuilder();
            connString1Builder.DataSource = datasource;
            connString1Builder.InitialCatalog = "IoTBatteryDatabase";
            connString1Builder.Encrypt = true;
            connString1Builder.TrustServerCertificate = false;
            connString1Builder.UserID = username;
            connString1Builder.Password = password;
            connString1Builder.ConnectTimeout = 30;
            return connString1Builder;
        }

        private static Object buildScoreRequest(List<BatteryTable> dataPoints)
        {
            Dictionary<string, List<Dictionary<string, string>>> inputs = new Dictionary<string, List<Dictionary<string, string>>> ();
            Dictionary<string, string> GlobalParameters = new Dictionary<string, string>();
            for (int i = 0; i < dataPoints.Count; i++)
            {
                string inputValue = String.Format("input{0}", i);
                inputs.Add(inputValue, new List<Dictionary<string, string>>()
                {
                    new Dictionary<string, string>()
                    {
                        {
                            "DateStamp", dataPoints[i].DateStamp.ToString()
                        },
                        {
                            "BatteryTemperature", dataPoints[i].BatteryTemperature.ToString()
                        },
                        {
                            "BatteryCurrent", dataPoints[i].BatteryCurrent.ToString()
                        },
                        {
                            "BatteryVoltage", dataPoints[i].BatteryVoltage.ToString()
                        },
                        {
                            "BatteryPower", dataPoints[i].BatteryPower.ToString()
                        }
                    }
                });
            }
            var scoreRequestObj = new
            {
                inputs,
                GlobalParameters
            };
            return scoreRequestObj;
        }

        private static List<BatteryTable> getDatabaseValues(SqlConnectionStringBuilder connectionString)
        {
            List<BatteryTable> dataPoints = new List<BatteryTable>();
            using (SqlConnection conn = new SqlConnection(connectionString.ToString()))
            {
                using (SqlCommand command = conn.CreateCommand())
                {
                    conn.Open();
                    string query = "Select * From dbo.BatteryTable";
                    string commandText = query;
                    command.CommandText = commandText;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        BatteryTable dataPoint = new BatteryTable()
                        {
                            DateStamp = reader.GetDateTime(0),
                            BatteryTemperature = reader.GetInt32(1),
                            BatteryCurrent = (double) reader.GetSqlDouble(2),
                            BatteryVoltage = (double) reader.GetSqlDouble(3),
                            BatteryPower = (double) reader.GetSqlDouble(4)
                        };
                        dataPoints.Add(dataPoint);
                    }
                }
            }
            return dataPoints;
        }
    }
}
