﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Diagnostics;
using System.IO;

namespace OnTrack.src.WebConnection
{
    /**
     *  @class WebHandler
     **/
    class WebHandler
    {
        private string method;
        private string postData;
        private byte[] postArray;
        private string contentType;

        /**
         *  @var Stream dataStream
         **/
        private Stream dataStream;

        private StreamReader reader;

        private string url;

        /**
         *  @var WebRequest request
         **/
        private WebRequest request;
        /**
         *  @var HttpWebResponse response
         **/
        private HttpWebResponse response;
        /**
         *  @var string responseData
         **/
        private string responseData;

        /**
         *  @note default constructor
         *  @return void
         **/
        public WebHandler(string url, string method, string postData)
        {
            this.url = url;
            this.method = method;
            this.postData = postData;
            this.postArray = Encoding.UTF8.GetBytes(this.postData);
            this.request = WebRequest.Create(url);
            this.request.Method = method;
            this.request.ContentType = "application/x-www-form-urlencoded";
            this.request.ContentLength = this.postArray.Length;
        }

        public void run() 
        {
            try {
                this.dataStream = this.request.GetRequestStream();
                this.dataStream.Write(postArray, 0, postArray.Length);
                this.dataStream.Close();
            } catch (WebException) {
                throw;
            }
        }

        public string getResponse()
        {
            string res = "";
            try
            {
                this.response = (HttpWebResponse)(this.request.GetResponse());
                this.dataStream = this.response.GetResponseStream();
                this.reader = new StreamReader(this.dataStream);
                res = this.reader.ReadToEnd();
                this.reader.Close();
                this.dataStream.Close();
                this.response.Close();
            }
            catch (WebException we) {
                Debug.WriteLine(we.Message);
                throw;
            } catch (ProtocolViolationException pve) {
                Debug.WriteLine(pve.Message);
                throw;
            } catch (ArgumentException ae) {
                Debug.WriteLine(ae.Message);
                throw;
            }
            return res;
        }

        public string getResponseStatusDescription()
        {
            this.response = (HttpWebResponse)(this.request.GetResponse());
            return this.response.StatusDescription;
        }

        /**
         *  @return string
         **/
        public string getResponseData()
        {
            return this.responseData;
        }
    }
}
