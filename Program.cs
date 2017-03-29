using Nsar.Pipes.NwccScanToLtarDataPortal;
using System;

namespace NwccScanToLtarDataPortal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Container container = new Container();

            // TODO: Move arg logic to container
            //var arguments = CommandLineParser.Parse(args);
            //container.ResolveScanToCORePipe().Execute(arguments.dateStart, arguments.dateEnd);

            DateTime fromDate = new DateTime(2016, 1, 1);
            container.ResolveScanToCORePipe().Execute(fromDate, DateTime.Today);
        }
    }
}