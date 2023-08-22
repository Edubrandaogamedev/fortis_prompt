using System;
using System.Collections.Generic;
using Modules.UtilityModule.Debug;
using UnityEngine;

namespace UtilitiesModule.Service
{
    public class ServiceLocator
    {
        private readonly Dictionary<Type, IService> _services = new();

        public static ServiceLocator Instance { get; private set; }


        private ServiceLocator() {}

        public static void Initialize()
        {
            Instance ??= new ServiceLocator();
        }
        
        public T Get<T>() where T : IService, new()
        {
            Type serviceConcreteType = typeof(T);

            if (!_services.ContainsKey(serviceConcreteType))
            {
                Debug.LogWarning($"Attempted to get the {serviceConcreteType} but it's not binded.");
                return default;
            }
            return (T)_services[serviceConcreteType];
        }

        public void CreateAndBindService<T>() where T: IService, new()
        {
            Type serviceConcreteType = typeof(T);
            if (_services.ContainsKey(serviceConcreteType))
            {
                Debug.LogWarning($"Attempted to bind the {serviceConcreteType} twice.");
                return;
            }
            _services[typeof(T)] = new T();
        }
        
        public void BindService<T>(T service) where T : IService
        {
            Type serviceConcreteType = typeof(T);
            if (_services.ContainsKey(serviceConcreteType))
            {
                Debug.LogWarning($"Attempted to bind the {service} twice.");
                return;
            }
            _services[typeof(T)] = service;
            
        }

        public void UnbindService<T>(T service) where T : IService
        {
            Type serviceConcreteType = typeof(T); 
            if (!_services.ContainsKey(serviceConcreteType))
            {
                Debug.LogWarning($"Attempted to unbind the {service} but is not registered.");
                return;
            }
            _services.Remove(serviceConcreteType);
        }
    }
}
