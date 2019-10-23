using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using TrelloNet;

namespace TrelloAdd
{
    class Program
    {

       
      
     
         

        static void Main(string[] args)
        {

            string[] JobDetails;
            JobDetails = new string[6];

            using (StreamWriter sw = new StreamWriter(@"C:\Users\Sumptons\Desktop\Temp.txt", true))
            

                try
            {

                Console.WriteLine("Enter Pace Job Number");
                string jn = Console.ReadLine();
                int i = 0;


                var connString = "Host=6.1.1.13;Username=epace_read;Password=epace;Database=epace";

                using


        (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();

                        // Retrieve all rows
                        using (var cmd = new NpgsqlCommand("SELECT job.ccmasterid, job.amounttoinvoice, Customer.arcustname, job.ccdescription, job.ccscheduledshipdate, jobpart.ccact2date FROM job INNER Join customer ON job.armasterid = customer.armasterid INNER Join jobpart ON job.ccmasterid = jobpart.ccmasterid WHERE  job.ccmasterid =" + jn, conn))
                        using (var reader = cmd.ExecuteReader())

                            //string[] JobDetails = new string[4];

                        while (reader.Read())
                            {

                             

                                JobDetails[i] = reader.GetValue(i++).ToString();

                                if (reader.GetValue(5).ToString() != "")
                                {
                                    JobDetails[5] = reader.GetValue(5).ToString();
                                }
                               

                            }
                      
                    }


                    
                }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e);

            }

                    

            

            }



            







        }
    }

