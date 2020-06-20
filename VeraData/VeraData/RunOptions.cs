using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace VeraData
{
    [Verb("dump", HelpText = "Dump all data about an app")]
    public class RunOptions
    {
        [Option("appid", Default = "", Required = true, HelpText = "Application Veracode ID")]
        public string AppId { get; set; }

        [Option("buildlimit", Default = null, HelpText = "Max Number of builds to download")]
        public int? BuildLimit { get; set; }

        [Option('f', "force", Default = false, HelpText = "Run anyway")]
        public bool Force { get; set; }
    }
}
