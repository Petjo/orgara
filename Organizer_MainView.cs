﻿
using System.ComponentModel;
using System.Xml;
using System.Xml.Linq;

namespace Organizer
{
    public class Organizer_MainView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }


        #region Common
        #endregion Common


        #region streaming

        private string received_bytes;
        private string streamposition;
        private string state;
        private string actual_stream_play_info;
        private bool data_recieved;
        private volatile bool close_stream;


        public string Received_bytes
        {
            get { return received_bytes; }
            set
            {
                if (received_bytes != value)

                {
                    received_bytes = value;
                    NotifyPropertyChanged("Received_bytes");
                }
            }
        }
        public string Streamposition
        {
            get { return streamposition; }
            set
            {
                if (streamposition != value)

                {
                    streamposition = value;
                    NotifyPropertyChanged("Streamposition");
                }
            }
        }
        public string State
        {
            get { return state; }
            set { state = value; NotifyPropertyChanged("State"); }
        }
        public string Actual_stream_play_info
        {
            get { return actual_stream_play_info; }
            set { actual_stream_play_info = value; NotifyPropertyChanged("Actual_stream_play_info"); }
        }

        public bool Data_recieved
        {
            get { return data_recieved; }
            set { data_recieved = value; NotifyPropertyChanged("Data_recieved"); }
        }
        public bool Close_stream
        {
            get { return close_stream; }
            set { close_stream = value; NotifyPropertyChanged("Close_stream"); }
        }

        #endregion streaming

        #region XML

        //Declaration
        private string _XML_Document_Name;
        private XmlDocument _XML_Document;
        private XDocument _X_Doc;

        public string XML_Document_Name
        {
            get { return _XML_Document_Name; }
            set { _XML_Document_Name = value; NotifyPropertyChanged("XML_Document_Name"); }
        }
        public XmlDocument XML_Document
        {
            get { return _XML_Document; }
            set
            {
                _XML_Document = value; NotifyPropertyChanged("XML_Document");
            }
        }
        public XDocument XDoc
        {
            get { return _X_Doc; }
            set { _X_Doc = value; NotifyPropertyChanged("XDoc"); }
        }

        //Values Elements
        private string _Station_Name;
        private string station_homepage;
        private string url;
        private string comment;
        private string genre;
        private int number_of_stations;


        public string Station_Name
        {
            get { return _Station_Name; }
            set { _Station_Name = value; NotifyPropertyChanged("Station_Name"); }
        }
        public string Station_homepage
        {
            get { return station_homepage; }
            set { station_homepage = value; NotifyPropertyChanged("Station_homepage"); }
        }
        public string Url
        {
            get { return url; }
            set
            {
                if (url != value)

                {
                    url = value;
                    NotifyPropertyChanged("Url");
                }
            }
        }
        public string Comment
        {
            get { return comment; }
            set { comment = value; NotifyPropertyChanged("Comment"); }
        }
        public string Genre
        {
            get { return genre; }
            set { genre = value; NotifyPropertyChanged("Genre"); }
        }
        public int Number_of_stations
        {
            get { return number_of_stations; }
            set { number_of_stations = value; NotifyPropertyChanged("Number_of_stations"); }
        }

        //Values Attributes
        private string standard_Station_Id;
        private string id;
        private string standard;

        public string Standard_Station_Id
        {
            get { return standard_Station_Id; }
            set { standard_Station_Id = value; NotifyPropertyChanged("Standard_Station_Id"); }
        }
        public string Id
        {
            get { return id; }
            set { id = value; NotifyPropertyChanged("Id"); }
        }
        public string Standard
        {
            get { return standard; }
            set { standard = value; NotifyPropertyChanged("Standard"); }
        }

        #endregion XML
    }
}
