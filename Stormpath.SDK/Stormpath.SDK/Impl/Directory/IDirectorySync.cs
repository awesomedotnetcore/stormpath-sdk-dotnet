﻿// <copyright file="IDirectorySync.cs" company="Stormpath, Inc.">
//      Copyright (c) 2015 Stormpath, Inc.
// </copyright>
// <remarks>
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </remarks>

using Stormpath.SDK.Directory;
using Stormpath.SDK.Group;
using Stormpath.SDK.Impl.Account;
using Stormpath.SDK.Impl.Resource;

namespace Stormpath.SDK.Impl.Directory
{
    internal interface IDirectorySync : ISaveableSync<IDirectory>, IDeletableSync, IAccountCreationActionsSync, IExtendableSync
    {
        /// <summary>
        /// Synchronously creates a new <see cref="IGroup"/> instance in this directory.
        /// </summary>
        /// <param name="group">The group to create/persist.</param>
        /// <returns>The newly-created <see cref="IGroup"/>.</returns>
        IGroup CreateGroup(IGroup group);
    }
}