# WhatsApp Cloud API parity audit

**Date of scan:** 2026-10-09
**Detected status:** `UPDATES_DETECTED (POSTMAN_COLLECTION_UNVERIFIED)`
**Current client Graph API version:** `v25.0`

## Source access and method

- [Meta WhatsApp Cloud API Postman collection](https://www.postman.com/meta/whatsapp-business-platform/collection/wlk6lh4/whatsapp-cloud-api?sideView=agentMode)
- [Meta WhatsApp changelog](https://developers.facebook.com/documentation/business-messaging/whatsapp/changelog)

The Postman page returned a client-rendered shell without collection items. The public `api.getpostman.com/collections/wlk6lh4` endpoint returned HTTP 401 because collection API access requires an API key. Consequently, this scan could not verify the collection's complete endpoint inventory, methods, authorization/header definitions, or request/response schemas. No collection-only endpoints are claimed as new or absent. The schema comparisons below are limited to changes confirmed through Meta's public changelog and linked API references; the collection limitation must be resolved before asserting full integration parity.

## Verified endpoint and schema differences

| Endpoint / event | Status | Field or pattern changes |
|---|---|---|
| `GET /{WABA-ID}` | Modified; implemented | Meta's October 8, 2025 changelog added `whatsapp_business_manager_messaging_limit`. Added it to the WABA response model and conditionally request it for Graph API v24.0+. |
| `GET /{PHONE-NUMBER-ID}` | Modified; implemented | The October 8, 2025 changelog added `whatsapp_business_manager_messaging_limit`; `messaging_limit_tier` now reflects the owning portfolio's limit for Graph API v24.0+. Added both response properties and conditionally request the fields for v24.0+. |
| `POST /{PHONE-NUMBER-ID}/messages` (template) | Modified; implemented | Meta's July 25, 2025 tap-target URL override uses a `tap_target_configuration` component parameter with `url` and `title`. Added typed request support. |
| `POST /{PHONE-NUMBER-ID}/messages` (audio) | Modified; already supported | The October 16, 2025 changelog added voice-message behavior. Both ID- and URL-based audio request models already expose the optional `audio.voice` field; no code change required. |
| `messages` webhook (image, video, audio, document, sticker) | Modified; implemented | The October 16, 2025 changelog added the media asset's `url` to incoming media objects. Added the optional `url` field to all five webhook payload models. |
| `messages` status webhook | Modified; implemented / already supported | The October 8, 2025 changelog made `conversation` conditional/omitted for v24.0+ except Free Entry Point messages; made it optional. The current reference also documents optional group-recipient fields and `biz_opaque_callback_data`; added these fields. The `played` status introduced November 3, 2025 is already accepted by the existing string-valued `status`. |
| `messages` text webhook | Modified; already supported | Meta's October 20, 2025 changelog says `ctwa_clid` is omitted for WhatsApp Status ad placements. The field already exists and is now annotated optional. |
| Unsupported messages webhook | No code change required | The November 3, 2025 update added the unsupported message's actual type. The existing `unsupported.type` field is an open string and already captures it. |
| Complete Postman collection | Unverified | Collection content could not be retrieved, so new endpoints and collection-only schema/header changes remain unknown. |

## Changelog review

The latest entry returned by Meta's changelog at scan time is May 12, 2026 (pricing-policy update; no client schema change). Reviewed Cloud API updates relevant to the library include:

- **December 8, 2025:** business portfolio pacing for template delivery; no confirmed request/response type change in the public description.
- **December 3, 2025:** coupon template codes may be up to 20 characters rather than 15; the existing request model uses an unconstrained string.
- **November 3, 2025:** unsupported-message webhook details and the `played` status; existing models already represent both.
- **October 20, 2025:** `ctwa_clid` is omitted for WhatsApp Status ad placements; now represented as optional.
- **October 16, 2025:** incoming media webhook URLs and voice messages; media URLs added, outgoing voice flag was already supported.
- **October 8, 2025:** portfolio-based messaging limit fields and conditional status `conversation`; response models, requested fields, and webhook nullability updated.
- **September 8, 2025:** Call Permissions Request message type; this repository already contains request types and client methods.
- **July 30, 2025:** media carousel template card body is no longer mandatory; existing component models allow omission.
- **July 25, 2025:** tap-target URL override; typed template-message payload support added.
- **July 15, 2025:** WhatsApp Business Calling API availability; this repository already contains call request/response types and client methods.

Embedded Signup-only changes, pricing/rate-card changes, and marketing-only changes were not treated as Cloud API client endpoints.

## Validation boundary

This report is not a complete Postman endpoint diff. The collection needs to be rescanned with authorized Postman API access (or an exported collection JSON) to verify the full operation, authorization, header, and schema inventory.
