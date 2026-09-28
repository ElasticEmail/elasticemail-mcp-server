# Contributing to the Elastic Email MCP Server

Thanks for taking the time to contribute! This document explains how to report problems, suggest changes and submit pull requests.

## How the server is organized

The server is a small ASP.NET Core app built on the [MCP C# SDK](https://github.com/modelcontextprotocol/csharp-sdk). It isn't generated, so pull requests to any part of it are welcome.

- `src/Tools`: the MCP tools. Each class is marked `[McpServerToolType]` and each tool method `[McpServerTool]`.
- `src/Models`: the input and output models the tools use.
- `src/Authorization`: the middleware that validates the `X-Auth-Token` header.
- `src/Service`, `src/Data`, `src/Utilities`: the HTTP client, data lookups and app setup.

A new tool class must also be registered with `.WithTools<T>()` in [`src/Utilities/WebAppBuilderExtensions.cs`](src/Utilities/WebAppBuilderExtensions.cs).

## Reporting bugs

Search [existing issues](https://github.com/ElasticEmail/elasticemail-mcp-server/issues) first. If nothing matches, open a new issue using the **Bug report** template and include:

- The commit or release you're running
- .NET SDK version (`dotnet --version`) and OS
- Your MCP client and its version (e.g. VS Code with GitHub Copilot, Cursor, Claude Code)
- The tool you called, its input and what it returned
- Relevant lines from the server's log file

**Never paste your API key** into an issue, log or code sample.

Questions about your Elastic Email account, sending limits, deliverability or billing aren't handled in this repository. The preferred way to get help is the **chat widget on [elasticemail.com](https://elasticemail.com)**.

Looking for sample code? See the [Elastic Email examples repository](https://github.com/ElasticEmail/elasticemail-examples), including the [AI agent examples](https://github.com/ElasticEmail/elasticemail-examples/tree/main/ai-agents-elasticemail-examples).

## Suggesting features

Open an issue using the **Feature request** template. Describe the use case first and the solution second. It helps us decide whether the change belongs in the server, the API or the documentation.

## Security issues

Please do **not** report security vulnerabilities in public issues. See [SECURITY.md](SECURITY.md).

## Development setup

Requirements: the [.NET SDK](https://dotnet.microsoft.com/download) 10.0 or later.

```bash
git clone https://github.com/ElasticEmail/elasticemail-mcp-server.git
cd elasticemail-mcp-server
dotnet build src/ElasticEmail.Mcp.Service.sln
dotnet run --project src/ElasticEmail.Mcp.Service.csproj
```

The repository has no automated tests yet. Test your change by connecting an MCP client (see the [README](README.md#quick-start)) and calling the tools you touched. The [MCP Inspector](https://github.com/modelcontextprotocol/inspector) (`npx @modelcontextprotocol/inspector`) is handy for calling tools directly.

Don't add new runtime dependencies without discussing it in an issue first.

## Pull requests

1. Fork the repository and create a branch from `master` (`git checkout -b fix/short-description`).
2. Keep each change focused. One logical change per pull request.
3. Make sure `dotnet build` passes and you've tried the affected tools from an MCP client.
4. Update the [README](README.md#tools) tool table if you add, rename or remove a tool.
5. Open a pull request against `master` and fill in the template.

A maintainer will review your pull request and may ask for changes.

## Code of Conduct

This project follows the [Contributor Covenant Code of Conduct](CODE_OF_CONDUCT.md). By participating, you agree to uphold it.

## License

By contributing, you agree that your contributions will be licensed under the [MIT License](LICENSE).
