using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                AppSqlite s = new AppSqlite(".", "master.db", APP_CONFIG.SECURITY.KEY_DB_DECRYPT_STR);
                s.OpenDB(false);
                string sql = "SELECT * FROM sqlite_master;";
                SQLiteQuery sQLiteQuery = s.CreateOuery(sql);
                List<string> list = new List<string>();
                while (sQLiteQuery.Step())
                {
                    if (sQLiteQuery.GetString("type").Equals("table"))
                    {
                        string @string = sQLiteQuery.GetString("tbl_name");
                        list.Add(@string);
                    }
                } //Get list of tables
                sQLiteQuery.Release();
                foreach (string s2 in list) //Use list of tables to do whatever; In this case, dumping all data
                {
                    if (File.Exists(s2 + ".txt"))
                    {
                        File.Delete(s2 + ".txt");
                    }
                    sql = "SELECT * FROM " + s2 + ";";
                    sQLiteQuery = s.CreateOuery(sql);
                    sQLiteQuery.Step();
                    try
                    {
                        foreach (string s4 in sQLiteQuery.Names)
                        {
                            File.AppendAllText(s2 + ".txt", s4 + ",");
                        }
                        File.AppendAllText(s2 + ".txt", "\r\n");
                        foreach (string s4 in sQLiteQuery.Names)
                        {
                            File.AppendAllText(s2 + ".txt", sQLiteQuery.GetString(s4) + ",");
                        }
                        File.AppendAllText(s2 + ".txt", "\r\n");
                        while (sQLiteQuery.Step())
                        {
                            foreach (string s4 in sQLiteQuery.Names)
                            {
                                File.AppendAllText(s2 + ".txt", sQLiteQuery.GetString(s4) + ",");
                            }
                            File.AppendAllText(s2 + ".txt", "\r\n");
                        }
                    }
                    catch
                    { }
                    sQLiteQuery.Release();
                }
            }
            catch
            {
                Environment.Exit(1);
            }
            Environment.Exit(0);
        }
    }
}
