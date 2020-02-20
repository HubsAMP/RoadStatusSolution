using Autofac;
using RoadStatus.Client.Autofac;
using RoadStatus.Service.Interfaces;
using System;
using System.Threading.Tasks;

namespace RoadStatus.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            GetRoadStatusAsync(args).Wait();
        }

        private static async Task GetRoadStatusAsync(string[] args)
        {
            var roadId = "";

            if (args.Length >= 1)
                roadId = args[0];

            var printService = RoadStatusContainer.Container.Resolve<IPrintService>();

            var result = await printService.PrintRoadStatusResponseAsync(roadId);
            printService.PrintOutPut();

            Environment.Exit(result);
        }
    }
}