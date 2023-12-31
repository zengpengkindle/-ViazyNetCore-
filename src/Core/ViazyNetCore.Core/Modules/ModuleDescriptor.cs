﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules
{
    public class ModuleDescriptor : IModuleDescriptor
    {
        public Type Type { get; }

        public Assembly Assembly { get; }

        public IInjectionModule Instance { get; }

        public bool IsLoadedAsPlugIn { get; }

        public IReadOnlyList<IModuleDescriptor> Dependencies => _dependencies.ToImmutableList();
        private readonly List<IModuleDescriptor> _dependencies;

        public ModuleDescriptor(
            [NotNull] Type type,
            [NotNull] IInjectionModule instance,
            bool isLoadedAsPlugIn)
        {
            Check.NotNull(type, nameof(type));
            Check.NotNull(instance, nameof(instance));

            if (!type.GetTypeInfo().IsAssignableFrom(instance.GetType()))
            {
                throw new ArgumentException($"Given module instance ({instance.GetType().AssemblyQualifiedName}) is not an instance of given module type: {type.AssemblyQualifiedName}");
            }

            Type = type;
            Assembly = type.Assembly;
            Instance = instance;
            IsLoadedAsPlugIn = isLoadedAsPlugIn;

            _dependencies = new List<IModuleDescriptor>();
        }

        public void AddDependency(IModuleDescriptor descriptor)
        {
            _dependencies.AddIfNotContains(descriptor);
        }

        public override string ToString()
        {
            return $"[ModuleDescriptor {Type.FullName}]";
        }
    }
}
