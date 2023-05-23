# Architecture Decision Record: Discard EF Core

## Context

This is a new project, so EF maybe makes no sense as we already have everything

## Decision

Use FluentMigrations + Dapper

## Status

Accepted

## Consequences

EF drawbacks:
	- It's a translator between C# and SQL and so sometimes translators translate things wrong.
	- We already know and have to know SQL, so better use it.
	- Full database control (queries, writes, keys, indexes...)
	- Full domain models control
	- Other tool/framework/query language to learn
