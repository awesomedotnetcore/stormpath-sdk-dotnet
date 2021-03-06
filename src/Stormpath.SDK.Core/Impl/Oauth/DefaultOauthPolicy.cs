﻿// <copyright file="DefaultOauthPolicy.cs" company="Stormpath, Inc.">
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
using Stormpath.SDK.Impl.Resource;
using Stormpath.SDK.Impl.Utility;
using Stormpath.SDK.Oauth;

namespace Stormpath.SDK.Impl.Oauth
{
    internal sealed partial class DefaultOauthPolicy :
        AbstractInstanceResource,
        IOauthPolicy,
        IOauthPolicySync
    {
        private static readonly string AccessTokenTtlPropertyName = "accessTokenTtl";
        private static readonly string RefreshTokenTtlPropertyName = "refreshTokenTtl";
        private static readonly string TokenEndpointPropertyName = "tokenEndpoint";
        private static readonly string ApplicationPropertyName = "application";

        internal IEmbeddedProperty Application => this.GetLinkProperty(ApplicationPropertyName);

        internal IEmbeddedProperty TokenEndpoint => this.GetLinkProperty(TokenEndpointPropertyName);

        TimeSpan IOauthPolicy.AccessTokenTimeToLive
            => Iso8601Duration.Parse(this.GetStringProperty(AccessTokenTtlPropertyName));

        TimeSpan IOauthPolicy.RefreshTokenTimeToLive
            => Iso8601Duration.Parse(this.GetStringProperty(RefreshTokenTtlPropertyName));

        string IOauthPolicy.TokenEndpointHref => this.TokenEndpoint.Href;

        public DefaultOauthPolicy(ResourceData data)
            : base(data)
        {
        }

        IOauthPolicy IOauthPolicy.SetAccessTokenTimeToLive(TimeSpan accessTokenTtl)
        {
            this.SetProperty(AccessTokenTtlPropertyName, Iso8601Duration.Format(accessTokenTtl));
            return this;
        }

        IOauthPolicy IOauthPolicy.SetAccessTokenTimeToLive(string accessTokenTtl)
        {
            this.SetProperty(AccessTokenTtlPropertyName, accessTokenTtl);
            return this;
        }

        IOauthPolicy IOauthPolicy.SetRefreshTokenTimeToLive(TimeSpan refreshTokenTtl)
        {
            this.SetProperty(RefreshTokenTtlPropertyName, Iso8601Duration.Format(refreshTokenTtl));
            return this;
        }

        IOauthPolicy IOauthPolicy.SetRefreshTokenTimeToLive(string refreshTokenTtl)
        {
            this.SetProperty(AccessTokenTtlPropertyName, refreshTokenTtl);
            return this;
        }
    }
}
