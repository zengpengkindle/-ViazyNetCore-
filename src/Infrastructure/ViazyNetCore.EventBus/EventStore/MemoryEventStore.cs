//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using ViazyNetCore.Handlers;

//namespace ViazyNetCore
//{
//    public class MemoryEventStore
//    {
//        /// <summary>
//        /// 定义锁对象
//        /// </summary>
//        private static readonly object LockObj = new object();

//        protected ConcurrentDictionary<Type, List<IEventHandlerFactory>> HandlerFactories { get; }
//        protected ConcurrentDictionary<Type, List<Type>> _eventAndHandlerMapping = new ConcurrentDictionary<Type, List<Type>>();

//        public MemoryEventStore()
//        {
//            this.HandlerFactories = new ConcurrentDictionary<Type, List<IEventHandlerFactory>>();
//        }

//        public IDisposable Subscribe(Type eventType, IEventHandlerFactory factory)
//        {
//            GetOrCreateHandlerFactories(eventType)
//                .Locking(factories =>
//                {
//                    if (!factory.IsInFactories(factories))
//                    {
//                        factories.Add(factory);
//                    }
//                }
//                );

//            return new EventHandlerFactoryUnregistrar(this, eventType, factory);
//        }

//        public void AddRegister<T, TH>() where T : IEventData where TH : IEventHandler
//        {
//            this.AddRegister(typeof(T), typeof(TH));
//        }


//        public void AddRegister(Type eventData, Type eventHandler)
//        {
//            lock (LockObj)
//            {
//                if (!this.HasRegisterForEvent(eventData))
//                {
//                    var handlers = new List<Type>();
//                    this._eventAndHandlerMapping.TryAdd(eventData, handlers);
//                }

//                if (this._eventAndHandlerMapping[eventData].All(h => h != eventHandler))
//                {
//                    this._eventAndHandlerMapping[eventData].Add(eventHandler);
//                }
//            }
//        }

//        public void RemoveRegister<T, TH>() where T : IEventData where TH : IEventHandler
//        {
//            var handlerToRemove = FindRegisterToRemove(typeof(T), typeof(TH));
//            if (handlerToRemove == null) return;
//            this.RemoveRegister(typeof(T), handlerToRemove);
//        }

//        public void Unsubscribe(Type eventType, IEventHandlerFactory factory)
//        {
//            GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Remove(factory));
//        }

//        private Type? FindRegisterToRemove(Type eventData, Type eventHandler)
//        {
//            if (!HasRegisterForEvent(eventData))
//            {
//                return null;
//            }

//            return this._eventAndHandlerMapping[eventData].FirstOrDefault(eh => eh == eventHandler);
//        }

//        public bool HasRegisterForEvent<T>() where T : IEventData
//        {
//            return this._eventAndHandlerMapping.ContainsKey(typeof(T));
//        }

//        public bool HasRegisterForEvent(Type eventData)
//        {
//            return this._eventAndHandlerMapping.ContainsKey(eventData);
//        }

//        public IEnumerable<Type> GetHandlersForEvent<T>() where T : IEventData
//        {
//            return GetHandlersForEvent(typeof(T));
//        }

//        public IEnumerable<Type> GetHandlersForEvent(Type eventData)
//        {
//            if (this.HasRegisterForEvent(eventData))
//            {
//                return this._eventAndHandlerMapping[eventData];
//            }

//            return new List<Type>();
//        }

//        public Type? GetEventTypeByName(string eventName)
//        {
//            return this._eventAndHandlerMapping.Keys.FirstOrDefault(eh => eh.Name == eventName);
//        }

//        public bool IsEmpty => !this._eventAndHandlerMapping.Keys.Any();

//        public void Clear() => this._eventAndHandlerMapping.Clear();

//        public bool TryAddRegister<T, TH>()
//            where T : IEventData
//            where TH : IEventHandler
//        {
//            throw new NotImplementedException();
//        }

//        public bool TryAddRegister(Type eventData, Type eventHandler)
//        {
//            lock (LockObj)
//            {
//                if (!this.HasRegisterForEvent(eventData))
//                {
//                    var handlers = new List<Type>();
//                    this._eventAndHandlerMapping.TryAdd(eventData, handlers);
//                }

//                if (this._eventAndHandlerMapping[eventData].All(h => h != eventHandler))
//                {
//                    this._eventAndHandlerMapping[eventData].Add(eventHandler);
//                    return true;
//                }
//                else
//                {
//                    return false;
//                }
//            }
//        }

//        public bool TryAddActionRegister<T>(Action<T> action) where T : IEventData
//        {
//            throw new NotImplementedException();
//        }

//        /// <summary>
//        /// 提供入口支持注册其它程序集中实现的IEventHandler
//        /// </summary>
//        /// <param name="assembly"></param>
//        public void RegisterAllEventHandlerFromAssembly(Assembly assembly)
//        {
//            foreach (var type in assembly.DefinedTypes)
//            {
//                if (typeof(IEventHandler).IsAssignableFrom(type))//判断当前类型是否实现了IEventHandler接口
//                {
//                    var handlerAsyncInterface = type.GetInterface("IEventHandlerAsync`1");//获取该类实现的泛型接口
//                    if (handlerAsyncInterface != null)
//                    {
//                        var eventAsycnDataType = handlerAsyncInterface.GetGenericArguments()[0]; // 获取泛型接口指定的参数类型

//                        this.AddRegister(eventAsycnDataType, type);
//                    }
//                }
//            }
//        }

//        public void RegisterModule(IEventModuls eventModuls)
//        {
//            eventModuls.RegisterEventHandler(this);
//        }

//        public void RegisterAllEventModulsFromAssembly(Assembly assembly)
//        {
//            foreach (var type in assembly.DefinedTypes)
//            {
//                if (typeof(IEventModuls).IsAssignableFrom(type))//判断当前类型是否实现了IEventHandler接口
//                {
//                    this.RegisterModule((IEventModuls)type);
//                }
//            }
//        }



//        public void RemoveActionRegister<T>(Action<T> action) where T : IEventData
//        {
//            var actionHandler = new ActionEventHandlerAsync<T>(action);
//            var handlerToRemove = FindRegisterToRemove(typeof(T), actionHandler.GetType());
//            if (handlerToRemove == null) return;
//            this.RemoveRegister(typeof(T), handlerToRemove);
//        }


//        public void RemoveRegister(Type eventData, Type eventHandler)
//        {
//            if (eventHandler != null)
//            {
//                lock (LockObj)
//                {
//                    this._eventAndHandlerMapping[eventData].Remove(eventHandler);
//                    if (!this._eventAndHandlerMapping[eventData].Any())
//                    {
//                        this._eventAndHandlerMapping.TryRemove(eventData, out var _);
//                    }
//                }
//            }
//        }

//        private List<IEventHandlerFactory> GetOrCreateHandlerFactories(Type eventType)
//        {
//            return HandlerFactories.GetOrAdd(eventType, (type) => new List<IEventHandlerFactory>());
//        }

//        private static bool ShouldTriggerEventForHandler(Type targetEventType, Type handlerEventType)
//        {
//            //Should trigger same type
//            if (handlerEventType == targetEventType)
//            {
//                return true;
//            }

//            //Should trigger for inherited types
//            if (handlerEventType.IsAssignableFrom(targetEventType))
//            {
//                return true;
//            }

//            return false;
//        }
//    }
//}
