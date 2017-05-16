using System;
using System.Collections.Generic;
using System.Text;

namespace Nsar.Pipes.NwccScanToLtarDataPortal
{
    /// <summary>
    /// Wires up all dependencies, called by composition root
    /// </summary>
    /// <returns>Engine that runs the program</returns>
    public class Container
    {
        public Engine ResolveScanToCORePipe()
        {
            var r = new Nsar.Nodes.NwccScan.ReportGenerator.ReportRetriever();
            var f = new Nsar.Nodes.NwccScan.ReportFormat.Formatter();
            var w = new Nsar.Nodes.LtarDataPortal.Meteorology.Load.FileSystemCsvWriter(@"C:\");
            var t = new Nsar.Nodes.LtarDataPortal.Meteorology.Transform.TransformTemporalMeasurement();
            var p = new CommandLineParser();

            return new Engine(r, f, w, t, p);
        }
    }
}
