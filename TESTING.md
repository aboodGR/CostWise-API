# CostWise Manual API Testing

## 1. Purpose

This file defines how CostWise API should be manually tested before deployment.

The goal is to verify:

- authentication works
- authorization works
- CRUD operations work
- user ownership is enforced
- invalid requests return correct status codes
- reports only use the authenticated user's data
- no user can access or modify another user's data

Testing should focus on real API behavior, not only whether the project builds.

---

## 2. Testing Rules

Before testing:

1. Read `AGENTS.md`.
2. Read `PROJECT.md`.
3. Build the project.
4. Start the API.
5. Use the actual configured API endpoints.
6. Do not modify application code unless a real bug is found.
7. Do not create migrations.
8. Do not modify database schema.
9. Do not weaken authentication or ownership checks just to make tests pass.

If a test fails:

1. Record the request.
2. Record the expected result.
3. Record the actual result.
4. Identify the likely file responsible.
5. Explain the bug before editing anything.
6. Make the smallest fix possible.
7. Rebuild.
8. Retest the failed scenario.
9. Retest related security scenarios.

Do not silently fix bugs without reporting them.

---

## 3. Test Users

Create at least two separate users.

Use names conceptually like:

- User A
- User B

Use valid test emails and passwords.

Never use real personal passwords.

The purpose of having two users is to verify that ownership isolation works correctly.

---

## 4. Authentication Tests

### Register User A

Test:

- valid email
- valid password

Expected:

- successful registration

Also test:

- duplicate email
- invalid email
- password shorter than allowed

Expected:

- correct validation or business error
- no duplicate user created

---

### Login User A

Test valid credentials.

Expected:

- successful response
- JWT token returned
- email returned

Save the JWT for later authenticated requests.

Also test:

- wrong password
- unknown email

Expected:

- 401 Unauthorized

---

### Protected Endpoint Without Token

Call a protected endpoint such as:

- Expenses
- Income
- Categories
- Reports

without a JWT.

Expected:

- 401 Unauthorized

---

### Invalid Token

Call a protected endpoint using an invalid or corrupted token.

Expected:

- 401 Unauthorized

---

## 5. Category Tests

Using User A token:

### Create Category

Create a category such as:

`Food`

Expected:

- success
- category response DTO returned
- UserId is not exposed or controlled by request

---

### Duplicate Category

Create another category named:

`Food`

for User A.

Expected:

- rejected
- correct 400-level response

---

### Get Categories

Expected:

- only User A categories returned

---

### Get Category By Id

Expected:

- correct User A category returned

Test invalid/nonexistent ID.

Expected:

- 404 Not Found

---

### Update Category

Update a category name.

Expected:

- success
- updated response DTO

Also update without changing the name.

Expected:

- should still succeed
- should not incorrectly report duplicate

---

### Delete Category

Test normal delete behavior.

Be careful if the category is referenced by Expenses.

Do not assume related Expense records should be deleted automatically.

---

## 6. Expense Tests

Using User A:

Create a valid category first.

### Create Expense

Example data:

- Title
- Amount
- CategoryId
- Date
- Description

Expected:

- success
- response DTO returned
- correct CategoryName returned
- authenticated UserId is used internally

---

### Invalid Category

Create Expense using a category ID that does not exist.

Expected:

- rejected
- correct 400-level response

---

### Get All Expenses

Expected:

- only User A expenses returned

---

### Get Expense By Id

Expected:

- correct expense returned
- correct CategoryName

Test nonexistent ID.

Expected:

- 404 Not Found

---

### Update Expense

Test:

- valid update
- valid category
- changed values

Expected:

- 200 OK
- updated DTO returned

Test nonexistent Expense ID.

Expected:

- 404 Not Found

Test invalid Category ID.

Expected:

- 400 Bad Request

---

### Delete Expense

Expected:

- successful deletion

Then request deleted expense again.

Expected:

- 404 Not Found

---

## 7. Income Tests

Using User A:

### Create Income

Expected:

- success
- response DTO returned

---

### Get All Income

Expected:

- only User A income returned

---

### Get Income By Id

Expected:

- correct income returned

