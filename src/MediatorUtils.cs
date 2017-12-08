using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac.Util;

namespace MediatR.Pair
{
    public static class MediatorPair
    {
        public static IEnumerable<Type> FindUnmatchedRequests(Assembly assembly)
        {
            var requests = assembly.GetTypes()
                .Where(t => t.IsClass && t.IsClosedTypeOf(typeof(IRequest<>)))
                .ToList();

            var handlerInterfaces = assembly.GetTypes()
                .Where(t => t.IsClass && (t.IsClosedTypeOf(typeof(IRequestHandler<>)) || t.IsClosedTypeOf(typeof(IRequestHandler<,>))))
                .SelectMany(t => t.GetInterfaces())
                .ToList();

            return (from request in requests
                let resultType = request.GetInterfaces()
                    .Single(i => i.IsClosedTypeOf(typeof(IRequest<>)) && i.GetGenericArguments().Any())
                    .GetGenericArguments()
                    .First()
                let handlerType = resultType == typeof(Unit)
                    ? typeof(IRequestHandler<>).MakeGenericType(request)
                    : typeof(IRequestHandler<,>).MakeGenericType(request, resultType)
                where handlerInterfaces.Any(t => t == handlerType) == false
                select request).ToList();
        }

    }
    
}
