﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MetisMercury.Classes;

namespace MetisMercury.DatabaseClasses
{
    class Visitor_DataHelper:DataHelper
    {
        
        public void CheckOut(string RFID)
        {
            MySqlCommand command = new MySqlCommand("UPDATE VISITOR SET RFID = NULL WHERE RFID = '" + RFID + "'", connection);

            try
            {
                connection.Open();
                MySqlDataReader r = command.ExecuteReader();

                r.Read();
                command.ExecuteScalar();
            }
            catch
            {
                MessageBox.Show("error occurred while checkout.");
            }
            finally
            {
                connection.Close();
            }
        }

       
        public int GetPresentParticipants()
        {
            MySqlCommand command = new MySqlCommand("SELECT COUNT(*) FROM VISITOR WHERE RFID IS NOT NULL", connection);

            try
            {
                connection.Open();
                MySqlDataReader r = command.ExecuteReader();

                r.Read();

                return Convert.ToInt32(command.ExecuteScalar());
            }
            catch
            {
                //in case if there is an error
                return -1;
            }
            finally
            {
                connection.Close();
            }
        }

       
        public int CalculateTotalBalance()
        {
            MySqlCommand command = new MySqlCommand("SELECT SUM(PRESENTBALANCE) FROM VISITOR", connection);

            try
            {
                connection.Open();
                MySqlDataReader r = command.ExecuteReader();

                r.Read();

                return Convert.ToInt32(command.ExecuteScalar());
            }
            catch
            {
                //in if there is an error
                return -1;
            }
            finally
            {
                connection.Close();
            }
        }

        
        public Visitor GetVisitor(string RFID)
        {
            MySqlCommand command = new MySqlCommand("SELECT * FROM VISITOR WHERE RFID='" + RFID + "'", connection);
            Visitor Visitor = null;

            try
            {
                connection.Open();
                MySqlDataReader r = command.ExecuteReader();

                r.Read();

                int EventID = Convert.ToInt16(r["EVENTID"]);
                string rfid = r["RFID"].ToString();

                string lName = r["LNAME"].ToString();
                string fName = r["FNAME"].ToString();
                string email = r["EMAIL"].ToString();

                bool CurrentlyCheckedIn;

                if (r["ISCHECKEDIN"].ToString() == "NO")
                {
                    CurrentlyCheckedIn = false;
                }
                else
                {// YES
                    CurrentlyCheckedIn = true;
                }

                bool hasCheckedIn;
                if (r["HASCHECKEDIN"].ToString() == "NO")
                {
                    hasCheckedIn = false;
                }
                else
                {// YES
                    hasCheckedIn = true;
                }

                bool statusOfPayment;
                if (r["STATUSOFPAYMENT"].ToString() == "NO")
                {
                    statusOfPayment = false;
                }
                else
                {// YES
                    statusOfPayment = true;
                }

                decimal PresentBalance = Convert.ToDecimal(r["PRESENTBALANCE"]);

                int spotID;
                if (r["SPOTID"] == DBNull.Value)
                {// The visitor doesn't have a camping spot.
                    spotID = 0;
                }
                else
                {// if participant books camping spot
                    spotID = Convert.ToInt32(r["SPOTID"]);
                }

                int boatid;
                if (r["boatid"] == DBNull.Value)
                {
                    boatid = 0;
                }
                else
                {
                    boatid = Convert.ToInt32(r["boatid"]);
                }

                return Visitor = new Visitor(EventID,PresentBalance, fName, lName, email, CurrentlyCheckedIn, hasCheckedIn, statusOfPayment, rfid, spotID,boatid);
            }

            catch (Exception)
            {
                MessageBox.Show("Error");
            }
            finally
            {
                connection.Close();
            }

            return Visitor;
        }

