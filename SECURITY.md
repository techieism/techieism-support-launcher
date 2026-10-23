# Security policy

Report suspected vulnerabilities privately to helpdesk@techieism.com. Do not put customer records, one-time codes, session authorization tokens or exploitation details against a live customer in public issues. This project is a development preview and has not received a security audit or production acceptance.

The boundary is a fixed HTTPS redemption service, verified-email/consent prerequisites and atomic single-use state, pinned Windows-verified upstream signer, immutable signed launcher, administrator consent, restricted temporary execution and server-side scoped session revocation. A downloaded hash alone is not an authenticity guarantee. A compromise of the backend or approved upstream signer remains a material risk.

Keep protections enabled. Do not remove empty-pin refusal, certificate validation, response-size limits, redirect refusal, manual signing approval or temporary-lease enforcement to work around a failed test. No arbitrary URLs/paths/commands are supported. Signing changes and all build workflow changes require review.
