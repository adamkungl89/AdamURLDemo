using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdamURL.Models
{
    public class ShortUrl
    {
        public int id { get; set; }
        public string longUrl { get; set; }
        public string key { get; set; }

        public ShortUrl(string longUrl)
        {
            this.longUrl = longUrl;
            this.key = generateKey();
        }

        public ShortUrl()
        {}

        private string generateKey()
        {
            string newKey;
            string range = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            char[] buildToken = new char[8];
            Random r = new Random();

            for (int i = 0; i < buildToken.Length; i++)
            {
                buildToken[i] = range[r.Next(range.Length)];
            }
            newKey = new string(buildToken);
            return newKey;
        }
    }
}
