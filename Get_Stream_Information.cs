﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Organizer
{
    public class Get_Stream_Information
    {
        List_Stations List_Stations;
        Organizer_MainView Omv;
        public Thread getdata;
        int select;
        string test_url;

        public Get_Stream_Information(Organizer_MainView omv, List_Stations li, int i)
        {
            select = i;
            Omv = omv;
            List_Stations = li;

            if (select == 0)
            {
                getdata = new Thread(Get_Data_actual_Stream);
                getdata.Start();
            }
            if (select == 1)
            {
                Thread getdata2 = new Thread(Get_Data_actual_Stream2);
                getdata2.Start();
            }
        }

        public Get_Stream_Information(string link, List_Stations ls)
        {
            test_url = link;
            List_Stations = ls;
            List_Stations.Request_answer = "Looking for request...";
            Thread t = new Thread(test_url_for_request);
            t.Start();
            t.Join();
        }


        //Get stream information
        public void Get_Data_actual_Stream()
        {
            String server = Omv.Url;
            String serverPath = "/";

            HttpWebRequest request = null; 
            HttpWebResponse response = null; 

            int metaInt = 0; 
            int count = 0; 
            int metadataLength = 0; 
            
            string metadataHeader = "";
            string oldMetadataHeader = null;

            byte[] buffer = new byte[512]; 
            Stream socketStream = null;
            Stream byteOut = null;

            try
            {
                request = (HttpWebRequest)WebRequest.Create(server);
                List_Stations.If_valid = true;
            }
            catch (WebException wex)
            {
                MessageBox.Show("Web Exception! Request failed: " + wex.ToString());
                List_Stations.If_valid = false;
                return;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Exception! Request failed: " + ex.Message);
                List_Stations.If_valid = false;
                return;
            }

            request.Headers.Clear();
            request.Headers.Add("GET", serverPath + " HTTP/1.0");
            request.Headers.Add("Icy-MetaData", "1");
            request.UserAgent = "WinampMPEG/5.09";

            try
            {
                response = (HttpWebResponse)request.GetResponse();
                List_Stations.If_valid = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception! Response failed: " + ex.Message);
                List_Stations.If_valid = false;
                return;
            }

            metaInt = Convert.ToInt32(
                      response.GetResponseHeader("icy-metaint"));

            try
            {
                socketStream = response.GetResponseStream();

                while (true && !List_Stations.End_get_stream_data)
                {
                    int bytes = socketStream.Read(buffer,
                                             0, buffer.Length);
                    if (bytes < 0)
                        return;

                    for (int i = 0; i < bytes; i++)
                    {
                        if (metadataLength != 0)
                        {
                            metadataHeader += Convert.ToChar(buffer[i]);
                            metadataLength--;

                            if (metadataLength == 0)
                            {
                                string fileName = "";

                                if (!metadataHeader.Equals(oldMetadataHeader))
                                {
                                    if (byteOut != null)
                                    {
                                        byteOut.Flush();
                                        byteOut.Close();
                                    }


                                    string[] actual_stream_info = Regex.Split(metadataHeader, "'");

                                    Omv.Actual_stream_play_info = actual_stream_info[1];
                                    Omv.Data_recieved = true;
                                    oldMetadataHeader = metadataHeader;
                                }
                                metadataHeader = "";
                            }
                        }
                        else
                        {
                            if (count++ < metaInt) 
                            {

                                if (byteOut != null)
                                {
                                    byteOut.Write(buffer, i, 1);
                                    if (count % 100 == 0)
                                        byteOut.Flush();
                                }
                            }
                            else
                            {
                                metadataLength = Convert.ToInt32(buffer[i]) * 16;
                                count = 0;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Stream Error: "+ ex.Message);
            }
            finally
            {
                if (byteOut != null)
                    byteOut.Close();
                if (socketStream != null)
                    socketStream.Close();
            }
        }

        // not neccessary/maybe for later 


        public void Get_Data_actual_Stream2()
        {
            String server = Omv.Url;
            String serverPath = "/";

            HttpWebRequest request = null;
            HttpWebResponse response = null; 

            int metaInt = 0; 
            int count = 0; 
            int metadataLength = 0; 

            string metadataHeader = "";
            string oldMetadataHeader = null;

            byte[] buffer = new byte[512];

            Stream socketStream = null;
            Stream byteOut = null;

            request = (HttpWebRequest)WebRequest.Create(server);
            request.Headers.Clear();
            request.Headers.Add("GET", serverPath + " HTTP/1.0");
            request.Headers.Add("Icy-MetaData", "1");
            request.UserAgent = "WinampMPEG/5.09";

            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            metaInt = Convert.ToInt32(
                      response.GetResponseHeader("icy-metaint"));

            try
            {
                socketStream = response.GetResponseStream();

                while (true && !Omv.Close_stream)
                {
                    int bytes = socketStream.Read(buffer,
                                             0, buffer.Length);
                    if (bytes < 0)
                        return;

                    for (int i = 0; i < bytes; i++)
                    {
                        if (metadataLength != 0)
                        {
                            metadataHeader += Convert.ToChar(buffer[i]);
                            metadataLength--;

                            if (metadataLength == 0)
                            {
                                string fileName = "";

                                if (!metadataHeader.Equals(oldMetadataHeader))
                                {
                                    if (byteOut != null)
                                    {
                                        byteOut.Flush();
                                        byteOut.Close();
                                    }

                                    string[] actual_stream_info = Regex.Split(metadataHeader, "'");
                                    Omv.Actual_stream_play_info = actual_stream_info[1];
                                    Omv.Data_recieved = true;
                                    oldMetadataHeader = metadataHeader;
                                }
                                metadataHeader = "";
                            }
                        }
                        else
                        {
                            if (count++ < metaInt) 
                            {
                                if (byteOut != null)
                                {
                                    byteOut.Write(buffer, i, 1);
                                    if (count % 100 == 0)
                                        byteOut.Flush();
                                }
                            }
                            else
                            {
                                metadataLength = Convert.ToInt32(buffer[i]) * 16;
                                count = 0;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (byteOut != null)
                    byteOut.Close();
                if (socketStream != null)
                    socketStream.Close();
            }
        }
        private Stream createNewFil2e(String destPath,
                                        String filename)
        {
            // remove characters, that are not allowed
            // in filenames. (quick and dirrrrrty ;) )
            filename = filename.Replace(":", "");
            filename = filename.Replace("/", "");
            filename = filename.Replace("\\", "");
            filename = filename.Replace("<", "");
            filename = filename.Replace(">", "");
            filename = filename.Replace("|", "");
            filename = filename.Replace("?", "");
            filename = filename.Replace("*", "");
            filename = filename.Replace("\"", "");

            try
            {
                // create directory, if it doesn't exist
                if (!Directory.Exists(destPath))
                    Directory.CreateDirectory(destPath);

                // create new file
                if (!File.Exists(destPath + filename + ".mp3"))
                {
                    return File.Create(destPath + filename + ".mp3");
                }
                // if file already exists, don't overwrite it. Instead,
                // create a new file named <filename>(i).mp3
                else
                {
                    for (int i = 1; ; i++)
                    {
                        if (!File.Exists(destPath + filename +
                                                  "(" + i + ").mp3"))
                        {
                            return File.Create(destPath + filename +
                                                      "(" + i + ").mp3");
                        }
                    }
                }
            }
            catch (IOException)
            {
                return null;
            }
        }



        public void test_url_for_request()
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            String server = test_url;

            try
            {
                List_Stations.Request_answer = "Looking for request...";
                request = (HttpWebRequest)WebRequest.Create(server);
                List_Stations.If_valid = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                List_Stations.If_valid = false;
            }
            try
            {
                List_Stations.Request_answer = "Looking for response...";
                response = (HttpWebResponse)request.GetResponse();
                List_Stations.If_valid = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                List_Stations.If_valid = false;
            }
        }
    }
}

