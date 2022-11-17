using System.Collections.Generic;

namespace Tools
{
    public static class ServiceLocator
    {
        private static Dictionary<int, object> serviceDictionary;

        static ServiceLocator()
        {
            serviceDictionary = new Dictionary<int, object>();
        }

        public static void RegisterService<T>(T service) where T : class
        {
            serviceDictionary[typeof(T).GetHashCode()] = service;
        }

        public static void DeregisterService<T>() where T : class
        {
            serviceDictionary.Remove(typeof(T).GetHashCode());
        }

        public static T GetService<T>() where T : class
        {
            object service;
            serviceDictionary.TryGetValue(typeof(T).GetHashCode(), out service);
            return (T)service;
        }
    }
}