Nonexistent ID:

- 404 Not Found

---

### Update Income

Expected:

- success
- updated response DTO

---

### Delete Income

Expected:

- success

Deleted record should no longer be available.

---

## 8. Report Tests

Create known values for User A.

Example:

Income:

`1000`

Expenses:

`200`
`300`

Expected summary:

- TotalIncome = 1000
- TotalExpenses = 500
- Balance = 500

The exact numbers used may differ, but calculate the expected result before calling the endpoint.

Verify the API result matches the manually calculated totals.

---

## 9. User Ownership Security Tests

These tests are critical.

Create and login User B.

Save User B's JWT.

User B must not be able to access User A's data.

---

### Expenses

Using User B token, try to:

- GET User A expense by ID
- UPDATE User A expense
- DELETE User A expense

Expected:

- User A data must not be exposed or modified

Use the API's existing not-found/security behavior.

---

### Income

Using User B token, try to:

- GET User A income
- UPDATE User A income
- DELETE User A income

Expected:

- User A data must not be exposed or modified

---

### Categories

Using User B token, try to:

- GET User A category
- UPDATE User A category
- DELETE User A category

Expected:

- User A data must not be exposed or modified

---

### Cross-User Category Attack

This test is especially important.

1. User A creates a Category.
2. Record its CategoryId.
3. Login as User B.
4. Try to create an Expense using User A's CategoryId.

Expected:

- request must fail
- User B must not be able to attach an Expense to User A's Category

Also test the same scenario when updating a User B Expense.

---

## 10. Report Isolation Test

Create different financial data for User A and User B.

Example:

User A:
- Income = 1000
- Expenses = 200

User B:
- Income = 5000
- Expenses = 1500

Call report endpoint using User A token.

Expected:

- only User A totals

Call using User B token.

Expected:

- only User B totals

No totals should leak between users.

---

## 11. Validation Tests

Test invalid DTO input.

Examples:

- missing required Title
- Title longer than maximum
- Amount below allowed minimum
- Amount above allowed maximum
- invalid CategoryId
- description above maximum length
- invalid email
- short password

Expected:

- 400 Bad Request where appropriate
- validation details returned by ASP.NET Core
- controller/service should not continue with invalid DTO data

---

## 12. Response DTO Check

Verify API responses do not expose unnecessary EF Core data.

Responses should not expose things such as:

- PasswordHash
- User navigation objects
- full Category navigation object when only CategoryName is needed
- internal ownership fields unless intentionally part of the API

Create/update Expense, Income, and Category endpoints should return their intended response DTO shape.

---

## 13. Status Code Check

Verify common outcomes:

- successful GET -> 200
- successful update -> 200
- successful create -> expected success code used by project
- validation failure -> 400
- invalid business input -> 400
- invalid login -> 401
- missing authentication -> 401
- missing owned resource -> 404

Do not accept incorrect status codes just because the response body looks correct.

---

## 14. Logging Check

While testing, inspect application logs.

Verify:

- request timing logs appear
- unexpected exceptions are visible
- passwords are never logged
- JWT tokens are not intentionally logged
- sensitive configuration is not logged

---

## 15. Build Check

Before declaring testing complete:

Run a final build.

Expected:

- 0 errors

Existing known warnings may remain unless they block functionality or indicate a serious issue.

Do not claim warnings were fixed unless they were actually addressed.

---

## 16. Final Testing Report

After testing, provide a concise report containing:

### Passed

List scenarios that passed.

### Failed

For each failed scenario include:

- endpoint
- request type
- expected behavior
- actual behavior
- likely cause

### Fixed During Testing

List bugs that were discovered and fixed.

### Not Tested

Clearly state anything that could not be tested.

### Final Result

State whether CostWise is ready for deployment.

Do not say deployment-ready if important authentication, ownership, CRUD, or report-isolation tests are still failing.

---

## 17. Main Testing Rule

Do not test only the happy path.

The most important CostWise tests are the ones that prove:

- one user cannot access another user's data
- one user cannot use another user's category
- reports cannot mix users
- protected endpoints reject unauthenticated requests

Security and ownership are part of correctness.