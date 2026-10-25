# Sales API

All sales endpoints require a valid ERP JWT. Send it as a bearer token:

```http
Authorization: Bearer <token>
```

| Method | Route | Purpose | Success |
|---|---|---|---|
| `POST` | `/Sales` | Complete a sale and reduce stock | `200 OK` |
| `GET` | `/Sales/{id}` | Get sale details | `200 OK` |
| `GET` | `/Sales?fromUtc=...&toUtc=...` | Get sales history, optionally filtered by UTC range | `200 OK` |

Checkout sends only product IDs and quantities. Prices and totals are read and calculated by the server:

```json
{
  "items": [
    { "productId": 1, "quantity": 2 },
    { "productId": 4, "quantity": 1 }
  ]
}
```

Inactive or unknown products, duplicate product lines, and invalid quantities return `400 Bad Request`. Insufficient stock returns `409 Conflict`. The sale, line items, stock reductions, and inventory transaction records are saved together in a database transaction. Stock reductions use conditional database updates to prevent concurrent checkouts from selling more than is available.
