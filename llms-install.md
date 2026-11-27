# Installing the Elastic Email MCP Server (for AI agents)

Use the **hosted** server. Do not clone or build this repository: it is only needed to self-host.

- Endpoint: `https://mcp.elasticemail.com` (Streamable HTTP)
- Authentication: an Elastic Email API key in the `X-Auth-Token` header

## Steps

1. Ask the user for their Elastic Email API key. They can create one in the Elastic Email app under
   **Settings > Manage API Keys**. The key needs these permissions: Account, Templates, Campaigns,
   Contacts, Files, Send HTTP, and at least "view" for Access Tokens.
2. Add this server to the MCP settings file (for Cline: `cline_mcp_settings.json`), replacing
   `YOUR_API_KEY` with the user's key:

```json
{
  "mcpServers": {
    "elasticemail": {
      "type": "streamableHttp",
      "url": "https://mcp.elasticemail.com",
      "headers": {
        "X-Auth-Token": "YOUR_API_KEY"
      }
    }
  }
}
```

3. Check the connection by calling a read-only tool, for example listing the user's contact lists.

## Troubleshooting

- `401 Unauthorized`: the API key is missing, wrong, or lacks the permissions listed above.
- The URL is for MCP clients only. It is not the Elastic Email REST API address.
- Clients that support MCP OAuth can connect without a key: leave out the `headers` block and sign in
  through `oidc.elasticemail.com` when prompted.
