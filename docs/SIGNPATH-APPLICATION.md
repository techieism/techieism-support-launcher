# SignPath application draft — not submitted

Project: Techieism Support Launcher

Organization/maintainer: Techieism India LLP; proposed GitHub owner `Techieism`; proposed repository `techieism-support-launcher`. Public repository/release URL must be inserted after ownership confirmation and actual publication.

License: MIT; all code included in the proposed signed launcher is maintained in this repository. Uses Windows/.NET system libraries. No upstream binaries are submitted for signing as our own work.

Purpose: customer-initiated remote IT support for Windows, operated by Techieism. The launcher requests administrator approval. Following separate website CAPTCHA, explicit terms/privacy acceptance and verified email, the user supplies an opaque short-lived, single-use support handoff. The launcher downloads a MeshCentral Assistant from a fixed Techieism HTTPS origin, validates integrity and the pinned publicly trusted upstream signer, and runs it elevated in an ACL-restricted temporary directory. The Assistant permits desktop/terminal/files access under server-side permissions and customer consent; these capabilities must be considered in eligibility review.

No vulnerability scanning, exploitation, UAC bypass, persistent installation, startup registration, concealed access, security disabling or arbitrary command/download addresses. Closing/End Support and server lifetime limits constrain reuse. A one-use handoff cannot be created solely by the launcher: current terms and email verification remain backend prerequisites.

Important disclosure: the signed launcher would retrieve and execute upstream code not produced by its own CI. Please review whether this delivery model, the upstream provenance/signature restrictions and the remote-support functionality satisfy your Foundation policies. We will not weaken checks or use the certificate to conceal unreviewed behavior. We understand that fresh projects may need a public unsigned release and an established reputation before acceptance.

Current state: source builds and isolated tests pass; upstream signer is deliberately unset and server handoff migration is not deployed. Production support currently uses a different unsigned wrapping workflow. That live executable is not the requested signed artifact.

Proposed roles: Nagesha as responsible maintainer/approver; exact GitHub identity and MFA verification pending. Manual release approval, GitHub-hosted builds, environment-scoped credentials and metadata restrictions planned.

Before submission: verify public owner, publish reviewed source and accurately labelled preview/release, link home/privacy/security/code-signing pages, confirm member identities/MFA, attach verifiable build/release links and disclose external code delivery. Applicant must review and accept any legal agreement personally; no application has been sent.
