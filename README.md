<div align="center">

<img src=".github/ee-logo.png" alt="Elastic Email" width="96" />

# Elastic Email MCP Server

The official [Model Context Protocol](https://modelcontextprotocol.io) server for the [Elastic Email](https://elasticemail.com) REST API v4. Written in C# / .NET.

[![MCP](https://img.shields.io/badge/MCP-Streamable%20HTTP-111111?logo=modelcontextprotocol&logoColor=white)](https://modelcontextprotocol.io)
[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/download/dotnet/10.0)
[![Tools](https://img.shields.io/badge/tools-26-0A7BBB)](#tools)
[![API](https://img.shields.io/badge/API-v4-0A7BBB)](https://elasticemail.com/developers/api-documentation/rest-api)
[![Hosted](https://img.shields.io/badge/hosted-mcp.elasticemail.com-0A7BBB)](https://help.elasticemail.com/en/articles/12595879-elastic-email-mcp)
[![License: MIT](https://img.shields.io/github/license/ElasticEmail/elasticemail-mcp-server?color=yellow)](LICENSE)

[![Latest release](https://img.shields.io/github/v/release/ElasticEmail/elasticemail-mcp-server?logo=github&label=release)](https://github.com/ElasticEmail/elasticemail-mcp-server/releases)
[![Last commit](https://img.shields.io/github/last-commit/ElasticEmail/elasticemail-mcp-server?logo=github)](https://github.com/ElasticEmail/elasticemail-mcp-server/commits/master)
[![Open issues](https://img.shields.io/github/issues/ElasticEmail/elasticemail-mcp-server?logo=github)](https://github.com/ElasticEmail/elasticemail-mcp-server/issues)
[![GitHub stars](https://img.shields.io/github/stars/ElasticEmail/elasticemail-mcp-server?style=flat&logo=github)](https://github.com/ElasticEmail/elasticemail-mcp-server/stargazers)

[Installation](#installation) •
[Quick start](#quick-start) •
[Tools](#tools) •
[Examples](#more-examples) •
[Contributing](#contributing)

</div>

---

## Features

- **Transactional and bulk email.** Let your AI assistant send single messages or bulk sends, with plain content or an existing template.
- **Contacts and lists.** Add, import, list and delete contacts, and create and manage contact lists.
- **Campaigns.** Create, update, pause and list campaigns, including A/X split tests and scheduled sends.
- **Segments and templates.** Create and read segments, and look up personal or global templates.
- **Statistics.** Read opens, clicks and other numbers for one campaign or all of them.
- **Works with any MCP client.** VS Code (GitHub Copilot agent mode), Cursor, Claude Code and other clients that speak MCP over HTTP.
- **Per-request authentication.** Each client sends its own Elastic Email API key. The server keeps no credentials of its own.

> [!TIP]
> Don't want to run a server? Elastic Email hosts this server at **`https://mcp.elasticemail.com`**. See the [hosted MCP guide](https://help.elasticemail.com/en/articles/12595879-elastic-email-mcp). This repository is for running it yourself.

## Requirements

| Requirement | Version |
| --- | --- |
| [.NET SDK](https://dotnet.microsoft.com/download/dotnet/10.0) | 10.0 or later |
| Network | Port **5001** open for HTTP, outbound HTTPS to `api.elasticemail.com` |
| MCP client | Any client that supports MCP over HTTP |

You'll also need an Elastic Email **API key**. You can create one in your [API settings](https://app.elasticemail.com/marketing/settings/new/manage-api). Give it **view and modify** access to Account, Templates, Campaigns, Contacts, Files and Send HTTP, and at least **view** access to Access Tokens. The server rejects keys that can't list API keys.

## Installation

Clone the repository and build it:

```bash
git clone https://github.com/ElasticEmail/elasticemail-mcp-server.git
cd elasticemail-mcp-server
dotnet build src/ElasticEmail.Mcp.Service.sln
```

The build restores its NuGet dependencies ([ModelContextProtocol.AspNetCore](https://www.nuget.org/packages/ModelContextProtocol.AspNetCore), [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json) and [Serilog.AspNetCore](https://www.nuget.org/packages/Serilog.AspNetCore)) for you.

## Quick start

> [!IMPORTANT]
> Elastic Email only sends from verified domains. Before your first send, [verify your sending domain](https://help.elasticemail.com/en/articles/4934400-how-to-verify-your-domain) and use an address on that domain as the sender.

### Run the server

```bash
dotnet run --project src/ElasticEmail.Mcp.Service.csproj
```

The server listens on `http://0.0.0.0:5001/` and writes daily rolling logs to the working directory.

### Connect VS Code (GitHub Copilot)

Add the server to `.vscode/mcp.json` in your workspace:

```json
{
  "inputs": [
    {
      "type": "promptString",
      "id": "elasticemail-api-key",
      "description": "Elastic Email API key",
      "password": true
    }
  ],
  "servers": {
    "elasticemail": {
      "type": "http",
      "url": "http://localhost:5001/",
      "headers": {
        "X-Auth-Token": "${input:elasticemail-api-key}"
      }
    }
  }
}
```

Then open Copilot Chat, switch to **Agent** mode, click **Start** above the server entry, and check that the Elastic Email tools are selected in the tools picker.

> [!TIP]
> Keep your API key out of files you commit. The `inputs` prompt above asks for it once and stores it securely in VS Code.

### Connect Claude Code

```bash
claude mcp add --transport http elasticemail http://localhost:5001/ \
  --header "X-Auth-Token: $ELASTICEMAIL_API_KEY"
```

### Connect Cursor

Add the server to `.cursor/mcp.json` (project) or `~/.cursor/mcp.json` (global):

```json
{
  "mcpServers": {
    "elasticemail": {
      "url": "http://localhost:5001/",
      "headers": {
        "X-Auth-Token": "YOUR_API_KEY"
      }
    }
  }
}
```

### Try it

Ask your assistant something like:

- "Check that the Elastic Email MCP server is ready."
- "Send a transactional email to john.doe@example.com from no-reply@yourdomain.com with the subject *Welcome aboard!*"
- "Create a list called *beta-testers* with jane@example.com and john@example.com."
- "Show me open and click statistics for all my campaigns."

The `from` address must use a domain you've [verified in your Elastic Email account](https://help.elasticemail.com/en/articles/4934400-how-to-verify-your-domain). If you leave it out, the server uses your account's default sender.

## More examples

More complete, runnable samples are in the **[Elastic Email examples repository](https://github.com/ElasticEmail/elasticemail-examples)**. It covers transactional email, SMTP, webhooks, inbound email, contacts and serverless platforms across 20+ languages and frameworks.

- 🤖 [AI agent examples](https://github.com/ElasticEmail/elasticemail-examples/tree/main/ai-agents-elasticemail-examples) (LangChain, OpenAI Agents, Vercel AI SDK)
- 🟣 [.NET / C# examples](https://github.com/ElasticEmail/elasticemail-examples/tree/main/dotnet-elasticemail-examples)
- 📂 [All examples](https://github.com/ElasticEmail/elasticemail-examples)

## Authentication

| Header | Sent by | Used for |
| --- | --- | --- |
| `X-Auth-Token` | Your MCP client, on every request | Your Elastic Email API key. The server checks it against `GET /v4/security/apikeys` and forwards it on every API call. |

Requests without the header, or with a key the API rejects, get `401 Unauthorized`.

> [!IMPORTANT]
> The server speaks plain HTTP, so the API key travels unencrypted. Keep it on `localhost` or a private network. If you expose it, put it behind a reverse proxy that terminates TLS.

## API limits

The server calls the Elastic Email API v4 with your key, so the API's limits apply:

- Up to **20 concurrent connections** per account
- A hard timeout of **600 seconds** per request

## Tools

The server exposes **26 tools** in 6 groups: `EmailSending`, `ContactsManagement`, `CampaignManagement`, `SegmentsManagement`, `TemplatesManagement` and `HealthCheck`. Most tools return the API's JSON response as a string. Tools marked **bool** return only `true` or `false`.

<details>
<summary><strong>Show all tools</strong></summary>

Group | Tool | Parameters | Description
------------ | ------------- | ------------- | -------------
*EmailSending* | **SendTransactionalEmail** | `data` | Send a transactional email, with content or a template
*EmailSending* | **SendBulkEmails** | `emailData` | Send email to many recipients, with content or a template
*ContactsManagement* | **FetchContacts** | `limit` (default 20) | Load contacts
*ContactsManagement* | **FetchContactHistory** | `email` | Load a contact's history
*ContactsManagement* | **AddContact** (bool) | `email`, `firstName?`, `lastName?` | Add a contact
*ContactsManagement* | **DeleteContacts** (bool) | `contacts?` or `rule?` | Delete contacts by email list or rule
*ContactsManagement* | **UploadContacts** | `fileName`, `base64Content`, `listName` | Import contacts from a file, creating the list if needed
*ContactsManagement* | **FetchLists** | | Load all contact lists
*ContactsManagement* | **FetchList** | `listName` | Load a list's details
*ContactsManagement* | **FetchListContacts** | `listName` | Load the contacts in a list
*ContactsManagement* | **CreateList** | `listName`, `emails` | Create a list with the given contacts
*ContactsManagement* | **AddContactsToList** | `listName`, `emails` | Add contacts to a list
*ContactsManagement* | **RemoveContactsFromList** (bool) | `listName`, `contacts?` or `rule?` | Remove contacts from a list
*CampaignManagement* | **CreateCampaign** | `campaignData` | Create and send a campaign
*CampaignManagement* | **ListCampaigns** | | Load campaigns
*CampaignManagement* | **GetCampaign** | `name` | Load a campaign
*CampaignManagement* | **UpdateCampaign** | `campaignData` | Update a campaign (change its status to start it again)
*CampaignManagement* | **PauseCampaign** (bool) | `name` | Pause a campaign
*CampaignManagement* | **GetCampaignStatistics** | `name` | Load one campaign's statistics
*CampaignManagement* | **GetAllCampaignStatistics** | | Load statistics for all campaigns
*SegmentsManagement* | **CreateSegment** | `segmentData` (`Name`, `Rule`) | Create a segment
*SegmentsManagement* | **GetSegments** | | Load segments
*SegmentsManagement* | **GetSegment** | `name` | Load a segment
*TemplatesManagement* | **FetchTemplates** | `scopes`, `templateTypes?` | Load templates, filtered by scope and type
*TemplatesManagement* | **FetchTemplate** | `name` | Load a template
*HealthCheck* | **IsReady** | | Check the server and its connection to the API

</details>

Your MCP client shows each tool's full description and input schema. The source is in [`src/Tools`](src/Tools).

## Versioning

The server follows the Elastic Email API v4. Releases and release notes are listed in [GitHub Releases](https://github.com/ElasticEmail/elasticemail-mcp-server/releases).

<details>
<summary>Build details</summary>

- API version: 4.0.0
- Target framework: `net10.0`
- MCP SDK: `ModelContextProtocol.AspNetCore` 0.4.0-preview.3
- Transport: HTTP (Streamable HTTP and SSE), port 5001

</details>

## Contributing

Contributions are welcome! Please read [CONTRIBUTING.md](CONTRIBUTING.md) before opening a pull request.

- 🐛 [Report a bug](https://github.com/ElasticEmail/elasticemail-mcp-server/issues/new?template=bug_report.md)
- 💡 [Request a feature](https://github.com/ElasticEmail/elasticemail-mcp-server/issues/new?template=feature_request.md)
- 🔒 [Report a security issue](SECURITY.md)

This project follows the [Contributor Covenant Code of Conduct](CODE_OF_CONDUCT.md).

## Support

> [!IMPORTANT]
> The fastest way to get help is the **chat widget on [elasticemail.com](https://elasticemail.com)**. Our support team can help with your account, sending, deliverability and API questions.

- 💬 [Chat with support on elasticemail.com](https://elasticemail.com) (preferred)
- 📚 [API documentation](https://elasticemail.com/developers/api-documentation/rest-api)
- 🧭 [Hosted MCP guide](https://help.elasticemail.com/en/articles/12595879-elastic-email-mcp)
- 🧪 [Examples repository](https://github.com/ElasticEmail/elasticemail-examples)
- 🐛 [GitHub issues](https://github.com/ElasticEmail/elasticemail-mcp-server/issues), for bugs in this server only

## License

Released under the [MIT License](LICENSE). Copyright © 2025–2026 Elastic Email.
