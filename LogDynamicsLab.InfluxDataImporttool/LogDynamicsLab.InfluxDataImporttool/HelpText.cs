using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogDynamicsLab.InfluxDataImporttool
{
    public static class HelpText
    {
        public static string _defaultDelimiter = ";";
        public static string _mandatoryFlag = "!! MANDATORY !!";
        public static string _helpInput = "--help";
        public static string _help = "General usage of importtool" + "\n" + "<CSV file path> " + "<Mandatory and additional parameters>" + "\n";
        public static string _delimiterInput = "--delimiter";
        public static string _delimiter = _delimiterInput + " <delimiter>" + "\n" + "\n" + "Sets the delimiter used in parsing the CSV file. Default setting is " + "<" + _defaultDelimiter + ">" +  "\n";
        public static string _serverInput = "--server";
        public static string _server = _serverInput + " <server>" + "\n" + "\n" + _mandatoryFlag + "\n" + "\n" + "Sets the server URL of the database." + "\n";
        public static string _userInput = "--user";
        public static string _user = _userInput + " <user>" + "\n" + "\n" + _mandatoryFlag + "\n" + "\n" + "Sets the username used for importing data to the selected database." + "\n";
        public static string _pwInput = "--pw";
        public static string _pw = _pwInput + " <pw>" + "\n" + "\n" + _mandatoryFlag + "\n" + "\n" + "Sets the password used for importing data to the selected database." + "\n";
        public static string _dbInput = "--db";
        public static string _db = _dbInput + " <db>" + "\n" + "\n" + _mandatoryFlag + "\n" + "\n" + "Sets the name of the database data is getting imported to." + "\n";
        public static string _portInput = "--port";
        public static string _port = _portInput + " <port>" + "\n" + "\n" + _mandatoryFlag + "\n" + "\n" + "Sets the port used for importing," + "\n";
        public static string _timeseriesnameInput = "--timeseriesname";
        public static string _timeseriesname = _timeseriesnameInput + " <timeseriesname>" + "\n" + "\n" + _mandatoryFlag + "\n" + "\n" + "Sets the name of the timeseries data is being added to." + "\n";
        public static string _timestampInput = "--timestamp";
        public static string _timestamp = _timestampInput + " <timestamp>" + "\n" + "\n" + _mandatoryFlag + "\n" + "\n" + "Sets the zero based index of the column used as the timestamp for each set of data." + "\n";

        public static Dictionary<string, string> _completeHelp = new Dictionary<string, string>()
        {
            {_helpInput,_help},
            {_delimiterInput,_delimiter},
            {_serverInput,_server},
            {_userInput,_user},
            {_pwInput,_pw},
            {_dbInput,_db},
            {_portInput,_port},
            {_timestampInput, _timestampInput},
            {_timeseriesnameInput,_timeseriesname}
        };
    }
}
