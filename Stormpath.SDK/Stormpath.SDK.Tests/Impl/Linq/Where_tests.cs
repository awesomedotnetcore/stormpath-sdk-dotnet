﻿// <copyright file="Where_tests.cs" company="Stormpath, Inc.">
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

using System;
using Shouldly;
using Stormpath.SDK.Tests.Helpers;
using Xunit;

namespace Stormpath.SDK.Tests.Impl.Linq
{
    public class Where_tests : Linq_tests
    {
        [Fact]
        public void Where_throws_for_constant()
        {
            Should.Throw<NotSupportedException>(() =>
            {
                var query = this.Harness.Queryable.Where(x => true);
                query.GeneratedArgumentsWere(this.Href, "<not evaluated>");
            });

            Should.Throw<NotSupportedException>(() =>
            {
                var query = this.Harness.Queryable.Where(x => false);
                query.GeneratedArgumentsWere(this.Href, "<not evaluated>");
            });
        }

        [Fact]
        public void Where_throws_for_unsupported_comparison_operators()
        {
            Should.Throw<NotSupportedException>(() =>
            {
                var query = this.Harness.Queryable.Where(x => x.Email != "foo");
                query.GeneratedArgumentsWere(this.Href, "<not evaluated>");
            });
        }

        [Fact]
        public void Where_throws_for_more_complex_overloads_of_helper_methods()
        {
            Should.Throw<NotSupportedException>(() =>
            {
                var query = this.Harness.Queryable.Where(x => x.Email.Equals("bar", StringComparison.CurrentCulture));
                query.GeneratedArgumentsWere(this.Href, "<not evaluated>");
            });

            Should.Throw<NotSupportedException>(() =>
            {
                var query = this.Harness.Queryable.Where(x => x.Email.StartsWith("foo", StringComparison.InvariantCultureIgnoreCase));
                query.GeneratedArgumentsWere(this.Href, "<not evaluated>");
            });
        }

        [Fact]
        public void Where_throws_for_unsupported_helper_methods()
        {
            Should.Throw<NotSupportedException>(() =>
            {
                var query = this.Harness.Queryable.Where(x => x.Email.ToUpper() == "FOO");
                query.GeneratedArgumentsWere(this.Href, "<not evaluated>");
            });
        }

        [Fact]
        public void Where_throws_for_binary_or()
        {
            Should.Throw<NotSupportedException>(() =>
            {
                var query = this.Harness.Queryable.Where(x => x.Email == "foo" || x.Email == "bar");
                query.GeneratedArgumentsWere(this.Href, "<not evaluated>");
            });
        }

        [Fact]
        public void Where_attribute_equals()
        {
            var query = this.Harness.Queryable
                .Where(x => x.Email == "tk421@deathstar.co");

            query.GeneratedArgumentsWere(this.Href, "email=tk421@deathstar.co");
        }

        [Fact]
        public void Where_attribute_equals_using_helper_method()
        {
            var query = this.Harness.Queryable
                .Where(x => x.Email.Equals("tk421@deathstar.co"));

            query.GeneratedArgumentsWere(this.Href, "email=tk421@deathstar.co");
        }

        [Fact]
        public void Where_attribute_equals_using_variable()
        {
            var email = "tk421@deathstar.co";

            var query = this.Harness.Queryable
                .Where(x => x.Email.Equals(email));

            query.GeneratedArgumentsWere(this.Href, "email=tk421@deathstar.co");
        }

        [Fact]
        public void Where_attribute_equals_using_formatted_string()
        {
            var domain = "deathstar.co";

            var query = this.Harness.Queryable
                .Where(x => x.Email.Equals($"tk421@{domain}"));

            query.GeneratedArgumentsWere(this.Href, "email=tk421@deathstar.co");
        }

        [Fact]
        public void Where_attribute_starts_with()
        {
            var query = this.Harness.Queryable
                .Where(x => x.Email.StartsWith("tk421"));

            query.GeneratedArgumentsWere(this.Href, "email=tk421*");
        }

        [Fact]
        public void Where_attribute_ends_with()
        {
            var query = this.Harness.Queryable
                .Where(x => x.Email.EndsWith("deathstar.co"));

            query.GeneratedArgumentsWere(this.Href, "email=*deathstar.co");
        }

