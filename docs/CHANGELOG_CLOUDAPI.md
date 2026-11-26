# WhatsApp Cloud API parity audit

**Date of scan:** 2026-11-27
**Detected status:** `MISSING_APIS_FOUND (POSTMAN_COLLECTION_UNVERIFIED)`
**Default Graph API version:** `v27.0`
**Latest WhatsApp changelog entry:** May 12, 2026

## Sources and access

- [Meta WhatsApp Cloud API Postman collection](https://www.postman.com/meta/whatsapp-business-platform/collection/wlk6lh4/whatsapp-cloud-api?sideView=agentMode)
- [Meta WhatsApp changelog](https://developers.facebook.com/documentation/business-messaging/whatsapp/changelog)
- [Meta Graph API changelog](https://developers.facebook.com/documentation/development/graph-api/changelog)

The Postman collection page returned a client-rendered shell without collection items. The public `api.getpostman.com/collections/wlk6lh4` request returned HTTP 401. Collection endpoint inventory, methods, authorization/header definitions, and request/response schemas therefore remain unverified; this audit does not claim complete Postman parity.

The WhatsApp changelog's latest entry is May 12, 2026 and describes pricing and Embedded Signup preview updates, not Cloud API client schema changes. The Graph API changelog identifies `v27.0` as the latest version, released November 4, 2026. Its v26/v27 changes do not list WhatsApp Cloud API endpoints. The client default was advanced from `v25.0` to `v27.0`.

## Endpoint and schema differences

| Endpoint / event | Status | Field or pattern changes |
|---|---|---|
| `GET /{PHONE-NUMBER-ID}` | Modified; implemented | Added `name_status`, `code_verification_status`, and `official_business_account` to the phone-number response fields and model. The existing v24+ portfolio messaging-limit fields remain version-gated. |
| `POST /{PHONE-NUMBER-ID}` | Missing; implemented | Added `new_display_name` request model and client methods for the documented display-name update operation. |
| `GET /{PHONE-NUMBER-ID}/official_business_account` | Missing; implemented | Added OBA status response model and client methods. |
| `POST /{PHONE-NUMBER-ID}/official_business_account` | Missing; implemented | Added the documented OBA application request fields (`business_website_url`, `primary_country_of_operation`, optional supporting information) and response model. |
| `POST /{PHONE-NUMBER-ID}/settings` | Modified; implemented | Added `storage_configuration` support for local storage (`status`, `data_localization_region`) and No Storage (`status`, `retention_minutes`). |
| `POST /{PHONE-NUMBER-ID}/media` | Modified; implemented | Added optional multipart `ttl_minutes` support for uploaded media. |
| `user_preferences` webhook | Missing; implemented | Added the preference event payload (`wa_id`, optional BSUIDs, `detail`, `category`, `value`, numeric `timestamp`) to webhook values. |
| `GET/POST /{WABA-ID}/template_groups`, `GET /{WABA-ID}/template_group_analytics`, `GET/POST/DELETE /{TEMPLATE-GROUP-ID}` | Missing; report only | Meta's February 14, 2025 changelog lists these operations, but the current reference did not expose usable operation schemas. No request or response models were guessed. |
| `GET /{SOLUTION-ID}/access_token`; `POST /{WABA-ID}/set_solution_migration_intent` | Missing; report only | The changelog lists these Multi-Partner Solution operations and the `app_id` migration-intent parameter. They remain outside this implementation because the request/response schemas and required partner-specific behavior were not verified. |
| Default Graph API version | Modified; implemented | Updated the library default from `v25.0` to the current `v27.0` release. Explicit version overrides remain supported. |

## Confirmed updates already supported

- **October 8, 2025:** Portfolio messaging-limit fields and the conditional status `conversation` object are represented in the existing models and version-gated endpoint selection.
- **October 16, 2025:** Incoming media webhook `url` fields and the outgoing audio `voice` option are represented.
- **October 20, 2025:** `ctwa_clid` is optional in incoming ad-click webhook data.
- **November 3, 2025:** The status string accepts `played`, and unsupported message types are represented as open strings.
- **September 8, 2025:** Call Permissions Request message types are already present.
- **July 25, 2025:** Tap-target URL override request support is already present.
- **December 3, 2025:** Coupon code length is unconstrained by the existing string model; no client change was needed.
- **December 8, 2025:** Portfolio pacing is documented as a delivery batching mechanism, but the changelog does not provide a client request/response schema to implement.

## Scope and validation boundary

Pricing, rate-card, Embedded Signup-only, and Marketing API changes are not treated as Cloud API client endpoints. The full endpoint/header/schema comparison must be repeated after the Postman collection can be exported or accessed with authorized API credentials. Template group and Multi-Partner Solution operations are explicitly recorded as gaps rather than represented as implemented.
