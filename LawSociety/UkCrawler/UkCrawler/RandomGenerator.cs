using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;


namespace GlobusRandom.classes
{
    class RandomGenerator
    {
        //public string GetRandomLine(string file)
        //{
        //    //Generic list for holding the lines
        //    List<string> lines = new List<string>();

        //    //Random class to generate our random number
        //    Random rnd = new Random();

        //    //Variable to hold our random line number
        //    int i = 0;

        //    try
        //    {
        //        if (File.Exists(file))
        //        {
        //            //StreamReader to read our file
        //            StreamReader reader = new StreamReader(file);

        //            //Now we loop through each line of our text file
        //            //adding each line to our list
        //            while (!(reader.Peek() == -1))
        //                lines.Add(reader.ReadLine());

        //            //Now we need a random number
        //            i = rnd.Next(lines.Count);

        //            //Close our StreamReader
        //            reader.Close();

        //            //Dispose of the instance
        //            reader.Dispose();

        //            //Now write out the random line to the TextBox
        //            return lines[i].Trim();
        //        }
        //        else
        //        {
        //            //file doesn't exist so return nothing
        //            return string.Empty;
        //        }
        //    }
        //    catch (IOException ex)
        //    {
        //        MessageBox.Show("Error: " + ex.Message);
        //        return string.Empty;
        //    }

        //}           // get random line function  ends

        string[] range_str = { "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz123456789.", "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz123456789_." };

        public string RandomPasswordAndAnswer()
        {
            Random random = new Random();
            int rand_len = random.Next(20, 23);
            int i, j;
            StringBuilder builder = new StringBuilder();
            for (i = 0; i < rand_len; i++)
            {
                j = random.Next(range_str[0].Length - 1);
                builder.Append(range_str[0][j]);
            }
            return builder.ToString();
        }

        public string AimRandomPasswordAndAnswer()
        {
            Random random = new Random();
            int rand_len = random.Next(10, 15);
            int i, j;
            StringBuilder builder = new StringBuilder();
            for (i = 0; i < rand_len; i++)
            {
                j = random.Next(range_str[0].Length - 1);
                builder.Append(range_str[0][j]);
            }
            return builder.ToString();
        }

        public string RandomYahooID()
        {
            Random random = new Random();
            int rand_len = random.Next(1, 3);
            int i, j;
            StringBuilder builder = new StringBuilder();
            j = random.Next(52);
            builder.Append(range_str[1][j]);
            for (i = 1; i < rand_len; i++)
            {
                j = random.Next(range_str[1].Length - 2);
                if (builder[i - 1] != '_' && builder[i - 1] != '.')
                    j = random.Next(range_str[1].Length - 1);
                if (builder[i - 1] != '_' && builder.ToString().Contains(".") == false)
                    j = random.Next(range_str[1].Length - 1);
                builder.Append(range_str[1][j]);
            }
            return builder.ToString();
        }
        public string txtreader(string FileName)
        {
            string[] res;
            if (File.Exists(FileName))
            {
                res = File.ReadAllLines(FileName);
                if (res.Length == 1)
                    return res[0];
                else
                {
                    Random r = new Random();
                    return res[r.Next(res.Length)];
                }
            }
            return null;
        }
        public int trial(string path)
        {
            int i;
            string a;
            if (File.Exists(path))
            {
                a = File.ReadAllText(path);
                i = int.Parse(a);
                if (i == 0) return i;
                i -= 1;
                File.WriteAllText(path, i.ToString());
            }
            else
            {
                i = 5;
                File.WriteAllText(path, "5");
            }
            return i;
        }

        //public List<string> DecaptchaDetail(string Path)
        //{
        //    List<string> tempAccounts = new List<string>();
        //    List<string> DecaptchaAccounts = new List<string>();

        //    tempAccounts = GlobusFileHelper.ReadFiletoStringList(Path);

        //    foreach (string AcctData in tempAccounts)
        //    {
        //        string[] tempArray = AcctData.Split(':');
        //        foreach (string accounts in tempAccounts)
        //        {
        //            DecaptchaAccounts.Add(accounts);
        //        }
        //        //DecaptchaHost = tempArray[0];
        //        //DecaptchaLogin = tempArray[1];
        //        //DecaptchaPassword = tempArray[2];
        //    }
        //    return DecaptchaAccounts;
        //}
    }
}
