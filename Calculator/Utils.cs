using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Calculator
{
    class Utils
    {
        public enum ConnectionStatus
        {
            NotConnected,
            LimitedAccess,
            Connected
        }

        public static ConnectionStatus CheckAPIConnection()
        {
            try
            {
                IPHostEntry entry = Dns.GetHostEntry("api.wolframalpha.com");
                if (entry.AddressList.Length == 0)
                {
                    return ConnectionStatus.NotConnected;
                }
                else
                {
                    if (!entry.AddressList[0].ToString().Equals("206.123.112.135"))
                        return ConnectionStatus.LimitedAccess;
                    else
                        return ConnectionStatus.Connected;
                }
            }
            catch
            {
                return ConnectionStatus.NotConnected;
            }
        }
    }
}
