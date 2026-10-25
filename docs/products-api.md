# Products API

All product endpoints require a valid ERP JWT. Send it as a bearer token:

```http
Authorization: Bearer <token>
```

The API uses the `/Products` route.

| Method | Route | Purpose | Success |
|---|---|---|---|
| `POST` | `/Products` | Create a product | `200 OK` |
| `GET` | `/Products` | List products | `200 OK` |
| `GET` | `/Products/{id}` | Get one product | `200 OK` |
| `PUT` | `/Products/{id}` | Update product details | `200 OK` |
| `DELETE` | `/Products/{id}` | Delete a product with no inventory or sales history | `204 No Content` |

Create and update request bodies use these fields:

```json
{
  "name": "Sparkling Water",
  "sku": "DRK-001",
  "description": "500 ml bottle",
  "sellingPrice": 0.75,
  "categoryId": 1,
  "isActive": true
}
```

Name and SKU are required. Name is limited to 200 characters, SKU to 100 characters, and description to 1,000 characters. Selling price must be greater than zero, and category ID must identify an existing category. New products are active by default if `isActive` is omitted.

Invalid fields, an unknown category, or a duplicate SKU return `400 Bad Request`. Missing products return `404 Not Found`. Products referenced by inventory transactions or sales cannot be deleted and return `409 Conflict`. Unauthenticated requests return `401 Unauthorized`.
