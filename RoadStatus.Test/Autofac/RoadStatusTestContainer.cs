using Autofac;
using RoadStatus.Test.Autofac.Modules;

namespace RoadStatus.Test.Autofac
{
    public static class RoadStatusTestContainer
    {
        public static IContainer Container { get; set; }

        static RoadStatusTestContainer()
        {
            Container = Build();
        }

        private static IContainer Build()
        {
            var container = new ContainerBuilder();

            container.RegisterModule(new RoadStatusTestModules());

            return container.Build();
        }
    }
}