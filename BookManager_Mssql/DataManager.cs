using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookManager
{
    class DataManager
    {
        public static List<Book> Books = new List<Book>();
        public static List<User> Users = new List<User>();

        static DataManager()
        {
            Load();
           
        }
        public static void JsonLoad()
        {
            string stBookValueJson = File.ReadAllText(@"./Books.json");
            JObject jsonObjectBook = JObject.Parse(stBookValueJson);
            Books = (from item in jsonObjectBook["books"]["book"]
                     select new Book()
                     {
                         Isbn = item["isbn"].ToString(),
                         Name = item["name"].ToString(),
                         Publisher = item["publisher"].ToString(),
                         Page = int.Parse(item["page"].ToString()),
                         BorrowedAt = DateTime.Parse(item["borrowedat"].ToString()),
                         isBorrowed = item["isborrowed"].ToString() != "0" ? true : false,
                         UserId = int.Parse(item["userid"].ToString()),
                         UserName = item["username"].ToString().Replace("{", "").Replace("}", "")
                     }).ToList<Book>();

            string strUserValueJson = File.ReadAllText(@"./Users.json");
            JObject jsonObjectUsers = JObject.Parse(strUserValueJson);
            Users = (from item in jsonObjectUsers["users"]["user"]
                     select new User()
                     {
                         Id = int.Parse(item["id"].ToString()),
                         Name = item["name"].ToString()
                     }).ToList<User>();
        }

        public static void SaveJson()
        {
            var jBookArray = new JArray();
            foreach(var item in Books)
            {
                var jBookobject = new JObject();
                jBookobject.Add("isbn", item.Isbn);
                jBookobject.Add("name", item.Name);
                jBookobject.Add("publisher", item.Publisher);
                jBookobject.Add("page", item.Page);
                jBookobject.Add("borrowedat", item.BorrowedAt.ToLongDateString());
                jBookobject.Add("isborrowed", item.isBorrowed ? "1":"0");
                jBookobject.Add("userid", item.UserId);
                jBookobject.Add("username", item.UserName);
                jBookArray.Add(jBookobject);

            }

            var jBookArrayObject = new JObject();
            jBookArrayObject.Add("book", jBookArray);

            var jBooksObject = new JObject();
            jBooksObject.Add("books", jBookArrayObject);

            var jUserArray = new JArray();
            foreach(var item in Users)
            {
                var jUserObject = new JObject();
                jUserObject.Add("id", item.Id);
                jUserObject.Add("name", item.Name);
                jUserArray.Add(jUserObject);
            }

            var jUserArrayObject = new JObject();
            jUserArrayObject.Add("user", jUserArray);
            var jUsersObject = new JObject();
            jUsersObject.Add("users", jUserArrayObject);

            File.WriteAllText(@"./Books.json", jBooksObject.ToString());
            File.WriteAllText(@"./Users.json", jUsersObject.ToString());

        }
        public static void Load()
        {
            JsonLoad();
            return;
            try
            {
                string booksOutput = File.ReadAllText(@"./Books.xml");
                XElement booksXElement = XElement.Parse(booksOutput);
                Books = (from item in booksXElement.Descendants("book")
                         select new Book()
                         {
                             Isbn = item.Element("isbn").Value,
                             Name = item.Element("name").Value,
                             Publisher = item.Element("publisher").Value,
                             Page = int.Parse(item.Element("page").Value),
                             BorrowedAt = DateTime.Parse(item.Element("borrowedAt").Value),
                             isBorrowed = item.Element("isBorrowed").Value != "0" ? true : false,
                             UserId = int.Parse(item.Element("userId").Value),
                             UserName = item.Element("userName").Value
                         }).ToList<Book>();

                string usersOutput = File.ReadAllText(@"./Users.xml");
                XElement usersXElement = XElement.Parse(usersOutput);
                Users = (from item in usersXElement.Descendants("user")
                         select new User()
                         {
                             Id = int.Parse(item.Element("id").Value),
                             Name = item.Element("name").Value
                         }).ToList<User>();
            }
            catch(FileLoadException )
            {
                Save();
            }
        }

        public static void Save()
        {
            SaveJson();
            return;
            
            string booksOutput = "";
            booksOutput += "<books>\n";
            foreach (var item in Books)
            {
                booksOutput += "<book>\n";
                booksOutput = booksOutput + " <isbn>" + item.Isbn + "</isbn>\n";
                booksOutput += " <name>" + item.Name + "</name>\n";
                booksOutput += " <publisher>" + item.Publisher + "</publisher>\n";
                booksOutput += " <page>" + item.Page + "</page>\n";
                booksOutput += " <borrowedAt>" + item.BorrowedAt + "</borrowedAt>\n";
                booksOutput += " <isBorrowed>" + (item.isBorrowed ? 1 : 0) + "</isBorrowed>\n";
                booksOutput += " <userId>" + item.UserId + "</userId>\n";
                booksOutput += " <userName>" + item.UserName + "</userName>\n";
                booksOutput += "</book>\n";
            }
            booksOutput += "</books>";

            string usersOutput = "";
            usersOutput += "<users>\n";
            foreach(var item in Users)
            {
                usersOutput += "<user>\n";
                usersOutput += " <id>" + item.Id + "</id>\n";
                usersOutput += " <name>" + item.Name + "</name>\n";
                usersOutput += "</user>\n";
            }
            usersOutput += "</users>";

            File.WriteAllText(@"./Books.xml", booksOutput);
            File.WriteAllText(@"./Users.xml", usersOutput);
        }


    }
}
