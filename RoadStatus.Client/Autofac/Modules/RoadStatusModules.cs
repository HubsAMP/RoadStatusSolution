using Autofac;
using RoadStatus.Service;
using RoadStatus.Service.Interfaces;

namespace RoadStatus.Client.Autofac.Modules
{
    public class RoadStatusModules : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new Config()).As<IConfig>();

            builder.Register(c => new RoadStatusService(new HttpClientHandler(), c.Resolve<IConfig>()))
                   .As<IRoadStatusService>();

            base.Load(builder);
        }
    }
}