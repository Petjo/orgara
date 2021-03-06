﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Xml;
using System.Xml.Linq;

namespace Organizer
{

    public partial class Window_save_station : Window
    {
        MainWindow MainWindow;
        List_Stations List_Stations;
        private string string_last_id;
        private string string_new_id;

        public Window_save_station(MainWindow mw, List_Stations ls)
        {
            MainWindow = mw;
            List_Stations = ls;
            InitializeComponent();
            this.DataContext = List_Stations;
            List_Stations.Request_answer = "Add here new radio station";
        } 

        private void btn_save_new_station_data_Click(object sender, RoutedEventArgs e)
        {
            List_Stations.Request_answer = "Testing URL...";

            Check_empty_fields();

            if (Check_if_station_is_doubled() != false)
            {
                MessageBox.Show("Station name or Stream URL is doubled! Please change name and/or URL.");
                return;
            }

            Get_new_id();

            //Add_new_entry_to_XML();

            Add_new_station_to_bindinglist();
        }

        private void Add_new_station_to_bindinglist()
        {
            Organizer_MainView omv = new Organizer_MainView();

            omv.Id = string_new_id;
            omv.Standard = "false";
            omv.Station_Name = tb_add_new_name.Text;
            omv.Station_homepage = tb_add_new_homepage.Text;
            omv.Url = tb_add_new_stream_url.Text;
            omv.Genre = tb_add_new_genre.Text;
            omv.Comment = tb_add_comment.Text;
            MainWindow.count_stations++;
            omv.Number_of_stations = MainWindow.count_stations;
            
            List_Stations.Stream_data_list.Add(new Get_Stream_Information(omv, List_Stations, 0));
            Thread.Sleep(2000);

            if (List_Stations.If_valid != true)
            {
                MessageBox.Show("Invalid MP3 stream link!");
                List_Stations.Request_answer = "Invalid link";
                return;
            }
            else
            {
                List_Stations.Request_answer = "Testing URL was succesful.";
                List_Stations.Stations_data_List.Add(omv);
            }

           
        }

        private void Check_empty_fields()
        {
            if (String.IsNullOrEmpty(tb_add_new_name.Text) && !String.IsNullOrEmpty(tb_add_new_stream_url.Text))
            {
                MessageBox.Show("Name of station can't be clear!");
                return;
            }

            if (!String.IsNullOrEmpty(tb_add_new_name.Text) && String.IsNullOrEmpty(tb_add_new_stream_url.Text))
            {
                MessageBox.Show("Link of station can't be clear!");
                return;
            }

            if (String.IsNullOrEmpty(tb_add_new_name.Text) && String.IsNullOrEmpty(tb_add_new_stream_url.Text))
            {
                MessageBox.Show("Stationname and stationlink can't be clear!");
                return;
            }
        }

        private bool Check_if_station_is_doubled()
        {
            bool isdoubled = false;

            for (int i = 0; i < List_Stations.Stations_data_List.Count; i++)
            {
                if (List_Stations.Stations_data_List[i].Station_Name == tb_add_new_name.Text || List_Stations.Stations_data_List[i].Url == tb_add_new_stream_url.Text)
                {
                    isdoubled = true;
                    break;
                }
                else
                {
                    isdoubled = false;
                }
            }

            return isdoubled;
        }

        private void Get_new_id()
        {

            XmlDocument doc = new XmlDocument();
            XmlNodeList elemList = doc.GetElementsByTagName("station");
            List<string> list_id = new List<string>();
            int id;

            doc.Load(List_Stations.XML_Document_Name);

            for (int i = 0; i < elemList.Count; i++)
            {
                list_id.Add(elemList[i].Attributes["id"].Value);
            }

            List<int> list_int = liststring_to_listint(list_id);
            string_last_id = Convert.ToString((id = return_id(list_int) - 1)); //-1 because of last node
            string_new_id = Convert.ToString(id = return_id(list_int));
        }

        private void Add_new_entry_to_XML()
        {

            var xml = XDocument.Load(List_Stations.XML_Document_Name);

            xml.Root                                                                              
                .Elements("station")                                                               
                .FirstOrDefault(r => r.Attribute("id").Value == string_last_id)                    
                .AddAfterSelf(new XElement("station",                                               
                                new XAttribute("id", string_new_id),
                                new XAttribute("standard", "false"),                                
                                new XElement("station_name", tb_add_new_name.Text),
                                new XElement("station_homepage", tb_add_new_name.Text),
                                new XElement("link", tb_add_new_stream_url.Text),
                                new XElement("comment", tb_add_comment.Text),
                                new XElement("genre", tb_add_new_genre.Text)
                                ));

            xml.Save(List_Stations.XML_Document_Name);
            string_last_id = null;
            string_new_id = null;
        }

        private List<int> liststring_to_listint(List<string> list)
        {
            List<int> list_int = new List<int>();

            for (int i = 0; i < list.Count; i++)
            {
                list_int.Add(Convert.ToInt32(list[i]));
            }

            return list_int;
        }

        private int return_id(List<int> list_int)
        {
            int a = 1;
            int b = a;

            for (int i = 0; i < list_int.Count; i++)
            {
                if (a == list_int[i])
                {
                    a++;
                }
                else
                {
                    break;
                }
            }

            return a;
        }

        private void btn_back_to_main_app_Click(object sender, RoutedEventArgs e)
        {
            List_Stations.Request_answer = "Add here new radio station";
            this.Close();
        }
    }
}
