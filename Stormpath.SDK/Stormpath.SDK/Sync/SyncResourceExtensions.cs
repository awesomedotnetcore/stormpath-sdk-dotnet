﻿// <copyright file="SyncResourceExtensions.cs" company="Stormpath, Inc.">
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

using Stormpath.SDK.Impl.Resource;
using Stormpath.SDK.Resource;

namespace Stormpath.SDK.Sync
{
    public static class SyncResourceExtensions
    {
        /// <summary>
        /// Synchronously creates or updates the resource.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <typeparam name="T">The <see cref="IResource"/> type.</typeparam>
        /// <returns>The persisted resource data.</returns>
        /// <exception cref="Error.ResourceException">The save operation failed.</exception>
        public static T Save<T>(this ISaveable<T> resource)
            where T : IResource
            => (resource as ISaveableSync<T>).Save();

        /// <summary>
        /// Synchronously deletes the resource.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <returns>Whether the delete operation succeeded.</returns>
        /// <exception cref="Error.ResourceException">The delete operation failed.</exception>
        public static bool Delete(this IDeletable resource)
            => (resource as IDeletableSync).Delete();
    }
}
