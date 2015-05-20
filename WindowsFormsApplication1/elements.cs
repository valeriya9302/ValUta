using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using MainWindow.scheme;

namespace MainWindow
{
    class elements
    {
        private struct Struct
        {
            public int id;
            public int pid;
            public string name;
            public string ntname;
            public string ptname;
            public string etname;
            public Struct(int _id, int _pid, string _name, string _ntname, string _ptname, string _etname)
            {
                id = _id;
                pid = _pid;
                name = _name;
                ntname = _ntname;
                ptname = _ptname;
                etname = _etname;
            }
        };
        private Tree<Struct> _struct;
        public List<Elems> _elems = new List<Elems>();

        public elements()
        {
            Struct root = new Struct();
            root.id = 0;
            root.pid = 0;
            root.name = "root";
            root.ptname = "";
            root.etname = "";
            root.ntname = "struct_level0";
            _struct = new Tree<Struct>(root);
        }

        public void load()
        {
            //Query.SendQueryInsert("CREATE TABLE [struct_level0]([ID] [int] IDENTITY(1,1) NOT NULL,[PID] [int] NOT NULL DEFAULT ((0)),[NAME] [varchar](50) NOT NULL,[NTNAME] [varchar](50) NULL, [PTNAME] [varchar](50) NULL,[ETNAME] [varchar](50) NULL)");
            /*List<string> str = Query.SendQuerySelect("SELECT * FROM [struct_level0]");
            for(int i = 0; i < str.Count;)
            //foreach(string tstr in str)
            {
                Struct temp = new Struct();
                temp.id = Convert.ToInt32(str[i++]);
                temp.pid = Convert.ToInt32(str[i++]);
                temp.name = str[i++];
                temp.ntname = str[i++];
                temp.ptname = str[i++];
                temp.etname = str[i++];
                _struct.Add(new Tree<Struct>(temp));
            }*/
            _struct = LoadTree(_struct, 0);
        }

        private Tree<Struct> LoadTree(Tree<Struct> __struct, int oldPid)
        {
            List<List<string>> fstr = Query.SendQuerySelect("SELECT * FROM [" + __struct.Value.ntname + "]");
            foreach(List<string> str in fstr)
            //for(int i = 0; i < str.Count;)
            {
                int i = 0;
                Struct tValue = new Struct(Convert.ToInt32(str[i++]), Convert.ToInt32(str[i++]), str[i++], str[i++], str[i++], str[i++]);
                if (tValue.pid != oldPid)
                    continue;
                Tree<Struct> tChildren = new Tree<Struct>(tValue);
                if (tValue.ntname.Equals(""))
                {
                    __struct.Add(tChildren);
                }
                else
                    __struct.Add(LoadTree(tChildren, tValue.id));
            }
            return __struct;
        }

        public List<string> GetLevel(List<int> index)
        {
            if (!(index[index.Count - 1] < _struct.Children.Count))
                return null;
            Tree<Struct> temp = _struct;
            for (int i = 1; i < index.Count; i++)
            {
                if (!(index[i] < temp.Children.Count))
                    return null;
                temp = temp.Children[index[i]];
            }

            List<string> res = new List<string>();
            foreach (Tree<Struct> _temp in temp.Children)
                res.Add(_temp.Value.name);
            return res;
        }

        public string getEtname(string[] patch)
        {
            Tree<Struct> __struct = _struct; 
            foreach (string name in patch)
            {
                foreach (Tree<Struct> temp in __struct.Children)
                {
                    if (temp.Value.name.Equals(name))
                    {
                        __struct = temp;
                        break;
                    }
                }
            }
            return __struct.Value.etname;
        }

        public List<Elems> getElements(string[] patch)
        {
            _elems.Clear();
            string tname = getEtname(patch);
            List<List<string>> fstr = Query.SendQuerySelect("SELECT * FROM [" + tname + "]");
            foreach(List<string> str in fstr)
            //for (int i = 0; i < str.Count; )
            {
                Elems tValue = new Elems(str, tname);
                //FIXME
                /*List<List<string>> img = Query.SendQuerySelect("SELECT IMAGE_ID FROM [image_manager] WHERE (EID = " + tValue.eid + ")");
                if(img.Count != 0 && img[0].Count != 0)
                    tValue.image_id = Convert.ToInt32(img[0][0]);*/
                //_elems.Add(tValue);
                _elems.Add(tValue);
            }
            return _elems;
        }
    }
}
