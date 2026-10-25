# Inventory API

All inventory endpoints require a valid ERP JWT. Send it as a bearer token:

```http
Authorization: Bearer <token>
```

| Method | Route | Purpose | Success |
|---|---|---|---|
| `POST` | `/Inventory/adjustments` | Add or remove stock and record the movement | `200 OK` |
| `GET` | `/Inventory/stock/{productId}` | Get one product's current stock and threshold | `200 OK` |
| `GET` | `/Inventory/low-stock` | List products at or below their low-stock threshold | `200 OK` |

Adjustment request example:

```json
{
  "productId": 1,
  "quantity": 12,
  "type": 1,
  "reason": "Opening stock"
}
```

`type` is `1` for `StockIn` and `2` for `StockOut`. Quantity must be positive and reason is required. An adjustment and its transaction record are saved in one database transaction. A stock-out that would make stock negative returns `409 Conflict`; an unknown product returns `404 Not Found`; invalid input returns `400 Bad Request`.
