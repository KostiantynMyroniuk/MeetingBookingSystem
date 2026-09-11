---
name: plan
description: Break a requested feature or multi-part change into small, independently buildable vertical slices before any code is written. Use when a request describes a feature, epic, or change spanning several layers and no slice breakdown exists yet. Writes a plan file that the `slice` skill executes one item at a time.
argument-hint: "[feature or change description]"
---

# Feature Planning

Turn a feature request into an ordered list of small vertical slices before implementation starts. This skill only plans — it never writes implementation code, migrations, or tests. Implementation happens afterward through the `slice` skill (with `build` and `conventions` for .NET specifics).

## When to Use

Use this skill when:

- the request describes a feature, epic, or change that clearly spans multiple layers (data model, application logic, API, migration);
- the user asks to "plan", "break this down", or wants an implementation approach agreed before code is written;
- no slice breakdown exists yet for this piece of work.

Skip for:

- a change that is already a single small task — go straight to `slice`;
- a plan file already exists for this feature and just needs execution — read it and hand off to `slice` instead of re-planning.

## Process

1. Read enough of the existing project (solution structure, existing conventions, relevant existing entities/endpoints) to know what already exists versus what's new. Don't do a full codebase audit — just enough to plan accurately.
2. Identify every layer the feature touches: data model, application/service logic, API surface, cross-cutting behavior (auth, caching, logging), and any required data migration.
3. Split the work into the smallest set of slices that are each independently buildable and independently testable. Default ordering: data model → application/service → API endpoint → migration. A migration is always its own slice, never merged with behavior.
4. For each slice, write:
   - a one-sentence description;
   - IN SCOPE / NOT IN SCOPE;
   - the expected test level (see `conventions` for the .NET test-level mapping);
   - a flag if the slice touches authentication/authorization or a database migration, since those get extra scrutiny in `conventions`.
5. Note open questions or architectural decisions the plan depends on. Do not silently decide them yourself if the existing codebase doesn't already answer them — planning inherits the same rule `slice` uses for implementation ("Existing Architecture Wins"): don't invent a new architecture, surface it as a question instead.
6. Write the plan to `.claude/docs/plan-<short-feature-name>.md`, creating the `docs` folder if it doesn't exist. This keeps the plan alive across context compaction and separate sessions.
7. Present the plan to the user and stop.

## Plan File Format

```markdown
# Plan: <feature name>

## Slices

### Slice 1: <one-sentence description>
IN SCOPE:
- ...
NOT IN SCOPE:
- ...
Test level: <unit/integration/API>
Flags: <none | touches-auth | includes-migration>

### Slice 2: ...
```

## Boundaries

- Do not generate code, migrations, or tests in this skill — that's `slice`'s job.
- Do not invent an architecture the project doesn't already have. If a real architectural decision is required and the codebase doesn't already answer it, list it as an open question for the user rather than deciding unilaterally.
- If the feature turns out to be trivial once you look at it (single file, no new layer), say so and recommend skipping straight to `slice` instead of producing a multi-slice plan.

## Handoff

End every planning run with:

```text
Plan written to .claude/docs/plan-<name>.md (<n> slices).

Say "build slice 1" or invoke /slice to start implementing.
```

Do not start implementing after presenting the plan, even if the next step seems obvious.
