using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CsvHelper;
using PreInO.DataAccess.Lib;

namespace LogDynamicsLab.InfluxDataImporttool
{
    class Program
    {
        static void Main(string[] args)
        {

            #region Global Variables
            List<string> columns = new List<string>();
            List<string> helpterms = new List<string>();
            List<KeyValuePair<DateTime, List<object>>> sub_data = new List<KeyValuePair<DateTime, List<object>>>();
            Dictionary<string, string> input_parameters = new Dictionary<string, string>();
            bool helpmodus = false;
            bool mandatoryMissing = false;
            #endregion

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == HelpText._helpInput)
                {
                    helpmodus = true;
                    break;
                }
            }
            if (!helpmodus)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i].Contains("--"))
                    {
                        input_parameters.Add(args[i], args[i + 1]);
                    }
                }
                if (args.Length != 0)
                {
                    input_parameters.Add("path_csv", args[0]);
                }
                else
                {
                    Console.WriteLine("Parameters needed. See --help for furher information." + "\n");
                    Console.ReadKey();
                    return;
                }
                string current_csv = @input_parameters["path_csv"];
                if (!File.Exists(current_csv))
                {
                    Console.WriteLine("File does not exist.");
                    return;
                }

                #region Check if all mandatory parameters exist

                if(!input_parameters.ContainsKey(HelpText._dbInput))
                {
                    Console.WriteLine("\n" + HelpText._dbInput + " is missing." + "\n");
                    mandatoryMissing = true;
                }
                if (!input_parameters.ContainsKey(HelpText._portInput))
                {
                    Console.WriteLine("\n" + HelpText._portInput + " is missing." + "\n");
                    mandatoryMissing = true;
                }
                if (!input_parameters.ContainsKey(HelpText._pwInput))
                {
                    Console.WriteLine("\n" + HelpText._pwInput + " is missing." + "\n");
                    mandatoryMissing = true;
                }
                if (!input_parameters.ContainsKey(HelpText._serverInput))
                {
                    Console.WriteLine("\n" + HelpText._serverInput + " is missing." + "\n");
                    mandatoryMissing = true;
                }
                if (!input_parameters.ContainsKey(HelpText._timeseriesnameInput))
                {
                    Console.WriteLine("\n" + HelpText._timeseriesnameInput + " is missing." + "\n");
                    mandatoryMissing = true;
                }
                if (!input_parameters.ContainsKey(HelpText._userInput))
                {
                    Console.WriteLine("\n" + HelpText._userInput + " is missing." + "\n");
                    mandatoryMissing = true;
                }
                //if (!input_parameters.ContainsKey(HelpText._timestampInput))
                //{
                //    Console.WriteLine("\n" + HelpText._timestamp + " is missing." + "\n");
                //    mandatoryMissing = true;
                //}

                if(mandatoryMissing)
                {
                    Console.ReadKey();
                    return;
                }

                #endregion

                PreInO.DataAccess.Lib.PreInODBClient database_client = new PreInO.DataAccess.Lib.PreInODBClient(input_parameters[HelpText._serverInput], Int32.Parse(input_parameters[HelpText._portInput]), input_parameters[HelpText._dbInput], input_parameters[HelpText._userInput], input_parameters[HelpText._pwInput]);

                using (var stream_reader = new StreamReader(current_csv, Encoding.Default))
                {
                    var parser = new CsvParser(stream_reader);
                    if (!input_parameters.ContainsKey(HelpText._delimiterInput))
                    {
                        parser.Configuration.Delimiter = HelpText._defaultDelimiter;
                    }
                    else
                    {
                        parser.Configuration.Delimiter = input_parameters[HelpText._delimiterInput];
                    }
                    bool columns_set = false;
                    int upload_count = 0;
                    while (true)
                    {
                        string[] row = parser.Read();
                        if (row == null)
                        {
                            break;
                        }
                        if (!columns_set)
                        {
                            columns = row.ToList<string>();

                            for (int i = 0; i < columns.Count; i++ )
                            {
                                string temp = columns[i];
                                int occurences = -1;
                                for (int j = 0; j < columns.Count; j++)
                                {
                                    if (temp == columns[j])
                                    {
                                        occurences++;
                                        if (occurences > 0)
                                        {
                                            columns[j] = columns[j] + "_" + occurences.ToString();
                                        }
                                    }
                                }
                            }

                            columns_set = true;
                        }
                        else
                        {
                            sub_data.Add(new KeyValuePair<DateTime, List<object>>(DateTime.Now, row.ToList<object>()));
                            upload_count++;
                        }
                        if (upload_count == 500)
                        {
                            database_client.PostTimeSeriesData(input_parameters[HelpText._timeseriesnameInput], sub_data, columns);
                            upload_count = 0;
                            sub_data = new List<KeyValuePair<DateTime, List<object>>>();
                        }
                    }
                    if (sub_data.Count != 0)
                    {
                        database_client.PostTimeSeriesData(input_parameters[HelpText._timeseriesnameInput], sub_data, columns);
                        upload_count = 0;
                        sub_data = new List<KeyValuePair<DateTime, List<object>>>();
                    }
                }
            }

            #region Generation of help Pages
            else
            {
                int helpCount = 0;
                if (args.Length == 1)
                {
                    foreach(KeyValuePair<string, string> kvp in HelpText._completeHelp)
                    {
                        Console.WriteLine(kvp.Value);
                        if (helpCount == 2)
                        {
                            Console.WriteLine("Press any key to continue.." + "\n");
                            Console.ReadKey();
                            helpCount = 0;
                        }
                        else
                        {
                            helpCount++;
                        }
                    }
                }
                else{
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (args[i].Contains("--") && args[i] != HelpText._helpInput)
                        {
                            helpterms.Add(args[i]);
                        }
                    }
                    foreach(string s in helpterms)
                    {
                        string toShow = "";

                        if (HelpText._completeHelp.ContainsKey(s))
                        {
                            toShow = HelpText._completeHelp[s];
                        }
                        else
                        {
                            Console.WriteLine("Invalid parameters "+ s +" . See --help for further information.");
                        }
                        Console.WriteLine(toShow);
                        if (helpCount == 2)
                        {
                            Console.WriteLine("Press any key to continue..");
                            Console.ReadKey();
                            helpCount = 0;
                        }
                        else
                        {
                            helpCount++;
                        }
                    }
                }
            }
            #endregion

            Console.ReadLine();
        }
    }
}
