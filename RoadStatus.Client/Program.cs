using Autofac;
using RoadStatus.Client.Autofac;
using RoadStatus.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadStatus.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            GetRoadStatus(args).Wait();
        }

        private static async Task GetRoadStatus(string[] args)
        {
            var roadId = "";
            if (args.Length >= 1)
                roadId = args[0];

            var roadStatusService = RoadStatusContainer.Container.Resolve<IRoadStatusService>();
            var roadStatusPrinter = new RoadStatusPrinter(roadStatusService);

            var result = await roadStatusPrinter.PrintRoadStatusResponse(roadId);

            Environment.Exit(result);
        }
    }
}