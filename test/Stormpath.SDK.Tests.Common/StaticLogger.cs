﻿// <copyright file="StaticLogger.cs" company="Stormpath, Inc.">
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

using System;
using Stormpath.SDK.Logging;

namespace Stormpath.SDK.Tests.Common
{
    public static class StaticLogger
    {
        private static readonly Lazy<ILogger> LoggerInstance =
            new Lazy<ILogger>(() => new InMemoryLogger());

        public static ILogger Instance => LoggerInstance.Value;

        private static readonly Lazy<string> LoggerResult =
            new Lazy<string>(() => Instance.ToString());

        public static string GetLog()
            => LoggerResult.Value;
    }
}
