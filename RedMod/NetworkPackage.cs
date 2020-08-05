using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedMod
{
    class NetworkPackage
    {
        public string data = "";
        private int id;


        public NetworkPackage(int id)
        {
            this.id = id;
            data += id + "|";
        }

        public static List<string> read_string_list(String data)
        {
            List<string> list = new List<string>();
            string[] data_split = data.Split('$');
            foreach(string s in data_split)
            {
                if(s != "" && s != null)
                {
                    list.Add(s);
                }
                
            }
            return list;
        }
        

        public void write(String data)
        {
            this.data += data + "|";
        }
        public void write_string_list(List<string> data)
        {
            string list = "";
            foreach(string s in data)
            {
                list += s + "$";
            }
            this.data += list + "|";
        }

    }
}
