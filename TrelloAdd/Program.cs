using Manatee.Trello;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TrelloAdd
{
    class Program
    {

       
      
     
         

        static async System.Threading.Tasks.Task Main(string[] args)
        {

            string[] JobDetails;
            JobDetails = new string[6];

            using (StreamWriter sw = new StreamWriter(@"C:\Users\Sumptons\Desktop\Temp.txt", true))
            

                try
            {

                Console.WriteLine("Enter Pace Job Number");
                string jn = Console.ReadLine();
                int i = 0;


                var connString = "Host=6.1.1.13;Username=epace_read;Password=epace;Database=epace"; // ** Connection Details ** // 

                using


        (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();

                        // ** Connection string ** //
                        using (var cmd = new NpgsqlCommand("SELECT job.ccmasterid, job.amounttoinvoice, Customer.arcustname, job.ccdescription, job.ccscheduledshipdate, jobpart.ccact2date FROM job INNER Join customer ON job.armasterid = customer.armasterid INNER Join jobpart ON job.ccmasterid = jobpart.ccmasterid WHERE  job.ccmasterid =" + jn, conn))
                        using (var reader = cmd.ExecuteReader())

                           

                        while (reader.Read()) //Loop through SQL reader //
                            {

                                JobDetails[i] = reader.GetValue(i++).ToString(); // Add job details to array // 

                                if (reader.GetValue(5).ToString() != "")
                                {
                                    JobDetails[5] = reader.GetValue(5).ToString(); // Proof Date //
                                }
                               

                            }
                      
                    }

                    TrelloAuthorization.Default.AppKey = "234d8eb40d3f3133b0812df057f7bdc3"; // Trello API key //
                    TrelloAuthorization.Default.UserToken = "181b84018f936bc8eaec476c963c3d84c1c811047d9ee116d57890de536f3ee7"; // Trello UserToken //


                    ITrelloFactory factory = new TrelloFactory();    // Get Trello board using board ID//
                    var board = factory.Board("5db19603e4428377d77963b1");
                    await board.Refresh();

                    var TDList = factory.List("5db19603e4428377d77963b2");
                    await TDList.Refresh();

                    var newCard = TDList.Cards.Add("Test Card");
                  
                    var Card = factory.Card(newCard.Result.Id);
                    await Card.Refresh();



                   Console.WriteLine(TDList.Name);
                   Console.WriteLine(board.Name);
                                          // Disaply board name //



                }

            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e);

            }

                    

            

            }



            







        }
    }

