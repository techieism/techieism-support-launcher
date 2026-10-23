# Code signing policy

Status: **prepared application, not approved, not signed**.

Repository owner: [techieism](https://github.com/techieism). Project name: Techieism Support Launcher. Maintainer organization: Techieism India LLP. The GitHub owner is currently a user account; this does not claim a separate GitHub organization has been created.

Proposed responsible maintainer and signing approver: Nagesha, Techieism India LLP. The exact GitHub member/team links must be confirmed before application. Contributors may propose pull requests; a named maintainer reviews each source/build/signing change. All maintainers and approvers must use MFA for GitHub and SignPath. No claim is made that MFA or repository branch protection has already been configured.

Every release needs an explicit trusted human signing approval. Signing must use a verified GitHub-hosted build of reviewed source and a protected `signing` environment. Do not sign pull-request artifacts, local uploads masquerading as CI output, customer-specific EXEs or modified upstream binaries. No private signing credentials belong in source or artifacts. Use project-scoped SignPath submitter credentials in GitHub environment secrets only after acceptance.

The intended signed artifact is the fixed launcher only, versioned and metadata-restricted. The launcher retrieves an upstream MeshCentral Assistant using a separately verified one-use customer authorization. This remote-code execution/downloader behavior, upstream source/build provenance and backend trust model must be disclosed to SignPath for explicit eligibility review. The project must not be presented as a harmless static utility or used to evade artifact review. Approval is discretionary; a new project may not yet meet verifiable-reputation requirements.

If approved, SignPath requires the following attribution on project and release/download pages:

> Free code signing provided by [SignPath.io](https://about.signpath.io), certificate by [SignPath Foundation](https://signpath.org).

That line is a prospective required attribution, **not a claim of current sponsorship or approval**. The Foundation certificate's publisher is SignPath Foundation, not Techieism India LLP. Configure product/company/version attributes accurately, verify the returned Authenticode signature and timestamp before release, and never alter/append data to the signed executable afterward.

Privacy policy: [PRIVACY.md](../PRIVACY.md). No customer data or secrets are sent to SignPath. Application and compliance details: [official terms](https://signpath.org/terms.html).
