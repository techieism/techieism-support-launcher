# Proposed server integration — not deployed

The working production path is deliberately untouched. These are release gates, not claims that the new endpoint exists.

## Handoff contract

After the existing successful CAPTCHA/current mandatory acceptance/email-OTP verification, issue a separate cryptographically random 32-byte base64url handoff code (43 characters). Store only a server-keyed digest, reference to the verified request/current acceptance, expiry (at most ten minutes), and consumed state. Never accept the email OTP as a handoff code. Rate-limit failures and never log codes, tokens, returned bodies or customer details.

`POST /support-api/launcher/redeem` accepts only JSON `{"handoff_code":"<43-character code>"}` over the public HTTPS origin. No email/name/phone/issue description belongs in this request. The native launcher supplies Origin but this is not authentication: possession of a valid unexpired handoff and server-side verified acceptance are required. Do not weaken browser origin/CSRF controls globally to implement this route.

Missing, malformed, expired, consumed, unverified or old-terms authorization returns a generic 403/400 without issuing an agent. Concurrent valid requests must yield at most one success. The route must recheck verified email, current acceptance version and one-use state transactionally with lease linkage. Bind each valid response to a fresh temporary lease under the existing enforced ad-hoc group. Preserve node binding, hard lifetime, five-minute recovery, immediate End Support revocation, managed-group isolation, consent history and reporting.

On success return raw **original generated Assistant**, never the current unsigned wrapper:

* 200, Content-Type `application/octet-stream`, Cache-Control `no-store`.
* `X-Techieism-Assistant-SHA256` is the SHA256 of the exact returned bytes.
* Body is at most 8 MiB. Do not redirect to a third-party binary/CDN.
* Consume authorization only after preparing a valid payload; enforce atomic single use before writing response. Define safe failure/retry behavior for a connection lost while delivering it. A failed delivery must never silently reactivate a consumed authorization.

The underlying Assistant certificate must be independently checked on Windows against the exact MeshCentral-generated per-session payload. Pin its verified leaf certificate thumbprint in reviewed launcher source, then exercise certificate rotation before release. If the upstream build has no valid public signature or its customization invalidates verification, stop: do not remove signature checks to make the prototype work. Ask SignPath about acceptable upstream packaging/provenance instead.

## Deployment gates

1. SignPath explicitly reviews this launcher as a remote-support downloader/runner, including external upstream agent provenance. A generic signing application must not conceal this behavior.
2. Verify native certificate identity and signature; configure the reviewed source pin. The current empty pin intentionally blocks all downloads/launches.
3. Implement the handoff store/route and frontend on staging, preserving existing verified-email/terms controls. Add expiry/replay/concurrency/old-terms/unverified tests.
4. Test Windows 10/11: UAC approved/denied, standard-user credentials, valid signature/pin mismatch, locked desktop, request help, engineer consent, input, same-device reconnect, End Support, stale launcher/code reuse, and 365-day consent linkage.
5. Obtain a signed release from reviewed GitHub-hosted build, verify timestamped Authenticode signature and checksums, and serve it as a static immutable EXE. Distribute handoff code separately after verification.
6. Back up existing portal/frontend/routing and deploy narrowly with rollback to current verified download. Never change managed-customer access as part of this migration.

Current source is a prepared prototype. No public release, signing acceptance, deployed redemption route or successful live Windows support session is asserted.
