using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MainWindow.scheme
{
    class NameDriver
    {
        private List<List<int>> names;
        private List<string> prefixs;

        public NameDriver()
        {
            names = new List<List<int>>();
            prefixs = new List<string>();
        }

        public string getFreeName(string prefix)
        {
            int i = prefixs.FindIndex(x => x.Equals(prefix));
            if (i == -1)
            {
                prefixs.Add(prefix);
                names.Add(new List<int>());
                names[names.Count - 1].Add(1);
                return prefix + "1";
            }
            if (names[i].Count == 0)
            {
                names[i].Add(1);
                return prefix + "1";
            }
            int tnum = names[i].FindIndex(x => x == 0);
            if (tnum != -1)
            {
                names[i][tnum] = tnum + 1;
                return prefix + (tnum + 1);
            }
            int num = names[i][names[i].Count - 1] + 1;
            names[i].Add(num);
            return prefix + num;
        }
    }
}
