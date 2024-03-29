﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace ZenAIO
{
    public class Proxy
    {

        private WebProxy proxy;

        public Proxy() : this("", 0)
        {

        }

        public Proxy(string url, int port, WebProxy proxy = null)
        {
            this.URL = url;
            this.Port = port;
            this.proxy = proxy;
        }

        public string URL
        {
            get; set;
        }

        public int Port
        {
            get; set;
        }

        public string Username
        {
            get; set;
        }

        public string Password
        {
            get; set;
        }

        public bool HasAuthentication()
        {
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
        }

        public WebProxy GetWebProxy()
        {
            if (proxy == null)
            {
                // client = new RestClient(URL);
                proxy = new WebProxy(URL + ":" + Port, false);
            }

            return proxy;
        }

        public string GetPrettyString()
        {
            return URL + ":" + Port;
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            else if (obj == null || !(obj is Proxy))
                return false;

            Proxy other = obj as Proxy;

            return URL == other.URL && Port == other.Port && Username == other.Username && Password == other.Username;
        }

    }
}
