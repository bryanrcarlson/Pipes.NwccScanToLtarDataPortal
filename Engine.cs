using Nsar.Common.UnitOfMeasure;
using Nsar.Nodes.LtarDataPortal.Meteorology.Core;
using Nsar.Nodes.LtarDataPortal.Meteorology.Dto;
using Nsar.Nodes.LtarDataPortal.Meteorology.Load;
using Nsar.Nodes.LtarDataPortal.Meteorology.Transform;
using Nsar.Nodes.NwccScan.ReportFormat;
using Nsar.Nodes.NwccScan.ReportGenerator;
using System;
using System.Collections.Generic;

namespace Nsar.Pipes.NwccScanToLtarDataPortal
{
    public class Engine
    {
        private readonly ReportRetriever grabber;
        private readonly Formatter formatter;
        private readonly ILoadMeteorology writer;
        private readonly TransformTemporalMeasurement transformer;
        private readonly CommandLineParser commandLineParser;

        public Engine(
            ReportRetriever dataGrabber,
            Formatter formatter,
            ILoadMeteorology writer,
            TransformTemporalMeasurement transformer,
            CommandLineParser commandLineParser)
        {
            this.grabber = dataGrabber;
            this.formatter = formatter;
            this.writer = writer;
            this.transformer = transformer;
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

            string content =
                grabber.GetHourly(
                    "2198:WA:SCAN",
                    dateStart,
                    dateEnd,
                    columns);

            List<ITemporalMeasurement> measurements = formatter.ParseData(content);

            List<COReDataRecord> data = new List<COReDataRecord>();
            data = transformer.Transform(
                LtarSiteAcronymCodes.CookAgronomyFarm,
                "002",
                RecordTypeCodes.LegacySiteSpecificDefinition,
                -8,
                measurements);

            writer.Load(data);
        }
    }
}
