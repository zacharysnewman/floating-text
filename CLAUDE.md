# CLAUDE.md

## Development Workflow

**Before changing any code, update `SPEC.md` first.**

`SPEC.md` is the single source of truth for all runtime behavior, public API contracts, field semantics, and invariants. Code must match the spec. If a spec entry is wrong, update the spec and then update the code — never the other way around.

### Steps

1. Update `SPEC.md` to reflect the intended behavior.
2. Implement the change to match the spec.
3. Do not deviate from the spec without updating it first.
