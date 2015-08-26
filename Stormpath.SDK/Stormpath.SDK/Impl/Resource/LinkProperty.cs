﻿// <copyright file="LinkProperty.cs" company="Stormpath, Inc.">
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

using System;
using Stormpath.SDK.Impl.Utility;

namespace Stormpath.SDK.Impl.Resource
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:Elements must appear in the correct order", Justification = "Reviewed.")]
    internal sealed class LinkProperty : ImmutableValueObject<LinkProperty>
    {
        private static Func<LinkProperty, LinkProperty, bool> EqualityFunction =>
            (a, b) => string.Equals(a?.Href, b?.Href, StringComparison.OrdinalIgnoreCase);

        public LinkProperty(string href)
            : base(EqualityFunction)
        {
            this.Href = href;
        }

        public string Href { get; }

        public override string ToString() => Href;
    }
}