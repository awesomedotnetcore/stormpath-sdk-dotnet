﻿// <copyright file="Sync_FirstOrDefault_tests.cs" company="Stormpath, Inc.">
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

using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Stormpath.SDK.Account;
using Stormpath.SDK.Sync;
using Stormpath.SDK.Tests.Fakes;
using Stormpath.SDK.Tests.Helpers;
using Xunit;

namespace Stormpath.SDK.Tests.Impl.Linq
{
    public class Sync_FirstOrDefault_tests : Linq_tests
    {
        [Fact]
        public void Returns_first_item()
        {
            var fakeDataStore = new FakeDataStore<IAccount>(new List<IAccount>()
                {
                    FakeAccounts.LukeSkywalker,
                    FakeAccounts.HanSolo
                });
            var harness = CollectionTestHarness<IAccount>.Create<IAccount>(this.Href, fakeDataStore);

            var luke = harness.Queryable.Synchronously().FirstOrDefault();

            luke.Surname.ShouldBe("Skywalker");
        }

        [Fact]
        public void Returns_null_when_no_items_exist()
        {
            var fakeDataStore = new FakeDataStore<IAccount>(Enumerable.Empty<IAccount>());
            var harness = CollectionTestHarness<IAccount>.Create<IAccount>(this.Href, fakeDataStore);

            var notLuke = harness.Queryable.Synchronously().FirstOrDefault();

            notLuke.ShouldBeNull();
        }
    }
}
