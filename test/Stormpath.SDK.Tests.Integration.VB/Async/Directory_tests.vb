' <copyright file="Directory_tests.vb" company="Stormpath, Inc.">
' Copyright (c) 2016 Stormpath, Inc.
'
' Licensed under the Apache License, Version 2.0 (the "License");
' you may not use this file except in compliance with the License.
' You may obtain a copy of the License at
'
'      http://www.apache.org/licenses/LICENSE-2.0
'
' Unless required by applicable law or agreed to in writing, software
' distributed under the License is distributed on an "AS IS" BASIS,
' WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
' See the License for the specific language governing permissions and
' limitations under the License.
' </copyright>

Option Strict On
Option Explicit On
Option Infer On
Imports Shouldly
Imports Stormpath.SDK.Directory
Imports Stormpath.SDK.Provider
Imports Stormpath.SDK.Tests.Common.Integration
Imports Xunit

Namespace Async
    <Collection(NameOf(IntegrationTestCollection))>
    Public Class Directory_tests
        Private ReadOnly fixture As TestFixture

        Public Sub New(fixture As TestFixture)
            Me.fixture = fixture
        End Sub

        <Theory>
        <MemberData(NameOf(TestClients.GetClients), MemberType:=GetType(TestClients))>
        Public Async Function Getting_tenant_directories(clientBuilder As TestClientProvider) As Task
            Dim client = clientBuilder.GetClient()
            Dim tenant = Await client.GetCurrentTenantAsync()
            Dim directories = Await tenant.GetDirectories().ToListAsync()

            directories.Count.ShouldNotBe(0)
        End Function

        <Theory>
        <MemberData(NameOf(TestClients.GetClients), MemberType:=GetType(TestClients))>
        Public Async Function Getting_directory_tenant(clientBuilder As TestClientProvider) As Task
            Dim client = clientBuilder.GetClient()
            Dim directory = Await client.GetResourceAsync(Of IDirectory)(Me.fixture.PrimaryDirectoryHref)

            ' Verify data from IntegrationTestData
            Dim tenantHref = (Await directory.GetTenantAsync()).Href
            tenantHref.ShouldBe(Me.fixture.TenantHref)
        End Function

        <Theory>
        <MemberData(NameOf(TestClients.GetClients), MemberType:=GetType(TestClients))>
        Public Async Function Getting_directory_applications(clientBuilder As TestClientProvider) As Task
            Dim client = clientBuilder.GetClient()
            Dim directory = Await client.GetDirectoryAsync(Me.fixture.PrimaryDirectoryHref)

            Dim apps = Await directory.GetApplications().ToListAsync()
            apps.Where(Function(x) x.Href = Me.fixture.PrimaryApplicationHref).Any().ShouldBeTrue()
        End Function

        <Theory>
        <MemberData(NameOf(TestClients.GetClients), MemberType:=GetType(TestClients))>
        Public Async Function Creating_disabled_directory(clientBuilder As TestClientProvider) As Task
            Dim client = clientBuilder.GetClient()
            Dim tenant = Await client.GetCurrentTenantAsync()

            Dim directoryName = $"My New Disabled Directory (.NET IT {fixture.TestRunIdentifier})"
            Dim created = Await tenant.CreateDirectoryAsync(directoryName, "A great directory for my app", DirectoryStatus.Disabled)
            created.Href.ShouldNotBeNullOrEmpty()
            Me.fixture.CreatedDirectoryHrefs.Add(created.Href)

            created.Name.ShouldBe(directoryName)
            created.Description.ShouldBe("A great directory for my app")
            created.Status.ShouldBe(DirectoryStatus.Disabled)

            Dim provider = Await created.GetProviderAsync()
            provider.ProviderId.ShouldBe("stormpath")

            ' Cleanup
            Assert.True(Await created.DeleteAsync())
            Me.fixture.CreatedDirectoryHrefs.Remove(created.Href)
        End Function

        <Theory>
        <MemberData(NameOf(TestClients.GetClients), MemberType:=GetType(TestClients))>
        Public Async Function Creating_directory_with_response_options(clientBuilder As TestClientProvider) As Task
            Dim client = clientBuilder.GetClient()
            Dim tenant = Await client.GetCurrentTenantAsync()

            Dim directory = client.Instantiate(Of IDirectory)().SetName($"My New Directory With Options (.NET IT {fixture.TestRunIdentifier})").SetDescription("Another great directory for my app").SetStatus(DirectoryStatus.Disabled)

            Await tenant.CreateDirectoryAsync(directory, Function(opt) opt.ResponseOptions.Expand(Function(x) x.GetCustomData()))

            directory.Href.ShouldNotBeNullOrEmpty()
            Me.fixture.CreatedDirectoryHrefs.Add(directory.Href)

            ' Cleanup
            Assert.True(Await directory.DeleteAsync())
            Me.fixture.CreatedDirectoryHrefs.Remove(directory.Href)
        End Function

        <Theory>
        <MemberData(NameOf(TestClients.GetClients), MemberType:=GetType(TestClients))>
        Public Async Function Modifying_directory(clientBuilder As TestClientProvider) As Task
            Dim client = clientBuilder.GetClient()
            Dim tenant = Await client.GetCurrentTenantAsync()

            Dim directoryName = $"My New Directory (.NET IT {fixture.TestRunIdentifier} - {clientBuilder.Name})"
            Dim newDirectory = client.Instantiate(Of IDirectory)()
            newDirectory.SetName(directoryName)
            newDirectory.SetDescription("Put some accounts here!")
            newDirectory.SetStatus(DirectoryStatus.Enabled)

            Dim created = Await tenant.CreateDirectoryAsync(newDirectory)
            created.Href.ShouldNotBeNullOrEmpty()
            Me.fixture.CreatedDirectoryHrefs.Add(created.Href)

            created.SetDescription("foobar")
            created.CustomData.Put("good", True)
            Dim updated = Await created.SaveAsync()

            updated.Name.ShouldBe(directoryName)
            updated.Description.ShouldBe("foobar")
            Dim customData = Await updated.GetCustomDataAsync()
            CBool(customData("good")).ShouldBe(True)

            ' Cleanup
            Assert.True(Await updated.DeleteAsync())
            Me.fixture.CreatedDirectoryHrefs.Remove(updated.Href)
        End Function

        <Theory>
        <MemberData(NameOf(TestClients.GetClients), MemberType:=GetType(TestClients))>
        Public Async Function Saving_with_response_options(clientBuilder As TestClientProvider) As Task
            Dim client = clientBuilder.GetClient()
            Dim tenant = Await client.GetCurrentTenantAsync()

            Dim directoryName = $"My New Directory #2 (.NET IT {fixture.TestRunIdentifier} - {clientBuilder.Name})"
            Dim newDirectory = client.Instantiate(Of IDirectory)()
            newDirectory.SetName(directoryName)
            newDirectory.SetDescription("Put some accounts here!")
            newDirectory.SetStatus(DirectoryStatus.Enabled)

            Dim created = Await tenant.CreateDirectoryAsync(newDirectory)
            created.Href.ShouldNotBeNullOrEmpty()
            Me.fixture.CreatedDirectoryHrefs.Add(created.Href)

            created.SetDescription("foobar")
            created.CustomData.Put("good", True)
            Await created.SaveAsync(Function(response) response.Expand(Function(x) x.GetCustomData()))

            ' Cleanup
            Assert.True(Await created.DeleteAsync())
            Me.fixture.CreatedDirectoryHrefs.Remove(created.Href)
        End Function

        <Theory>
        <MemberData(NameOf(TestClients.GetClients), MemberType:=GetType(TestClients))>
        Public Async Function Creating_facebook_directory(clientBuilder As TestClientProvider) As Task
            Dim client = clientBuilder.GetClient()
            Dim tenant = Await client.GetCurrentTenantAsync()

            Dim directoryName = $"My New Facebook Directory (.NET IT {fixture.TestRunIdentifier} - {clientBuilder.Name})"

            Dim instance = client.Instantiate(Of IDirectory)()
            instance.SetName(directoryName)
            Dim created = Await tenant.CreateDirectoryAsync(instance, Function(options) options.ForProvider(client.Providers().Facebook().Builder().SetClientId("foobar").SetClientSecret("secret123!").Build()))

            created.Href.ShouldNotBeNullOrEmpty()
            Me.fixture.CreatedDirectoryHrefs.Add(created.Href)

            created.Name.ShouldBe(directoryName)

            Dim provider = TryCast(Await created.GetProviderAsync(), IFacebookProvider)
            provider.ShouldNotBeNull()
            provider.Href.ShouldNotBeNullOrEmpty()

            provider.ProviderId.ShouldBe("facebook")
            provider.ClientId.ShouldBe("foobar")
            provider.ClientSecret.ShouldBe("secret123!")

            ' Cleanup
            Assert.True(Await created.DeleteAsync())
            Me.fixture.CreatedDirectoryHrefs.Remove(created.Href)
        End Function

        <Theory>
        <MemberData(NameOf(TestClients.GetClients), MemberType:=GetType(TestClients))>
        Public Async Function Creating_github_directory(clientBuilder As TestClientProvider) As Task
            Dim client = clientBuilder.GetClient()
            Dim tenant = Await client.GetCurrentTenantAsync()

            Dim directoryName = $"My New Github Directory (.NET IT {fixture.TestRunIdentifier} - {clientBuilder.Name})"

            Dim instance = client.Instantiate(Of IDirectory)()
            instance.SetName(directoryName)
            Dim created = Await tenant.CreateDirectoryAsync(instance, Function(options) options.ForProvider(client.Providers().Github().Builder().SetClientId("foobar").SetClientSecret("secret123!").Build()))

            created.Href.ShouldNotBeNullOrEmpty()
            Me.fixture.CreatedDirectoryHrefs.Add(created.Href)

            created.Name.ShouldBe(directoryName)

            Dim provider = TryCast(Await created.GetProviderAsync(), IGithubProvider)
            provider.ShouldNotBeNull()
            provider.Href.ShouldNotBeNullOrEmpty()

            provider.ProviderId.ShouldBe("github")
            provider.ClientId.ShouldBe("foobar")
            provider.ClientSecret.ShouldBe("secret123!")

            ' Cleanup
            Assert.True(Await created.DeleteAsync())
            Me.fixture.CreatedDirectoryHrefs.Remove(created.Href)
        End Function

        <Theory>
        <MemberData(NameOf(TestClients.GetClients), MemberType:=GetType(TestClients))>
        Public Async Function Creating_google_directory(clientBuilder As TestClientProvider) As Task
            Dim client = clientBuilder.GetClient()
            Dim tenant = Await client.GetCurrentTenantAsync()

            Dim directoryName = $"My New Google Directory (.NET IT {fixture.TestRunIdentifier} - {clientBuilder.Name})"

            Dim instance = client.Instantiate(Of IDirectory)()
            instance.SetName(directoryName)
            Dim created = Await tenant.CreateDirectoryAsync(instance, Function(options) options.ForProvider(client.Providers().Google().Builder().SetClientId("foobar").SetClientSecret("secret123!").SetRedirectUri("foo://bar").Build()))

            created.Href.ShouldNotBeNullOrEmpty()
            Me.fixture.CreatedDirectoryHrefs.Add(created.Href)

            created.Name.ShouldBe(directoryName)

            Dim provider = TryCast(Await created.GetProviderAsync(), IGoogleProvider)
            provider.ShouldNotBeNull()
            provider.Href.ShouldNotBeNullOrEmpty()

            provider.ProviderId.ShouldBe("google")
            provider.ClientId.ShouldBe("foobar")
            provider.ClientSecret.ShouldBe("secret123!")
            provider.RedirectUri.ShouldBe("foo://bar")

            ' Cleanup
            Assert.True(Await created.DeleteAsync())
            Me.fixture.CreatedDirectoryHrefs.Remove(created.Href)
        End Function

        <Theory>
        <MemberData(NameOf(TestClients.GetClients), MemberType:=GetType(TestClients))>
        Public Async Function Creating_linkedin_directory(clientBuilder As TestClientProvider) As Task
            Dim client = clientBuilder.GetClient()
            Dim tenant = Await client.GetCurrentTenantAsync()

            Dim directoryName = $"My New LinkedIn Directory (.NET IT {fixture.TestRunIdentifier} - {clientBuilder.Name})"

            Dim instance = client.Instantiate(Of IDirectory)()
            instance.SetName(directoryName)
            Dim created = Await tenant.CreateDirectoryAsync(instance, Function(options) options.ForProvider(client.Providers().LinkedIn().Builder().SetClientId("foobar").SetClientSecret("secret123!").Build()))

            created.Href.ShouldNotBeNullOrEmpty()
            Me.fixture.CreatedDirectoryHrefs.Add(created.Href)

            created.Name.ShouldBe(directoryName)

            Dim provider = TryCast(Await created.GetProviderAsync(), ILinkedInProvider)
            provider.ShouldNotBeNull()
            provider.Href.ShouldNotBeNullOrEmpty()

            provider.ProviderId.ShouldBe("linkedin")
            provider.ClientId.ShouldBe("foobar")
            provider.ClientSecret.ShouldBe("secret123!")

            ' Cleanup
            Assert.True(Await created.DeleteAsync())
            Me.fixture.CreatedDirectoryHrefs.Remove(created.Href)
        End Function

        <Theory>
        <MemberData(nameof(TestClients.GetClients), MemberType := GetType(TestClients))>
        Public Async Function Getting_and_modifying_account_creation_policy(clientBuilder As TestClientProvider) As Task
        	Dim client = clientBuilder.GetClient()
        	Dim tenant = Await client.GetCurrentTenantAsync()

        	Dim directory = Await client.CreateDirectoryAsync($"Account Creation Policy Test (.NET IT {this.fixture.TestRunIdentifier} - {clientBuilder.Name} - VB)", "Testing Account Creation Policy", DirectoryStatus.Enabled)

        	directory.Href.ShouldNotBeNullOrEmpty()
        	Me.fixture.CreatedDirectoryHrefs.Add(directory.Href)

        	Dim accountCreationPolicy = Await directory.GetAccountCreationPolicyAsync()

        	' Default values
        	accountCreationPolicy.VerificationEmailStatus.ShouldBe(Mail.EmailStatus.Disabled)
        	accountCreationPolicy.VerificationSuccessEmailStatus.ShouldBe(Mail.EmailStatus.Disabled)
        	accountCreationPolicy.WelcomeEmailStatus.ShouldBe(Mail.EmailStatus.Disabled)

        	' Update
        	accountCreationPolicy.SetVerificationEmailStatus(Mail.EmailStatus.Enabled) _
            .SetVerificationSuccessEmailStatus(Mail.EmailStatus.Enabled) _
            .SetWelcomeEmailStatus(Mail.EmailStatus.Enabled)
        	Dim updatedPolicy = Await accountCreationPolicy.SaveAsync()

        	accountCreationPolicy.VerificationEmailStatus.ShouldBe(Mail.EmailStatus.Enabled)
        	accountCreationPolicy.VerificationSuccessEmailStatus.ShouldBe(Mail.EmailStatus.Enabled)
        	accountCreationPolicy.WelcomeEmailStatus.ShouldBe(Mail.EmailStatus.Enabled)

        	' Cleanup
        	(Call Await directory.DeleteAsync()).ShouldBeTrue()
        	Me.fixture.CreatedDirectoryHrefs.Remove(directory.Href)
        End Function

        <Theory> _
        <MemberData(nameof(TestClients.GetClients), MemberType := GetType(TestClients))> _
        Public Async Function Getting_and_modifying_password_policy(clientBuilder As TestClientProvider) As Task
        	Dim client = clientBuilder.GetClient()
        	Dim tenant = Await client.GetCurrentTenantAsync()

        	Dim directory = Await client.CreateDirectoryAsync("Password Policy Test (.NET IT {this.fixture.TestRunIdentifier} - {clientBuilder.Name} - VB)", "Testing Password Policy", DirectoryStatus.Enabled)

        	directory.Href.ShouldNotBeNullOrEmpty()
        	Me.fixture.CreatedDirectoryHrefs.Add(directory.Href)
        
        	Dim passwordPolicy = Await directory.GetPasswordPolicyAsync()

        	' Default values
        	passwordPolicy.ResetEmailStatus.ShouldBe(Mail.EmailStatus.Enabled)
        	passwordPolicy.ResetSuccessEmailStatus.ShouldBe(Mail.EmailStatus.Enabled)
        	passwordPolicy.ResetTokenTtl.ShouldBe(24)

        	' Update
        	passwordPolicy.SetResetEmailStatus(Mail.EmailStatus.Disabled) _
            .SetResetEmailSuccessStatus(Mail.EmailStatus.Disabled) _
            .SetResetTokenTtl(48)
        	Dim updatedPolicy = Await passwordPolicy.SaveAsync()

        	passwordPolicy.ResetEmailStatus.ShouldBe(Mail.EmailStatus.Disabled)
        	passwordPolicy.ResetSuccessEmailStatus.ShouldBe(Mail.EmailStatus.Disabled)
        	passwordPolicy.ResetTokenTtl.ShouldBe(48)

        	' Cleanup
        	(Call Await directory.DeleteAsync()).ShouldBeTrue()
        	Me.fixture.CreatedDirectoryHrefs.Remove(directory.Href)
        End Function

        <Theory> _
        <MemberData(nameof(TestClients.GetClients), MemberType := GetType(TestClients))> _
        Public Async Function Getting_and_modifying_password_strength_policy(clientBuilder As TestClientProvider) As Task
        	Dim client = clientBuilder.GetClient()
        	Dim tenant = Await client.GetCurrentTenantAsync()

        	Dim directory = Await client.CreateDirectoryAsync("Password Strength Policy Test (.NET IT {this.fixture.TestRunIdentifier} - {clientBuilder.Name} - VB)", "Testing Password Strength Policy", DirectoryStatus.Enabled)

        	directory.Href.ShouldNotBeNullOrEmpty()
        	Me.fixture.CreatedDirectoryHrefs.Add(directory.Href)

        	Dim passwordPolicy = Await directory.GetPasswordPolicyAsync()
        	Dim strengthPolicy = Await passwordPolicy.GetPasswordStrengthPolicyAsync()

        	' Default values
        	strengthPolicy.MaximumLength.ShouldBe(100)
        	strengthPolicy.MinimumDiacritic.ShouldBe(0)
        	strengthPolicy.MinimumLength.ShouldBe(8)
        	strengthPolicy.MinimumLowercase.ShouldBe(1)
        	strengthPolicy.MinimumNumeric.ShouldBe(1)
        	strengthPolicy.MinimumSymbols.ShouldBe(0)
        	strengthPolicy.MinimumUppercase.ShouldBe(1)

        	' Update
        	strengthPolicy.SetMaximumLength(50) _
            .SetMinimumDiacritic(2) _
            .SetMinimumLength(3) _
            .SetMinimumLowercase(4) _
            .SetMinimumNumeric(5) _
            .SetMinimumSymbols(6) _
            .SetMinimumUppercase(7)
        	Dim updatedPolicy = Await strengthPolicy.SaveAsync()

        	strengthPolicy.MaximumLength.ShouldBe(50)
        	strengthPolicy.MinimumDiacritic.ShouldBe(2)
        	strengthPolicy.MinimumLength.ShouldBe(3)
        	strengthPolicy.MinimumLowercase.ShouldBe(4)
        	strengthPolicy.MinimumNumeric.ShouldBe(5)
        	strengthPolicy.MinimumSymbols.ShouldBe(6)
        	strengthPolicy.MinimumUppercase.ShouldBe(7)

        	' Cleanup
        	(Call Await directory.DeleteAsync()).ShouldBeTrue()
        	Me.fixture.CreatedDirectoryHrefs.Remove(directory.Href)
        End Function
    End Class
End Namespace