        [Fact]
        public void Where_attribute_contains()
        {
            var query = this.Harness.Queryable
                .Where(x => x.Email.Contains("421"));

            query.GeneratedArgumentsWere(this.Href, "email=*421*");
        }

        [Fact]
        public void Where_multiple_attributes_with_and()
        {
            var query = this.Harness.Queryable
                .Where(x => x.Email == "tk421@deathstar.co" && x.Username == "tk421");

            query.GeneratedArgumentsWere(this.Href, "email=tk421@deathstar.co&username=tk421");
        }

        [Fact]
        public void Where_multiple_wheres()
        {
            var query = this.Harness.Queryable
                .Where(x => x.Email == "tk421@deathstar.co")
                .Where(x => x.Username.StartsWith("tk421"));

            query.GeneratedArgumentsWere(this.Href, "email=tk421@deathstar.co&username=tk421*");
        }

        [Fact]
        public void Where_date_attribute_greater_than()
        {
            var testDate = new DateTimeOffset(2015, 01, 01, 06, 00, 00, TimeSpan.Zero);
            var query = this.Harness.Queryable
                .Where(x => x.CreatedAt > testDate);

            query.GeneratedArgumentsWere(this.Href, "createdAt=(2015-01-01T06:00:00Z,]");
        }

        [Fact]
        public void Where_date_attribute_greater_than_or_equalto()
        {
            var testDate = new DateTimeOffset(2015, 01, 01, 06, 00, 00, TimeSpan.Zero);
            var query = this.Harness.Queryable
                .Where(x => x.CreatedAt >= testDate);

            query.GeneratedArgumentsWere(this.Href, "createdAt=[2015-01-01T06:00:00Z,]");
        }

        [Fact]
        public void Where_date_attribute_less_than()
        {
            var testDate = new DateTimeOffset(2016, 01, 01, 12, 00, 00, TimeSpan.Zero);
            var query = this.Harness.Queryable
                .Where(x => x.ModifiedAt < testDate);

            query.GeneratedArgumentsWere(this.Href, "modifiedAt=[,2016-01-01T12:00:00Z)");
        }

        [Fact]
        public void Where_date_attribute_less_than_or_equalto()
        {
            var testDate = new DateTimeOffset(2016, 01, 01, 12, 00, 00, TimeSpan.Zero);
            var query = this.Harness.Queryable
                .Where(x => x.ModifiedAt <= testDate);

            query.GeneratedArgumentsWere(this.Href, "modifiedAt=[,2016-01-01T12:00:00Z]");
        }

        [Fact]
        public void Where_date_attribute_between()
        {
            var testStartDate = new DateTimeOffset(2015, 01, 01, 00, 00, 00, TimeSpan.Zero);
            var testEndDate = new DateTimeOffset(2015, 12, 31, 23, 59, 59, TimeSpan.Zero);
            var query = this.Harness.Queryable
                .Where(x => x.CreatedAt > testStartDate && x.CreatedAt <= testEndDate);

            query.GeneratedArgumentsWere(this.Href, "createdAt=(2015-01-01T00:00:00Z,2015-12-31T23:59:59Z]");
        }

        [Fact]
        public void Where_date_attribute_using_implicit_DateTime()
        {
            var testDate = new DateTime(2016, 01, 01, 12, 00, 00);
            var query = this.Harness.Queryable
                .Where(x => x.ModifiedAt < testDate);

            var timezoneOffset = new DateTimeOffset(testDate).Offset;
            var adjustedHour = (int)(12 - timezoneOffset.TotalHours);

            query.GeneratedArgumentsWere(this.Href, $"modifiedAt=[,2016-01-01T{adjustedHour}:00:00Z)");
        }

        [Fact]
        public void Where_date_attribute_equals()
        {
            var testDate = new DateTime(2016, 01, 01, 12, 00, 00);

            Should.Throw<NotSupportedException>(() =>
            {
                var query = this.Harness.Queryable
                    .Where(x => x.ModifiedAt == testDate);

                query.GeneratedArgumentsWere(this.Href, "<not evaluated>");
            });
        }

        [Fact]
        public void Alternate_query_syntax_is_okay()
        {
            var query = from account in this.Harness.Queryable
                        where account.Email == "tk421@deathstar.co"
                        select account;

            query.GeneratedArgumentsWere(this.Href, "email=tk421@deathstar.co");
        }
    }
}
