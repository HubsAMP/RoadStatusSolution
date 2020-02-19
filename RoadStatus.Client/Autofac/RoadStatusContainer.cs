using Autofac;
using RoadStatus.Client.Autofac.Modules;

namespace RoadStatus.Client.Autofac
{
    public static class RoadStatusContainer
    {
        public static IContainer Container { get; set; }

        static RoadStatusContainer()
        {
            Container = Build();
        }

        private static IContainer Build()
        {
            var container = new ContainerBuilder();

            container.RegisterModule(new RoadStatusModules());

            return container.Build();
        }
    }
}