# Techieism Support Launcher

Open-source Windows launcher maintained by **Techieism India LLP**. MIT licensed.

Public source: https://github.com/techieism/techieism-support-launcher . GitHub owner: `techieism`.

**Status: unsigned development preview, not a production support download. SignPath has not approved or signed this project.** The release signer pin is deliberately unset and this preview refuses live support. Current production customers continue using the existing verified support workflow.

## Purpose and behavior

The planned fixed, signed launcher requests Windows administrator approval, opens the user-requested support website, and exchanges a separate one-time support code for a temporary MeshCentral Assistant. It does not install a persistent agent or service, register startup tasks, bypass UAC, or disable security software. The customer must approve elevation; an authorized person must enter any required administrator credentials locally.

The launcher only contacts `https://help.techieism.com/support-api/launcher/redeem` for the executable. It does not accept arbitrary download addresses, command lines, local executable paths or server-supplied signer identities. Redirects are refused. Downloads have an 8 MiB limit and HTTPS/hash validation; the downloaded executable additionally must pass Windows Authenticode verification and match the signer pinned in reviewed source. A response checksum establishes transfer integrity, not independent publisher identity.

## Customer authorization remains on the server

The intended sequence is:

1. Customer visits [the support page](https://help.techieism.com/support.html), supplies contact details, completes CAPTCHA and explicitly accepts current terms/privacy/safety notices.
2. Customer verifies their email using the email OTP on that website.
3. The website issues a separate short-lived, one-use handoff code. This is not the email OTP or a password.
4. Customer opens the unchanged launcher, approves Windows elevation, and pastes the handoff code.
5. The server atomically redeems it, links the accepted terms and verified email to a new same-device temporary authorization, and returns the authorized Assistant. Customers still use Request Help and approve engineer access.
6. Brief interruptions have the existing five-minute same-device reconnect grace. Engineer End Support permanently revokes the server authorization; a reusable launcher does not revive it. A new session requires fresh verification and acceptance.

No session code is embedded in or appended to the signed launcher. It must remain byte-for-byte unchanged after signing. The original Assistant is downloaded separately; this project does not sign upstream binaries under its own project identity.

**The new handoff endpoint and frontend code have not been deployed.** See [server integration and release blockers](docs/SERVER-INTEGRATION.md). Do not replace the working production download with this preview.

## Build and test

On Windows with .NET Framework 4.8 installed:

```powershell
.\build.ps1
```

The script builds `out/TechieismSupportLauncher.exe` and runs isolated console tests. Running the build/tests does not launch the administrator GUI or connect to a real customer. No external NuGet packages are required. The EXE is unsigned until accepted by the signing program and processed through a reviewed release workflow.

Tests cover handoff-code validation, malformed/oversized PE data, mismatched or missing response hashes, expected origin, and preview signer refusal. Real Windows/UAC, upstream signer validation, server redemption and same-device reconnect/End Support tests are required before a production release.

## Source and supply chain

This repository contains only our launcher source, tests, documentation and build/signing configuration. No customer records, server database, certificates/private keys, SMTP/OAuth/CAPTCHA secrets, MeshCentral device-group IDs, SSH files or live authorization tokens are included. The MeshCentral server/Assistant are external dependencies; see [third-party notices](THIRD-PARTY-NOTICES.md).

## Code signing policy

**Application pending; no signing service or certificate has been granted.** See [Code signing policy](docs/CODE-SIGNING.md), [privacy](PRIVACY.md), and [security](SECURITY.md). Prospective SignPath attribution is documented there and must not be represented as an existing endorsement.

## Removal

Close the Assistant and launcher. There is no installed service to uninstall. The launcher attempts to remove its own temporary extraction directory on exit. If interrupted or the machine crashes, a local administrator can remove only that run's `TechieismSupport-<random-id>` directory after ensuring no process from it is running. Close/End Support on the server to revoke authorization; deleting local files is not a substitute for server revocation.
