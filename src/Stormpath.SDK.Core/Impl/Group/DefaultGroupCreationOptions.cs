﻿// <copyright file="DefaultGroupCreationOptions.cs" company="Stormpath, Inc.">
// Copyright (c) 2016 Stormpath, Inc.
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

using Stormpath.SDK.Group;
using Stormpath.SDK.Resource;

namespace Stormpath.SDK.Impl.Group
{
    internal sealed class DefaultGroupCreationOptions : IGroupCreationOptions
    {
        private readonly IRetrievalOptions<IGroup> responseOptions;

        public DefaultGroupCreationOptions(IRetrievalOptions<IGroup> responseOptions)
        {
            this.responseOptions = responseOptions;
        }

        public string GetQueryString()
        {
            if (this.responseOptions == null)
            {
                return string.Empty;
            }

            return this.responseOptions.ToString();
        }
    }
}
