using MetWriter;
using Nsar.Common.UnitOfMeasure;
using Nsar.Nodes.CORe.MetWriter;
using Nsar.Nodes.NwccScan.ReportFormat;
using Nsar.Nodes.NwccScan.ReportGenerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nsar.Pipes.NwccScanToLtarDataPortal
{
    public class Engine
    {
        private readonly ReportRetriever grabber;
        private readonly Formatter formatter;
        private readonly CsvWriter writer;
        private readonly CommandLineParser commandLineParser;

        public Engine(
            ReportRetriever dataGrabber,
            Formatter formatter,
            CsvWriter writer,
            CommandLineParser commandLineParser)
        {
            this.grabber = dataGrabber;
            this.formatter = formatter;
            this.writer = writer;
            this.commandLineParser = commandLineParser;
        }

        public void ParseArguments(string[] args)
        {
            var arguments = commandLineParser.Parse(args);
        }

        public void Execute(DateTime dateStart, DateTime dateEnd /*, columns */)
        {
            // TODO: Consider a DataTable class (or somesort) or mapper that maps headers, columns, to Measurements.  Will need one for the parser and the writer.  Potentially allow definition in a txt file for customization without coding exp.

            List<DataColumns> columns = new List<DataColumns>()
            {
                DataColumns.TAVG,
                DataColumns.WSPDV,
                DataColumns.WDIRV,
                DataColumns.RHUM,
                DataColumns.PRCP,
                DataColumns.BATT
            };

            // Single responsibility principle says I should have a "connection" class
            //and a "read" class.  But I'm putting them together here.

            //DateTime today = DateTime.Today;
            //today = new DateTime(2016, 9, 1);
            string content =
                grabber.GetHourly(
                    "2198:WA:SCAN",
                    //new DateTime(2013, 07, 24),
                    dateStart,
                    //new DateTime(2016, 09, 11),
                    dateEnd,
                    columns);

            List<ITemperalMeasurement> measurements = formatter.ParseData(content);

            writer.CreateDataRecord(
                LtarSiteAcronymCodes.CookAgronomyFarm,
                "002",
                RecordTypeCodes.LegacySiteSpecificDefinition,
                -8,
                measurements);

            writer.Write();

            // SOLID be damned, this class connects to filesystem and writes to it
            //COReWriter.W
        }
    }
}
