using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using AutoMapper;
using IdentityManagerWebApp.Infrastructure.Mapper;

namespace IdentityManagerWebApp
{
    public static class AutomapperConfiguration
    {
        public static void Configure()
        {
            var types = Assembly.GetExecutingAssembly().GetExportedTypes();

            //RowVersion y Blobs
            Mapper.CreateMap<byte[], string>().ConvertUsing(a => a != null ? Convert.ToBase64String(a) : string.Empty);
            Mapper.CreateMap<string, byte[]>().ConvertUsing(a => string.IsNullOrEmpty(a) ? null : Convert.FromBase64String(a));

            LoadStandardMappings(types);
            LoadCustomMappings(types);
        }

        private static void LoadStandardMappings(Type[] types)
        {
            var maps = types
                .Where(t => !t.IsAbstract &&
                            !t.IsInterface)
                .Select(t => new
                {
                    Destination = t,
                    Source = t.GetInterfaces()
                        .FirstOrDefault(i => i.IsGenericType &&
                                             i.GetGenericTypeDefinition() == typeof(IMapFrom<>))?.GetGenericArguments()[0]
                }).Where(m => m.Source != null);

            foreach (var map in maps)
            {
                //registro las 2 direcciones
                Mapper.CreateMap(map.Source, map.Destination).ReverseMap();
            }
        }

        private static void LoadCustomMappings(Type[] types)
        {
            var maps = types
                .Where(t => !t.IsAbstract &&
                            !t.IsInterface &&
                            typeof(IHaveCustomMappings).IsAssignableFrom(t))
                .Select(t => (IHaveCustomMappings)Activator.CreateInstance(t));

            foreach (var map in maps)
            {
                map.CreateMappings(Mapper.Configuration);
            }

        }
    }
}