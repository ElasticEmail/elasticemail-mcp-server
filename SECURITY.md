# Security Policy

## Supported versions

Security fixes are released for the latest [release](https://github.com/ElasticEmail/elasticemail-mcp-server/releases) of this server. Please update to the latest version before reporting an issue.

| Version | Supported          |
| ------- | ------------------ |
| 1.0.x   | :white_check_mark: |

## Reporting a vulnerability

**Please do not open a public GitHub issue for security vulnerabilities.**

Report them privately by one of these methods:

- [GitHub private vulnerability reporting](https://github.com/ElasticEmail/elasticemail-mcp-server/security/advisories/new)
- Email **integrations@elasticemail.com** with the subject `Security: elasticemail-mcp-server`

Please include:

- A description of the issue and its impact
- Steps to reproduce, or a proof of concept
- The affected version or commit

We will acknowledge your report, investigate, and keep you updated on the fix. Please give us reasonable time to release a fix before disclosing the issue publicly.

## Running the server safely

The server listens on plain HTTP (port 5001), and every MCP client sends its API key in the `X-Auth-Token` header. Keep the server on `localhost` or a private network. If you have to expose it, put it behind a reverse proxy that terminates TLS.

## API key safety

If you think an API key has been exposed (in a commit, log, issue or screenshot), revoke it right away in your [Elastic Email API settings](https://app.elasticemail.com/marketing/settings/new/manage-api) and create a new one.
