using Ninject.Modules;
using Services.Math;
using Services.Math.Impl;

namespace Services.NinjectModules
{
    /// <summary>
    /// Module for registering math-related services
    /// </summary>
    public class MathModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IGenerateRandomNumbers>().To<GenerateRandomNumbers>().InSingletonScope();
        }
    }
}