        /// <summary>
        /// Retrieves details about a participant under a given Event ID.
        /// </summary>
        /// <param name="ID">Given account ID.</param>
        /// <returns>Participant</returns>
        public Visitor GetAVisitor(int ID)
        {
          //  String eventnr = Convert.ToString(ID);
            MySqlCommand command = new MySqlCommand("SELECT * FROM VISITOR WHERE EVENTID=" + ID, connection );
            Visitor Visitor = null;

            try
            {
                connection.Open();
                MySqlDataReader r = command.ExecuteReader();

                r.Read();

                int eventid = Convert.ToInt32(r["EVENTID"]);
                string rfid = r["RFID"].ToString();

                string lName = r["LNAME"].ToString();
                string fName = r["FNAME"].ToString();
                string email = r["EMAIL"].ToString();

                bool isCheckedIn;

                if (r["ISCHECKEDIN"].ToString() == "NO")
                {
                    isCheckedIn = false;
                }
                else
                {// YES
                    isCheckedIn = true;
                }

                bool hasCheckedIn;
                if (r["HASCHECKEDIN"].ToString() == "NO")
                {
                    hasCheckedIn = false;
                }
                else
                {// YES
                    hasCheckedIn = true;
                }

                bool statusOfPayment;

                if (r["STATUSOFPAYMENT"].ToString() == "NO")
                {
                    statusOfPayment = false;
                }
                else
                {// YES
                    statusOfPayment = true;
                }

                decimal currentBalance = Convert.ToDecimal(r["PRESENTBALANCE"]);

                int spotID;
                if (r["SPOTID"] == DBNull.Value) 
                {// in case there is no camping spot
                    spotID = 0;
                }
                else
                {// The participant/co-camper has a camping spot.
                    spotID = Convert.ToInt32(r["SPOTID"]);
                }
                int boatid;
                if (r["boatid"] == DBNull.Value)
                {
                    boatid = 0;
                }
                else
                {
                    boatid = Convert.ToInt32(r["boatid"]);
                }


                return Visitor = new Visitor(eventid, currentBalance, fName, lName, email, isCheckedIn, hasCheckedIn, statusOfPayment,rfid,spotID,boatid);
            }
            catch
            {
                MessageBox.Show("Error occurred.");
            }
            finally
            {
                connection.Close();
            }

            return Visitor; // no visitor found.
        }

       
        /// get all visitor details from the database.
      
       
        public List<Visitor> GetAllVisitors()
        {
            MySqlCommand command = new MySqlCommand("SELECT * FROM VISITOR WHERE RFID=", connection);

            List<Visitor> Nrrows = new List<Visitor>();

            try
            {
                connection.Open();
                MySqlDataReader r = command.ExecuteReader();

                while (r.Read())
                {
                    int EventID = Convert.ToInt16(r["EVENTID"]);
                    string RFID = r["RFID"].ToString();

                    string lName = r["LNAME"].ToString();
                    string fName = r["FNAME"].ToString();
                    string email = r["EMAIL"].ToString();

                    bool isCheckedIn;
                    if (r["ISCHECKEDIN"].ToString() == "NO")
                    {
                        isCheckedIn = false;
                    }
                    else
                    {
                        isCheckedIn = true;
                    }

                    bool hasCheckedIn;
                    if (r["HASCHECKEDIN"].ToString() == "NO")
                    {
                        hasCheckedIn = false;
                    }
                    else
                    {
                        hasCheckedIn = true;
                    }

                    bool StatusOfPayment;

                    if (r["STATUSOFPAYMENT"].ToString() == "NO")
                    {
                        StatusOfPayment = false;
                    }
                    else
                    {
                        StatusOfPayment = true;
                    }

                    decimal PresentBalance = Convert.ToDecimal(r["PRESENTBALANCE"]);

                    int spotID;
                    if (r["SPOTID"] == DBNull.Value) 
                    {
                        spotID = 0; // The VISITOR doesn't have a camping spot.
                    }
                    else
                    {
                        spotID = Convert.ToInt32(r["SPOTID"]);
                    }

                    int boatid;

                    if (r["boatid"] == DBNull.Value)
                    {
                        boatid = 0;
                    }
                    else
                    {
                        boatid = Convert.ToInt32(r["boatid"]);
                    }


                    Nrrows.Add(new Visitor(EventID, PresentBalance, fName, lName, email, isCheckedIn, hasCheckedIn, StatusOfPayment, RFID, spotID,boatid));
                }
            }
            catch
            {
                MessageBox.Show("Error occur while getting all participant");
            }
            finally
            {
                connection.Close();
            }

            return Nrrows;
        }

        /// <summary>
        /// Assigns the RFID tag number to the participant and updates their check-in status.
       
        /// <returns>Check-in status</returns>
        public bool UpdateRFIDStatus(string TagNr)
        {
            //if (visitor.IsChekedin)
            //{// visitor is already inside the event.
            //    return false;
            //}

            MySqlCommand command = new MySqlCommand("UPDATE VISITOR SET RFID = '" + TagNr + "', ISCHECKEDIN = '0' WHERE EVENTID = " + 0, connection);

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Error updating status of RFID.");
            }
            finally
            {
                connection.Close();
            }

            return true;
        }

        public int AssignTheRFID(string rfid)
        {   //Probably you expected a return-value of type bool:
            //true if the query was executed succesfully and false otherwise.
            //But what if you executed a delete-query? Or an update-query?
            //The return-value is teh number of records affected.
            string Query = string.Format("INSERT INTO RFID(RFID)" +
                 "VALUES({0})", rfid);
            MySqlCommand command = new MySqlCommand(Query, connection);

            try
            {
                connection.Open();
                int nrOfRecordsChanged = command.ExecuteNonQuery();
                return nrOfRecordsChanged;
            }
            catch
            {
                
                MessageBox.Show("Error");
                return -1;//which means the try-block was not executed succesfully, so  . . .
            }
            finally
            {
                connection.Close();
            }
        }

    }
}
