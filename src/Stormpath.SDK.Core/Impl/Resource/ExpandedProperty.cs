﻿// <copyright file="ExpandedProperty.cs" company="Stormpath, Inc.">
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

using Map = System.Collections.Generic.IDictionary<string, object>;

namespace Stormpath.SDK.Impl.Resource
{
    /// <summary>
    /// Represents embedded resource data that is returned from a link expansion request.
    /// </summary>
    internal sealed class ExpandedProperty : IEmbeddedProperty
    {
        private readonly Map data;
        private readonly string href;

        public ExpandedProperty(Map data)
        {
            object href;
            if (data.TryGetValue("href", out href))
            {
                this.href = href.ToString();
            }

            this.data = data;
        }

        public string Href => this.href;

        public Map Data => this.data;
    }
}
