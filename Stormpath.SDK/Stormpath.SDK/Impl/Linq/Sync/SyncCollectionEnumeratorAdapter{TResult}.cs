﻿// <copyright file="SyncCollectionEnumeratorAdapter{TResult}.cs" company="Stormpath, Inc.">
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

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Stormpath.SDK.Impl.Linq.Executor;

namespace Stormpath.SDK.Impl.Linq.Sync
{
    internal sealed class SyncCollectionEnumeratorAdapter<TResult> : IEnumerable<TResult>
    {
        private readonly IAsyncExecutor<TResult> executor;
        private readonly CancellationToken cancellationToken;

        public SyncCollectionEnumeratorAdapter(IAsyncExecutor<TResult> executor, CancellationToken cancellationToken = default(CancellationToken))
        {
            this.executor = executor;
            this.cancellationToken = cancellationToken;
        }

        public IEnumerator<TResult> GetEnumerator()
        {
            while (this.executor.MoveNext())
            {
                this.cancellationToken.ThrowIfCancellationRequested();

                foreach (var item in this.executor.CurrentPage)
                {
                    yield return item;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
