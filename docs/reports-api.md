# Reports API

The reports endpoint requires a valid ERP JWT. Send it as a bearer token:

```http
Authorization: Bearer <token>
```

`GET /Reports?fromUtc=2026-10-01T00:00:00Z&toUtc=2026-10-31T23:59:59Z&groupBy=day`

Both UTC date/time parameters are required. `groupBy` can be `day` or `month` and defaults to `day`. The response includes total sales, sale count, current low-stock products, the ten best-selling products in the selected period, and sales totals by the selected period. A reversed date range or unsupported `groupBy` value returns `400 Bad Request`.
