# Architecture Decision Record: Discard EF Core

## Context

Writing all SQL by hand and migrations is a lot of work. More productivity is needed.

## Decision

Use EF Core + separated sql solution project for database structure + deploy + migrations

## Status

Accepted

## Consequences

EF drawbacks:
	- It's a translator between C# and SQL and so sometimes translators translate things wrong. => We can always query using dapper or fallback to SQL easily
	- We already know and have to know SQL, so better use it. => We will still use it
	- Full database control (queries, writes, keys, indexes...) => Full database control as well, EF wont touch db structure, only use.
	- Full domain models control => Not full but almost full control, minimum changes only for EF.
	- Other tool/framework/query language to learn => Worth it by productivity and quite used tool
