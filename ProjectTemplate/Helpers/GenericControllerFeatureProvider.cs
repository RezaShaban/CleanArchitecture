using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.ComponentModel;
using System.Reflection;
using WebUI.Controllers;

namespace InstaMarket.Api
{
    public class GenericControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        public GenericControllerFeatureProvider()
        {

        }
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            IList<TypeInfo> controllers = feature.Controllers;
            AddCrudControllers(controllers);

        }

        private void AddCrudControllers(IList<TypeInfo> controllers)
        {
            var candidates = typeof(Application.DependencyInjection).Assembly.GetExportedTypes().Where(x => x.GetInterfaces().Any(x => x.Name == typeof(IRequest<>).Name))
                .GroupBy(x => x.GetInterfaces().FirstOrDefault(x => x.Name == typeof(IRequest<>).Name).GenericTypeArguments.FirstOrDefault().Name, o => o,
                    (key, g) => new
                    {
                        TypeName = key,
                        Types = g
                    }).ToList();

            foreach (var candidate in candidates)
            {
                if (candidate.Types.Any(x => x.GetInterface(nameof(IDeleteCommand)) is not null))
                {
                    var controllerType = typeof(DeleteCommandController<>).MakeGenericType(
                    candidate.Types.FirstOrDefault(x => x.Name.Contains("Delete"))
                    );
                    controllers.Add(controllerType.GetTypeInfo());
                }

                if (candidate.Types.Any(x => x.GetInterface(nameof(ICreateCommand)) is not null))
                {
                    //var typeeee = ((System.Type[])((System.Reflection.TypeInfo)typeof(CreateCommandController<>).GetGenericArguments()[0]).ImplementedInterfaces)[0];
                    var createControllerType = typeof(CreateCommandController<>).MakeGenericType(
                        candidate.Types.FirstOrDefault(x => x.Name.Contains("Create"))
                        );
                    controllers.Add(createControllerType.GetTypeInfo());
                }
                if (candidate.Types.Any(x => x.GetInterface(nameof(IUpdateCommand)) is not null))
                {
                    var updateControllerType = typeof(UpdateCommandController<>).MakeGenericType(
                    candidate.Types.FirstOrDefault(x => x.Name.Contains("Update"))
                    );
                    controllers.Add(updateControllerType.GetTypeInfo());
                }
            }
        }
    }

    public class GenericControllerRouteConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            if (controller.ControllerType.IsGenericType)
            {
                Type entityType = controller.ControllerType.GetGenericArguments().FirstOrDefault();
                var entityInterfaces = entityType.GetTypeInfo().ImplementedInterfaces;
                var entityName = entityInterfaces.FirstOrDefault(x => x.Name == typeof(IRequest<>).Name).GenericTypeArguments.FirstOrDefault().Name;
                controller.ControllerName = entityName;
            }
        }
    }
}
