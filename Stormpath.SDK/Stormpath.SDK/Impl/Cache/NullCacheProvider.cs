﻿// <copyright file="NullCacheProvider.cs" company="Stormpath, Inc.">
// Copyright (c) 2015 Stormpath, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using Stormpath.SDK.Cache;

namespace Stormpath.SDK.Impl.Cache
{
    internal sealed class NullCacheProvider : ISynchronousCacheProvider, IAsynchronousCacheProvider
    {
        bool ICacheProvider.IsAsynchronousSupported => true;

        bool ICacheProvider.IsSynchronousSupported => true;

        ISynchronousCache<K, V> ISynchronousCacheProvider.GetCache<K, V>(string name)
        {
            return new NullCache<K, V>();
        }

        Task<IAsynchronousCache<K, V>> IAsynchronousCacheProvider.GetCacheAsync<K, V>(string name, CancellationToken cancellationToken)
        {
            return Task.FromResult<IAsynchronousCache<K, V>>(new NullCache<K, V>());
        }
    }
}
