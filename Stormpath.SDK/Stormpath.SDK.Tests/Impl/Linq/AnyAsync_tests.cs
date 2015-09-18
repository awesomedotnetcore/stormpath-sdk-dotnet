﻿// <copyright file="AnyAsync_tests.cs" company="Stormpath, Inc.">
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

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Stormpath.SDK.Account;
using Stormpath.SDK.Tests.Fakes;
using Stormpath.SDK.Tests.Helpers;
using Xunit;

namespace Stormpath.SDK.Tests.Impl.Linq
{
    public class AnyAsync_tests : Linq_tests
    {
        [Fact]
        public async Task Returns_false_for_empty_collection()
        {
            var fakeDataStore = new FakeDataStore<IAccount>(Enumerable.Empty<IAccount>());
            var harness = CollectionTestHarness<IAccount>.Create<IAccount>(this.Href, fakeDataStore);

            (await harness.Queryable.AnyAsync()).ShouldBeFalse();
        }

        [Fact]
        public async Task Returns_true_for_nonempty_collection()
        {
            var fakeDataStore = new FakeDataStore<IAccount>(Enumerable.Repeat(FakeAccounts.R2D2, 73));
            var harness = CollectionTestHarness<IAccount>.Create<IAccount>(this.Href, fakeDataStore);

            (await harness.Queryable.AnyAsync()).ShouldBeTrue();
        }

        [Fact]
        public async Task Limits_result_to_one_item()
        {
            var fakeDataStore = new FakeDataStore<IAccount>(Enumerable.Empty<IAccount>());
            var harness = CollectionTestHarness<IAccount>.Create<IAccount>(this.Href, fakeDataStore);

            await harness.Queryable.AnyAsync();

            fakeDataStore.WasCalledWithArguments<IAccount>(this.Href, "limit=1");
        }
    }
}