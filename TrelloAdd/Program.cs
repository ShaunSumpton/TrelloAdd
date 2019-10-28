using Manatee.Trello;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TrelloAdd
{
    class Program
    {

        
        static async System.Threading.Tasks.Task Main(string[] args)
        {

        
            ArrayList JobDetails = new ArrayList();




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

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())




                        while (JobDetails.Count < 5)
                        {
                            while (reader.Read()) //Loop through SQL reader //
                            {

                                switch (JobDetails.Count)
                                {


                                    case 0:

                                        JobDetails.Add(reader[0]);
                                        JobDetails.Add(reader[1]);
                                        JobDetails.Add(reader[2]);
                                        JobDetails.Add(reader[3]);
                                        JobDetails.Add(reader[4]);
                                        break;

                                    case 5:
                                        JobDetails.Clear();
                                        JobDetails.Add(reader[0]);
                                        JobDetails.Add(reader[1]);
                                        JobDetails.Add(reader[2]);
                                        JobDetails.Add(reader[3]);
                                        JobDetails.Add(reader[4]);

                                        break;

                                }

                                if (reader.GetValue(5).ToString() != "")
                                {
                                    JobDetails.Add(reader[5]); // Proof Date //
                                }


                            }

                        }
                }


                TrelloAuthorization.Default.AppKey = "234d8eb40d3f3133b0812df057f7bdc3"; // Trello API key //
                TrelloAuthorization.Default.UserToken = "0e956ba7f0000d7ca7db8504e58a3301d45102e400297f230bfbdda2acc30e1e"; // Trello UserToken //


                ITrelloFactory factory = new TrelloFactory();    // Get Trello board using board ID//
                var board = factory.Board("5db19603e4428377d77963b1");
                await board.Refresh();

                var TDList = factory.List("5db19603e4428377d77963b2");
                await TDList.Refresh();

                var newCard = TDList.Cards.Add(JobDetails[0].ToString().Trim() + " - " + JobDetails[2].ToString().Trim());

                var Card = factory.Card(newCard.Result.Id);
                Card.DueDate = (DateTime)JobDetails[4];

                var desc = board.CustomFields.FirstOrDefault(f => f.Name == ":.");
                var field = desc.SetValueForCard(Card, JobDetails[3].ToString());



                if (JobDetails.Count == 6) // * UPDATE TO CUSTOM FIELDS *//
                {

                    var ProofDate = board.CustomFields.FirstOrDefault(f => f.Name == "Proof Date");
                    var field1 = ProofDate.SetValueForCard(Card,(DateTime)JobDetails[5]);
                }

                await Card.Refresh();
                await Card.Labels.Refresh();

            }

            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e);

            }





        }
    }
}






