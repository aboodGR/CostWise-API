# Categories API

All category endpoints require a valid ERP JWT. Send it as a bearer token:

```http
Authorization: Bearer <token>
```

The API uses the `/Categories` route.

| Method | Route | Purpose | Success |
|---|---|---|---|
| `POST` | `/Categories` | Create a category | `200 OK` |
| `GET` | `/Categories` | List categories | `200 OK` |
| `GET` | `/Categories/{id}` | Get one category | `200 OK` |
| `PUT` | `/Categories/{id}` | Rename a category | `200 OK` |
| `DELETE` | `/Categories/{id}` | Delete a category with no products | `204 No Content` |

Create and update request bodies contain a required name of at most 100 characters:

```json
{
  "name": "Drinks"
}
```

Duplicate names return `400 Bad Request`. Updating or deleting a missing category returns `404 Not Found`. Deleting a category that still has products returns `409 Conflict`. Invalid request data returns `400 Bad Request`, and unauthenticated requests return `401 Unauthorized`.
