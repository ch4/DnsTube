﻿using System;
using System.Linq;
using System.Net.Http;
using System.Collections.Generic;

namespace DnsTube
{
	public class Utility
	{
        public static List<string> GetLocalIPAddresses()
        {
            var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            var addresses = new List<string>();
            foreach (var ip in host.AddressList)
            {
                //Filter for ipv4 addresses
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    addresses.Add(ip.ToString());
                }
            }
            return addresses;
        }

        public static string GetPublicIpAddress(HttpClient Client, out string errorMesssage)
		{
			string publicIpAddress = null;
			var maxAttempts = 3;
			var attempts = 0;
			errorMesssage = null;

			while (publicIpAddress == null && attempts < maxAttempts)
			{
				try
				{
					attempts++;
					var response = Client.GetStringAsync("http://ipv4.icanhazip.com/").Result;
					var candidatePublicIpAddress = response.Replace("\n", "");

					if (!IsValidIp4Address(candidatePublicIpAddress))
						throw new Exception($"Malformed response, expected IP address: {response}");

					publicIpAddress = candidatePublicIpAddress;
				}
				catch (Exception e)
				{
					if (attempts >= maxAttempts)
						errorMesssage = e.Message;
				}
			}
			return publicIpAddress;
		}

		public static string GetDateString()
		{
			return DateTime.Now.ToString("yyyy-MM-dd h:mm:ss tt");
		}

		public static bool IsValidIp4Address(string ipString)
		{
			if (String.IsNullOrWhiteSpace(ipString))
				return false;

			string[] splitValues = ipString.Split('.');
			if (splitValues.Length != 4)
				return false;

			byte tempForParsing;
			return splitValues.All(r => byte.TryParse(r, out tempForParsing));
		}
	}
}
