﻿// <copyright file="IAsynchronousNonceStore.cs" company="Stormpath, Inc.">
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

using System.Threading;
using System.Threading.Tasks;

namespace Stormpath.SDK.IdSite
{
    /// <inheritdoc/>
    /// <seealso cref="IIdSiteAsyncCallbackHandler.SetNonceStore(INonceStore)"/>
    public interface IAsynchronousNonceStore : INonceStore
    {
        /// <summary>
        /// Determines whether this <see cref="INonceStore"/> contains the given nonce.
        /// </summary>
        /// <param name="nonce">The nonce to check.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><see langword="true"/> if the specified nonce is present in this <see cref="INonceStore"/>; <see langword="false"/> otherwise.</returns>
        Task<bool> ContainsNonceAsync(string nonce, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Adds the specified nonce to the store.
        /// </summary>
        /// <param name="nonce">The nonce to add.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <seealso cref="ContainsNonceAsync(string, CancellationToken)"/>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task PutNonceAsync(string nonce, CancellationToken cancellationToken = default(CancellationToken));
    }
}
