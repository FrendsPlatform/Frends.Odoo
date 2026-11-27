# Changelog

## [2.0.0] - 2026-11-23
### Added
- **[Breaking Change]** Added the `Connection` tab (parameter tab between `Input` and `Options`).
  - `Connection.OdooUrl` (moved from `Options.OdooUrl`).
  - `Connection.Username` (moved from `Options.Username`).
  - `Connection.Password` (moved from `Options.Password`).
  - `Connection.Database` (moved from `Options.Database`).
- `Options.ErrorMessageOnFailure` (string, default empty). Overrides the error message on failure.

### Changed
- **[Breaking Change]** `Result.Error` changed type from `string` to `Error { string Message, Exception AdditionalInfo }`.
- **[Breaking Change]** Failures now throw by default. The replacement `Options.ThrowErrorOnFailure` defaults to `true`, while the old `ThrowExceptionOnErrorResponse` defaulted to `false`.
- Upgraded the Task to .NET 8.

## [1.1.0] - 2024-08-23
### Changed
- Updated the Newtonsoft.Json package to version 13.0.3.

## [1.0.0] - 2024-05-30
### Changed
- Initial implementation of Frends.Odoo.Request
