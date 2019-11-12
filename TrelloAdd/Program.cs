using Manatee.Trello;
using Npgsql;
using System;
using System.Collections;
using System.Linq;

namespace TrelloAdd
{
    class Program
    {


        static async System.Threading.Tasks.Task Main(string[] args)
        {

            string SQLPRoofDate;
            ArrayList JobDetails = new ArrayList();
            bool done = false;



            try
            {
                do
                {

                    Console.WriteLine("Enter Pace Job Number");
                    string jn = Console.ReadLine();
                    int i = 0;
                    JobDetails.Clear();


                    var connString = "Host=6.1.1.13;Username=epace_read;Password=epace;Database=epace"; // ** Connection Details ** // 

                    //var conn1 = @"Data Source=AG00072\SQLEXPRESS; Database=TrelloJobs; Connection Timeout=30;Integrated Security=SSPI;";

                    using (var conn = new NpgsqlConnection(connString))
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
                                            JobDetails.Insert(0, reader[0]);
                                            JobDetails.Insert(1, reader[1]);
                                            JobDetails.Insert(2, reader[2]);
                                            JobDetails.Insert(3, reader[3]);
                                            JobDetails.Insert(4, reader[4]);

                                            break;

                                    }

                                    if (reader.GetValue(5).ToString() != "")
                                    {
                                        JobDetails.Add(reader[5]); // Proof Date //
                                    }


                                }

                            }
                    }

                    //using (var conn2 = new SqlConnection(conn1))
                    // {
                    // conn2.Open();



                    if (JobDetails.Count == 5)
                    {
                        SQLPRoofDate = "N/A";

                    }
                    else
                    {
                        SQLPRoofDate = JobDetails[5].ToString();

                    }

                    //  using (var cmdd = new SqlCommand("INSERT INTO Jobs VALUES ('" + JobDetails[0] + "','" + JobDetails[2] + "','" + JobDetails[3] + "','" + SQLPRoofDate + "','To Start','" + JobDetails[4] + "')", conn2))
                    // using (SqlDataReader r1 = cmdd.ExecuteReader()) ;




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



                    if (JobDetails.Count == 6)
                    {

                        var ProofDate = board.CustomFields.FirstOrDefault(f => f.Name == "Proof Date");
                        var field1 = ProofDate.SetValueForCard(Card, (DateTime)JobDetails[5]);
                    }

                    await Card.Refresh();
                    await Card.Labels.Refresh();

                    Console.WriteLine("Type N to input another Job Number or X to Exit");
                    var e = Console.ReadLine();

                    if (e == "X")
                    {
                        done = true;
                    }


                } while (done != true);
            }




            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e);

            }
        }
    }
}












