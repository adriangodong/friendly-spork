# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## Unreleased

### Changed

- Auth0ClientFactory constructor parameter now requires IAuth0AccessTokenFactory interface instead.

### Fixed

- Fixed service registration by providing factory method to use the correct constructor overload.

## [0.0.1] - 2018-12-14

### Added

- First implementation.
