# First public preview and signing application

Prepared on23October2026 IST. This checklist distinguishes completed local work from external prerequisites.

Completed locally:
* MIT source, Windows administrator manifest, descriptive product/version metadata.
* Fixed-origin retrieval, no redirect/arbitrary URL support, bounded PE/hash validation, Windows trust check and fail-closed signer placeholder.
* No customer tokens, upstream executable or production data in the public-source package.
* Source build and14 isolated launcher checks pass; one-use handoff reference tests include concurrent replay, expiry and authorization checks.
* Proposed GitHub-hosted build, manual approval signing job, artifact configuration, privacy/security/license/third-party notices and candid application draft.

Not completed:
* GitHub owner `techieism` is confirmed. Verify final publication/build links before application. Organization display name is not a repository handle.
* Publish the source and a clearly labelled unsigned non-production preview built by GitHub Actions. Record actual release URL and build provenance. Configure MFA and named reviewers/approvers.
* Pin the SignPath action to an independently reviewed commit; configure protected signing environment/manual reviewers. Keep SIGNPATH_ENABLED unset until approved.
* Apply for Foundation review. A certificate is not guaranteed; this is a new remote-support downloader and may fail reputation/eligibility requirements.
* Independently verify the actual upstream per-session Assistant's Authenticode signature and pin the approved signer in reviewed source. Do not substitute a server response field for that trust decision.
* Integrate the reference handoff store with the existing verified-email/current-acceptance and transactional lease creation on staging. The reference store alone is not a production HTTP adapter.
* Run the full Windows/UAC/customer-consent/5-minute reconnect/End Support test matrix before production deployment.

Do not claim this preview is signed, SignPath-approved, production-ready or deployed. Do not claim the fixed-EXE handoff route exists on the live server.
