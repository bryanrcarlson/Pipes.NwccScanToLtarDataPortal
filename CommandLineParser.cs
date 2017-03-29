using System;
using System.Collections.Generic;
using System.Text;

namespace Nsar.Pipes.NwccScanToLtarDataPortal
{
    public class CommandLineParser
    {
        public (DateTime dateStart, DateTime dateEnd) Parse(string[] args)
        {
            if(args.Length < 2)
            {
                throw new ArgumentNullException("Either dateStart and/or dateEnd are undefined");
            }

            DateTime start;
            DateTime end;

            try
            {
                start = DateTime.Parse(args[0]);
            }
            catch (FormatException e)
            {
                throw new ArgumentException("Start date format is incorrect", e);
            }

            try
            {
                end = DateTime.Parse(args[1]);
            }
            catch (FormatException e)
            {
                throw new ArgumentException("End date format is incorrect", e);
            }

            return (start, end);
        }
    }
}
