using FluentValidation;
using Microsoft.Extensions.DependencyModel;
using System.Reflection;
using System.Runtime.Loader;

namespace Infrastructure.ValidationManager
{
    public class ValidatorContainer
    {
        private static Dictionary<string, Type> validatorContainers=new Dictionary<string, Type>();
        public static Dictionary<string, Type> ValidatorContainers => validatorContainers;
        public ValidatorContainer()
        {
            foreach (var typeInfo in GetTypeInfoList())
            {
                validatorContainers.Add(typeInfo.FullName, typeInfo);
            }
        }
        private static List<TypeInfo> GetTypeInfoList()
        {
            var compilationLibrary = DependencyContext.Default
                .CompileLibraries
                .Where(x => !x.Serviceable
                && x.Type == "project")
                .ToList();
            List<TypeInfo> typeInfoList = new List<TypeInfo>();
            foreach (var _compilation in compilationLibrary)
            {
                try
                {
                   var assembly= AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(_compilation.Name));
                    typeInfoList.AddRange(assembly.DefinedTypes.Where(t => typeof(IValidator).IsAssignableFrom(t) && !t.IsAbstract));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(_compilation.Name + ex.Message);
                }
            }

            return typeInfoList;
        }
    }
}
