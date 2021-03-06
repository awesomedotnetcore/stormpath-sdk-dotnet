﻿// <copyright file="FieldConverterList_tests.cs" company="Stormpath, Inc.">
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
using System.Collections.Generic;
using Shouldly;
using Stormpath.SDK.Account;
using Stormpath.SDK.Impl.Serialization.FieldConverters;
using Xunit;

namespace Stormpath.SDK.Tests
{
    public class FieldConverterList_tests
    {
        [Fact]
        public void Returns_null_and_false_if_no_converter_can_handle_field()
        {
            var converterList = new FieldConverterList();
            var dummyField = new KeyValuePair<string, object>("foo", "bar");
            var testTarget = Type.GetType(nameof(IAccount));

            var result = converterList.TryConvertField(dummyField, testTarget);

            result.Success.ShouldBeFalse();
            result.Value.ShouldBeNull();
        }
    }
}
