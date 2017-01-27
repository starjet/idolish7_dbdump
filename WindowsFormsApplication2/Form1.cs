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
            if (Environment.GetCommandLineArgs().Count() > 1 && Environment.GetCommandLineArgs()[1] == "-tempfolder")
            {
                try
                {
                    Directory.SetCurrentDirectory(".\\Temp");
                    try
                    {
                        Directory.CreateDirectory("_db");
                    }
                    catch { }
                    try
                    {
                        File.Copy(HashFilename("db\\resource.db", false), "_db\\resource.db", true);
                    }
                    catch
                    {
                        Environment.Exit(1);
                    }
                    try
                    {
                        File.Copy(HashFilename("db\\master.db", false), "_db\\master.db", true);
                    }
                    catch { }
                    AppSqlite s = new AppSqlite("_db", "resource.db", APP_CONFIG.SECURITY.KEY_DB_DECRYPT_STR);
                    s.OpenDB(false);
                    string sql = "SELECT * FROM resource_file;";
                    SQLiteQuery sQLiteQuery = s.CreateOuery(sql);
                    while (sQLiteQuery.Step())
                    {
                        string fullpath = sQLiteQuery.GetString("file_name");
                        string hashedfullpath = HashFilename(fullpath, false);
                        fullpath = fullpath.Replace("/", "\\");
                        hashedfullpath = hashedfullpath.Replace("/", "\\");
                        try
                        {
                            if (File.Exists(hashedfullpath))
                            {
                                Directory.CreateDirectory("_" + Path.GetDirectoryName(fullpath));
                            }
                        }
                        catch { }
                        try
                        {
                            File.Copy(hashedfullpath, "_" + fullpath);
                        }
                        catch { }
                    }
                    Environment.Exit(0);
                }
                catch { }
            }

            if (Environment.GetCommandLineArgs().Count() > 1 && Environment.GetCommandLineArgs()[1] == "-decryptfile")
            {
                try
                {
                    string file = Environment.GetCommandLineArgs()[2];
                    byte[] input = File.ReadAllBytes(file);
                    byte[] output = UtilSecurity.DecryptionBytes(input, APP_CONFIG.SECURITY.RIJINDAEL_MANAGED);
                    file = Path.GetFileName(file);
                    File.WriteAllBytes("decrypted-" + file, output);
                }
                catch
                {
                    Environment.Exit(1);
                }
            }
            else
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
                                string s3 = sQLiteQuery.GetString(s4);
                                s3 = s3.Replace("\n", "\\n");
                                File.AppendAllText(s2 + ".txt", s3 + ",");
                            }
                            File.AppendAllText(s2 + ".txt", "\r\n");
                            while (sQLiteQuery.Step())
                            {
                                foreach (string s4 in sQLiteQuery.Names)
                                {
                                    string s3 = sQLiteQuery.GetString(s4);
                                    s3 = s3.Replace("\n", "\\n");
                                    File.AppendAllText(s2 + ".txt", s3 + ",");
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
            }
            Environment.Exit(0);
        }

        public string HashFilename(string filePath, bool isHashed)
        {
            if (!isHashed)
            {
                string fileName = Path.GetFileName(filePath);
                string directoryName = Path.GetDirectoryName(filePath);
                directoryName = directoryName.Replace("\\", "/");
                filePath = Util.GetMD5Value(directoryName + "kEfGhnNmeu4YYuhv") + "/" + Util.GetMD5Value(fileName + "kEfGhnNmeu4YYuhv");
            }
            return filePath;
        }
    }
}
