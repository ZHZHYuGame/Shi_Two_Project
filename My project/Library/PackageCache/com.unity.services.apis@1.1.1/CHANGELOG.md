# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [1.1.0] - 2024-09-13

### Changed
- Multiplay Admin APIs no longer under 'ENABLE_SERVICES_EXPERIMENTAL_APIS' define symbol. Client does not include by default, requires flag to be included.
- MatchmakerTicketsAPI no longer under 'ENABLE_SERVICES_EXPERIMENTAL_APIS' define symbol. Clients only include with flag though.
- IServerClient.SignInFromServer no longer under 'ENABLE_SERVICES_EXPERIMENTAL_APIS' define symbol
- ProxyApi no longer under 'ENABLE_SERVICES_EXPERIMENTAL_APIS' define symbol (SignInFromServer depends on this)
- Game.Friends API no longer under 'ENABLE_SERVICES_EXPERIMENTAL_APIS' define symbol

## [1.0.1] - 2024-07-31

### Fixed
- Deserialization error in Observability GetLogs function

### Changed
- Moved functionality from multiple services under the `ENABLE_SERVICES_EXPERIMENTAL_APIS` define symbol

## [0.2.0] - 2024-03-20

### Added
- Adding Lobby to Trusted client.
- Adding Lobby Editor Sample.

### Removed
- Remove Cloud Content Delivery Game & Admin Apis.

## [0.1.1] - 2024-02-22

### Fixed
- Adding player names api to Trusted and Server clients.
- Adding cloud save files api to Admin client.
- Adding `WaitForCompletion` method in operation for better coroutine support.
- Adding cloud save editor samples for protected and public data.
- Validating proper configuration for editor samples.
- Updating documentation.

## [0.0.14] - 2024-01-26

### Fixed
- Fixed issue with ApiOperation<T> when used with the base class.
- Adding editor samples.

## [0.0.13] - 2024-01-26

### Fixed
- Fixed internal requirement

## [0.0.12] - 2024-01-12

### Added
- Fixing issue with model generation and any type support

## [0.0.11] - 2024-01-09

### Added
- Adding Multiplay server apis
- Minor tweaks to file inclusions and interface compatibility.

## [0.0.10] - 2024-01-05

### Added
- Adding Missing xml doc

## [0.0.9] - 2023-11-23

### Updated
- Updated apis
- Updated documentation
- Changed trusted client preprocessor to use at runtime to ENABLE_RUNTIME_TRUSTED_APIS
- Pointed readme to documentation

## [0.0.8] - 2023-10-25

### Added
- Regenerated apis.
- Adding Player Authentication  Api.
- Adding Cloud Save Admin Api.
- Adding Observability Logs Admin Api.
- Adding Scheduler Admin Api.
- Adding Triggers Admin Api.

## [0.0.7] - 2023-09-20

### Added
- Regenerated apis.
- Added more authentication methods for game client.

## [0.0.6] - 2023-08-03

### Added
- Update to Player Authentication Api and fix for UGC Admin apis base url.

## [0.0.5] - 2023-06-13

### Added
- Updating cloud save apis and user generated content apis.

## [0.0.4] - 2023-06-07

### Added
- Adding CloudSave Game Data and CloudSave Files apis.

### Fixed
- Fixed authorization on Player Authentication apis.

## [0.0.3] - 2023-05-05

### Added
- Added callback methods for all operations.
- Operations no longer throw an api exception on error. You can use EnsureSuccessful after completion to throw.
- Added `IServerApiClient` client for server use cases.
- Replace `ITrustedApiClient` with `IAdminGameApiClient`.

## [0.0.2] - 2023-04-17

### Fixed
- Cloud code admin apis to properly support service account credentials
- Properly disposing web requests

## [0.0.1] - 2023-03-14

### Added
- Added `ApiService` entry point which can create the different types of api clients.
- Added `IGameApiClient`, `IAdminApiClient` and `ITrustedApiClient`.
- Added generated public apis.

### This is the first release of *com.unity.services.apis*.
